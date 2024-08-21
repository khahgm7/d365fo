
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
/p:BuildTasksDirectory="$(NugetsPath)\$(PlatCompiler)\DevAlm" /p:MetadataDirectory="$(MetadataPath)" /p:FrameworkDirectory="$(NugetsPath)\$(PlatCompiler)" /p:ReferenceFolder="$(NugetsPath)\$(PlatBuildXpp)\$(RefNet40);$(NugetsPath)\$(AppBuildXpp)\$(RefNet40);$(NugetsPath)\$(AppSuiteBuildXpp)\$(RefNet40);$(MetadataPath);$(Build.BinariesDirectory)" /p:ReferencePath="$(NugetsPath)\$(PlatCompiler)" /p:OutputDirectory="$(Build.BinariesDirectory)"
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