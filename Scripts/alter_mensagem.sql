select* from TB_Mensagem
alter table [dbo].[TB_Mensagem] add [ID_TIPO_TEMPLATE] int not null default 0
alter table [dbo].[TB_Mensagem] add [Discriminator] int null