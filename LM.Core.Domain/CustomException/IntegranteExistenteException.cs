namespace LM.Core.Domain.CustomException
{
    public class IntegranteExistenteException : PropertyException
    {
        public IntegranteExistenteException() : base("", "") { }
        public IntegranteExistenteException(string property, string propertyName)
            : base(string.Format("O {0} informado já está cadastrado!", propertyName), property)
        {
        }
    }
}
