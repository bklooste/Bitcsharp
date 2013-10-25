include ../makefile.inc

IncludeDirs = -I../include

all: llvmsrtl.s
    	

llvmsrtl.s:
	@echo "   Creating llvm assembly code"
	@$(CC) -emit-llvm -S *.c $(CCFlags) $(IncludeDirs)
	@mv llvmsrtl.s ../build/llvmsrtl.ll

clean:
