using LM.Core.Domain.Repositorio;
using System;

namespace LM.Core.RepositorioEF
{
    public class UnitOfWorkEF : IUnitOfWork, IDisposable
    {
        private readonly ContextoEF _contexto = new ContextoEF();
        private IRepositorioIntegrante _integranteRepo;
        private IRepositorioPontoDemanda _pontoDemandaRepo;
        
        public IRepositorioIntegrante IntegranteRepo
        {
            get { return _integranteRepo ?? (_integranteRepo = new IntegranteEF(_contexto)); }
        }

        public IRepositorioPontoDemanda PontoDemandaRepo
        {
            get { return _pontoDemandaRepo ?? (_pontoDemandaRepo = new PontoDemandaEF(_contexto)); }
        }

        public void SalvarAlteracoes()
        {
            _contexto.SaveChanges();
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _contexto.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
