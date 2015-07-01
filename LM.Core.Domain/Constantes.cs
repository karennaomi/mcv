
namespace LM.Core.Domain
{
    public class Constantes
    {
        public struct RegexTemplates
        {
            public const string EmailRegex = @"^(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})$";
        }

        public struct Usuario
        {
            public const int TamanhoMinimoSenha = 6;
        }

        public struct Integrante
        {
            public const int IdadeMinimaCadastro = 13;
            public const int DiasMinimosParaConvidarNovamente = 1;
            public const int TamanhoMaximoNome = 70;
            public const int TamanhoMaximoEmail = 50;
            public const string SexoMasculino = "m";
            public const string SexoFeminino = "f";
        }

        public struct Produto
        {
            public const int TamanhoMaximoEan = 13;
        }

        public struct Categoria
        {
            public const int CategoriaPadrao = 3;
            public const string CategoriaImagemPadrao = "/categorias/placeholder.png";    
        }
        
        public struct PontoDemanda
        { 
            public const short QuantidadeDiasCoberturaEstoque = 3;
        }
    }
}
