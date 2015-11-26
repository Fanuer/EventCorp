#Requires -Version 3.0

<#
.SYNOPSIS
Erstellt einen virtuellen Microsoft Azure-Computer für ein Visual Studio-Webprojekt und stellt diesen bereit.
Eine detailliertere Dokumentation finden Sie unter: http://go.microsoft.com/fwlink/?LinkID=394472 

.EXAMPLE
PS C:\> .\Publish-WebApplicationVM.ps1 `
-Configuration .\Configurations\WebApplication1-VM-dev.json `
-WebDeployPackage ..\WebApplication1\WebApplication1.zip `
-VMPassword @{Name = "admin"; Password = "password"} `
-AllowUntrusted `
-Verbose


#>
[CmdletBinding(HelpUri = 'http://go.microsoft.com/fwlink/?LinkID=391696')]
param
(
    [Parameter(Mandatory = $true)]
    [ValidateScript({Test-Path $_ -PathType Leaf})]
    [String]
    $Configuration,

    [Parameter(Mandatory = $false)]
    [String]
    $SubscriptionName,

    [Parameter(Mandatory = $false)]
    [ValidateScript({Test-Path $_ -PathType Leaf})]
    [String]
    $WebDeployPackage,

    [Parameter(Mandatory = $false)]
    [Switch]
    $AllowUntrusted,

    [Parameter(Mandatory = $false)]
    [ValidateScript( { $_.Contains('Name') -and $_.Contains('Password') } )]
    [Hashtable]
    $VMPassword,

    [Parameter(Mandatory = $false)]
    [ValidateScript({ !($_ | Where-Object { !$_.Contains('Name') -or !$_.Contains('Password')}) })]
    [Hashtable[]]
    $DatabaseServerPassword,

    [Parameter(Mandatory = $false)]
    [Switch]
    $SendHostMessagesToOutput = $false
)


function New-WebDeployPackage
{
    #Funktion zum Entwickeln und Verpacken Ihrer Webanwendung erstellen

    #Verwenden Sie 'MsBuild.exe', um Ihre Webanwendung zu erstellen. Hilfe dazu finden Sie in der MSBuild-Befehlszeilenreferenz unter http://go.microsoft.com/fwlink/?LinkId=391339.
}

function Test-WebApplication
{
    #Diese Funktion bearbeiten, um Komponententests an Ihrer Webanwendung durchzuführen

    #Schreiben Sie eine Funktion zum Ausführen von Komponententests für Ihre Webanwendung (unter Verwendung von 'VSTest.Console.exe'). Hilfe dazu finden Sie in der VSTest.Console-Befehlszeilenreferenz unter http://go.microsoft.com/fwlink/?LinkId=391340.
}

function New-AzureWebApplicationVMEnvironment
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [Object]
        $Configuration,

        [Parameter (Mandatory = $false)]
        [AllowNull()]
        [Hashtable]
        $VMPassword,

        [Parameter (Mandatory = $false)]
        [AllowNull()]
        [Hashtable[]]
        $DatabaseServerPassword
    )
   
    $VMInfo = New-AzureVMEnvironment `
        -CloudServiceConfiguration $Config.cloudService `
        -VMPassword $VMPassword

    # Erstellen Sie die SQL-Datenbanken. Die Verbindungszeichenfolge wird für die Bereitstellung verwendet.
    $connectionString = New-Object -TypeName Hashtable
    
    if ($Config.Contains('databases'))
    {
        @($Config.databases) |
            Where-Object {$_.connectionStringName -ne ''} |
            Add-AzureSQLDatabases -DatabaseServerPassword $DatabaseServerPassword |
            ForEach-Object { $connectionString.Add($_.Name, $_.ConnectionString) }           
    }
    
    return @{ConnectionString = $connectionString; VMInfo = $VMInfo}   
}

function Publish-AzureWebApplicationToVM
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [Object]
        $Config,

        [Parameter(Mandatory = $false)]
        [AllowNull()]
        [Hashtable]
        $ConnectionString,

        [Parameter(Mandatory = $true)]
        [ValidateScript({Test-Path $_ -PathType Leaf})]
        [String]
        $WebDeployPackage,
        
        [Parameter(Mandatory = $false)]
        [AllowNull()]
        [Hashtable]
        $VMInfo           
    )
    $waitingTime = $VMWebDeployWaitTime

    $result = $null
    $attempts = 0
    $allAttempts = 60
    do 
    {
        $result = Publish-WebPackageToVM `
            -VMDnsName $VMInfo.VMUrl `
            -IisWebApplicationName $Config.webDeployParameters.IisWebApplicationName `
            -WebDeployPackage $WebDeployPackage `
            -UserName $VMInfo.UserName `
            -UserPassword $VMInfo.Password `
            -AllowUntrusted:$AllowUntrusted `
            -ConnectionString $ConnectionString
         
        if ($result)
        {
            Write-VerboseWithTime ($scriptName + ' Die Veröffentlichung auf dem virtuellen Computer war erfolgreich.')
        }
        elseif ($VMInfo.IsNewCreatedVM -and !$Config.cloudService.virtualMachine.enableWebDeployExtension)
        {
            Write-VerboseWithTime ($scriptName + ' "enableWebDeployExtension" muss auf $true festgelegt werden.')
        }
        elseif (!$VMInfo.IsNewCreatedVM)
        {
            Write-VerboseWithTime ($scriptName + ' Der vorhandene virtuelle Computer unterstützt Web Deploy nicht.')
        }
        else
        {
            Write-VerboseWithTime ('{0}: Publishing to VM failed. Attempt {1} of {2}.' -f $scriptName, ($attempts + 1), $allAttempts)
            Write-VerboseWithTime ('{0}: Publishing to VM will start after {1} seconds.' -f $scriptName, $waitingTime)
            
            Start-Sleep -Seconds $waitingTime
        }
                                                                                                                       
         $attempts++
    
         #Führen Sie die Veröffentlichung erneut aus, aber nur für den neu erstellten virtuellen Computer mit installiertem Web Deploy. 
    } While( !$result -and $VMInfo.IsNewCreatedVM -and $attempts -lt $allAttempts -and $Config.cloudService.virtualMachine.enableWebDeployExtension)
    
    if (!$result)
    {                    
        Write-Warning ' Fehler bei der Veröffentlichung auf dem virtuellen Computer. Dieser kann durch ein nicht vertrauenswürdiges oder ungültiges Zertifikat verursacht werden. Sie können -AllowUntrusted festlegen, um nicht vertrauenswürdige Zertifikate zu akzeptieren.'
        throw ($scriptName + ' Fehler beim Veröffentlichen auf virtuellem Computer.')
    }
}

