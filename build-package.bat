@echo off
cls
Tools\NAnt\NAnt.exe package -buildfile:ccnet.build -D:CCNetLabel=1.8.2.2 -nologo -logfile:nant-build-package.log.txt %*
echo %time% %date%
pause