
namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioTemplateMensagem
    {
        TemplateMensagem ObterPorTipoTemplate(TipoTemplateMensagem tipo);
    }
}
