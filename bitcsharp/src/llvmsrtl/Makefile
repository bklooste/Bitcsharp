include Makefile.inc

all:
	@echo Building LLVM# runtime Library
	@echo $(CCFLAGS)	
	@echo Using help of standard C library - $(USE_C_STDLIB_CONFIG)
	@make all -i -C llvmsrtl/
	@make all -i -C build/

clean:
	@echo Cleaning files
	@make clean -i -C llvmsrtl/
	@make clean -i -C build/