# Hauptroutine des Skripts
Set-StrictMode -Version 3
Import-Module Azure

try {
    $AzureToolsUserAgentString = New-Object -TypeName System.Net.Http.Headers.ProductInfoHeaderValue -ArgumentList 'VSAzureTools', '1.5'
    [Microsoft.Azure.Common.Authentication.AzureSession]::ClientFactory.UserAgents.Add($AzureToolsUserAgentString)
} catch {}

Remove-Module AzureVMPublishModule -ErrorAction SilentlyContinue
$scriptDirectory = Split-Path -Parent $PSCmdlet.MyInvocation.MyCommand.Definition
Import-Module ($scriptDirectory + '\AzureVMPublishModule.psm1') -Scope Local -Verbose:$false

New-Variable -Name VMWebDeployWaitTime -Value 30 -Option Constant -Scope Script 
New-Variable -Name AzureWebAppPublishOutput -Value @() -Scope Global -Force
New-Variable -Name SendHostMessagesToOutput -Value $SendHostMessagesToOutput -Scope Global -Force

try
{
    $originalErrorActionPreference = $Global:ErrorActionPreference
    $originalVerbosePreference = $Global:VerbosePreference
    
    if ($PSBoundParameters['Verbose'])
    {
        $Global:VerbosePreference = 'Continue'
    }
    
    $scriptName = $MyInvocation.MyCommand.Name + ':'
    
    Write-VerboseWithTime ($scriptName + ' Starten')
    
    $Global:ErrorActionPreference = 'Stop'
    Write-VerboseWithTime ('{0} $ErrorActionPreference ist auf {1} festgelegt.' -f $scriptName, $ErrorActionPreference)
    
    Write-Debug ('{0}: $PSCmdlet.ParameterSetName = {1}' -f $scriptName, $PSCmdlet.ParameterSetName)

    # Prüfen Sie, ob Sie Azure-Modul Version 0.7.4 oder höher verwenden.
	$validAzureModule = Test-AzureModule

    if (-not ($validAzureModule))
    {
         throw 'Azure PowerShell konnte nicht geladen werden. Wechseln Sie zu "http://go.microsoft.com/fwlink/?LinkID=320552", um die neueste Version zu installieren. Wenn Azure PowerShell bereits installiert ist, müssen Sie den Computer möglicherweise neu starten oder das Modul manuell importieren.'
    }

    # Speichern Sie das aktuelle Abonnement. Es wird später im Skript auf den aktuellen Status zurückgesetzt.
    Backup-Subscription -UserSpecifiedSubscription $SubscriptionName
        
    if ($SubscriptionName)
    {

        # Falls Sie einen Abonnementnamen angegeben haben, prüfen Sie, ob das Abonnement in Ihrem Konto vorhanden ist.
        if (!(Get-AzureSubscription -SubscriptionName $SubscriptionName))
        {
            throw ("{0}: Der Abonnementname $SubscriptionName wurde nicht gefunden." -f $scriptName)

        }

        # Legen Sie für das angegebene Abonnement aktuell fest.
        Select-AzureSubscription -SubscriptionName $SubscriptionName | Out-Null

        Write-VerboseWithTime ('{0}: Das Abonnement ist auf {1} festgelegt.' -f $scriptName, $SubscriptionName)
    }

    $Config = Read-ConfigFile $Configuration -HasWebDeployPackage:([Bool]$WebDeployPackage)

    #Webanwendung entwickeln und verpacken
    New-WebDeployPackage

    #Komponententest für die Webanwendung ausführen
    Test-WebApplication

    #Azure-Umgebung gemäß Beschreibung in der JSON-Konfigurationsdatei erstellen

    $newEnvironmentResult = New-AzureWebApplicationVMEnvironment -Configuration $Config -DatabaseServerPassword $DatabaseServerPassword -VMPassword $VMPassword

    #Webanwendungspaket bereitstellen, wenn $WebDeployPackage vom Benutzer angegeben wird 
    if($WebDeployPackage)
    {
        Publish-AzureWebApplicationToVM `
            -Config $Config `
            -ConnectionString $newEnvironmentResult.ConnectionString `
            -WebDeployPackage $WebDeployPackage `
            -VMInfo $newEnvironmentResult.VMInfo
    }
}
finally
{
    $Global:ErrorActionPreference = $originalErrorActionPreference
    $Global:VerbosePreference = $originalVerbosePreference

    # Das ursprüngliche aktuelle Abonnement auf den aktuellen Status zurücksetzen
	if($validAzureModule)
	{
   	    Restore-Subscription
	}   

    Write-Output $Global:AzureWebAppPublishOutput    
    $Global:AzureWebAppPublishOutput = @()
}
