﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LM.Core.Domain {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class LMResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal LMResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LM.Core.Domain.LMResource", typeof(LMResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ops! O campo de e-mail deve conter um endereço válido..
        /// </summary>
        public static string Default_Validation_Email {
            get {
                return ResourceManager.GetString("Default_Validation_Email", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O campo {0} deve possuir no máximo {1} caracteres..
        /// </summary>
        public static string Default_Validation_MaxLength {
            get {
                return ResourceManager.GetString("Default_Validation_MaxLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O campo {0} deve possuir no mínimo {1} caracteres..
        /// </summary>
        public static string Default_Validation_MinLength {
            get {
                return ResourceManager.GetString("Default_Validation_MinLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O formato do campo {0} é inválido..
        /// </summary>
        public static string Default_Validation_RegularExpression {
            get {
                return ResourceManager.GetString("Default_Validation_RegularExpression", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O campo {0} é de preenchimento obrigatório..
        /// </summary>
        public static string Default_Validation_Required {
            get {
                return ResourceManager.GetString("Default_Validation_Required", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O campo {0} selecionado é inválido..
        /// </summary>
        public static string DefaultValidation_Selected {
            get {
                return ResourceManager.GetString("DefaultValidation_Selected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Não pode atualizar um usuário do sistema..
        /// </summary>
        public static string Integrante_NaoPodeAtualizarQuemJaEhUsuario {
            get {
                return ResourceManager.GetString("Integrante_NaoPodeAtualizarQuemJaEhUsuario", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Não pode desativar integrante..
        /// </summary>
        public static string Integrante_NaoPodeDesativar {
            get {
                return ResourceManager.GetString("Integrante_NaoPodeDesativar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Este integrante não pode ser convidado..
        /// </summary>
        public static string Integrante_NaoPodeSerConvidado {
            get {
                return ResourceManager.GetString("Integrante_NaoPodeSerConvidado", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usuário desativado..
        /// </summary>
        public static string Usuario_LoginDesativado {
            get {
                return ResourceManager.GetString("Usuario_LoginDesativado", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Seu e-mail ou senha não conferem. Tente novamente!.
        /// </summary>
        public static string Usuario_LoginInvalido {
            get {
                return ResourceManager.GetString("Usuario_LoginInvalido", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usuário não encontrado.
        /// </summary>
        public static string Usuario_NaoEncontrado {
            get {
                return ResourceManager.GetString("Usuario_NaoEncontrado", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O usuário deve ter {0} anos ou mais..
        /// </summary>
        public static string Usuario_Validation_DataNascimento {
            get {
                return ResourceManager.GetString("Usuario_Validation_DataNascimento", resourceCulture);
            }
        }
    }
}
