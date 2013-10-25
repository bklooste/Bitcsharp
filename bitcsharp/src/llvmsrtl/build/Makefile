include ../makefile.inc

IncludeDirs = -I../include

all: libllvmsrtl.a

libllvmsrtl.a: llvmsrtl.bc
	llvm-ar rcs libllvmsrtl.a llvmsrtl.bc

llvmsrtl.bc:
	llvm-as llvmsrtl.ll
	cp llvmsrtl.bc ../../../output
	cp llvmsrtl.bc ../../llvm-test

clean:
	@echo Removing Library - llvmsrtl.a
	@rm *.bc *.ll *.a
