# Yugen.MotoGP

$AppxBundle="D:\yugen-moto-gp\AppPackages\Yugen.MotoGp.Uwp_2.0.0.0_x64_ARM64_bundle.msixupload"
$AppxBundlePlatforms="x64|ARM64"
$AppxPackageDir="D:\yugen-moto-gp\AppPackages"
$Platform="x64"
$Solution="D:\yugen-moto-gp\Yugen.MotoGp.slnx"

& 'C:\Program Files\Microsoft Visual Studio\18\Enterprise\MSBuild\Current\Bin\MSBuild.exe' `
$Solution `
/p:AppxBundle="Never" `
/p:AppxPackageDir=$AppxPackageDir `
/p:AppxPackageSigningEnabled="false" `
/p:Configuration="Release" `
/p:GenerateAppxPackageOnBuild="true" `
/p:Platform="$Platform" `
/p:UapAppxPackageBuildMode="StoreUpload" `
/v:q

& 'C:\Program Files\Microsoft Visual Studio\18\Enterprise\MSBuild\Current\Bin\MSBuild.exe' `
$Solution `
/p:AppxBundle="Never" `
/p:AppxPackageDir=$AppxPackageDir `
/p:AppxPackageSigningEnabled="false" `
/p:Configuration="Release" `
/p:GenerateAppxPackageOnBuild="true" `
/p:Platform="arm64" `
/p:UapAppxPackageBuildMode="StoreUpload" `
/v:q

& 'C:\Program Files\Microsoft Visual Studio\18\Enterprise\MSBuild\Current\Bin\MSBuild.exe' `
$Solution `
/p:AppxBundle="Always" `
/p:AppxBundlePlatforms="$AppxBundlePlatforms" `
/p:AppxPackageDir=$AppxPackageDir `
/p:AppxPackageSigningEnabled="false" `
/p:BuildAppxUploadPackageForUap="true" `
/p:Configuration="Release" `
/p:GenerateAppxPackageOnBuild="true" `
/p:Platform="$Platform" `
/p:PublishAppxPackage="true" `
/p:UapAppxPackageBuildMode="StoreUpload" `
/v:q


dotnet publish `
/p:AppxBundle="Always" `
/p:AppxBundlePlatforms="$AppxBundlePlatforms" `
/p:AppxPackageDir=$AppxPackageDir `
/p:AppxPackageSigningEnabled="false" `
/p:Configuration="Release" `
/p:GenerateAppxPackageOnBuild="true" `
/p:Platform="$Platform" `
/p:PublishAppxPackage="true"


dotnet publish `
/p:AppxBundle="Always" `
/p:AppxBundlePlatforms="$AppxBundlePlatforms" `
/p:AppxPackageDir=$AppxPackageDir `
/p:AppxPackageSigningEnabled="false" `
/p:BuildAppxUploadPackageForUap="true" `
/p:Configuration="Release" `
/p:GenerateAppxPackageOnBuild="true" `
/p:Platform="$Platform" `
/p:PublishAppxPackage="true" `
/p:UapAppxPackageBuildMode="StoreUpload"


Set PublishAppxPackage=true in the .csproj - This enables the publish target to be called correctly recursively (for self-contained, AOT, etc.)
AppxBundle=Always and AppxBundlePlatforms="x86|x64|ARM64 to set what platforms go in the bundle.
The runtime identifiers which I think are already in your project (for self-contained)
BuildAppxUploadPackageForUap=true if you want to generate the .msixupload file.

Then, you can execute dotnet publish -p:Platform=x64 -p:GenerateAppxPackageOnBuild=true after setting those properties.

And then dotnet publish -p:GenerateAppxPackageOnBuild=true -p:Platform=ARM64
(assuming you are in an arm64 machine, if you are in a x64 pass x64 as platform. The child builds for the bundle should now get the correct RID).

## 
https://learn.microsoft.com/en-us/windows/apps/package-and-deploy/ci-for-winui3?pivots=winui3-packaged-csharp
https://github.com/microsoft/WindowsAppSDK/issues/1808
https://github.com/microsoft/WindowsAppSDK/issues/6321
https://github.com/microsoft/WindowsAppSDK/issues/6498
https://github.com/microsoft/WindowsAppSDK/issues/6508