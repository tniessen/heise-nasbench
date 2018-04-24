mkdir dist

msbuild /target:Rebuild /property:Configuration=Release;Platform=Win32
mkdir dist\Win32
copy Release\fileperf2.exe dist\Win32\fileperf2.exe
copy nasbench\bin\Release\nasbench.exe dist\Win32\nasbench.exe

msbuild /target:Rebuild /property:Configuration=Release;Platform=x64
mkdir dist\x64
copy x64\Release\fileperf2.exe dist\x64\fileperf2.exe
copy nasbench\bin\Release\nasbench.exe dist\x64\nasbench.exe
