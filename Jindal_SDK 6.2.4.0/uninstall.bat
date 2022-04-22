echo zkemkeeper.dll version 6
Del %windir%\system32\commpro.dll 
Del %windir%\system32\comms.dll 
Del %windir%\system32\zkemsdk.dll 
Del %windir%\system32\rscagent.dll 
Del %windir%\system32\rscomm.dll 
Del %windir%\system32\zkemkeeper.dll 
Del %windir%\system32\tcpcomm.dll 
Del %windir%\system32\usbcomm.dll 


Del %windir%\SysWow64\commpro.dll 
Del %windir%\SysWow64\comms.dll 
Del %windir%\SysWow64\zkemsdk.dll 
Del %windir%\SysWow64\rscagent.dll 
Del %windir%\SysWow64\rscomm.dll 
Del %windir%\SysWow64\zkemkeeper.dll 
Del %windir%\SysWow64\tcpcomm.dll 
Del %windir%\SysWow64\usbcomm.dll

REGSVR32 %windir%\system32\zkemkeeper.dll -u
REGSVR32 %windir%\SysWow64\zkemkeeper.dll -u

echo set up successful



pause