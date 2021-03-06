alter table [dbo].[TB_Compra_Item] add ID_PEDIDO bigint null
ALTER TABLE [dbo].[TB_Compra_Item]  WITH CHECK ADD  CONSTRAINT [FK_TB_COMPRA_ITEM_TB_PEDIDO] FOREIGN KEY([ID_PEDIDO]) REFERENCES [dbo].[TB_Pedido] ([ID_PEDIDO])
update TB_Compra_Item set ID_PEDIDO = p.ID_PEDIDO from TB_Compra_Item_Pedido p inner join TB_Compra_Item c on p.ID_COMPRA_ITEM = c.ID_COMPRA_ITEM
ALTER TABLE [dbo].[TB_Compra_Item] add Discriminator varchar(50)
update [dbo].[TB_Compra_Item] set Discriminator = 'ListaCompraItem' where ID_LISTA_PRODUTO_ITEM is not null
update [dbo].[TB_Compra_Item] set Discriminator = 'PedidoCompraItem' where ID_PEDIDO is not null