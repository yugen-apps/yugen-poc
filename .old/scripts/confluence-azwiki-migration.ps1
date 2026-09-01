$Project = "BATHSPA"
$File = "Data-Audit---hierarchy-data---Complete_2845671425.html".Replace(".html","")
$File = "2022-09-07-Meeting-notes_2920415237.html".Replace(".html","")
$InputFilePath = ".\input\$Project\$File.html"
$OutputFilePath = ".\output\$Project\$File.md"
.\pandoc.exe $InputFilePath -f html -t gfm -s -o $OutputFilePath
(Get-Content $OutputFilePath).Replace("attachments/","/.attachments/") | Set-Content $OutputFilePath
(Get-Content $OutputFilePath).Replace("images/","/.images/") | Set-Content $OutputFilePath
(Get-Content $OutputFilePath).Replace(".html)",")") | Set-Content $OutputFilePath




$folders = Get-ChildItem .\input
foreach ($folder in $folders)
{
    $Project = $folder.Name
    # New-Item -Path .\output\${FolderName} -ItemType Directory
    
    $files = Get-ChildItem -recurse .\input\${Project} -File -Include "*.html"
    foreach ($file in $files)
    {
        $FileName = $file.Name.Replace(".html","")
        $InputFilePath = ".\input\$Project\$FileName.html"
        $OutputFilePath = ".\output\$Project\$FileName.md"

        Write-Output $InputFilePath $OutputFilePath
        .\pandoc.exe $InputFilePath -f html -t gfm -s -o $OutputFilePath
        (Get-Content $OutputFilePath).Replace("attachments/","/.attachments/") | Set-Content $OutputFilePath
        (Get-Content $OutputFilePath).Replace("images/","/.images/") | Set-Content $OutputFilePath
        (Get-Content $OutputFilePath).Replace(".html)",")") | Set-Content $OutputFilePath
    }
}

