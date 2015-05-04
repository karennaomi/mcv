
using System;

namespace LM.Core.Domain
{
    public enum TipoPersona
    {
        NaoReconhecido = 0,
        Familia = 1,
        Empregado = 2,
        Pet = 3
    }

    public class Persona
    {
        public int Id { get; set; }
        public string Perfil { get; set; }
        public string Sexo { get; set; }
        public string FaixaEtaria { get; set; }
        public int IdadeInicial { get; set; }
        public int IdadeFinal { get; set; }
        public int Tipo { get; set; }

        public string Descricao()
        {
            switch (Perfil)
            {
                case "BEBÊ": 
                    return string.Format("BEBÊ {0} 0-3 ANOS", Sexo);
                case "IDOSO":
                    return Sexo == "M" ? "IDOSO +65" : "IDOSA +65";
                case "EMPREGADO":
                    return Sexo == "M" ? "EMPREGADO" : "EMPREGADA";
                case "CRIANÇA":
                case "ADOLESCENTE":
                    return Sexo == "M" ? string.Format("MENINO {0}-{1} ANOS", IdadeInicial, IdadeFinal) : string.Format("MENINA {0}-{1} ANOS", IdadeInicial, IdadeFinal);
                case "ADULTO":
                    return Sexo == "M" ? string.Format("HOMEM {0}-{1} ANOS", IdadeInicial, IdadeFinal) : string.Format("MULHER {0}-{1} ANOS", IdadeInicial, IdadeFinal);
                case "PET DOG":
                    return "CACHORRO";
                case "PET CAT":
                    return "GATO";
                default: throw new ApplicationException("Perfil inválido");        
            }
        }

        public string ObterImage()
        {
            var nomeImagem = Perfil.Contains("PET") ? string.Format("galeria-{0}.png", Perfil) : string.Format("galeria-{0}-{1}-{2}-{3}.png", Perfil, Sexo, IdadeInicial, IdadeFinal);
            return nomeImagem.Replace("Ê", "E").Replace("ê", "e").Replace("ç", "c").Replace("Ç", "C").Replace(" ", "_").ToLower();
        }
    }
}
