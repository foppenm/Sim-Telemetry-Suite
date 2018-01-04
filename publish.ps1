cls

$configuration = "Release"
$publishUrl = "$PSScriptRoot\_publish"

$msbuildCommand = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe"

. $msbuildCommand .\Bridge\rF2\Bridge.rF2.vcxproj /p:configuration=$configuration /p:platform=win32
. $msbuildCommand .\Bridge\rF2\Bridge.rF2.vcxproj /p:configuration=$configuration /p:platform=x64
New-Item -ItemType Directory -Path "$publishUrl\Bridge" -Force -Verbose
Copy-Item -Path ".\Bridge\**\bin\**\$configuration\*.dll" -Destination "$publishUrl\Bridge" -Force -Recurse -Verbose

dotnet publish .\Receiver\Receiver\Receiver.csproj -c $configuration -o "$publishUrl\Receiver"

dotnet msbuild .\dashboard\Dashboard.sln `
    /p:Configuration=$configuration `
    /p:DeployOnBuild=True `
    /p:DeployDefaultTarget=WebPublish `
    /p:WebPublishMethod=FileSystem `
    /p:DeleteExistingFiles=True `
    /p:publishUrl=$publishUrl\Dashboard
