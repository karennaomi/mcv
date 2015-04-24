using System;
using System.Collections.ObjectModel;

namespace LM.Core.Domain
{
    public enum IntegrantePapel : long
    {
        Administrador = 1,
        Colaborador = 2
    }

    public class Integrante
    {
        public Integrante()
        {
            Ativo = true;
            DataInclusao = DateTime.Now;
        }

        public Integrante(Usuario usuario)
        {
            Ativo = true;
            DataInclusao = DateTime.Now;
            Usuario = usuario;
            DataNascimento = usuario.DataNascimento;
            EhUsuarioSistema = true;
            Nome = usuario.Nome;
            Papel = IntegrantePapel.Administrador;
            GrupoDeIntegrantes = new GrupoDeIntegrantes{ Nome = "Grupo do " + usuario.Nome};
        }

        public long Id { get; set; }
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string EmailConvite { get; set; }
        public bool EhUsuarioSistema { get; set; }
        public bool EhUsuarioConvidado { get; set; }
        public DateTime? DataConvite { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public string Telefone { get; set; }
        public int? DDD { get; set; }
        public IntegrantePapel Papel { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual GrupoDeIntegrantes GrupoDeIntegrantes { get; set; }
        public virtual Persona Persona { get; set; }

        public int ObterIdade()
        {
            return LMHelper.ObterIdade(DataNascimento);
        }
    }
}
