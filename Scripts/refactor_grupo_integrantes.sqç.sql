alter table TB_Grupo_Integrante add ID_INTEGRANTE bigint
alter table TB_Grupo_Integrante add ID_PONTO_REAL_DEMANDA bigint
alter table TB_Grupo_Integrante add ID_PAPEL_INTEGRANTE int not null default 2

insert into TB_Grupo_Integrante select 'grupo', null, i.ID_INTEGRANTE, p.ID_PONTO_REAL_DEMANDA, 2 from TB_Grupo_Integrante g inner join TB_Integrante i on g.ID_GRUPO_INTEGRANTE = i.ID_GRUPO_INTEGRANTE
		inner join TB_Ponto_Real_Demanda p on p.ID_GRUPO_INTEGRANTE = g.ID_GRUPO_INTEGRANTE 

alter table TB_Integrante alter column ID_GRUPO_INTEGRANTE bigint null
alter table TB_Ponto_Real_Demanda alter column ID_GRUPO_INTEGRANTE bigint null

update TB_Integrante set ID_GRUPO_INTEGRANTE = null
update TB_Ponto_Real_Demanda set ID_GRUPO_INTEGRANTE = null

delete from TB_Grupo_Integrante where ID_INTEGRANTE is null and ID_PONTO_REAL_DEMANDA is null

select * from TB_Grupo_Integrante