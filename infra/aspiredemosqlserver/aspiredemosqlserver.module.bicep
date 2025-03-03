@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

param principalId string

param principalName string

resource aspiredemosqlserver 'Microsoft.Sql/servers@2021-11-01' = {
  name: take('aspiredemosqlserver-${uniqueString(resourceGroup().id)}', 63)
  location: location
  properties: {
    administrators: {
      administratorType: 'ActiveDirectory'
      login: principalName
      sid: principalId
      tenantId: subscription().tenantId
      azureADOnlyAuthentication: true
    }
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    version: '12.0'
  }
  tags: {
    'aspire-resource-name': 'aspiredemosqlserver'
  }
}

resource sqlFirewallRule_AllowAllAzureIps 'Microsoft.Sql/servers/firewallRules@2021-11-01' = {
  name: 'AllowAllAzureIps'
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
  parent: aspiredemosqlserver
}

resource aspiredemodb 'Microsoft.Sql/servers/databases@2021-11-01' = {
  name: 'aspiredemodb'
  location: location
  parent: aspiredemosqlserver
}

output sqlServerFqdn string = aspiredemosqlserver.properties.fullyQualifiedDomainName