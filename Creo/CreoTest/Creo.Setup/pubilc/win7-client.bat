@echo off

ver|find " 5." > nul && goto xpOrwin2003
ver|find " 6." > nul && goto vistaOrwin7

:xpOrwin2003
  echo "xp or win2003"
  call :operate "C:\Windows\System32\LoginSetting.ini"
  call :operate "%ALLUSERSPROFILE%\Application Data\PLM\LoginSetting.ini"
  call :operate "C:\Windows\System32\proegm.txt"
  call :operate "C:\Windows\System32\Login.Html"

goto end

:vistaOrwin7

if /i "%PROCESSOR_IDENTIFIER:~0,3%" == "X86" ( 
  echo "win7 32"
) else (
  echo "win7 64"
  call :operate "C:\Windows\SysWow64\LoginSetting.ini"
  call :operate "C:\Windows\SysWow64\proegm.txt"
  call :operate "C:\Windows\SysWow64\Login.Html"
)

  call :operate "C:\Windows\System32\LoginSetting.ini"
  call :operate "%ALLUSERSPROFILE%\PLM\LoginSetting.ini"
  call :operate "C:\Windows\System32\proegm.txt"
  call :operate "C:\Windows\System32\Login.Html"

goto end

:end
echo Ö´ÐÐÍê³É¡£¡£¡£
pause>nul
exit

:operate
SETLOCAL

  if not exist %1 (
    echo.> %1
  )
  cacls %1 /e /t /c /g everyone:f

SETLOCAL
goto :eof