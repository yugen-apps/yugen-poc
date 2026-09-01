//  targetScope = 'resourceGroup'
param containerappName string
param managedEnvironmentsId string

resource containerapp 'Microsoft.App/containerapps@2026-01-01' = {
  name: containerappName
  location: resourceGroup().location
  identity: {
    type: 'None'
  }
  properties: {
    environmentId: managedEnvironmentsId
    workloadProfileName: 'Consumption'
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: true
        targetPort: 1433
        exposedPort: 1433
        transport: 'Tcp'
        traffic: [
          {
            weight: 100
            latestRevision: true
          }
        ]
        allowInsecure: false
        stickySessions: {
          affinity: 'none'
        }
        additionalPortMappings: []
      }
      registries: []
      maxInactiveRevisions: 100
      identitySettings: []
    }
    template: {
      containers: [
        {
          image: 'mcr.microsoft.com/mssql/server:2025-latest'
          name: 'sqlserver2019container'
          command: []
          env: [
            {
              name: 'ACCEPT_EULA'
              value: 'Y'
            }
            {
              name: 'MSSQL_SA_PASSWORD'
              value: 'Password@12345#'
            }
            {
              name: 'MSSQL_PID'
              value: 'Developer'
            }
          ]
          resources: {
          cpu: json('0.5')
            memory: '1Gi'
          }
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 1
        cooldownPeriod: 300
        pollingInterval: 30
      }
    }
  }
}

// /subscriptions/ae0647c4-c323-493b-8233-99012b801938/resourceGroups/rg-tmp/providers/Microsoft.App/containerapps/ca-tmp
output containerapp_id string = containerapp.id
// ca-tmp
output containerapp_name string = containerapp.name
