
namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioTemplateMensagem
    {
        T ObterPorTipo<T>(TipoTemplateMensagem tipo) where T : TemplateMensagem;
    }
}
