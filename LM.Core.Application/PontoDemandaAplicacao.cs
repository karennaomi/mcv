﻿using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IPontoDemandaAplicacao
    {
        PontoDemanda Criar(long usuarioId, PontoDemanda pontoDemanda);
        IList<PontoDemanda> Listar(long usuarioId);
        PontoDemanda Obter(long usuarioId, long pontoDemandaId);
        PontoDemanda DefinirFrequenciaDeConsumo(long usuarioId, long pontoDemandaId, int frequencia);
        long VerificarPontoDemanda(long usuarioId, long pontoDemandaId);
    }

    public class PontoDemandaAplicacao : IPontoDemandaAplicacao
    {
        private readonly IRepositorioPontoDemanda _repositorio;
        public PontoDemandaAplicacao(IRepositorioPontoDemanda repositorio)
        {
            _repositorio = repositorio;
        }

        public PontoDemanda Criar(long usuarioId, PontoDemanda pontoDemanda)
        {
            return _repositorio.Criar(usuarioId, pontoDemanda);
        }

        public IList<PontoDemanda> Listar(long usuarioId)
        {
            return _repositorio.Listar(usuarioId);
        }

        public PontoDemanda Obter(long usuarioId, long pontoDemandaId)
        {
            return _repositorio.Obter(usuarioId, pontoDemandaId);
        }

        public PontoDemanda DefinirFrequenciaDeConsumo(long usuarioId, long pontoDemandaId, int frequencia)
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
            }
            pontoDemanda.QuantidadeDiasCoberturaEstoque = 3;
            _repositorio.SalvarAlteracoes();
            return pontoDemanda;
        }

        public long VerificarPontoDemanda(long usuarioId, long pontoDemandaId)
        {
            var pontosDemanda = Listar(usuarioId);
            if (pontosDemanda.All(p => p.Id != pontoDemandaId)) throw new PontoDemandaInvalidoException("Ponto de demanda não pertence ao usuário atual.");
            return pontoDemandaId;
        }
    }
}
