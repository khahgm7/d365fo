# Prerequisite

- Download Nuget.exe from [Available NuGet Distribution Versions](https://www.nuget.org/downloads). Go with the latest.
- Make sure we have enough storage for Artifact:
	- One can check at DevOps > Organisation settings > Artifact > Storage.
	- If there's not enough storage, delete current feeds. Notice that after deletion, feeds will come to "Deleted feeds", this will need Permanent deletion. Both option, deletion and permanent deletion, can be found under feeds' settings.

# Push

- Open PowerShell/ISE and run below script

```ps

$NugetPath = "G:\Nuget\"
$PackagePath = "G:\Nuget\40-Current\*.nupkg"
$Feed = "BTZ_Feed_10.0.40"
$ApiString = "az"
$TimeOut = 1200

Set-Location $NugetPath

.\nuget.exe push $PackagePath -Source $Feed -ApiKey $ApiString -Timeout $TimeOut

```

- Similar steps can be run under Package Manager Console in Visual Studio.