
namespace LM.Core.Domain
{
    public enum StatusCadastro
    {
        UsuarioNaoCadastrado = 0,
        CadastroIniciado = 1,
        InicioCadastroRedeSocial = 2,
        InicioCadastroProprietario = 3,
        EtapaDeInformacoesPessoaisCompleta = 4,
        EtapaDeEnderecoDoPontoDeDemandaCompleta = 5,
        EtapaDeInformacoesDoPontoDeDemandaCompleta = 6,
        EtapaDoGrupoDeIntegrantesCompleta = 7,
        EtapaDasLojasFavoritasCompleta = 8,
        EtapaDeAceitacaoRecusaDaListaDefaultCompleta = 9,
        EdicaoDaListaDefault = 10,
        FrequenciaDeCompraCompleta = 11,
        TelaDeLoading = 12,
        UsuarioConvidado = 20,
        UsuarioOk = 99
    }
}
