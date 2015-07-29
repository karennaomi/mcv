USE [CORREIOS]
GO

/****** Object:  Table [dbo].[CEP_TODOS]    Script Date: 6/16/2015 3:06:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[EnderecoCorreios](
	[Uf] [varchar](2) NULL,
	[CidadeId] [varchar](50) NULL,
	[Cidade] [varchar](500) NULL,
	[Bairro] [varchar](500) NULL,
	[Logradouro] [varchar](500) NULL,
	[Cep] [varchar](8) NOT NULL,
	[Complemento] [varchar](500) NULL,
	[Nome] [varchar](500) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

insert into [EnderecoCorreios] 
select log_logradouro.ufe_sg, log_localidade.loc_nu, log_localidade.loc_no,  log_bairro.bai_no, log_logradouro.tlo_tx+' ' + log_logradouro.log_no as log_no, log_logradouro.cep,  log_logradouro.log_complemento,'' as nome
from log_logradouro, log_localidade, log_bairro
where log_logradouro.loc_nu= log_localidade.loc_nu  and log_logradouro.bai_nu_ini=log_bairro.bai_nu and log_logradouro.log_sta_tlo ='s' and log_logradouro.cep != ''
union
select log_logradouro.ufe_sg, log_localidade.loc_nu, log_localidade.loc_no,  log_bairro.bai_no,  log_logradouro.log_no as log_no, log_logradouro.cep,  log_logradouro.log_complemento,'' as nome
from log_logradouro, log_localidade, log_bairro
where log_logradouro.loc_nu= log_localidade.loc_nu  and log_logradouro.bai_nu_ini=log_bairro.bai_nu  and log_logradouro.log_sta_tlo ='n' and log_logradouro.cep != ''
union 
select log_localidade.ufe_sg, log_localidade.loc_nu, log_localidade.loc_no,'' as bai_no,'' as log_no, log_localidade.cep, '' as log_complemento,'' as nome
from log_localidade
where log_localidade.cep != ''
union 
select log_cpc.ufe_sg, log_localidade.loc_nu, log_localidade.loc_no,'' as  bai_no, log_cpc.cpc_endereco as log_no, log_cpc.cep,'' as log_complemento,cpc_no as nome
from log_cpc,  log_localidade
where  log_cpc.loc_nu=log_localidade.loc_nu  and log_cpc.cep != ''
union
select  log_grande_usuario.ufe_sg, log_localidade.loc_nu, log_localidade.loc_no, log_bairro.bai_no as  bai_no,  log_grande_usuario.gru_endereco as log_no,  log_grande_usuario.cep,'' as log_complemento,gru_no as nome
from log_grande_usuario,  log_localidade, log_bairro
where  log_grande_usuario.loc_nu=log_localidade.loc_nu and  log_grande_usuario.bai_nu = log_bairro.bai_nu and log_grande_usuario.cep != ''
union
select  log_unid_oper.ufe_sg, log_localidade.loc_nu, log_localidade.loc_no, log_bairro.bai_no as  bai_no,  log_unid_oper.uop_endereco as log_no, log_unid_oper.cep,'' as log_complemento, uop_no as nome
from log_unid_oper,  log_localidade, log_bairro
where  log_unid_oper.loc_nu=log_localidade.loc_nu and  log_unid_oper.bai_nu = log_bairro.bai_nu and log_unid_oper.cep != '';

delete from [EnderecoCorreios] where Nome = 'UD Barra do Garças'

select Cep, count(Cep) from [EnderecoCorreios] group by Cep having count(Cep) > 1

ALTER TABLE [EnderecoCorreios] ADD CONSTRAINT PK_CEP PRIMARY KEY CLUSTERED (Cep);
