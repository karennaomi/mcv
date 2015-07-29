namespace LM.Core.RepositorioEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_Animal",
                c => new
                    {
                        ID_ANIMAL = c.Int(nullable: false, identity: true),
                        NM_ANIMAL = c.String(),
                        FL_ATIVO = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID_ANIMAL);
            
            CreateTable(
                "dbo.TB_CATEGORIA",
                c => new
                    {
                        ID_CATEGORIA = c.Int(nullable: false, identity: true),
                        NM_CATEGORIA = c.String(),
                        NM_HEXA_COR = c.String(),
                        FL_CATEGORIA_ATIVA = c.Boolean(nullable: false),
                        ID_CATEGORIA_PAI = c.Int(),
                    })
                .PrimaryKey(t => t.ID_CATEGORIA)
                .ForeignKey("dbo.TB_CATEGORIA", t => t.ID_CATEGORIA_PAI)
                .Index(t => t.ID_CATEGORIA_PAI);
            
            CreateTable(
                "dbo.TB_IMAGEM",
                c => new
                    {
                        ID_IMAGEM = c.Int(nullable: false, identity: true),
                        TX_URL_IMAGEM = c.String(),
                        ID_IMAGEM_INTERFACE = c.Int(nullable: false),
                        ID_IMAGEM_RESOLUCAO = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID_IMAGEM);
            
            CreateTable(
                "dbo.TB_CIDADE",
                c => new
                    {
                        ID_CIDADE = c.Int(nullable: false, identity: true),
                        NM_CIDADE = c.String(),
                        ID_UF = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.ID_CIDADE)
                .ForeignKey("dbo.TB_UF", t => t.ID_UF, cascadeDelete: true)
                .Index(t => t.ID_UF);
            
            CreateTable(
                "dbo.TB_UF",
                c => new
                    {
                        ID_UF = c.Short(nullable: false, identity: true),
                        SG_UF = c.String(),
                        NM_UF = c.String(),
                    })
                .PrimaryKey(t => t.ID_UF);
            
            CreateTable(
                "dbo.TB_Compra",
                c => new
                    {
                        ID_COMPRA = c.Long(nullable: false, identity: true),
                        ID_LOJA = c.Int(nullable: false),
                        DT_INICIO_COMPRA = c.DateTime(),
                        DT_FIM_COMPRA = c.DateTime(),
                        DT_CAPTURA_PRIMEIRO_ITEM_COMPRA = c.DateTime(),
                        DT_INC = c.DateTime(),
                        DT_ALT = c.DateTime(),
                        ID_INTEGRANTE = c.Long(nullable: false),
                        ID_PONTO_REAL_DEMANDA = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID_COMPRA)
                .ForeignKey("dbo.TB_INTEGRANTE", t => t.ID_INTEGRANTE, cascadeDelete: true)
                .ForeignKey("dbo.TB_PONTO_REAL_DEMANDA", t => t.ID_PONTO_REAL_DEMANDA, cascadeDelete: true)
                .Index(t => t.ID_INTEGRANTE)
                .Index(t => t.ID_PONTO_REAL_DEMANDA);
            
            CreateTable(
                "dbo.TB_INTEGRANTE",
                c => new
                    {
                        ID_INTEGRANTE = c.Long(nullable: false, identity: true),
                        NM_INTEGRANTE = c.String(nullable: false, maxLength: 70),
                        TX_EMAIL = c.String(),
                        NR_CPF = c.String(),
                        DT_NASCIMENTO = c.DateTime(),
                        FL_USUARIO_CONVIDADO = c.Boolean(nullable: false),
                        DT_CONVITE = c.DateTime(),
                        DT_INC = c.DateTime(),
                        DT_ALT = c.DateTime(),
                        FL_ATIVO = c.Boolean(nullable: false),
                        TX_NUMERO_TELEFONE = c.String(),
                        TX_SEXO = c.String(),
                        ID_TIPO = c.Int(nullable: false),
                        ID_ANIMAL = c.Int(),
                        ID_USUARIO = c.Long(),
                    })
                .PrimaryKey(t => t.ID_INTEGRANTE)
                .ForeignKey("dbo.TB_USUARIO", t => t.ID_USUARIO)
                .Index(t => t.ID_USUARIO);
            
            CreateTable(
                "dbo.TB_GRUPO_INTEGRANTE",
                c => new
                    {
                        ID_GRUPO_INTEGRANTE = c.Long(nullable: false, identity: true),
                        NM_GRUPO_INTEGRANTE = c.String(),
                        ID_PAPEL_INTEGRANTE = c.Int(nullable: false),
                        ID_INTEGRANTE = c.Long(nullable: false),
                        ID_PONTO_REAL_DEMANDA = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID_GRUPO_INTEGRANTE)
                .ForeignKey("dbo.TB_INTEGRANTE", t => t.ID_INTEGRANTE, cascadeDelete: true)
                .ForeignKey("dbo.TB_PONTO_REAL_DEMANDA", t => t.ID_PONTO_REAL_DEMANDA, cascadeDelete: true)
                .Index(t => t.ID_INTEGRANTE)
                .Index(t => t.ID_PONTO_REAL_DEMANDA);
            
            CreateTable(
                "dbo.TB_PONTO_REAL_DEMANDA",
                c => new
                    {
                        ID_PONTO_REAL_DEMANDA = c.Long(nullable: false, identity: true),
                        NM_PONTO_REAL_DEMANDA = c.String(),
                        QT_DIAS_ALERTA_REPOSICAO = c.Short(),
                        QT_DIAS_COBERTURA_ESTOQUE = c.Short(),
                        DT_INC = c.DateTime(),
                        DT_ALT = c.DateTime(),
                        ID_TIPO_PONTO_REAL_DEMANDA = c.Int(),
                        FL_ATIVO = c.Boolean(nullable: false),
                        ID_ENDERECO = c.Long(nullable: false),
                        ID_USUARIO_CRIADOR = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID_PONTO_REAL_DEMANDA)
                .ForeignKey("dbo.TB_ENDERECO", t => t.ID_ENDERECO, cascadeDelete: true)
                .ForeignKey("dbo.TB_USUARIO", t => t.ID_USUARIO_CRIADOR, cascadeDelete: true)
                .Index(t => t.ID_ENDERECO)
                .Index(t => t.ID_USUARIO_CRIADOR);
            
            CreateTable(
                "dbo.TB_ENDERECO",
                c => new
                    {
                        ID_ENDERECO = c.Long(nullable: false, identity: true),
                        NM_ENDERECO = c.String(),
                        NR_ENDERECO = c.Int(),
                        NM_ENDERECO_COMPLEMENTO = c.String(),
                        TX_ENDERECO_ALIAS = c.String(),
                        NR_ENDERECO_CEP = c.String(),
                        NM_ENDERECO_BAIRRO = c.String(),
                        NR_ENDERECO_LATITUDE = c.Decimal(nullable: false, precision: 20, scale: 15),
                        NR_ENDERECO_LONGITUDE = c.Decimal(nullable: false, precision: 20, scale: 15),
                        DT_INC = c.DateTime(),
                        DT_ALT = c.DateTime(),
                        ID_CIDADE = c.Int(),
                    })
                .PrimaryKey(t => t.ID_ENDERECO)
                .ForeignKey("dbo.TB_CIDADE", t => t.ID_CIDADE)
                .Index(t => t.ID_CIDADE);
            
            CreateTable(
                "dbo.TB_LISTA_PRODUTO",
                c => new
                    {
                        ID_LISTA_PRODUTO = c.Long(nullable: false, identity: true),
                        NM_LISTA = c.String(),
                        TX_STATUS_LISTA = c.String(),
                        DT_INC = c.DateTime(),
                        DT_ALT = c.DateTime(),
                        ID_PONTO_REAL_DEMANDA = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID_LISTA_PRODUTO)
                .ForeignKey("dbo.TB_PONTO_REAL_DEMANDA", t => t.ID_PONTO_REAL_DEMANDA, cascadeDelete: true)
                .Index(t => t.ID_PONTO_REAL_DEMANDA);
            
            CreateTable(
                "dbo.TB_LISTA_PRODUTO_ITEM",
                c => new
                    {
                        ID_LISTA_PRODUTO_ITEM = c.Long(nullable: false, identity: true),
                        DT_INC = c.DateTime(),
                        DT_ALT = c.DateTime(),
                        QT_CONSUMO = c.Decimal(precision: 18, scale: 2),
                        QT_ESTOQUE = c.Decimal(precision: 18, scale: 2),
                        QT_ESTIMADA_ESTOQUE = c.Decimal(precision: 18, scale: 2),
                        QT_SUGESTAO_COMPRA = c.Decimal(precision: 18, scale: 2),
                        TX_STATUS_ITEM = c.String(),
                        VL_CONSUMO_MEDIO_INTEGRANTE = c.Decimal(precision: 18, scale: 2),
                        FL_ALERTA_SUGESTAO_COMPRA = c.Boolean(nullable: false),
                        FL_NBO = c.Boolean(nullable: false),
                        ID_PERIODO_CONSUMO = c.Int(nullable: false),
                        ID_PRODUTO = c.Int(nullable: false),
                        ID_LISTA_PRODUTO = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID_LISTA_PRODUTO_ITEM)
                .ForeignKey("dbo.TB_PERIODO_CONSUMO", t => t.ID_PERIODO_CONSUMO, cascadeDelete: true)
                .ForeignKey("dbo.TB_PRODUTO", t => t.ID_PRODUTO, cascadeDelete: true)
                .ForeignKey("dbo.TB_LISTA_PRODUTO", t => t.ID_LISTA_PRODUTO, cascadeDelete: true)
                .Index(t => t.ID_PERIODO_CONSUMO)
                .Index(t => t.ID_PRODUTO)
                .Index(t => t.ID_LISTA_PRODUTO);
            
            CreateTable(
                "dbo.TB_PERIODO_CONSUMO",
                c => new
                    {
                        ID_PERIODO_CONSUMO = c.Int(nullable: false, identity: true),
                        NM_PERIODO_CONSUMO = c.String(),
                        VL_FATOR_CONVERSAO_DIA = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.ID_PERIODO_CONSUMO);
            
            CreateTable(
                "dbo.TB_PRODUTO",
                c => new
                    {
                        ID_PRODUTO = c.Int(nullable: false, identity: true),
                        CD_PRODUTO_EAN = c.String(nullable: false, maxLength: 13),
                        FL_PRODUTO_ATIVO = c.Boolean(nullable: false),
                        DT_INC = c.DateTime(),
                        DT_ALT = c.DateTime(),
                        TX_ORIGEM = c.String(),
                    })
                .PrimaryKey(t => t.ID_PRODUTO);
            
            CreateTable(
                "dbo.TB_PRODUTO_MASTER_DATA",
                c => new
                    {
                        ID_PRODUTO = c.Int(nullable: false),
                        NM_PRODUTO_COMPLETO = c.String(),
                        TX_MARCA_PRODUTO = c.String(),
                    })
                .PrimaryKey(t => t.ID_PRODUTO)
                .ForeignKey("dbo.TB_PRODUTO", t => t.ID_PRODUTO)
                .Index(t => t.ID_PRODUTO);
            
            CreateTable(
                "dbo.TB_Produto_Preco",
                c => new
                    {
                        ID_PRODUTO_PRECO = c.Int(nullable: false, identity: true),
                        VL_PRECO_MIN_PRODUTO = c.Decimal(precision: 18, scale: 2),
                        VL_PRECO_MAX_PRODUTO = c.Decimal(precision: 18, scale: 2),
                        DT_PRECO_PRODUTO = c.DateTime(),
                        FL_ATIVO = c.Boolean(),
                        DT_INC = c.DateTime(),
                        DT_ALT = c.DateTime(),
                        ID_PRODUTO = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID_PRODUTO_PRECO)
                .ForeignKey("dbo.TB_PRODUTO", t => t.ID_PRODUTO, cascadeDelete: true)
                .Index(t => t.ID_PRODUTO);
            
            CreateTable(
                "dbo.TB_Loja",
                c => new
                    {
                        ID_LOJA = c.Int(nullable: false, identity: true),
                        NM_LOJA = c.String(),
                        ID_LOJA_LOCALIZADOR = c.String(),
                        TX_ORIGEM = c.String(),
                        DT_INC = c.DateTime(),
                        DT_ALT = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID_LOJA);
            
            CreateTable(
                "dbo.TB_Loja_Master_Data",
                c => new
                    {
                        ID_LOJA = c.Int(nullable: false),
                        NM_RAZAO_SOCIAL = c.String(),
                        DT_INC = c.DateTime(),
                        DT_ALT = c.DateTime(),
                        ID_ENDERECO = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID_LOJA)
                .ForeignKey("dbo.TB_ENDERECO", t => t.ID_ENDERECO, cascadeDelete: true)
                .ForeignKey("dbo.TB_Loja", t => t.ID_LOJA)
                .Index(t => t.ID_LOJA)
                .Index(t => t.ID_ENDERECO);
            
            CreateTable(
                "dbo.TB_USUARIO",
                c => new
                    {
                        ID_USUARIO = c.Long(nullable: false, identity: true),
                        TX_LOGIN = c.String(nullable: false),
                        TX_SENHA = c.String(nullable: false),
                        FL_ATIVO = c.Boolean(nullable: false),
                        TX_DeviceId = c.String(),
                        TX_DeviceType = c.String(),
                        DT_INC = c.DateTime(),
                        DT_ALT = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID_USUARIO);
            
            CreateTable(
                "dbo.TB_Contrato",
                c => new
                    {
                        ID_CONTRATO = c.Int(nullable: false, identity: true),
                        NM_CONTRATO = c.String(),
                        NR_CONTRATO = c.Int(nullable: false),
                        DT_INI_VIGENCIA = c.DateTime(nullable: false),
                        DT_FIM_VIGENCIA = c.DateTime(nullable: false),
                        TX_CONTEUDO = c.String(),
                        FL_ATIVO = c.Boolean(nullable: false),
                        DT_INC = c.DateTime(nullable: false),
                        DT_ALT = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID_CONTRATO);
            
            CreateTable(
                "dbo.TB_STATUS_USUARIO_PONTO_DEMANDA",
                c => new
                    {
                        ID_STATUS_USUARIO_PONTO_DEMANDA = c.Long(nullable: false, identity: true),
                        ID_PONTO_REAL_DEMANDA = c.Long(),
                        DT_INC = c.DateTime(),
                        DT_ALT = c.DateTime(),
                        ID_STATUS_CADASTRO = c.Int(nullable: false),
                        ID_USUARIO = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID_STATUS_USUARIO_PONTO_DEMANDA)
                .ForeignKey("dbo.TB_USUARIO", t => t.ID_USUARIO, cascadeDelete: true)
                .Index(t => t.ID_USUARIO);
            
            CreateTable(
                "dbo.TB_Compra_Item",
                c => new
                    {
                        ID_COMPRA_ITEM = c.Long(nullable: false, identity: true),
                        ID_PRODUTO = c.Int(),
                        ID_STATUS_COMPRA = c.Short(nullable: false),
                        DT_COMPRA = c.DateTime(),
                        QT_COMPRA = c.Decimal(precision: 18, scale: 2),
                        VL_PRECO_ITEM = c.Decimal(precision: 18, scale: 2),
                        DT_INC = c.DateTime(),
                        DT_ALT = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        ID_COMPRA = c.Long(nullable: false),
                        ID_LISTA_PRODUTO_ITEM = c.Long(),
                        ID_PEDIDO = c.Long(),
                    })
                .PrimaryKey(t => t.ID_COMPRA_ITEM)
                .ForeignKey("dbo.TB_Compra", t => t.ID_COMPRA, cascadeDelete: true)
                .ForeignKey("dbo.TB_LISTA_PRODUTO_ITEM", t => t.ID_LISTA_PRODUTO_ITEM)
                .ForeignKey("dbo.TB_PEDIDO", t => t.ID_PEDIDO)
                .Index(t => t.ID_COMPRA)
                .Index(t => t.ID_LISTA_PRODUTO_ITEM)
                .Index(t => t.ID_PEDIDO);
            
            CreateTable(
                "dbo.TB_Compra_Item_Substituto",
                c => new
                    {
                        ID_COMPRA_ITEM_SUBSTITUTO = c.Long(nullable: false),
                        TX_MOTIVO = c.String(),
                        DT_INC = c.DateTime(),
                        ID_COMPRA_ITEM_PRINCIPAL = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID_COMPRA_ITEM_SUBSTITUTO)
                .ForeignKey("dbo.TB_Compra_Item", t => t.ID_COMPRA_ITEM_PRINCIPAL)
                .ForeignKey("dbo.TB_Compra_Item", t => t.ID_COMPRA_ITEM_SUBSTITUTO)
                .Index(t => t.ID_COMPRA_ITEM_SUBSTITUTO)
                .Index(t => t.ID_COMPRA_ITEM_PRINCIPAL);
            
            CreateTable(
                "dbo.TB_PEDIDO",
                c => new
                    {
                        ID_PEDIDO = c.Long(nullable: false, identity: true),
                        DT_INC = c.DateTime(storeType: "smalldatetime"),
                        DT_ALT = c.DateTime(storeType: "smalldatetime"),
                        QT_SOLICITADA = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ID_STATUS_PEDIDO = c.Int(nullable: false),
                        DT_PEDIDO = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        ID_INTEGRANTE = c.Long(nullable: false),
                        ID_PONTO_REAL_DEMANDA = c.Long(nullable: false),
                        ID_PRODUTO = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID_PEDIDO)
                .ForeignKey("dbo.TB_INTEGRANTE", t => t.ID_INTEGRANTE, cascadeDelete: true)
                .ForeignKey("dbo.TB_PONTO_REAL_DEMANDA", t => t.ID_PONTO_REAL_DEMANDA, cascadeDelete: true)
                .ForeignKey("dbo.TB_PRODUTO", t => t.ID_PRODUTO, cascadeDelete: true)
                .Index(t => t.ID_INTEGRANTE)
                .Index(t => t.ID_PONTO_REAL_DEMANDA)
                .Index(t => t.ID_PRODUTO);
            
            CreateTable(
                "dbo.TB_CompraAtiva",
                c => new
                    {
                        ID_CompraAtiva = c.Int(nullable: false, identity: true),
                        DT_Inicio = c.DateTime(nullable: false),
                        DT_Fim = c.DateTime(),
                        ID_Ponto_Real_Demanda = c.Long(nullable: false),
                        ID_Usuario = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID_CompraAtiva)
                .ForeignKey("dbo.TB_PONTO_REAL_DEMANDA", t => t.ID_Ponto_Real_Demanda, cascadeDelete: true)
                .ForeignKey("dbo.TB_USUARIO", t => t.ID_Usuario, cascadeDelete: false)
                .Index(t => t.ID_Ponto_Real_Demanda)
                .Index(t => t.ID_Usuario);
            
            CreateTable(
                "dbo.TB_Contato",
                c => new
                    {
                        ID_CONTATO = c.Int(nullable: false, identity: true),
                        TX_NOME = c.String(),
                        TX_EMAIL = c.String(),
                        TX_MENSAGEM = c.String(),
                        DT_INC = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID_CONTATO);
            
            CreateTable(
                "dbo.TB_Email_Capturado",
                c => new
                    {
                        ID_CAPTURED_EMAIL = c.Int(nullable: false, identity: true),
                        TX_EMAIL = c.String(),
                        DT_INC = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID_CAPTURED_EMAIL);
            
            CreateTable(
                "dbo.TB_Fila",
                c => new
                    {
                        ID_FILA = c.Long(nullable: false, identity: true),
                        ID_TIPO_SERVICO = c.Int(nullable: false),
                        TX_FILA_ORIGEM = c.String(),
                        FL_ORDEM_EXECUCAO = c.Int(),
                        FL_STATUS_FILA = c.String(),
                        FL_STATUS_PROCESSAMENTO = c.String(),
                        DT_INC = c.DateTime(),
                        DT_ALT = c.DateTime(),
                        ID_TIPO_OPERACAO = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID_FILA);
            
            CreateTable(
                "dbo.TB_Fila_Mensagem",
                c => new
                    {
                        ID_FILA_MENSAGEM = c.Long(nullable: false, identity: true),
                        DT_INC = c.DateTime(),
                        TX_EMAIL_DESTINO = c.String(),
                        TX_ASSUNTO = c.String(),
                        TX_CORPO = c.String(),
                        TX_NUMERO_DESTINO = c.String(),
                        TX_MENSAGEM_SMS = c.String(),
                        ID_FILA = c.Long(nullable: false),
                        ID_TIPO_MENSAGEM = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID_FILA_MENSAGEM)
                .ForeignKey("dbo.TB_Fila", t => t.ID_FILA, cascadeDelete: true)
                .Index(t => t.ID_FILA);
            
            CreateTable(
                "dbo.TB_Fila_Produto",
                c => new
                    {
                        ID_FILA_PRODUTO = c.Long(nullable: false, identity: true),
                        ID_PRODUTO = c.Int(nullable: false),
                        CD_PRODUTO_EAN = c.String(),
                        TX_DESCRICAO_PRODUTO = c.String(),
                        TX_PRODUTO_IMAGEM_URL = c.String(),
                        DT_INC = c.DateTime(),
                        ID_FILA = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID_FILA_PRODUTO)
                .ForeignKey("dbo.TB_Fila", t => t.ID_FILA, cascadeDelete: true)
                .Index(t => t.ID_FILA);
            
            CreateTable(
                "dbo.TB_Token_Smv",
                c => new
                    {
                        TOKEN_SMV = c.Guid(nullable: false),
                        DT_INICIO = c.DateTime(nullable: false),
                        DT_VALIDADE = c.DateTime(nullable: false),
                        ID_USUARIO = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TOKEN_SMV);
            
            CreateTable(
                "dbo.TB_USUARIO_RECUPERAR_SENHA",
                c => new
                    {
                        ID_RECUPERAR_SENHA = c.Int(nullable: false, identity: true),
                        TX_TOKEN = c.Guid(nullable: false),
                        DT_INC = c.DateTime(nullable: false),
                        ID_USUARIO = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID_RECUPERAR_SENHA)
                .ForeignKey("dbo.TB_USUARIO", t => t.ID_USUARIO, cascadeDelete: true)
                .Index(t => t.ID_USUARIO);
            
            CreateTable(
                "dbo.TB_Mensagem",
                c => new
                    {
                        ID_MENSAGEM = c.Int(nullable: false, identity: true),
                        TX_MENSAGEM = c.String(),
                        ID_TIPO_TEMPLATE = c.Int(nullable: false),
                        TX_ASSUNTO = c.String(),
                        TX_URL_HTML = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID_MENSAGEM);
            
            CreateTable(
                "dbo.TB_Categoria_Imagem",
                c => new
                    {
                        ID_CATEGORIA = c.Int(nullable: false),
                        ID_IMAGEM = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID_CATEGORIA, t.ID_IMAGEM })
                .ForeignKey("dbo.TB_CATEGORIA", t => t.ID_CATEGORIA, cascadeDelete: true)
                .ForeignKey("dbo.TB_IMAGEM", t => t.ID_IMAGEM, cascadeDelete: true)
                .Index(t => t.ID_CATEGORIA)
                .Index(t => t.ID_IMAGEM);
            
            CreateTable(
                "dbo.TB_PRODUTO_CATEGORIA",
                c => new
                    {
                        ID_PRODUTO = c.Int(nullable: false),
                        ID_CATEGORIA = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID_PRODUTO, t.ID_CATEGORIA })
                .ForeignKey("dbo.TB_PRODUTO", t => t.ID_PRODUTO, cascadeDelete: true)
                .ForeignKey("dbo.TB_CATEGORIA", t => t.ID_CATEGORIA, cascadeDelete: true)
                .Index(t => t.ID_PRODUTO)
                .Index(t => t.ID_CATEGORIA);
            
            CreateTable(
                "dbo.TB_PRODUTO_IMAGEM",
                c => new
                    {
                        ID_PRODUTO = c.Int(nullable: false),
                        ID_IMAGEM = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID_PRODUTO, t.ID_IMAGEM })
                .ForeignKey("dbo.TB_PRODUTO", t => t.ID_PRODUTO, cascadeDelete: true)
                .ForeignKey("dbo.TB_IMAGEM", t => t.ID_IMAGEM, cascadeDelete: true)
                .Index(t => t.ID_PRODUTO)
                .Index(t => t.ID_IMAGEM);
            
            CreateTable(
                "dbo.TB_Produto_Ponto_Real_Demanda",
                c => new
                    {
                        ID_PRODUTO = c.Int(nullable: false),
                        ID_PONTO_REAL_DEMANDA = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID_PRODUTO, t.ID_PONTO_REAL_DEMANDA })
                .ForeignKey("dbo.TB_PRODUTO", t => t.ID_PRODUTO, cascadeDelete: true)
                .ForeignKey("dbo.TB_PONTO_REAL_DEMANDA", t => t.ID_PONTO_REAL_DEMANDA, cascadeDelete: true)
                .Index(t => t.ID_PRODUTO)
                .Index(t => t.ID_PONTO_REAL_DEMANDA);
            
            CreateTable(
                "dbo.TB_Ponto_Real_Demanda_Loja",
                c => new
                    {
                        ID_PONTO_REAL_DEMANDA = c.Long(nullable: false),
                        ID_LOJA = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID_PONTO_REAL_DEMANDA, t.ID_LOJA })
                .ForeignKey("dbo.TB_PONTO_REAL_DEMANDA", t => t.ID_PONTO_REAL_DEMANDA, cascadeDelete: true)
                .ForeignKey("dbo.TB_Loja", t => t.ID_LOJA, cascadeDelete: true)
                .Index(t => t.ID_PONTO_REAL_DEMANDA)
                .Index(t => t.ID_LOJA);
            
            CreateTable(
                "dbo.TB_Contrato_Usuario",
                c => new
                    {
                        ID_USUARIO = c.Long(nullable: false),
                        ID_CONTRATO = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID_USUARIO, t.ID_CONTRATO })
                .ForeignKey("dbo.TB_USUARIO", t => t.ID_USUARIO, cascadeDelete: true)
                .ForeignKey("dbo.TB_Contrato", t => t.ID_CONTRATO, cascadeDelete: true)
                .Index(t => t.ID_USUARIO)
                .Index(t => t.ID_CONTRATO);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TB_USUARIO_RECUPERAR_SENHA", "ID_USUARIO", "dbo.TB_USUARIO");
            DropForeignKey("dbo.TB_Fila_Produto", "ID_FILA", "dbo.TB_Fila");
            DropForeignKey("dbo.TB_Fila_Mensagem", "ID_FILA", "dbo.TB_Fila");
            DropForeignKey("dbo.TB_CompraAtiva", "ID_Usuario", "dbo.TB_USUARIO");
            DropForeignKey("dbo.TB_CompraAtiva", "ID_Ponto_Real_Demanda", "dbo.TB_PONTO_REAL_DEMANDA");
            DropForeignKey("dbo.TB_Compra", "ID_PONTO_REAL_DEMANDA", "dbo.TB_PONTO_REAL_DEMANDA");
            DropForeignKey("dbo.TB_Compra_Item", "ID_PEDIDO", "dbo.TB_PEDIDO");
            DropForeignKey("dbo.TB_PEDIDO", "ID_PRODUTO", "dbo.TB_PRODUTO");
            DropForeignKey("dbo.TB_PEDIDO", "ID_PONTO_REAL_DEMANDA", "dbo.TB_PONTO_REAL_DEMANDA");
            DropForeignKey("dbo.TB_PEDIDO", "ID_INTEGRANTE", "dbo.TB_INTEGRANTE");
            DropForeignKey("dbo.TB_Compra_Item", "ID_LISTA_PRODUTO_ITEM", "dbo.TB_LISTA_PRODUTO_ITEM");
            DropForeignKey("dbo.TB_Compra_Item_Substituto", "ID_COMPRA_ITEM_SUBSTITUTO", "dbo.TB_Compra_Item");
            DropForeignKey("dbo.TB_Compra_Item_Substituto", "ID_COMPRA_ITEM_PRINCIPAL", "dbo.TB_Compra_Item");
            DropForeignKey("dbo.TB_Compra_Item", "ID_COMPRA", "dbo.TB_Compra");
            DropForeignKey("dbo.TB_Compra", "ID_INTEGRANTE", "dbo.TB_INTEGRANTE");
            DropForeignKey("dbo.TB_GRUPO_INTEGRANTE", "ID_PONTO_REAL_DEMANDA", "dbo.TB_PONTO_REAL_DEMANDA");
            DropForeignKey("dbo.TB_PONTO_REAL_DEMANDA", "ID_USUARIO_CRIADOR", "dbo.TB_USUARIO");
            DropForeignKey("dbo.TB_STATUS_USUARIO_PONTO_DEMANDA", "ID_USUARIO", "dbo.TB_USUARIO");
            DropForeignKey("dbo.TB_INTEGRANTE", "ID_USUARIO", "dbo.TB_USUARIO");
            DropForeignKey("dbo.TB_Contrato_Usuario", "ID_CONTRATO", "dbo.TB_Contrato");
            DropForeignKey("dbo.TB_Contrato_Usuario", "ID_USUARIO", "dbo.TB_USUARIO");
            DropForeignKey("dbo.TB_Ponto_Real_Demanda_Loja", "ID_LOJA", "dbo.TB_Loja");
            DropForeignKey("dbo.TB_Ponto_Real_Demanda_Loja", "ID_PONTO_REAL_DEMANDA", "dbo.TB_PONTO_REAL_DEMANDA");
            DropForeignKey("dbo.TB_Loja_Master_Data", "ID_LOJA", "dbo.TB_Loja");
            DropForeignKey("dbo.TB_Loja_Master_Data", "ID_ENDERECO", "dbo.TB_ENDERECO");
            DropForeignKey("dbo.TB_LISTA_PRODUTO", "ID_PONTO_REAL_DEMANDA", "dbo.TB_PONTO_REAL_DEMANDA");
            DropForeignKey("dbo.TB_LISTA_PRODUTO_ITEM", "ID_LISTA_PRODUTO", "dbo.TB_LISTA_PRODUTO");
            DropForeignKey("dbo.TB_LISTA_PRODUTO_ITEM", "ID_PRODUTO", "dbo.TB_PRODUTO");
            DropForeignKey("dbo.TB_Produto_Preco", "ID_PRODUTO", "dbo.TB_PRODUTO");
            DropForeignKey("dbo.TB_Produto_Ponto_Real_Demanda", "ID_PONTO_REAL_DEMANDA", "dbo.TB_PONTO_REAL_DEMANDA");
            DropForeignKey("dbo.TB_Produto_Ponto_Real_Demanda", "ID_PRODUTO", "dbo.TB_PRODUTO");
            DropForeignKey("dbo.TB_PRODUTO_MASTER_DATA", "ID_PRODUTO", "dbo.TB_PRODUTO");
            DropForeignKey("dbo.TB_PRODUTO_IMAGEM", "ID_IMAGEM", "dbo.TB_IMAGEM");
            DropForeignKey("dbo.TB_PRODUTO_IMAGEM", "ID_PRODUTO", "dbo.TB_PRODUTO");
            DropForeignKey("dbo.TB_PRODUTO_CATEGORIA", "ID_CATEGORIA", "dbo.TB_CATEGORIA");
            DropForeignKey("dbo.TB_PRODUTO_CATEGORIA", "ID_PRODUTO", "dbo.TB_PRODUTO");
            DropForeignKey("dbo.TB_LISTA_PRODUTO_ITEM", "ID_PERIODO_CONSUMO", "dbo.TB_PERIODO_CONSUMO");
            DropForeignKey("dbo.TB_PONTO_REAL_DEMANDA", "ID_ENDERECO", "dbo.TB_ENDERECO");
            DropForeignKey("dbo.TB_ENDERECO", "ID_CIDADE", "dbo.TB_CIDADE");
            DropForeignKey("dbo.TB_GRUPO_INTEGRANTE", "ID_INTEGRANTE", "dbo.TB_INTEGRANTE");
            DropForeignKey("dbo.TB_CIDADE", "ID_UF", "dbo.TB_UF");
            DropForeignKey("dbo.TB_CATEGORIA", "ID_CATEGORIA_PAI", "dbo.TB_CATEGORIA");
            DropForeignKey("dbo.TB_Categoria_Imagem", "ID_IMAGEM", "dbo.TB_IMAGEM");
            DropForeignKey("dbo.TB_Categoria_Imagem", "ID_CATEGORIA", "dbo.TB_CATEGORIA");
            DropIndex("dbo.TB_Contrato_Usuario", new[] { "ID_CONTRATO" });
            DropIndex("dbo.TB_Contrato_Usuario", new[] { "ID_USUARIO" });
            DropIndex("dbo.TB_Ponto_Real_Demanda_Loja", new[] { "ID_LOJA" });
            DropIndex("dbo.TB_Ponto_Real_Demanda_Loja", new[] { "ID_PONTO_REAL_DEMANDA" });
            DropIndex("dbo.TB_Produto_Ponto_Real_Demanda", new[] { "ID_PONTO_REAL_DEMANDA" });
            DropIndex("dbo.TB_Produto_Ponto_Real_Demanda", new[] { "ID_PRODUTO" });
            DropIndex("dbo.TB_PRODUTO_IMAGEM", new[] { "ID_IMAGEM" });
            DropIndex("dbo.TB_PRODUTO_IMAGEM", new[] { "ID_PRODUTO" });
            DropIndex("dbo.TB_PRODUTO_CATEGORIA", new[] { "ID_CATEGORIA" });
            DropIndex("dbo.TB_PRODUTO_CATEGORIA", new[] { "ID_PRODUTO" });
            DropIndex("dbo.TB_Categoria_Imagem", new[] { "ID_IMAGEM" });
            DropIndex("dbo.TB_Categoria_Imagem", new[] { "ID_CATEGORIA" });
            DropIndex("dbo.TB_USUARIO_RECUPERAR_SENHA", new[] { "ID_USUARIO" });
            DropIndex("dbo.TB_Fila_Produto", new[] { "ID_FILA" });
            DropIndex("dbo.TB_Fila_Mensagem", new[] { "ID_FILA" });
            DropIndex("dbo.TB_CompraAtiva", new[] { "ID_Usuario" });
            DropIndex("dbo.TB_CompraAtiva", new[] { "ID_Ponto_Real_Demanda" });
            DropIndex("dbo.TB_PEDIDO", new[] { "ID_PRODUTO" });
            DropIndex("dbo.TB_PEDIDO", new[] { "ID_PONTO_REAL_DEMANDA" });
            DropIndex("dbo.TB_PEDIDO", new[] { "ID_INTEGRANTE" });
            DropIndex("dbo.TB_Compra_Item_Substituto", new[] { "ID_COMPRA_ITEM_PRINCIPAL" });
            DropIndex("dbo.TB_Compra_Item_Substituto", new[] { "ID_COMPRA_ITEM_SUBSTITUTO" });
            DropIndex("dbo.TB_Compra_Item", new[] { "ID_PEDIDO" });
            DropIndex("dbo.TB_Compra_Item", new[] { "ID_LISTA_PRODUTO_ITEM" });
            DropIndex("dbo.TB_Compra_Item", new[] { "ID_COMPRA" });
            DropIndex("dbo.TB_STATUS_USUARIO_PONTO_DEMANDA", new[] { "ID_USUARIO" });
            DropIndex("dbo.TB_Loja_Master_Data", new[] { "ID_ENDERECO" });
            DropIndex("dbo.TB_Loja_Master_Data", new[] { "ID_LOJA" });
            DropIndex("dbo.TB_Produto_Preco", new[] { "ID_PRODUTO" });
            DropIndex("dbo.TB_PRODUTO_MASTER_DATA", new[] { "ID_PRODUTO" });
            DropIndex("dbo.TB_LISTA_PRODUTO_ITEM", new[] { "ID_LISTA_PRODUTO" });
            DropIndex("dbo.TB_LISTA_PRODUTO_ITEM", new[] { "ID_PRODUTO" });
            DropIndex("dbo.TB_LISTA_PRODUTO_ITEM", new[] { "ID_PERIODO_CONSUMO" });
            DropIndex("dbo.TB_LISTA_PRODUTO", new[] { "ID_PONTO_REAL_DEMANDA" });
            DropIndex("dbo.TB_ENDERECO", new[] { "ID_CIDADE" });
            DropIndex("dbo.TB_PONTO_REAL_DEMANDA", new[] { "ID_USUARIO_CRIADOR" });
            DropIndex("dbo.TB_PONTO_REAL_DEMANDA", new[] { "ID_ENDERECO" });
            DropIndex("dbo.TB_GRUPO_INTEGRANTE", new[] { "ID_PONTO_REAL_DEMANDA" });
            DropIndex("dbo.TB_GRUPO_INTEGRANTE", new[] { "ID_INTEGRANTE" });
            DropIndex("dbo.TB_INTEGRANTE", new[] { "ID_USUARIO" });
            DropIndex("dbo.TB_Compra", new[] { "ID_PONTO_REAL_DEMANDA" });
            DropIndex("dbo.TB_Compra", new[] { "ID_INTEGRANTE" });
            DropIndex("dbo.TB_CIDADE", new[] { "ID_UF" });
            DropIndex("dbo.TB_CATEGORIA", new[] { "ID_CATEGORIA_PAI" });
            DropTable("dbo.TB_Contrato_Usuario");
            DropTable("dbo.TB_Ponto_Real_Demanda_Loja");
            DropTable("dbo.TB_Produto_Ponto_Real_Demanda");
            DropTable("dbo.TB_PRODUTO_IMAGEM");
            DropTable("dbo.TB_PRODUTO_CATEGORIA");
            DropTable("dbo.TB_Categoria_Imagem");
            DropTable("dbo.TB_Mensagem");
            DropTable("dbo.TB_USUARIO_RECUPERAR_SENHA");
            DropTable("dbo.TB_Token_Smv");
            DropTable("dbo.TB_Fila_Produto");
            DropTable("dbo.TB_Fila_Mensagem");
            DropTable("dbo.TB_Fila");
            DropTable("dbo.TB_Email_Capturado");
            DropTable("dbo.TB_Contato");
            DropTable("dbo.TB_CompraAtiva");
            DropTable("dbo.TB_PEDIDO");
            DropTable("dbo.TB_Compra_Item_Substituto");
            DropTable("dbo.TB_Compra_Item");
            DropTable("dbo.TB_STATUS_USUARIO_PONTO_DEMANDA");
            DropTable("dbo.TB_Contrato");
            DropTable("dbo.TB_USUARIO");
            DropTable("dbo.TB_Loja_Master_Data");
            DropTable("dbo.TB_Loja");
            DropTable("dbo.TB_Produto_Preco");
            DropTable("dbo.TB_PRODUTO_MASTER_DATA");
            DropTable("dbo.TB_PRODUTO");
            DropTable("dbo.TB_PERIODO_CONSUMO");
            DropTable("dbo.TB_LISTA_PRODUTO_ITEM");
            DropTable("dbo.TB_LISTA_PRODUTO");
            DropTable("dbo.TB_ENDERECO");
            DropTable("dbo.TB_PONTO_REAL_DEMANDA");
            DropTable("dbo.TB_GRUPO_INTEGRANTE");
            DropTable("dbo.TB_INTEGRANTE");
            DropTable("dbo.TB_Compra");
            DropTable("dbo.TB_UF");
            DropTable("dbo.TB_CIDADE");
            DropTable("dbo.TB_IMAGEM");
            DropTable("dbo.TB_CATEGORIA");
            DropTable("dbo.TB_Animal");
        }
    }
}
