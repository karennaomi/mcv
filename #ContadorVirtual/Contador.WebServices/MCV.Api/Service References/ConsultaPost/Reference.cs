﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MCV.Api.ConsultaPost {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://receita.fazenda.gov.br/", ConfigurationName="ConsultaPost.ConsultaMatrizSoap")]
    public interface ConsultaMatrizSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://receita.fazenda.gov.br/ConsultaMatriz", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Xml.XmlNode ConsultaMatriz(string strCNPJBasico);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://receita.fazenda.gov.br/ConsultaMatriz", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Xml.XmlNode> ConsultaMatrizAsync(string strCNPJBasico);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ConsultaMatrizSoapChannel : MCV.Api.ConsultaPost.ConsultaMatrizSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ConsultaMatrizSoapClient : System.ServiceModel.ClientBase<MCV.Api.ConsultaPost.ConsultaMatrizSoap>, MCV.Api.ConsultaPost.ConsultaMatrizSoap {
        
        public ConsultaMatrizSoapClient() {
        }
        
        public ConsultaMatrizSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ConsultaMatrizSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ConsultaMatrizSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ConsultaMatrizSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Xml.XmlNode ConsultaMatriz(string strCNPJBasico) {
            return base.Channel.ConsultaMatriz(strCNPJBasico);
        }
        
        public System.Threading.Tasks.Task<System.Xml.XmlNode> ConsultaMatrizAsync(string strCNPJBasico) {
            return base.Channel.ConsultaMatrizAsync(strCNPJBasico);
        }
    }
}