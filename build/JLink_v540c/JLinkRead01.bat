ECHO 
start /wait "J-Link Commander" "JLink.exe" "-device A8107M0 -CommandFile CommandFile.jlink"
ECHO error level is %ERRORLEVEL%
PAUSE