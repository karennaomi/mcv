
namespace LM.Core.Domain
{
    public class Constantes
    {
        public struct Usuario
        {
            public const string TamanhoMinimoSenhaString = "6";
            public const int TamanhoMinimoSenha = 6;
        }

        public struct Integrante
        {
            public const int IdadeMinimaCadastro = 13;
            public const int DiasMinimosParaConvidarNovamente = 1;
        }

        public struct Produto
        {
            public const string TamanhoMaximoEanString = "13";
            public const int TamanhoMaximoEan = 13;
        }

        public struct Categoria
        {
            public const int CategoriaPadrao = 3;
            public const string CategoriaImagemPadrao = "http://www.smvcompany.com.br/imagens-prod/placeholder.png";    
        }
        
        public struct PontoDemanda
        { 
            public const short QuantidadeDiasCoberturaEstoque = 3;
        }
    }
}
