Remove-Item –Path .\_site –Recurse
Remove-Item –Path .\docfx.console\content\obj –Recurse
Rename-Item -Path ".\docfx.console\" -NewName "docfx.console.2.59.2"
Move-Item -Path ".\docfx.console\content\templates" -Destination ".\"


$sourceDir = "C:\Dev\.tools"
$docfxDir = "docfx.console"
$project = ".\project\Class1.cs"
$docfxMetadataDest = "$sourceDir\docfx.console\content\obj\api"
$docfxJson = ".\docfx.console\content\docfx.json"

Move-Item -Path ".\templates\" -Destination ".\docfx.console\content\"
Get-ChildItem -Path "./" -Filter "$docfxDir.*" | Rename-Item -NewName $docfxDir
.\docfx.console\tools\docfx.exe metadata $project -o $docfxMetadataDest -t default,templates/singulinkfx
.\docfx.console\tools\docfx.exe $docfxJson -o .\
.\docfx.console\tools\docfx.exe serve .\_site\