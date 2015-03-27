alter table [dbo].[TB_CompraAtiva] drop column [ID_Integrante]
alter table [dbo].[TB_CompraAtiva] add [ID_Usuario] bigint not null

ALTER TABLE [dbo].[TB_CompraAtiva]  WITH CHECK ADD  CONSTRAINT [FK_1_CompraAtiva_Usuario] FOREIGN KEY([ID_Usuario])
REFERENCES [dbo].[TB_Usuario] ([ID_Usuario])

CREATE NONCLUSTERED INDEX IX_CompraAtiva_ID_Ponto_Real_Demanda ON [dbo].[TB_CompraAtiva] (ID_Ponto_Real_Demanda); 