﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LM.Core.Domain
{
    public enum IntegrantePapel : long
    {
        Administrador = 1,
        Colaborador = 2
    }

    public enum TipoIntegrante
    {
        NaoReconhecido = 0,
        Familia = 1,
        Empregado = 2,
        Pet = 3
    }

    public class Integrante : IValidatableObject
    {
        public Integrante()
        {
            Ativo = true;
            DataInclusao = DateTime.Now;
            EhUsuarioSistema = true;
            Papel = IntegrantePapel.Administrador;
        }

        public long Id { get; set; }
        
        [Required(ErrorMessage = "O campo [Nome] é de preenchimento obrigatório!", AllowEmptyStrings = false)]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "O campo [Email] é de preenchimento obrigatório!", AllowEmptyStrings = false)]
        public string Email { get; set; }
        
        public string Cpf { get; set; }
        
        [DisplayName("Data de Nascimento")]
        [Required(ErrorMessage = "O campo [Data de Nascimento] é de preenchimento obrigatório!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
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
        [Required(ErrorMessage = "O campo [Sexo] é de preenchimento obrigatório!")]
        public string Sexo { get; set; }
        public IntegrantePapel Papel { get; set; }
        public TipoIntegrante Tipo { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual GrupoDeIntegrantes GrupoDeIntegrantes { get; set; }

        public int ObterIdade()
        {
            return LMHelper.ObterIdade(DataNascimento);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (Sexo.ToLower() != "m" && Sexo.ToLower() != "f")
            {
                results.Add(new ValidationResult("O sexo selecionado é inválido: " + Sexo, new[] { "Sexo" }));
            }
            return results;
        }
    }
}
