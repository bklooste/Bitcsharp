#ifndef LLVMSRT_STARTUP
    #define LLVMSRT_STARTUP
    
    #include <llvmsrtl/stdlib.h>
	#include <llvmsrtl/gc.h>
    
	/*
    #if USE_C_STDLIB != 1
        
        void inline __attribute__((always_inline)) __ls_main(int argc,char* argv[])
        {
        }

    #else

        // don't do anything coz its all done by crt0 and blah blah blah...               
        void inline __attribute__((always_inline)) __ls_main(int argc,char* argv[])
        {
        }
        
    #endif
	*/

	#if TARGET_TYPE == 0

	#include <stdio.h>

	int __llvmsharp_argc;
	char** __llvmsharp_argv;

	void inline __attribute__((always_inline)) __llvmsharp_init(int argc,char* argv[]){
		// assign argc and argv values
		__llvmsharp_argc=argc;
		__llvmsharp_argv=argv;
		__llvmsharp_gc_init();
	}

	/*
	int main(int argc,char* argv[])
	{
		//llvm_gc_initialize(8*1024*1024);
		//llvm_gc_collect();
		__ls_main(argc,argv);
		return 0;
	}
	*/

	#endif

#endif