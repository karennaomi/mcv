using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LM.Core.Domain
{
    public class LMRequiredAttribute : RequiredAttribute
    {
        public LMRequiredAttribute()
        {
            ErrorMessageResourceName = LMResource.Default_Validation_Required;
            ErrorMessageResourceType = typeof(LMResource);
            AllowEmptyStrings = false;
        }
    }

    public class LMMaxLengthAttribute : MaxLengthAttribute
    {
        public LMMaxLengthAttribute(int length) : base(length)
        {
            ErrorMessageResourceName = LMResource.Default_Validation_MaxLength;
            ErrorMessageResourceType = typeof(LMResource);
        }
    }

    public class LMMinLengthAttribute : MinLengthAttribute
    {
        public LMMinLengthAttribute(int length) : base(length)
        {
            ErrorMessageResourceName = LMResource.Default_Validation_MinLength;
            ErrorMessageResourceType = typeof(LMResource);
        }
    }

    public class LMEmailAttribute : RegularExpressionAttribute
    {
        public LMEmailAttribute() : base(Constantes.RegexTemplates.EmailRegex)
        {
            ErrorMessageResourceName = LMResource.Default_Validation_Email;
            ErrorMessageResourceType = typeof(LMResource);
        }
    }
}
