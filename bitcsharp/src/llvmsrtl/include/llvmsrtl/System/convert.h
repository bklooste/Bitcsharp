#ifndef LLVMSRT_SYSTEM_CONVERT
    #define LLVMSRT_SYSTEM_CONVERT

    #include <llvmsrtl/types.h>

    #if USE_C_STDLIB != 1
        
    #else

        #include <stdio.h>

		Int32 inline __llvmsharp_system_convert_string_to_int32(__llvmsharp_stringHeader* strHdr){
			Char* strInt = (Char*)(((__llvmsharp_stringHeader*)(strHdr))+1);
			Int32 i32= 0;
			if(EOF == sscanf(strInt,"%d", &i32))
			{
				printf("Couldn't convert %s to System.Int32",strInt);
				exit(1);
			}
			return i32;
		}

		Single inline __llvmsharp_system_convert_string_to_single(__llvmsharp_stringHeader* strHdr){
			Char* strSingle = (Char*)(((__llvmsharp_stringHeader*)(strHdr))+1);
			Single single = 0;
			if(EOF == sscanf(strSingle,"%f", &single))
			{
				printf("Couldn't convert %s to System.Single", strSingle);
				exit(1);
			}
			return single;
		}

		__llvmsharp_stringHeader* __llvmsharp_system_convert_int_to_string(Int32 value){
			char str[CHAR_BUFF_SIZE];
			snprintf(str,sizeof(str),"%d",value);
			return __llvmsharp_System_String_ctor_charPtr(str);
		}

		__llvmsharp_stringHeader* __llvmsharp_system_convert_single_to_string(Single value){
			char str[CHAR_BUFF_SIZE];
			snprintf(str,sizeof(str),"%f",value);
			return __llvmsharp_System_String_ctor_charPtr(str);
		}
		       
    #endif
    
#endif
