using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.Application
{
    public interface IPontoDemandaAplicacao
    {
        PontoDemanda Criar(long usuarioId, PontoDemanda pontoDemanda);
        IList<PontoDemanda> Listar(long usuarioId);
        PontoDemanda Obter(long usuarioId, long pontoDemandaId);
        PontoDemanda DefinirFrequenciaDeCompra(long usuarioId, long pontoDemandaId, int frequencia);
        long VerificarPontoDemanda(long usuarioId, long pontoDemandaId);
        Loja AdicionarLojaFavorita(long usuarioId, long pontoDemandaId, Loja loja);
        void RemoverLojaFavorita(long usuarioId, long pontoDemandaId, string localizadorId);
    }

    public class PontoDemandaAplicacao : IPontoDemandaAplicacao
    {
        private readonly IRepositorioPontoDemanda _repositorio;
        private readonly IUsuarioAplicacao _appUsuario;
        public PontoDemandaAplicacao(IRepositorioPontoDemanda repositorio, IUsuarioAplicacao appUsuario)
        {
            _repositorio = repositorio;
            _appUsuario = appUsuario;
        }

        public PontoDemanda Criar(long usuarioId, PontoDemanda pontoDemanda)
        {
            pontoDemanda = _repositorio.Criar(usuarioId, pontoDemanda);
            _appUsuario.AtualizarStatusCadastro(usuarioId, StatusCadastro.EtapaDeInformacoesDoPontoDeDemandaCompleta, pontoDemanda.Id);
            return pontoDemanda;
        }

        public IList<PontoDemanda> Listar(long usuarioId)
        {
            return _repositorio.Listar(usuarioId);
        }

        public PontoDemanda Obter(long usuarioId, long pontoDemandaId)
        {
            return _repositorio.Obter(usuarioId, pontoDemandaId);
        }

        public PontoDemanda DefinirFrequenciaDeCompra(long usuarioId, long pontoDemandaId, int frequencia)
        {
            var pontoDemanda = Obter(usuarioId, pontoDemandaId);
            switch (frequencia)
            {
                case 1:
                    pontoDemanda.QuantidadeDiasAlertaReposicao = 7;
                    break;
                case 2:
                    pontoDemanda.QuantidadeDiasAlertaReposicao = 14;
                    break;
                case 3:
                    pontoDemanda.QuantidadeDiasAlertaReposicao = 28;
                    break;
                default:
                    throw new ApplicationException("Frequência inválida");
            }
            pontoDemanda.QuantidadeDiasCoberturaEstoque = 3;
            _appUsuario.AtualizarStatusCadastro(usuarioId, StatusCadastro.FrequenciaDeCompraCompleta, pontoDemandaId);
            _repositorio.Salvar();
            return pontoDemanda;
        }

        public long VerificarPontoDemanda(long usuarioId, long pontoDemandaId)
        {
            var pontosDemanda = Listar(usuarioId);
            if (pontosDemanda.All(p => p.Id != pontoDemandaId)) throw new PontoDemandaInvalidoException("Ponto de demanda não pertence ao usuário atual.");
            return pontoDemandaId;
        }

        public Loja AdicionarLojaFavorita(long usuarioId, long pontoDemandaId, Loja loja)
        {
            var pontoDemanda = Obter(usuarioId, pontoDemandaId);
            return _repositorio.AdicionarLojaFavorita(usuarioId, pontoDemanda, loja);
        }

        public void RemoverLojaFavorita(long usuarioId, long pontoDemandaId, string localizadorId)
        {
            var pontoDemanda = Obter(usuarioId, pontoDemandaId);
            var loja = pontoDemanda.LojasFavoritas.SingleOrDefault(l => l.Idlocalizador == localizadorId);
            if(loja == null) throw new ObjetoNaoEncontradoException("Loja não encontrada.");
            pontoDemanda.LojasFavoritas.Remove(loja);
            _repositorio.Salvar();
        }
    }
}
