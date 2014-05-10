@echo off

rem 获取插件安装路径
rem HKEY_LOCAL_MACHINE\SOFTWARE\Kingdee\金蝶 K3 WISE PLM客户端组件集
rem Path : c:\Program Files\Kingdee\K3PLM\Integration

rem web站点安装目录
rem HKEY_LOCAL_MACHINE\SOFTWARE\Kingdee\KDMIDDLEWARE\K3PLM
rem 64位
rem HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Kingdee\KDMIDDLEWARE\K3PLM
rem Path : D:\Program Files\kingdee\K3PLM\web

setlocal 

echo 开始服务器部署
if /i "%PROCESSOR_IDENTIFIER:~0,3%" == "X86" ( 
goto x86
) else (
goto x64
)

:x86
for /f "tokens=1,2,* " %%i in ('REG QUERY "HKEY_LOCAL_MACHINE\SOFTWARE\Kingdee\KDMIDDLEWARE\K3PLM" ^| find /i "Path"') do set "PLMPath=%%k"
goto copyoperate

:x64
for /f "tokens=1,2,* " %%i in ('REG QUERY "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Kingdee\KDMIDDLEWARE\K3PLM" ^| find /i "Path"') do set "PLMPath=%%k"
goto copyoperate

:copyoperate

copy /y "Creo.Server.dll" "%PLMPath%\bin\Creo.Server.dll"
if not exist "%PLMPath%\Integration_back.config.xml" (
	copy /y "%PLMPath%\Integration.config.xml" "%PLMPath%\Integration_back.config.xml"
)
copy /y "ZSKIntegration.js" "%PLMPath%\Javascript\Integration\ZSKIntegration.js"

ModifyConfig.exe "%PLMPath%\Integration.config.xml" IntegrationConfiguration/appSettings update PROE "Creo.Server.CreoOperate, Creo.Server"

::ModifyConfig.exe "%PLMPath%\Document\ElectronIntegration.aspx" IntegrationConfiguration/appSettings replace "var isimport = false;" "if(!bomtips()) {return false;};var isimport = false;"
::ModifyConfig.exe "%PLMPath%\Document\Class\Document.js" IntegrationConfiguration/appSettings append "/***DocumentDownload-SetFileWriteAttributes(LocalFileName)***/" ";function DocumentDownload(FtpFileName, LocalFileName) {if (!CheckFtp()) return false;if (FtpFileName == \"\" || LocalFileName == \"\") return false;result = fileControler.Controler.DownloadFile(FtpFileName, LocalFileName);/***DocumentDownload-SetFileWriteAttributes(LocalFileName)***/SetFileWriteAttributes(LocalFileName);}"

echo 服务器部署完成
echo.
echo.
echo 开始插件部署

set Integration=%PLMPath%\..\Integration\Integration Setup

::copy /y "Kingdee.PLM.Integration.Setup.Zuken.dll" "%Integration%\KDSetup\Dll\Kingdee.PLM.Integration.Setup.Zuken.dll"
copy /y "Kingdee.PLM.Integration.Client.Proe.dll" "%Integration%\Resources\Common\Dll\Kingdee.PLM.Integration.Client.Proe.dll"
copy /y "Intgration.Common.dll" "%Integration%\Resources\Common\Dll\Intgration.Common.dll"


echo 插件部署完成
echo.
echo 在客户端安装插件即可使用
echo.
pause