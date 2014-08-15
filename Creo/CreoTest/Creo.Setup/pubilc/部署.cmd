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

echo f|xcopy /r /y "Creo.Server.dll" "%PLMPath%\bin\Creo.Server.dll"
if not exist "%PLMPath%\Integration_back.config.xml" (
	echo f|xcopy /r /y "%PLMPath%\Integration.config.xml" "%PLMPath%\Integration_back.config.xml"
)
::copy /y "ZSKIntegration.js" "%PLMPath%\Javascript\Integration\ZSKIntegration.js"

ModifyConfig.exe "%PLMPath%\Integration.config.xml" IntegrationConfiguration/appSettings update PROE "Creo.Server.CreoOperate, Creo.Server"
ModifyConfig.exe "%PLMPath%\web.config" configuration/appSettings add Multi-Configuration true

ModifyConfig.exe "%PLMPath%\Javascript\Integration\3DCadIntegration.js" 3D replace "if ((ot & notcheckout) === notcheckout) return;" "if ((ot & notcheckout)===notcheckout) return;if(o.__filemd5===o.__FileMD5__DB) return;"

::添加openCreo
ModifyConfig.exe "%PLMPath%\Document\Class\Document.js" 3D replace "fileControler.Controler.Execute(LocalFileName);" "if(/prt.\d*$/.test(LocalFileName) || /asm.\d*$/.test(LocalFileName)) openCreo(LocalFileName);else fileControler.Controler.Execute(LocalFileName);"
ModifyConfig.exe "%PLMPath%\Document\Class\Document.js" 3D append "/***function openCreo(filepath)***/" ";function openCreo(filepath){/***function openCreo(filepath)***/ var ws = new ActiveXObject('Wscript.Shell'); var cmd = 'openAssistent.exe \"' + filepath + '\"'; ws.run(cmd);}"

echo 服务器部署完成
echo.
echo.
echo 开始插件部署

set Integration=%PLMPath%\..\Integration\Integration Setup

if exist "%Integration%\KDSetup\Dll\Kingdee.PLM.Integration.Setup.Proe.dll" (
echo f|xcopy /r /y "%Integration%\KDSetup\Dll\Kingdee.PLM.Integration.Setup.Proe.dll" "%Integration%\KDSetup\Kingdee.PLM.Integration.Setup.Proe.dll"
attrib -r "%Integration%\KDSetup\Dll\Kingdee.PLM.Integration.Setup.Proe.dll" && del "%Integration%\KDSetup\Dll\Kingdee.PLM.Integration.Setup.Proe.dll"
)

echo f|xcopy /r /y "Kingdee.PLM.Integration.Setup.Creo.dll" "%Integration%\KDSetup\Dll\Kingdee.PLM.Integration.Setup.Creo.dll"
echo f|xcopy /r /y "Intgration.Common.dll" "%Integration%\KDSetup\Dll\Intgration.Common.dll"
echo f|xcopy /r /y "Kingdee.PLM.Integration.Client.Proe.dll" "%Integration%\Resources\Common\Dll\Kingdee.PLM.Integration.Client.Proe.dll"
echo f|xcopy /r /y "Intgration.Common.dll" "%Integration%\Resources\Common\Dll\Intgration.Common.dll"
echo f|xcopy /r /y "Message.txt" "%Integration%\Resources\Proe\Text\chinese_cn\Message.txt"
echo f|xcopy /r /y "PLM.dll" "%Integration%\Resources\Proe\PLM.dll"
echo f|xcopy /r /y "PLM64.dll" "%Integration%\Resources\Proe\PLM64.dll"
echo f|xcopy /r /y "openAssistent.exe" "%Integration%\Resources\Proe\openAssistent.exe"
echo f|xcopy /r /y "openAssistent64.exe" "%Integration%\Resources\Proe\openAssistent64.exe"

echo 插件部署完成
echo.
echo 在客户端安装插件即可使用
echo.
pause