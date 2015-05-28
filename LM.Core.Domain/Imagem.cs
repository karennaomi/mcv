
namespace LM.Core.Domain
{
    public enum ImagemInterface
    {
        NotAssigned = 1,
        Web = 2,
        Android = 3,
        Ios = 4,
        All = 5
    }

    public enum ImagemResolucao
    {
        NotAssigned = 1,
        ObjetoCapturadoViaRobo = 2,
        Alta = 3,
        Media = 4,
        Baixa = 5,
        ExtraAlta = 6
    }

    public class Imagem
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public ImagemInterface Interface { get; set; }
        public ImagemResolucao Resolucao { get; set; }
    }
}
