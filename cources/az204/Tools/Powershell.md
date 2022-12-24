##Commands

###Installing Powerhell 7
$PSVersionTable
winget install --id Microsoft.Powershell --source winget
winget search Microsoft.PowerShell
winget install --id Microsoft.Powershell --source winget

##Installing Azure powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
Install-Module -Name Az -Scope CurrentUser -Repository PSGallery -Force -AllowClobber

##Azure powershell
Connect-AzAccount
Install Visual Studio Code
Install Extensions: Powershell, Azure Powershell Tools

