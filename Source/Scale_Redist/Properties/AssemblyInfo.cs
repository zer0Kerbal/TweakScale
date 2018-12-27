using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Scale_Redist")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Scale_Redist")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("01c8d239-4233-4a83-ae50-3e1a12cff502")]

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

// this is the API definition of TweakScale.
// Other mods can include this in their distribution and compile against it without depending on a TweakScale version.
// It does not depend on KSP and has not been touched since biotronic wrote it.
// So its version should stay at 1.0, to make clear that other mods do not need to compile against a new version
// (pellinor)
//
// On the other hand, TweakScale is on a corner. It's not feasible to update third-party add-ons to fix problems
// related to "unholly interactions between add-ons", but it's not convenient to keep things inside TweakScale and
// take the heat everytime someone else borks somehow.
// I see no other way out of this mess but to deprecate the original Scale_Redist and promote a new one, not to be used
// by the add-ons themselves, but by specialized 'integrators" that should be developed apart from TweakScale.
// If some add-one borks, instead of "fixing" TweakScale (and taking the blame), the fix should go to the proper "integrator".
// TweakScale will have a lot less updates this way, and so we don't risk breaking up what's working fine for everybody else.
// (lisias)
[assembly: AssemblyVersion("1.1")]
[assembly: AssemblyFileVersion(TweakScale.Version.Number)]
