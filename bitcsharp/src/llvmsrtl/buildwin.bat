@echo on

set LLVM=llvm
set CLANG=clang-cl
set INCLUDE=include
set LLVM_DIR=..\..\..\output\
set BASEDIR=llvmsrtl
set INCLUDE_PATH=..\include
set LIB_INCLUDE="c:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\include"



cd %BASEDIR% 
%LLVM_DIR%%CLANG% llvmsrtl.c -o llvmsrtl.exe /showIncludes  /I%INCLUDE_PATH% -D"USE_C_STDLIB=0"
REM %LLVM_DIR%%CLANG% llvmsrtl.c -o llvmsrtl.exe /showIncludes  /I%INCLUDE_PATH% /I%LIB_INCLUDE% 
REM The -o hello.exe is required because clang currently outputs a.out when neither -o nor -c are given.
REM C:\..> llvm-dis < hello.bc | more
REM C:\..> llc -filetype=obj hello.bc
cd ..

pause
