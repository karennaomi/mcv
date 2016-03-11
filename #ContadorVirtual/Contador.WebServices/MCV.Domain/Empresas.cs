namespace MCV.Domain
{
    public class Empresas
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CNPJ { get; set; }
        //public Endereco Endereco { get; set; }
        public bool Ativa { get; set; }
        public bool AtivaMCV { get; set; }
        public bool OptanteSimples { get; set; }
        public int QtdeFuncionario { get; set; }
        public int IdTipoEmpresa { get; set; }
        public string LoginPrefeitura { get; set; }
        public string SenhaPrefeitura { get; set; }

        public Empresas()
        {
            //Endereco = new Endereco();
        }

    }


}