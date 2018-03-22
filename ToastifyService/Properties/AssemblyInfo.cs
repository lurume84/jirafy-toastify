using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ToastifyService")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Alessandro Attard Barbini")]
[assembly: AssemblyProduct("ToastifyService")]
[assembly: AssemblyCopyright("Copyright ©  2018 Alessandro Attard Barbini")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("44e4fea2-aef8-43b2-b2f6-1846795db487")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.10.4.*")]
#pragma warning disable CS7035 // The specified version string does not conform to the recommended format - major.minor.build.revision
#if DEBUG
[assembly: AssemblyFileVersion("1.10.4 [DEBUG BUILD]")]
#elif TEST_RELEASE
[assembly: AssemblyFileVersion("1.10.4 [TEST BUILD]")]
#endif
#pragma warning restore CS7035 // The specified version string does not conform to the recommended format - major.minor.build.revision