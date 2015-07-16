using System.Text;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface IContatoAplicacao
    {
        Contato Criar(Contato contato, string emailDestinatario);
    }

    public class ContatoAplicacao : IContatoAplicacao
    {
        private readonly IRepositorioContato _repositorio;
        private readonly INotificacaoAplicacao _appNotificacao;
        public ContatoAplicacao(IRepositorioContato repositorio, INotificacaoAplicacao appNotificacao)
        {
            _repositorio = repositorio;
            _appNotificacao = appNotificacao;
        }

        public Contato Criar(Contato contato, string emailDestinatario)
        {
            contato =_repositorio.Criar(contato);
            var corpo = new StringBuilder();
            corpo.Append("<p>Nome: ");
            corpo.Append(contato.Nome);
            corpo.Append("</p>");
            corpo.Append("<p>E-mail: ");
            corpo.Append(contato.Email);
            corpo.Append("</p>");
            corpo.Append("<p>Mensagem:</p>");
            corpo.Append(contato.Mensagem);
            _appNotificacao.EnviarEmail("[Lista Mágica] Contato do site", corpo.ToString(), emailDestinatario);
            return contato;
        }
    }
}
