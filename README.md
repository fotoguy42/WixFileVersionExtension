# WixFileVersionExtension

A [WiX](http://wixtoolset.org/) extension to read values from FileVersionInfo for a file on disk.

[![Build status](https://ci.appveyor.com/api/projects/status/tc19l20j9vdb13y8?svg=true)](https://ci.appveyor.com/project/fotoguy42/wixfileversionextension)

## Installation

When using the NuGet package the reference doesn't get added automatically and Visual Studio 2017 seems to no longer support custom install scripts. The reference will have to be added/updated manually by selecting the DLL from the packages directory.

## Notes
You do not need this extension if you are trying to set the Version on your Product node. Instead, you should bind to the File Version of the binary your msi is being built for. 

For Example
```
<Product Id="*" Name="Cool New App" Language="1033" Version="!(bind.FileVersion.filblahblahblah)"  Manufacturer="Acme Inc" UpgradeCode="badbadbad-3651-4351-9181-24c55144d91c">
```
**filblahblahblah** is the File ID of the file you want to get your version number from.
To use in Bootstrap assemblies, you don't have File ID (that I'm aware of), so this extension you pass in the filename to set a variable that you can then use throughout the bundle.
```
	<?define ProductVersion="$(fileVersion.ProductVersion($(var.MyAppOnstaller.TargetPath)))" ?>
	<Bundle Name="My App Bootstrapper" Version="$(var.ProductVersion)" Manufacturer="ACME Inc" UpgradeCode="badbadbad-7a11-40c0-9cca-bf86464980fa">
```


## Authors

* **Chris Kaczor** - *Initial work* - https://github.com/ckaczor - https://chriskaczor.com
* **Todd Kneib** - *Added getting MSI Properties* - https://github.com/fotoguy42

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
