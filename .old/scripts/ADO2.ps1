#=== Login ===

$personalAccessToken = ""
$token = [System.Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes(":$($personalAccessToken)"))
$header = @{authorization = "Basic $token"}

#=== Config ===

[string]$organization = "kortext"  # the name of your DevOps organization
[string]$project = "KLP" #Project Queried
[string]$apiVersion = "7.0"
[string]$baseUrl = "https://dev.azure.com"
[string]$organizationBaseUrl = "${baseUrl}/${organization}/_apis"
[string]$projectBaseUrl = "${baseUrl}/${organization}/${project}/_apis"

#=== Get Build Pipelines  ===

$url = "${projectBaseUrl}/build/definitions/?api-version=${apiVersion}"
$result = Invoke-RestMethod -Uri $url -headers $header -Method Get

#=== Get Build Pipeline Retentions  ===

[string]$definitionId = "206" 
$url = "${projectBaseUrl}/build/retention/leases?api-version=${apiVersion}&definitionId=${definitionId}"
$result = Invoke-RestMethod -Uri $url -headers $header -Method Get
DeleteAllLease $result

#=== Delete Build Pipeline Retentions  ===
function DeleteAllLease($definition) {
    foreach ($lease in $definition.value) {
        $leaseId = $lease.leaseId
        $leaseUrl = "${projectBaseUrl}/build/retention/leases?ids=${leaseId}&api-version=${apiVersion}"
        Invoke-RestMethod -Uri $leaseUrl -headers $header -Method DELETE
        Write-Host "${leaseId} - Deleted"
    }
}

#=== Get Processes  ===

$url = "${organizationBaseUrl}/work/processes?api-version=${apiVersion}"
$result = Invoke-RestMethod -Uri $url -headers $header -Method Get

#=== Get Process  ===

[string]$processTypeId = "b427bf27-7fcc-4658-b7fd-aec95f0f221c"
$url = "${organizationBaseUrl}/work/processes/${processTypeId}?api-version=${apiVersion}"
$result = Invoke-RestMethod -Uri $url -headers $header -Method Get

#=== Get Fields ===

[string]$workItemTypes = "KLPAgile.UserStory"
[string]$fieldRefName = "Custom.ValueAreaSubCategory"
$url = "${organizationBaseUrl}/work/processes/${processTypeId}/workItemTypes/${workItemTypes}/fields/${fieldRefName}?api-version=${apiVersion}"
$result = Invoke-RestMethod -Uri $url -headers $header -Method Get

#=== Get the list of work items in this release ===

[string]$queryId = "783f8ee0-9426-442f-90f2-35c38e5a007a" #id of ther query being output
$url = "${projectBaseUrl}/wit/wiql/${queryId}?api-version=${apiVersion}"
$result = Invoke-RestMethod -Uri $url -headers $header -Method Get

#=== Get the details for each work item ===

$releaseNotes = "<html><body>"
foreach($reportItem in $result.workItems)
{
    #=== Get the work item details ===
    Write-Host " - Getting release item details: " $reportItem.Url
    $reportItemDetails = Invoke-RestMethod -Uri $reportItem.Url -headers $header -Method Get

    #=== Add the work items details to the output ===
    $workItemUrl =  $UriOrga + "DefaultCollection/" + $project + "/_workitems#_a=edit&fullScreen=true&id=" + $reportItemDetails.Id
    $releaseNotes += "<p><a href='" + $workItemUrl + "'>" + $reportItemDetails.fields.'System.WorkItemType' + " " + $reportItemDetails.Id + "</a> " + $reportItemDetails.fields.'System.Title' +"<br/>"
    $releaseNotes += "<b>Description</b><br/>" + $reportItemDetails.fields.'System.Title' + "</p>"
}
$releaseNotes += "</body></html>"
Write-Host $releaseNotes 

#=== Get Agent Pools ===

$url = "${organizationBaseUrl}/distributedtask/pools/?api-version=${apiVersion}"
$result = Invoke-RestMethod -Uri $url -headers $header -Method Get

#=== Delete Agent Pool ===

[string]$poolId = "9"
$url = "${organizationBaseUrl}/distributedtask/pools/${poolId}?api-version=${apiVersion}"
Invoke-RestMethod -Uri $url -headers $header -Method DELETE

#=== Update Agent Pool ===

[string]$poolId = "9"
$url = "${organizationBaseUrl}/distributedtask/pools/${poolId}?api-version=${apiVersion}"
$body = @{
        "isLegacy" = "false"
}  | ConvertTo-Json -Depth 5
Invoke-RestMethod -Uri $url -headers $header -Method PATCH -Body $body -ContentType "application/json"

#=== Get Agents ===

[string]$poolId = "9"
$url = "${organizationBaseUrl}/distributedtask/pools/${poolId}/agents?api-version=${apiVersion}"
$result = Invoke-RestMethod -Uri $url -headers $header -Method Get

#=== Delete Agent ===

[string]$poolId = "9"
[string]$agentId = "14"
$url = "${organizationBaseUrl}/distributedtask/pools/${poolId}/agents/${agentId}?api-version=${apiVersion}"
Invoke-RestMethod -Uri $url -headers $header -Method DELETE

#=== Get Package Version ===

[string]$feedId = "klp-native-apps-artifacts"
[string]$packageName = "rmsdk-lib-android"
[string]$packageVersion = "2.1.3"
$url = "https://pkgs.dev.azure.com/${organization}/${project}/_apis/packaging/feeds/${feedId}/upack/packages/${packageName}/versions/${packageVersion}?api-version=${apiVersion}-preview"
$result = Invoke-RestMethod -Uri $url -headers $header -Method Get


#=== Update Package Version ===

[string]$feedId = "klp-native-apps-artifacts"
[string]$packageName = "rmsdk-lib-android"
[string]$packageVersion = "2.1.3"


$body = @{
  views = @{
        op = 'add'
        path = '/views/-'
        value = 'prerelease'
    }
}  | ConvertTo-Json -Depth 5

$url = "https://pkgs.dev.azure.com/${organization}/${project}/_apis/packaging/feeds/${feedId}/upack/packages/${packageName}/versions/${packageVersion}?api-version=${apiVersion}-preview"
Invoke-RestMethod -Uri $url -headers $header -Method PATCH -Body $body -ContentType "application/json"

#=== Get Result ===

$resultString = $result  | ConvertTo-Json
Write-Host $resultString


# Body

$body = @{
        "name" = "TestProject"
        "description" = "Test Project"
        "ProjectVisibility" = "private"
        "capabilities" = @{
            "versioncontrol" = @{
                "sourceControlType" = "Git"
            }
            "processTemplate" = @{
                "templateTypeId" = "b8a3a935-7e91-48b8-a94c-606d37c3e9f2"
            }
        }
}  | ConvertTo-Json -Depth 5