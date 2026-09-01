# Works Produce an msixupload file

& 'C:\Program Files\Microsoft Visual Studio\18\Enterprise\MSBuild\Current\Bin\MSBuild.exe' `
. `
/p:AppxBundle="Never" `
/p:AppxPackageSigningEnabled="false" `
/p:Configuration="Release" `
/p:GenerateAppxPackageOnBuild="true" `
/p:Platform="x64" `
/p:UapAppxPackageBuildMode="StoreUpload" `
/v:q

# error MSB4044: The "WinAppSdkGenerateAppxSymbolPackage" task was not given a value for the required parameter "MsPdbCmfExeFullpath".

& 'C:\Program Files\Microsoft Visual Studio\18\Enterprise\MSBuild\Current\Bin\MSBuild.exe' `
. `
/p:AppxBundle="Always" `
/p:AppxBundlePlatforms="x64|ARM64" `
/p:AppxPackageSigningEnabled="false" `
/p:BuildAppxUploadPackageForUap="true" `
/p:Configuration="Release" `
/p:GenerateAppxPackageOnBuild="true" `
/p:Platform="x64" `
/p:PublishAppxPackage="true" `
/p:UapAppxPackageBuildMode="StoreUpload" `
/v:q

# error MSB4044: The "WinAppSdkGenerateAppxSymbolPackage" task was not given a value for the required parameter "MsPdbCmfExeFullpath".

& 'C:\Program Files\Microsoft Visual Studio\18\Enterprise\MSBuild\Current\Bin\MSBuild.exe' `
. `
/p:AppxBundle="Always" `
/p:AppxBundlePlatforms="x64|ARM64" `
/p:AppxPackageSigningEnabled="false" `
/p:BuildAppxUploadPackageForUap="true" `
/p:Configuration="Release" `
/p:GenerateAppxPackageOnBuild="true" `
/p:Platform="x64" `
/p:PublishAppxPackage="true" `
/p:UapAppxPackageBuildMode="SideloadOnly" `
/v:q

# create to different .msix, weirdly in 2 different paths: 
## bin\ARM64\Release\net8.0-windows10.0.19041.0\win-arm64\WinUiApp_1.0.0.0_arm64.msix
## AppPackages\WinUiApp_1.0.0.0_Test\WinUiApp_1.0.0.0_x64.msix

dotnet publish `
/p:AppxBundle="Always" `
/p:AppxBundlePlatforms="x64|ARM64" `
/p:AppxPackageSigningEnabled="false" `
/p:Configuration="Release" `
/p:GenerateAppxPackageOnBuild="true" `
/p:Platform="x64" `
/p:PublishAppxPackage="true"

# D:\packages\NuGet\cache\microsoft.windows.sdk.buildtools.msix\1.7.251221100\build\Microsoft.Windows.SDK.BuildTools.MSIX.Packaging.targets(3100,5): error MSB4044: The "WinAppSdkGenerateAppxSymbolPackage" task was not given a value for the required parameter "MsPdbCmfExeFullpath".

dotnet publish `
/p:AppxBundle="Always" `
/p:AppxBundlePlatforms="x64|ARM64" `
/p:AppxPackageSigningEnabled="false" `
/p:BuildAppxUploadPackageForUap="true" `
/p:Configuration="Release" `
/p:GenerateAppxPackageOnBuild="true" `
/p:Platform="x64" `
/p:PublishAppxPackage="true" `
/p:UapAppxPackageBuildMode="SideloadOnly"

#  D:\packages\NuGet\cache\microsoft.windows.sdk.buildtools.msix\1.7.251221100\build\Microsoft.Windows.SDK.BuildTools.MSIX.Packaging.targets(3100,5): error MSB4044: The "WinAppSdkGenerateAppxSymbolPackage" task was not given a value for the required parameter "MsPdbCmfExeFullpath".

dotnet publish `
/p:AppxBundle="Always" `
/p:AppxBundlePlatforms="x64|ARM64" `
/p:AppxPackageSigningEnabled="false" `
/p:BuildAppxUploadPackageForUap="true" `
/p:Configuration="Release" `
/p:GenerateAppxPackageOnBuild="true" `
/p:Platform="x64" `
/p:PublishAppxPackage="true" `
/p:UapAppxPackageBuildMode="StoreUpload"