select * from TB_Usuario
select * from TB_Integrante

alter table tb_integrante add TX_EMAIL varchar(255)
alter table tb_integrante add NR_CPF varchar(20)
alter table tb_integrante add ID_USUARIO varchar(20)
alter table tb_integrante add ID_TIPO int default 1
update tb_integrante set NM_INTEGRANTE = u.NM_USUARIO from tb_usuario u inner join tb_integrante i on u.ID_USUARIO = i.ID_USUARIO
update tb_integrante set TX_EMAIL = u.TX_EMAIL from tb_usuario u inner join tb_integrante i on u.ID_USUARIO = i.ID_USUARIO
update tb_integrante set DT_NASCIMENTO = u.DT_NASCIMENTO from tb_usuario u inner join tb_integrante i on u.ID_USUARIO = i.ID_USUARIO
update tb_integrante set TX_SEXO = u.TX_SEXO from tb_usuario u inner join tb_integrante i on u.ID_USUARIO = i.ID_USUARIO
update tb_integrante set NR_CPF = u.NR_CPF from tb_usuario u inner join tb_integrante i on u.ID_USUARIO = i.ID_USUARIO
alter table tb_usuario add FL_ATIVO bit not null default 1

-- Propriedades obsoletas
--alter table tb_usuario drop column NM_USUARIO
--alter table tb_usuario drop column TX_EMAIL
--alter table tb_usuario drop column DT_NASCIMENTO
--alter table tb_usuario drop column TX_SEXO
--alter table tb_usuario drop column NR_CPF
--alter table tb_integrante drop column TX_EMAIL_CONVITE