
# Pipeline

![1_Pipeline](/1_Pipeline.png)

# Compile and Build package

![2_CompileAndBuildPackage](/2_CompileAndBuildPackage.png)

# Nuget install

![3_NugetInstall](/3_NugetInstall.png)

```ps
install -Noninteractive $(NugetConfigsPath)\packages.config -ConfigFile $(NugetConfigsPath)\nuget.config -Verbosity Detailed -ExcludeVersion -OutputDirectory "$(NugetsPath)"
```

# Build solution

![4_BuildSolution](/4_BuildSolution.png)

```ps
/p:BuildTasksDirectory="$(Pipeline.Workspace)\NuGets\Microsoft.Dynamics.AX.Platform.CompilerPackage\DevAlm"
/p:MetadataDirectory="$(Build.SourcesDirectory)\Metadata"
/p:FrameworkDirectory="$(Pipeline.Workspace)\NuGets\Microsoft.Dynamics.AX.Platform.CompilerPackage"
/p:ReferenceFolder="$(Pipeline.Workspace)\NuGets\Microsoft.Dynamics.AX.Platform.DevALM.BuildXpp\ref\net40;$(Pipeline.Workspace)\NuGets\Microsoft.Dynamics.AX.Application1.DevALM.BuildXpp\ref\net40;$(Pipeline.Workspace)\NuGets\Microsoft.Dynamics.AX.Application2.DevALM.BuildXpp\ref\net40;$(Pipeline.Workspace)\NuGets\Microsoft.Dynamics.AX.ApplicationSuite.DevALM.BuildXpp\ref\net40;$(Build.SourcesDirectory)\Metadata;$(Build.BinariesDirectory)"
/p:ReferencePath="$(Pipeline.Workspace)\NuGets\Microsoft.Dynamics.AX.Platform.CompilerPackage" /p:OutputDirectory="$(Build.BinariesDirectory)"
```

# Use Nuget

![5_UseNuget](/5_UseNuget.png)

# Create deployable package

![6_CreatePackage](/6_CreatePackage.png)

```ps
$(NuGetsPath)\$(PlatCompiler)
$(Build.BinariesDirectory)
$(Build.ArtifactStagingDirectory)\$(Build.BuildNumber).zip
```

# Publish artifact

![7_PublishArtifact](/7_PublishArtifact.png)

```ps
$(Build.ArtifactStagingDirectory)
```