using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
using LM.Core.Application.Properties;

[assembly: AssemblyTitle("LM.Core.Application")]
[assembly: AssemblyDescription("Camada de aplicação do projeto Lista Mágica")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Lista Mágica")]
[assembly: AssemblyProduct("LM.Core.Application")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("7129c7d9-bfea-452a-b2d6-b28da0670498")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(Version.CurrentVersion)]
[assembly: AssemblyFileVersion(Version.CurrentVersion)]
[assembly: AssemblyInformationalVersion(Version.CurrentVersion)]

namespace LM.Core.Application.Properties
{
    internal struct Version
    {
        public const string CurrentVersion = "1.14.3";
    }
}