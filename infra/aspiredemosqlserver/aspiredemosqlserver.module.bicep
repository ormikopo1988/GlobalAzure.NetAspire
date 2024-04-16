targetScope = 'resourceGroup'

@description('')
param location string = resourceGroup().location

@description('')
param principalId string

@description('')
param principalName string


resource sqlServer_AlbGRuv5W 'Microsoft.Sql/servers@2020-11-01-preview' = {
  name: toLower(take('aspiredemosqlserver${uniqueString(resourceGroup().id)}', 24))
  location: location
  tags: {
    'aspire-resource-name': 'aspiredemosqlserver'
  }
  properties: {
    version: '12.0'
    publicNetworkAccess: 'Enabled'
    administrators: {
      administratorType: 'ActiveDirectory'
      login: principalName
      sid: principalId
      tenantId: subscription().tenantId
      azureADOnlyAuthentication: true
    }
  }
}

resource sqlFirewallRule_AKp4wcLSZ 'Microsoft.Sql/servers/firewallRules@2020-11-01-preview' = {
  parent: sqlServer_AlbGRuv5W
  name: 'AllowAllAzureIps'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource sqlDatabase_lOnIWbv5p 'Microsoft.Sql/servers/databases@2020-11-01-preview' = {
  parent: sqlServer_AlbGRuv5W
  name: 'aspiredemodb'
  location: location
  properties: {
  }
}

output sqlServerFqdn string = sqlServer_AlbGRuv5W.properties.fullyQualifiedDomainName
