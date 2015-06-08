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
        public CompraAtivaEF(ContextoEF contexto)
        {
            _contexto = contexto;
        }

        public CompraAtiva Obter(long pontoDemandaId)
        {
            return _contexto.ComprasAtivas.FirstOrDefault(c => c.PontoDemanda.Id == pontoDemandaId && !c.FimCompra.HasValue);
        }

        public CompraAtiva AtivarCompra(long usuarioId, long pontoDemandaId)
        {
            var compraAtiva = _contexto.ComprasAtivas.Create();
            compraAtiva.InicioCompra = DateTime.Now;

            var pontoDemanda = _contexto.PontosDemanda.Create();
            pontoDemanda.Id = pontoDemandaId;
            compraAtiva.PontoDemanda = pontoDemanda;

            var usuario = _contexto.Usuarios.Create();
            usuario.Id = usuarioId;
            compraAtiva.Usuario = usuario;

            _contexto.Entry(compraAtiva.Usuario).State = EntityState.Unchanged;
            _contexto.Entry(compraAtiva.PontoDemanda).State = EntityState.Unchanged;
            _contexto.ComprasAtivas.Add(compraAtiva);
            _contexto.SaveChanges();
            return compraAtiva;
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
