Copy commpro.dll %windir%\sysWOW64\
Copy comms.dll %windir%\sysWOW64\
Copy zkemsdk.dll %windir%\sysWOW64\
Copy rscagent.dll %windir%\sysWOW64\
Copy rscomm.dll %windir%\sysWOW64\
Copy zkemkeeper.dll %windir%\sysWOW64\
Copy tcpcomm.dll %windir%\sysWOW64\
Copy usbcomm.dll %windir%\sysWOW64\


REGSVR32 %windir%\sysWOW64\zkemkeeper.dll
echo set up successful


pause