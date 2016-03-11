using System;
using System.Text.RegularExpressions;

namespace LM.Core.Application
{
    public class TemplateProcessor
    {
        public static string ProcessTemplate(string template, object entityInstance)
        {
            var regex = new Regex("{(.+?)}", RegexOptions.Singleline);
            var matchEvaluator = new MatchEvaluator(m => GetValues(m, entityInstance));
            return regex.Replace(template, matchEvaluator);
        }

        private static string GetValues(Match match, object entityInstance)
        {
            var obj = GetPropertyValue(entityInstance, match.Groups[1].Value);
            var valor = obj != null ? obj.ToString() : string.Empty;
            return valor;
        }

        private static object GetPropertyValue(object obj, string property)
        {
            try
            {
                if (!property.Contains(".")) return obj.GetType().GetProperty(property).GetValue(obj, null);
                var objProp = property.Remove(property.IndexOf("."));
                var prop = property.Substring(property.IndexOf(".") + 1);

                if (objProp == obj.GetType().Name) return GetPropertyValue(obj, prop);

                var newObj = obj.GetType().GetProperty(objProp).GetValue(obj, null);
                return GetPropertyValue(newObj, prop);
            }
            catch (NullReferenceException ex)
            {
                throw new ApplicationException(String.Format("Propriedade inválida no template: {0}", property), ex);
            }
        }
    }
}
