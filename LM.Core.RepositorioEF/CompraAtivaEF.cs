using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System;
using System.Data.Entity;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class CompraAtivaEF : IRepositorioCompraAtiva
    {
        private readonly ContextoEF _contexto;
        public CompraAtivaEF()
        {
            _contexto = new ContextoEF();
        }

        public CompraAtiva AtivarCompra(long usuarioId, long pontoDemandaId)
        {
             var compraAtiva = _contexto.ComprasAtivas.Add(new CompraAtiva
             {
                 PontoDemanda = new PontoDemanda { Id = pontoDemandaId},
                 Usuario = new Usuario { Id = usuarioId },
                 InicioCompra = DateTime.Now
             });
            _contexto.Entry(compraAtiva.PontoDemanda).State= EntityState.Unchanged;
            _contexto.Entry(compraAtiva.Usuario).State = EntityState.Unchanged;
            _contexto.SaveChanges();
            return compraAtiva;
        }

        public CompraAtiva FinalizarCompra(long usuarioId, long pontoDemandaId)
        {
            var compraAtiva = Obter(usuarioId, pontoDemandaId);
            if (compraAtiva == null) throw new ApplicationException("Nenhuma compra ativa para o ponto de demanda especificado.");
            compraAtiva.FimCompra = DateTime.Now;
            _contexto.SaveChanges();
            return compraAtiva;
        }

        private CompraAtiva Obter(long usuarioId, long pontoDemandaId)
        {
            return _contexto.ComprasAtivas.FirstOrDefault(c => c.PontoDemanda.Id == pontoDemandaId && c.Usuario.Id == usuarioId && c.FimCompra == null);
        }
    }
}
