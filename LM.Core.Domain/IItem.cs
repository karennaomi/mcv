using System;

namespace LM.Core.Domain
{
    public interface IItem
    {
        long Id { get; set; }
        DateTime? DataInclusao { get; set; }
        DateTime? DataAlteracao { get; set; }
        Produto Produto { get; set; }
        PontoDemanda ObterPontoDemanda();
        decimal ObterQuantidadeParaCompra();
    }
}
