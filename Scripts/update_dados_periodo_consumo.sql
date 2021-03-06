select * from [TB_Periodo_Consumo]

alter table [TB_Lista_Produto_Item] drop constraint FK_2_ListaProdutoItem_PeriodoConsumo

delete from [TB_Periodo_Consumo] 

select distinct ID_PERIODO_CONSUMO from [TB_Lista_Produto_Item]

insert into [TB_Periodo_Consumo] values(1, 'Diário', 1)
insert into [TB_Periodo_Consumo] values(2, 'Semanal', 7)
insert into [TB_Periodo_Consumo] values(3, 'Mensal', 30)
insert into [TB_Periodo_Consumo] values(4, 'Eventual', 1)

update [TB_Lista_Produto_Item] set ID_PERIODO_CONSUMO = 1 where ID_PERIODO_CONSUMO = 0
update [TB_Lista_Produto_Item] set ID_PERIODO_CONSUMO = 2 where ID_PERIODO_CONSUMO = 3

ALTER TABLE [TB_Lista_Produto_Item] ADD CONSTRAINT FK_2_ListaProdutoItem_PeriodoConsumo FOREIGN KEY (ID_PERIODO_CONSUMO)  REFERENCES [TB_Periodo_Consumo] (ID_PERIODO_CONSUMO)