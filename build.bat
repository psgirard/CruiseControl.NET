@echo off


cls
Tools\NAnt\NAnt.exe clean build -buildfile:ccnet.build -D:CCNetLabel=1.8.2.2 -D:codemetrics.output.type=HtmlFile -nologo -logfile:nant-build.log.txt %*
echo %time% %date%
pause

