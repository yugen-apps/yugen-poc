//  targetScope = 'resourceGroup'
param managedEnvironmentName string
param subnetName string
param vnetId string

resource managedEnvironment 'Microsoft.App/managedEnvironments@2026-01-01' = {
  name: managedEnvironmentName
  location: resourceGroup().location
  properties: {
    appLogsConfiguration: {}
    zoneRedundant: false
    kedaConfiguration: {}
    daprConfiguration: {}
    customDomainConfiguration: {}
    workloadProfiles: [
      {
        workloadProfileType: 'Consumption'
        name: 'Consumption'
      }
    ]
    peerAuthentication: {
      mtls: {
        enabled: false
      }
    }
    peerTrafficConfiguration: {
      encryption: {
        enabled: false
      }
    }
    publicNetworkAccess: 'Enabled'
    vnetConfiguration: {
      internal: false
      infrastructureSubnetId: '${vnetId}/subnets/${subnetName}'
    }
    // infrastructureResourceGroup: resourceGroupName
  }
}

// /subscriptions/ae0647c4-c323-493b-8233-99012b801938/resourceGroups/rg-tmp/providers/Microsoft.App/managedEnvironments/cae-tmp
output managedEnvironment_id string = managedEnvironment.id
// cae-tmp
output managedEnvironment_name string = managedEnvironment.name
