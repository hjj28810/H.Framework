using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace H.Framework.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataValidationAttribute : ValidationAttribute
    {
        public DataValidationAttribute(Type validatorType, string method, string argProperty = null)
        {
            ValidatorType = validatorType;
            Method = method;
            ArgProperty = argProperty;
        }

        public Type ValidatorType { get; set; }

        public string Method { get; set; }

        public string ArgProperty { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var param = new List<object> { value };
            if (!string.IsNullOrWhiteSpace(ArgProperty))
                param.Add(ArgProperty == "this" ? validationContext.ObjectInstance : validationContext.ObjectInstance.GetType().GetProperty(ArgProperty)?.GetValue(validationContext.ObjectInstance));
            return (ValidationResult)ValidatorType.GetMethod(Method)?.Invoke(null, param.ToArray());
        }
    }
}