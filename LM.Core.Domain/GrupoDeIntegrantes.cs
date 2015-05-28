
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
        public PapelIntegrante Papel { get; set; }
        public virtual Integrante Integrante { get; set; }
        public virtual PontoDemanda PontoDemanda { get; set; }
    }
}
