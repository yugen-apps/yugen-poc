//  targetScope = 'resourceGroup'
param subnetName string
param vnetName string

resource vnet 'Microsoft.Network/virtualNetworks@2025-07-01' = {
  name: vnetName
  location: resourceGroup().location
  properties: {    
    addressSpace: {
      addressPrefixes: [
        '10.0.0.0/16'
      ]
    }
    privateEndpointVNetPolicies: 'Disabled'
    subnets: [
      {
        name: subnetName
        properties: {
          addressPrefix: '10.0.0.0/23'
          serviceEndpoints: []
          delegations: [
            {
              name: 'Microsoft.App.environments'
              properties: {
                serviceName: 'Microsoft.App/environments'
              }
              type: 'Microsoft.Network/virtualNetworks/subnets/delegations'
            }
          ]
          privateEndpointNetworkPolicies: 'Disabled'
          privateLinkServiceNetworkPolicies: 'Enabled'
        }
      }
    ]
    virtualNetworkPeerings: []
    enableDdosProtection: false
  }
}

// /subscriptions/ae0647c4-c323-493b-8233-99012b801938/resourceGroups/rg-tmp/providers/Microsoft.Network/virtualNetworks/vnet-tmp
output vnet_id string = vnet.id
// vnet-tmp
output vnet_name string = vnet.name
// 0caf75a4-aa05-4cb4-8b02-70add8fb1c09
output vnet_resource_id string = vnet.properties.resourceGuid
