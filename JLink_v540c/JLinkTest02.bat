start /wait "J-Link Commander" "L:\PRG\JLink\JLinkA8107m0\JLink_v540c\JLink.exe -device A8107M0 -CommandFile CommandWriteFile.jlink" 
ECHO error level is %ERRORLEVEL%
exit