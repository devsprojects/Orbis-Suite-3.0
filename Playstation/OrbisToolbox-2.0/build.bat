SETLOCAL EnableDelayedExpansion

Rem Libraries to link in
set libraries=-lSceLibcInternal -lSceLibcInternalExt -lkernel -lmonosgen -lSceSystemService -lSceLncUtil -lSceUserService -lSceRegMgr -lSceFreeType -lSceSysCore -lSceSystemStateMgr -lSceNet

Rem Read the script arguments into local vars
set intdir=%1
set targetname=%~2
set outputPath=%~3

set outputElf=%intdir%%targetname%.elf
set outputOelf=%intdir%%targetname%.oelf
set outputPrx=%intdir%%targetname%.prx
set outputStub=%intdir%%targetname%_stub.so

Rem Compile object files for all the source files   -DORBIS_TOOLBOX_DEBUG
for %%f in (*.cpp) do (
    clang++ -cc1 -triple x86_64-scei-ps4-elf -I"%OO_PS4_TOOLCHAIN%\include" -I"%OO_PS4_TOOLCHAIN%\\include\\c++\\v1" -emit-obj -o %intdir%\%%~nf.o %%~nf.cpp
)

Rem Compile object files for all the assembly files
for %%f in (*.s) do (
	clang -m64 -nodefaultlibs -nostdlib --target=x86_64-scei-ps4-elf -c -o %intdir%\%%~nf.o %%~nf.s
)

Rem Get a list of object files for linking
set obj_files=
for %%f in (%intdir%\\*.o) do set obj_files=!obj_files! .\%%f

Rem Link the input ELF
ld.lld -m elf_x86_64 -pie --script "%OO_PS4_TOOLCHAIN%\link.x" --eh-frame-hdr -o "%outputElf%" "-L%OO_PS4_TOOLCHAIN%\lib" %libraries% --verbose "%OO_PS4_TOOLCHAIN%\lib\crtlib.o" %obj_files%

Rem Create stub shared libraries
for %%f in (*.cpp) do (
    clang++ -target x86_64-pc-linux-gnu -ffreestanding -nostdlib -fno-builtin -fPIC -c -I"%OO_PS4_TOOLCHAIN%\include" -I"%OO_PS4_TOOLCHAIN%\\include\\c++\\v1" -o %intdir%\%%~nf.o.stub %%~nf.cpp
)

set stub_obj_files=
for %%f in (%intdir%\\*.o.stub) do set stub_obj_files=!stub_obj_files! .\%%f

clang++ -target x86_64-pc-linux-gnu -shared -fuse-ld=lld -ffreestanding -nostdlib -fno-builtin "-L%OO_PS4_TOOLCHAIN%\lib" %libraries% %stub_obj_files% -o "%outputStub%"

Rem Create the prx
%OO_PS4_TOOLCHAIN%\bin\windows\create-fself.exe -in "%outputElf%" --out "%outputOelf%" --lib "%outputPrx%" --paid 0x3800000000010003

Rem Cleanup
copy "%outputPrx%" "%outputPath%\Playstation\Build\pkg\Orbis Toolbox\%targetname%.sprx"
del "%outputPrx%"
