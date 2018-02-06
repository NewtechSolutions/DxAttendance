echo zkemkeeper.dll version 6
Del /y commpro.dll %windir%\system32\
Del /y comms.dll %windir%\system32\
Del /y zkemsdk.dll %windir%\system32\
Del /y rscagent.dll %windir%\system32\
Del /y rscomm.dll %windir%\system32\
Del /y zkemkeeper.dll %windir%\system32\
Del /y tcpcomm.dll %windir%\system32\
Del /y usbcomm.dll %windir%\system32\


REGSVR32 %windir%\system32\zkemkeeper.dll -u
echo set up successful


pause