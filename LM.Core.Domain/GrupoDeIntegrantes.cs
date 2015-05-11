using System.Collections.Generic;
using System.Linq;

namespace LM.Core.Domain
{
    public class GrupoDeIntegrantes
    {
        public GrupoDeIntegrantes()
        {
            Nome = "Grupo de integrantes";
        }
        public long Id { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<Integrante> Integrantes { get; set; }
        public virtual ICollection<PontoDemanda> PontosDemanda { get; set; }

        public IEnumerable<Integrante> IntegrantesAtivos()
        {
            return Integrantes.Where(i => i.Ativo);
        }
    }
}
