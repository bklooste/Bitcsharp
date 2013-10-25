#ifndef LLVMSRT_SYSTEM_ENVIRONMENT
    #define LLVMSRT_SYSTEM_ENVIRONMENT

    #if USE_C_STDLIB != 1
        
    #else
        
    #endif

	#include <stdlib.h>
	#include <llvmsrtl/types.h>

	static __llvmsharp_stringHeader* __llvmsharp_newline = NULL;
	
	__llvmsharp_stringHeader* __llvmsharp_getNewline(void){
		if(__llvmsharp_newline!=NULL)
			return __llvmsharp_newline;
		else{
			#if OSTYPE == windows
				__llvmsharp_newline = __llvmsharp_System_String_ctor_charPtr("\r\n");
			#elif
				__llvmsharp_newline =  __llvmsharp_System_String_ctor_charPtr("\n");
			#endif
			return __llvmsharp_newline;
		}
	}

	void __llvmsharp_system_environment_exit(Int32 exitCode){
		exit(exitCode);
	}
    
#endif
