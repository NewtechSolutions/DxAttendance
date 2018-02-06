Copy /y commpro.dll %windir%\system32\
Copy /y comms.dll %windir%\system32\
Copy /y zkemsdk.dll %windir%\system32\
Copy /y rscagent.dll %windir%\system32\
Copy /y rscomm.dll %windir%\system32\
Copy /y zkemkeeper.dll %windir%\system32\
Copy /y tcpcomm.dll %windir%\system32\
Copy /y usbcomm.dll %windir%\system32\


REGSVR32 %windir%\system32\zkemkeeper.dll
echo set up successful


pause