@echo off & if not "%ECHO%"=="" echo %ECHO%

setlocal
set LOCALDIR=%~dp0

echo Windows Registry Editor Version 5.00 > NSpecTDNet.reg
echo [HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\MutantDesign\TestDriven.NET\TestRunners\NSpec] >> NSpecTDNet.reg
echo "Application"="" >> NSpecTDNet.reg
echo "AssemblyPath"="%LOCALDIR:\=\\%NSpec.TDNetRunner.dll" >> NSpecTDNet.reg
echo "TargetFrameworkAssemblyName"="NSpec" >> NSpecTDNet.reg
echo "TypeName"="NSpec.TDNetRunner.TDNetNSpecRunner" >> NSpecTDNet.reg
echo @="50" >> NSpecTDNet.reg

regedit NSpecTDNet.reg

del NSpecTDNet.reg