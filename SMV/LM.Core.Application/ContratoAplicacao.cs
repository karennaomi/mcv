using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface IContratoAplicacao
    {
        Contrato ObterAtivo();
    }

    public class ContratoAplicacao : IContratoAplicacao
    {
        private readonly IRepositorioContrato _repositorio;
        public ContratoAplicacao(IRepositorioContrato repositorio)
        {
            _repositorio = repositorio;
        }

        public Contrato ObterAtivo()
        {
            return _repositorio.ObterAtivo();
        }
    }
}
