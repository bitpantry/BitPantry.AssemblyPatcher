[![Build status](https://ci.appveyor.com/api/projects/status/aw962iaoqgsi5iab?svg=true)](https://ci.appveyor.com/project/bitpantry/bitpantry-assemblypatcher)
[![Nuget Downloads](https://img.shields.io/nuget/dt/bitpantry.assemblypatcher.svg)](http://nuget.org/packages/bitpantry.assemblypatcher)
[![Nuget Current Version](https://img.shields.io/nuget/v/bitpantry.assemblypatcher.svg)](http://nuget.org/packages/bitpantry.assemblypatcher)
[![Nuget Pre-version](https://img.shields.io/nuget/vpre/bitpantry.assemblypatcher.svg)](http://nuget.org/packages/bitpantry.assemblypatcher)


# Command Line Usage
The following usage description can be used to interact with the assembly patcher executable from the command line.

`BitPantry.AssemblyPatcher.exe <solutionDirectoryPath> <targetProjectFilePath> <versionPattern> <alternativeAssemblyInfoFilePath>`

* **solutionDirectoryPath** is the fully qualified or relative path to the solution directory of the project being patched
* **targetProjectFilePath** is the fully qualified or relative path to the project file of the assembly being patched
* **versionPattern** is a pattern (similar to a string format) that will be used to patch the version - the versionPattern specifies how each part of the pattern should be patched
* **alternateAssemblyInfoFilePath** is an optional parameter that should be provided if the name or location of your AssemblyInfo.cs file is not the default name or location (relative to the targetProjectFilePath specified above

The assembly patcher patches both the AssemblyVersion and AssemblyFileVersion attributes in the target AssemblyInfo file after which the project should be compiled in order to create the assembly with the patched version number. The Assembly Patcher does not patch the actual compiled assembly.

## Version Patterns
As described above, a version pattern should be provided when invoking the assembly patcher in order to specify how each part of the version should be patched. An example version pattern might resemble the following.

"1.0.{tokenOne}.{tokenTwo}"

As you can see, a version pattern is made up of two different types of elements, or parts.


1. **Literals** are specific integer values that should replace the existing values in the VersionInfo file - in the example above, the values "1" and "0", for the Major and Minor versions respectively, are literals and any AssemblyInfo file where the version pattern is applied will necessarily have a Major version of "1" and a Minor version of "0"

2. **Tokens** represent specific patching logic that should be applied - the available out of the box tokens as well as information on how to create your own custom patching logic is described below

### Out of the Box Tokens

As described above, adding tokens to the version pattern will tell AssemblyPatcher to apply certain logic when patching specific version elements, or parts, in the target AssemblyInfo file. The following table describes the out of the box tokens that can be used to apply certain logic.

| Token | Description |
---|---
{increment} | The _increment_ token tells AssemblyPatcher to simply increment the current value. For example, if the current version is "1.0.2.**3**" and the pattern is "1.0.2.**{increment}**", the resulting version will be "1.0.2.**4**
{#} | The _#_ sign represents the Pass Through token. When applied the current value will be passed through unchanged during the patching process. For example, if the current version is "1.0.2.**3**" and the pattern is "1.0.2.**{#}**", the resulting version will be "1.0.2.**3**
{tfsChangeSet} | The _tfsChangeSet_ token tells AssemblyPatcher to use the TFS change set number of the target project working copy as the new version part value. For example, if the current version is "1.2.0.**0**" and the pattern is "1.2.0.**{tfsChangeSet}**" and the working copy change set number is **4387**, the resulting version will be "1.2.0.**4387**"

AssemblyPatcher can be easily extended so that you can also create your own custom patching logic.

### Custom Patching Logic

* Implement the _BitPantry.AssemblyPatcher.IVersionPartGenerator_ interface in a separate assembly
* Update your entry application's app.config file to associate a token with your new VersionPartGenerator

#### Implementing IVersionPartGenerator
The IVersionPartGenerator class is listed below.

```javascript
namespace BitPantry.AssemblyPatcher
{
    public interface IVersionPartGenerator
    {
        /// <summary>
        /// Generates a version part
        /// </summary>
        /// <param name="currentPartValue">The current value of the part</param>
        /// <param name="ctx">The context of the operation</param>
        /// <returns>The result of the generation operation</returns>
        int Generate(int currentPartValue, VersionPartPatchingContext ctx);
    }
}
```

When your generator is invoked, it will be passed the value two variables.

First, the **currentPartValue** represents the current value of the part your generator is patching. For example, if the current version is "1.0.2.**3**" and the version pattern is "1.0.2.**{yourToken}**, your generate function will be passed **3** in the currentPartValue variable.

Second, the **ctx** is the current _VersionPartPatchingContext_ which may be useful depending on what type of logic your generator is executing. The following is a code listing for the _VersionPartPatchingContext_.

```javascript
namespace BitPantry.AssemblyPatcher
{
    /// <summary>
    /// Provides the context of the generation operation
    /// </summary>
    public class VersionPartPatchingContext
    {
        /// <summary>
        /// The root directory of the solution
        /// </summary>
        public string SolutionRootPath { get; set; }

        /// <summary>
        /// The target project file
        /// </summary>
        public string TargetProjectFile { get; set; }


        public VersionPartPatchingContext(string solutionRootPath, string targetProjectFile)
        {
            SolutionRootPath = solutionRootPath;
            TargetProjectFile = targetProjectFile;
        }

        public override string ToString()
        {
            return string.Format("{0} :: \"{1}\" \"{2}\"",
                this.GetType().FullName,
                SolutionRootPath,
                TargetProjectFile);
        }
    }
}
```

#### Updating The App.Config
Once you have created your custom implementation of IVersionPartGenerator, you can update the entry application's app.config to load the new generator.

First, define the **assemblyPatcher** custom configuration section at the top of your app.config file.

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="**assemblyPatcher**" type="BitPantry.AssemblyPatcher.Configuration.AssemblyPatcherConfiguration, BitPantry.AssemblyPatcher" />
  </configSections>
```

The name of the custom section must be **assemblyPatcher**, otherwise it will not be loaded.

Next, add the custom configuration section to reference your newly created generator.

```
  <assemblyPatcher>
    <partGenerators>
      <add token="{yourToken}" type="<your type>" />
    </partGenerators>
  </assemblyPatcher>
```

For each custom generator, specify a token and the generator type. Please be aware that while tokens cannot be re-used, the types can. Specifying a token that has already been defined will result in a load exception.

Note - the assembly where your custom generator can be found must be in a location that can be referenced by using a fully qualified type name.

# Using the Referenced Assembly
In addition to invoking the AssemblyPatcher from the command line, you can also reference the assembly directly to use in your own implementations.

First, add a reference to BitPantry.AssemblyPatcher.exe.

Next, use the static `VersionPatcher` utility class to patch your assemblies.

```javascript
/// <summary>
/// Patches the assembly indicated by the provided path information
/// </summary>
/// <param name="solutionDirectoryPath">The directory path to the solution</param>
/// <param name="targetProjectFilePath">The file path to the target csproj of the project to patch</param>
/// <param name="versionPattern">The version format - i.e., "1.0.{token}.{token}"</param>
/// <param name="assemblyInfoFilePath">If the assembly info file is different than the default filename 
/// and location, provide the full path and filename</param>
/// <returns>The patched assembly info file object</returns>
public static AssemblyInfoFile Patch(
	string solutionDirectoryPath, 
	string targetProjectFilePath, 
	string versionPattern,
	string assemblyInfoFilePath = null)
```
For example, 

```javascript
AssemblyInfoFile patchedFile = VersionPatcher.Patch(
	"C:\\projects\\mySolution"
	"C:\\projects\\mySolution\\myProject\\myProject.csproj",
	"{#}.{#}.{tfsChangeSet}.{increment}");
```
