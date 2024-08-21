https://learn.microsoft.com/en-us/dynamics365/fin-ops-core/dev-itpro/dev-tools/hosted-build-automation


For version 10.0.40 or later, use the following packages.config layout:
```xml
<?xml version="1.0" encoding="utf-8"?>
<packages>
    <package id="Microsoft.Dynamics.AX.Platform.DevALM.BuildXpp" version="7.0.7279.40" targetFramework="net40" />
    <package id="Microsoft.Dynamics.AX.Application1.DevALM.BuildXpp" version="10.0.1935.21" targetFramework="net40" />
    <package id="Microsoft.Dynamics.AX.Application2.DevALM.BuildXpp" version="10.0.1935.21" targetFramework="net40" />
    <package id="Microsoft.Dynamics.AX.ApplicationSuite.DevALM.BuildXpp" version="10.0.1935.21" targetFramework="net40" />
    <package id="Microsoft.Dynamics.AX.Platform.CompilerPackage" version="7.0.7279.40" targetFramework="net40" />
</packages>
```