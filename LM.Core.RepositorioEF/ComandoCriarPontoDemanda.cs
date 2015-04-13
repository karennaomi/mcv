using System.Data.Entity;
using System.Linq;
using LM.Core.Domain;

namespace LM.Core.RepositorioEF
{
    public class ComandoCriarPontoDemanda
    {
        private readonly ContextoEF _contexto;
        private readonly UsuarioEF _usuarioRepo;
        private readonly CidadeEF _cidadeRepo;
        private readonly long _usuarioId;
        private PontoDemanda _novoPontoDemanda;
        public ComandoCriarPontoDemanda(long usuarioId, PontoDemanda novoPontoDemanda)
        {
            _contexto = new ContextoEF();
            _usuarioRepo = new UsuarioEF(_contexto);
            _cidadeRepo = new CidadeEF(_contexto);
            _usuarioId = usuarioId;
            _novoPontoDemanda = novoPontoDemanda;
        }

        public PontoDemanda Executar()
        {
            _novoPontoDemanda.GrupoDeIntegrantes = ObterGrupoDeIntegrantesDoUsuario();
            _novoPontoDemanda.Endereco.Cidade = _cidadeRepo.Buscar(_novoPontoDemanda.Endereco.Cidade.Nome);
            _novoPontoDemanda = _contexto.PontosDemanda.Add(_novoPontoDemanda);
            if (_novoPontoDemanda.LojasFavoritas != null)
            {
                foreach (var loja in _novoPontoDemanda.LojasFavoritas)
                {
                    loja.Info.Endereco.Cidade = _cidadeRepo.Buscar(loja.Info.Endereco.Cidade.Nome);
                }
                _cidadeRepo.LimparCidadeNovas();
            }
            
            _contexto.SaveChanges();
            
            _usuarioRepo.AtualizarStatusCadastro(_usuarioId, StatusCadastro.EtapaDeInformacoesDoPontoDeDemandaCompleta, _novoPontoDemanda.Id);
            _contexto.SaveChanges();

            return _novoPontoDemanda;
        }

        private GrupoDeIntegrantes ObterGrupoDeIntegrantesDoUsuario()
        {
            var usuario = _usuarioRepo.Obter(_usuarioId);
            return usuario.Integrante.GrupoDeIntegrantes;
        }

        //private Cidade BuscarCidade(string cidadeNome)
        //{
        //    var cidade = _cidadeRepo.Buscar(cidadeNome);
        //    var cidadeLocal = _contexto.Cidades.Local.SingleOrDefault(c => c.Id == cidade.Id);
        //    if (cidadeLocal != null)
        //    {
        //        return cidadeLocal;
        //    }
        //    _contexto.Entry(cidade).State = EntityState.Unchanged;
        //    return cidade;
        //}
    }
}
