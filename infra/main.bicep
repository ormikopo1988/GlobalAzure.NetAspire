targetScope = 'subscription'

@minLength(1)
@maxLength(64)
@description('Name of the environment that can be used as part of naming resource convention, the name of the resource group for your application will use this name, prefixed with rg-')
param environmentName string

@minLength(1)
@description('The location used for all deployed resources')
param location string

@description('Id of the user to assign application roles')
param userPrincipalId string


var tags = {
  'azd-env-name': environmentName
}

resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'rg-${environmentName}'
  location: location
  tags: tags
}

module resources 'resources.bicep' = {
  scope: rg
  name: 'resources'
  params: {
    location: location
    tags: tags
    userPrincipalId: userPrincipalId
  }
}

module aspiredemoapplicationinsights 'aspiredemoapplicationinsights/aspire.hosting.azure.bicep.appinsights.bicep' = {
  name: 'aspiredemoapplicationinsights'
  scope: rg
  params: {
    location: location
    appInsightsName: 'aspiredemoapplicationinsights'
    logAnalyticsWorkspaceId: resources.outputs.AZURE_LOG_ANALYTICS_WORKSPACE_ID
  }
}
module aspiredemosqlserver 'aspiredemosqlserver/aspire.hosting.azure.bicep.sql.bicep' = {
  name: 'aspiredemosqlserver'
  scope: rg
  params: {
    location: location
    databases: ['aspiredemodb']
    principalId: resources.outputs.MANAGED_IDENTITY_PRINCIPAL_ID
    principalName: resources.outputs.MANAGED_IDENTITY_NAME
    serverName: 'aspiredemosqlserver'
  }
}
module cache 'cache/aspire.hosting.azure.bicep.redis.bicep' = {
  name: 'cache'
  scope: rg
  params: {
    location: location
    keyVaultName: resources.outputs.SERVICE_BINDING_KV265DAFE5_NAME
    redisCacheName: 'cache'
  }
}
output MANAGED_IDENTITY_CLIENT_ID string = resources.outputs.MANAGED_IDENTITY_CLIENT_ID
output MANAGED_IDENTITY_NAME string = resources.outputs.MANAGED_IDENTITY_NAME
output AZURE_LOG_ANALYTICS_WORKSPACE_NAME string = resources.outputs.AZURE_LOG_ANALYTICS_WORKSPACE_NAME
output AZURE_CONTAINER_REGISTRY_ENDPOINT string = resources.outputs.AZURE_CONTAINER_REGISTRY_ENDPOINT
output AZURE_CONTAINER_REGISTRY_MANAGED_IDENTITY_ID string = resources.outputs.AZURE_CONTAINER_REGISTRY_MANAGED_IDENTITY_ID
output AZURE_CONTAINER_APPS_ENVIRONMENT_ID string = resources.outputs.AZURE_CONTAINER_APPS_ENVIRONMENT_ID
output AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN string = resources.outputs.AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN
output SERVICE_BINDING_KV265DAFE5_ENDPOINT string = resources.outputs.SERVICE_BINDING_KV265DAFE5_ENDPOINT

output ASPIREDEMOAPPLICATIONINSIGHTS_APPINSIGHTSCONNECTIONSTRING string = aspiredemoapplicationinsights.outputs.appInsightsConnectionString
output ASPIREDEMOSQLSERVER_SQLSERVERFQDN string = aspiredemosqlserver.outputs.sqlServerFqdn