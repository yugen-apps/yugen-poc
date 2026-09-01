targetScope = 'subscription'

param locationName string
param resourceGroupName string

resource resourceGroup 'Microsoft.Resources/resourceGroups@2025-04-01' = {
  name: resourceGroupName
  location: locationName
}

//
output resourceGroup_id string = resourceGroup.id
// rg-tmp
output resourceGroup_name string = resourceGroup.name
// West Europe
output resourceGroup_location string = resourceGroup.location

param vnetName string
param subnetName string

module vnet 'virtualNetworks.bicep' = {
  name: '${deployment().name}-${vnetName}'
  scope: resourceGroup
  params: {
    subnetName: subnetName
    vnetName: vnetName
  }
}

param managedEnvironmentName string

module managedEnvironment 'managedEnvironments.bicep' = {
  name: '${deployment().name}-${managedEnvironmentName}'
  scope: resourceGroup
  params: {
    managedEnvironmentName: managedEnvironmentName
    subnetName: subnetName
    vnetId: vnet.outputs.vnet_id
  }
}

param containerappName string

module containerapp 'containerapps.bicep' = {
  name: '${deployment().name}-${containerappName}'
  scope: resourceGroup
  params: {
    containerappName: containerappName
    managedEnvironmentsId: managedEnvironment.outputs.managedEnvironment_id
  }
}
