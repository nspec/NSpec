@echo off & if not "%ECHO%"=="" echo %ECHO%

setlocal
set LOCALDIR=%~dp0

echo Windows Registry Editor Version 5.00 > NSpecTDNet.reg
echo [HKEY_CURRENT_USER\Software\MutantDesign\TestDriven.NET\TestRunners\NSpec] >> NSpecTDNet.reg
echo "Application"="" >> NSpecTDNet.reg
echo "AssemblyPath"="%LOCALDIR:\=\\%NSpec.TDNetRunner.dll" >> NSpecTDNet.reg
echo "TargetFrameworkAssemblyName"="NSpec" >> NSpecTDNet.reg
echo "TypeName"="NSpec.TDNetRunner.TDNetNSpecRunner" >> NSpecTDNet.reg
echo @="5" >> NSpecTDNet.reg

regedit NSpecTDNet.reg

del NSpecTDNet.reg