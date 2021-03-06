alter table TB_Grupo_Integrante add ID_INTEGRANTE bigint
alter table TB_Grupo_Integrante add ID_PONTO_REAL_DEMANDA bigint
alter table TB_Grupo_Integrante add ID_TIPO_INTEGRANTE int not null default 2


update TB_Grupo_Integrante set ID_INTEGRANTE = i.ID_INTEGRANTE, ID_PONTO_REAL_DEMANDA = p.ID_PONTO_REAL_DEMANDA 
	from TB_Grupo_Integrante g inner join TB_Integrante i on g.ID_GRUPO_INTEGRANTE = i.ID_GRUPO_INTEGRANTE
		inner join TB_Ponto_Real_Demanda p on p.ID_GRUPO_INTEGRANTE = g.ID_GRUPO_INTEGRANTE


select g.*, i.ID_INTEGRANTE, p.ID_PONTO_REAL_DEMANDA from TB_Grupo_Integrante g inner join TB_Integrante i on g.ID_GRUPO_INTEGRANTE = i.ID_GRUPO_INTEGRANTE
		inner join TB_Ponto_Real_Demanda p on p.ID_GRUPO_INTEGRANTE = g.ID_GRUPO_INTEGRANTE
	
