using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System;
using System.Data.Entity;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class CompraAtivaEF : IRepositorioCompraAtiva
    {
        private readonly ContextoEF _contextoEF;
        public CompraAtivaEF()
        {
            _contextoEF = new ContextoEF();
        }

        public CompraAtiva Obter(long pontoDemandaId, long usuarioId)
        {
            return _contextoEF.ComprasAtivas.FirstOrDefault( c => c.PontoDemanda.Id == pontoDemandaId && c.Usuario.Id == usuarioId && c.FimCompra == null);
        }

        public CompraAtiva AtivarCompra(long pontoDemandaId, long usuarioId)
        {
             var compraAtiva = _contextoEF.ComprasAtivas.Add(new CompraAtiva
             {
                 PontoDemanda = new PontoDemanda { Id = pontoDemandaId},
                 Usuario = new Usuario { Id = usuarioId },
                 InicioCompra = DateTime.Now
             });
            _contextoEF.Entry(compraAtiva.PontoDemanda).State= EntityState.Unchanged;
            _contextoEF.Entry(compraAtiva.Usuario).State = EntityState.Unchanged;
            _contextoEF.SaveChanges();
            return compraAtiva;
        }

        public CompraAtiva FinalizarCompra(CompraAtiva compraAtiva)
        {
            compraAtiva.FimCompra = DateTime.Now;
            _contextoEF.SaveChanges();
            return compraAtiva;
        }
    }
}
