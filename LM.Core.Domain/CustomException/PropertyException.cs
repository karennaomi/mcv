using System;

namespace LM.Core.Domain.CustomException
{
    public class PropertyException : ApplicationException
    {
        public string Property { get; set; }

        public PropertyException(string message, string property) : base(message)
        {
            Property = property;
        }
    }
}
