workflow Set-AzureRmSqlDatabaseTier
{
    param
    (
        # Name of Resource Group
        [parameter(Mandatory=$true)] 
        [string] $ResourceGroupName,

        # Name of the Azure SQL Database server (Ex: bzb98er9bp)
        [parameter(Mandatory=$true)] 
        [string] $SqlServerName,

        # Target Azure SQL Database name 
        [parameter(Mandatory=$true)] 
        [string] $DatabaseName,

        # Desired Azure SQL Database edition {Basic, Standard, Premium}
        [parameter(Mandatory=$true)] 
        [string] $Edition,

        # Desired Pricing Tier {Basic, S0, S1, S2, P1, P2, P3}
        [parameter(Mandatory=$true)] 
        [string] $NewPricingTier
    )

    inlinescript
    {
        $connectionName = "AzureRunAsConnection"
        try
        {
            # Get the connection "AzureRunAsConnection "
            $servicePrincipalConnection=Get-AutomationConnection -Name $connectionName         

            "Logging in to Azure..."
            Add-AzureRmAccount `
                -ServicePrincipal `
                -TenantId $servicePrincipalConnection.TenantId `
                -ApplicationId $servicePrincipalConnection.ApplicationId `
                -CertificateThumbprint $servicePrincipalConnection.CertificateThumbprint 
        }
        catch {
            if (!$servicePrincipalConnection)
            {
                $ErrorMessage = "Connection $connectionName not found."
                throw $ErrorMessage
            } else{
                Write-Error -Message $_.Exception
                throw $_.Exception
            }
        }

        ## Set-AzureRmContext -SubscriptionId $subscriptionId
        ## Set-AzureRmSqlDatabase -DatabaseName $DatabaseName -ServerName $SqlServerName -ResourceGroupName $ResourceGroupName -Edition $Edition -RequestedServiceObjectiveName $NewPricingTier

        Write-Output "Begin vertical scaling ..."
        Set-AzureRmSqlDatabase `
            -DatabaseName $Using:DatabaseName `
            -ServerName $Using:SqlServerName `
            -ResourceGroupName $Using:ResourceGroupName `
            -Edition $Using:Edition `
            -RequestedServiceObjectiveName $Using:NewPricingTier

        # Output final status message
        Write-Output "Scaled the pricing tier of $Using:DatabaseName to $Using:Edition - $Using:NewPricingTier"
        Write-Output "Completed vertical scale"
    }
}