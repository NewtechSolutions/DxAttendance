Copy /y commpro.dll %windir%\sysWOW64\
Copy /y comms.dll %windir%\sysWOW64\
Copy /y zkemsdk.dll %windir%\sysWOW64\
Copy /y rscagent.dll %windir%\sysWOW64\
Copy /y rscomm.dll %windir%\sysWOW64\
Copy /y zkemkeeper.dll %windir%\sysWOW64\
Copy /y tcpcomm.dll %windir%\sysWOW64\
Copy /y usbcomm.dll %windir%\sysWOW64\


REGSVR32 %windir%\sysWOW64\zkemkeeper.dll
echo set up successful


pause