using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace H.Framework.WPF.Infrastructure.Utilities
{
    public static class ValidationExtension
    {
        public static string ValidateProperty(this object obj, string columnName)
        {
            var vc = new ValidationContext(obj);
            vc.MemberName = columnName;
            var res = new List<ValidationResult>();
            Validator.TryValidateProperty(obj.GetType().GetProperty(columnName).GetValue(obj, null), vc, res);
            if (res.Count > 0)
            {
                return string.Join(Environment.NewLine, res.Select(r => r.ErrorMessage).ToArray());
            }
            return string.Empty;
        }

        public static string ValidateProperty<MetaDataType>(this object obj, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return string.Empty;

            var targetType = obj.GetType();
            //也可以利用 MetadataType 在分离类上声明
            //var targetMetadataAttr = targetType.GetCustomAttributes(false)
            //    .FirstOrDefault(a => a.GetType() == typeof(MetadataTypeAttribute)) as MetadataTypeAttribute;
            //if (targetMetadataAttr != null && targetType != targetMetadataAttr.MetadataClassType)
            //{
            //    TypeDescriptor.AddProviderTransparent(
            //           new AssociatedMetadataTypeTypeDescriptionProvider(targetType, targetMetadataAttr.MetadataClassType), targetType);
            //}
            if (targetType != typeof(MetaDataType))
            {
                TypeDescriptor.AddProviderTransparent(
                       new AssociatedMetadataTypeTypeDescriptionProvider(targetType, typeof(MetaDataType)), targetType);
            }

            var propertyValue = targetType.GetProperty(propertyName).GetValue(obj, null);
            var validationContext = new ValidationContext(obj, null, null);
            validationContext.MemberName = propertyName;
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateProperty(propertyValue, validationContext, validationResults);

            if (validationResults.Count > 0)
            {
                return validationResults.First().ErrorMessage;
            }
            return string.Empty;
        }
    }
}