using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public class EnderecoCorreiosService : ICepAplicacao
    {
        private readonly IRepositorioCorreios _correiosRepo;
        public EnderecoCorreiosService(IRepositorioCorreios correiosRepo)
        {
            _correiosRepo = correiosRepo;
        }

        public Endereco BuscarPorCep(string cep)
        {
            var enderecoCorreios = _correiosRepo.BuscarPorCep(cep);
            if(enderecoCorreios == null) throw new ObjetoNaoEncontradoException("Cep não encontrado.");
            return enderecoCorreios.ObterEndereco();
        }
    }
}
