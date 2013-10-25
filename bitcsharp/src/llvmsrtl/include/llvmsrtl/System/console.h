#ifndef LLVMSRT_SYSTEM_CONSOLE
    #define LLVMSRT_SYSTEM_CONSOLE

    #include <llvmsrtl/types.h>
    #include <llvmsrtl/System/environment.h>

    #if USE_C_STDLIB != 1
        
    #else

        #include <stdio.h>

		void inline __llvmsharp_system_console_writeline(){
			printf((Char*)(((__llvmsharp_stringHeader*)(__llvmsharp_getNewline()))+1));
		}
       
        void inline __llvmsharp_system_console_write_int(int i){
			printf("%d",i);
		}

		void inline __llvmsharp_system_console_write_string(__llvmsharp_stringHeader *strHdr){
			Char* str = (Char*)(((__llvmsharp_stringHeader*)(strHdr))+1);
			printf(str);
		}

        void inline __llvmsharp_system_console_write_single(float f)
		{
			printf("%f",f);
		}		

		__llvmsharp_stringHeader* __llvmsharp_system_console_readline(){
			char line[CHAR_BUFF_SIZE] = "";
			UInt32 i;
			for(i =0; i < CHAR_BUFF_SIZE-1; ++i){
				int ch = fgetc(stdin);
				if(ch == '\n' || ch == EOF)
					break;
				line[i] = ch;
			}
			line[i] = '\0';
			return __llvmsharp_System_String_ctor_charPtr(line);
		}

    #endif
    
#endif
