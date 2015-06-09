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
        PontoDemanda Atualizar(long usuarioId, PontoDemanda pontoDemanda);
        void Desativar(long usuarioId, long pontoDemandaId);
        IList<PontoDemanda> Listar(long usuarioId);
        PontoDemanda Obter(long usuarioId, long pontoDemandaId);
        PontoDemanda DefinirFrequenciaDeCompra(long usuarioId, long pontoDemandaId, int frequencia);
        long VerificarPontoDemanda(long usuarioId, long pontoDemandaId);
        Loja AdicionarLojaFavorita(long usuarioId, long pontoDemandaId, Loja loja);
        void RemoverLojaFavorita(long usuarioId, long pontoDemandaId, string localizadorId);
        void RemoverIntegrante(long usuarioId, long pontoDemandaId, long integranteId);
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

        public PontoDemanda Atualizar(long usuarioId, PontoDemanda pontoDemanda)
        {
            return _repositorio.Atualizar(usuarioId, pontoDemanda);
        }

        public IList<PontoDemanda> Listar(long usuarioId)
        {
            return _repositorio.Listar(usuarioId);
        }

        public PontoDemanda Obter(long usuarioId, long pontoDemandaId)
        {
            return _repositorio.Obter(usuarioId, pontoDemandaId);
        }

        public void Desativar(long usuarioId, long pontoDemandaId)
        {
            if(Listar(usuarioId).Count == 1) throw new ApplicationException("Não pode desativar o único ponto de demanda existente.");
            var pontoDemanda = Obter(usuarioId, pontoDemandaId);
            pontoDemanda.Ativo = false;
            pontoDemanda.DataAlteracao = DateTime.Now;
            _repositorio.Salvar();
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
            _appUsuario.AtualizarStatusCadastro(usuarioId, StatusCadastro.UsuarioOk, pontoDemandaId);
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
            var loja = pontoDemanda.LojasFavoritas.SingleOrDefault(l => l.LocalizadorId == localizadorId);
            if(loja == null) throw new ObjetoNaoEncontradoException("Loja não encontrada.");
            pontoDemanda.LojasFavoritas.Remove(loja);
            _repositorio.Salvar();
        }

        public void RemoverIntegrante(long usuarioId, long pontoDemandaId, long integranteId)
        {
            var pontoDemanda = Obter(usuarioId, pontoDemandaId);
            var grupoIntegrante = pontoDemanda.GruposDeIntegrantes.SingleOrDefault(g => g.Integrante.Id == integranteId);
            if (grupoIntegrante == null) throw new ObjetoNaoEncontradoException("Integrante não pertence ao ponto de demanda atual.");
            if(pontoDemanda.UsuarioCriador.Id == grupoIntegrante.Integrante.Usuario.Id) throw new ApplicationException("Não pode excluir o usuario criador do ponto.");
            pontoDemanda.GruposDeIntegrantes.Remove(grupoIntegrante);
            _repositorio.Salvar();
        }
    }
}
