using System;
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
            object arg = null;
            if (ArgProperty == "this")
                arg = validationContext.ObjectInstance;
            else
                arg = validationContext.ObjectInstance.GetType().GetProperty(ArgProperty).GetValue(validationContext.ObjectInstance);
            return (ValidationResult)ValidatorType.GetMethod(Method).Invoke(null, new object[] { value, arg });
        }
    }
}