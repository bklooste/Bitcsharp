#ifndef LLVMSRT_STDLIB
    #define LLVMSRT_STDLIB

    // do stuffs out here        
    
    #if USE_C_STDLIB != 1
    
//        #include <llvmsrtl/stdlib_.h>
        
        char* __llvmsharp_int_to_charp(int i)
        {
               static char a[7];
               char *b = a + sizeof(a) - 1;
               int   sign = (i < 0);

               if (sign)
                  i = -i;
               *b = 0;
               do
               {
                  *--b = '0' + (i % 10);
                  i /= 10;
               }
               while (i);
               if (sign)
                  *--b = '-';
               return b;
        }
        
    #else

        #include <stdlib.h>
       
        void inline   __llvmsharp_exit(int status)
        {
            exit(status);
        }
        
    #endif
    
#endif
