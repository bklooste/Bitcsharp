#ifndef LLVMSRT_SYSTEM_STRING
 #define LLVMSRT_SYSTEM_STRING

 #include <llvmsrtl/types.h>

 #include <stdlib.h>
 #include <string.h>

 #define __LS_StringToBuffer(strHdr) (Char*)(((__llvmsharp_stringHeader*)(strHdr))+1)

 static __llvmsharp_stringHeader* __llvmsharp_System_String_AllocString(Int32 length);
 void __llvmsharp_String_freeIfRequired(__llvmsharp_stringHeader* strHdr);
 __llvmsharp_stringHeader* __llvmsharp_System_String_ctor_charPtr(Char *value);
 __llvmsharp_stringHeader* __llvmsharp_System_String_newString(__llvmsharp_stringHeader* stdHdr);
  __llvmsharp_stringHeader* __llvmsharp_System_String_concat(__llvmsharp_stringHeader* stdHdr1,__llvmsharp_stringHeader* stdHdr2);
 Int32 __llvmsharp_System_String_getHashCode(__llvmsharp_stringHeader* strHdr);
 Char __llvmsharp_System_String_getChar(__llvmsharp_stringHeader* strHdr,Int32 pos);
 Int32 __llvmsharp_System_String_getLength(__llvmsharp_stringHeader* strHdr);
 Bool __llvmsharp_String_compare(__llvmsharp_stringHeader* strHdr1,__llvmsharp_stringHeader* strHdr2);
 
 static __llvmsharp_stringHeader* __llvmsharp_System_String_AllocString(Int32 length){
	__llvmsharp_stringHeader* str;

	str = (__llvmsharp_stringHeader*)malloc(sizeof(__llvmsharp_stringHeader) + length);

	if(str){
		//str->capacity=length;
		str->length=length;
		return str;
	}else{
		exit(-1000);
		return 0;
	}
 }

 void __llvmsharp_String_freeIfRequired(__llvmsharp_stringHeader* strHdr){
	if(strHdr==NULL)
		return;
	free(strHdr);
	strHdr=NULL;
 }

 __llvmsharp_stringHeader* __llvmsharp_System_String_ctor_charPtr(Char *value){
	__llvmsharp_stringHeader* str;
	Int32 length=strlen(value);
	
	str = __llvmsharp_System_String_AllocString(length);

	strcpy(__LS_StringToBuffer(str), value);

	return str;
 }

 __llvmsharp_stringHeader* __llvmsharp_System_String_newString(__llvmsharp_stringHeader* stdHdr){
	return __llvmsharp_System_String_ctor_charPtr(__LS_StringToBuffer(stdHdr));
 }

 __llvmsharp_stringHeader* __llvmsharp_System_String_concat(__llvmsharp_stringHeader* strHdr1, __llvmsharp_stringHeader* strHdr2){
	int totalLength = strHdr1->length + strHdr2->length - 1;
	char tmp[totalLength];
	strcpy(tmp,__LS_StringToBuffer(strHdr1));
	strcat(tmp,__LS_StringToBuffer(strHdr2));
	return __llvmsharp_System_String_ctor_charPtr(tmp);
 }

 Int32 __llvmsharp_System_String_getHashCode(__llvmsharp_stringHeader* strHdr){
	Int32 hash=0;
	Char* buff = __LS_StringToBuffer(strHdr);
	Int32 len = strHdr->length;
	
	int i;
	for(i=0;i<len;++i)
		hash=(hash<<5)+hash+(Int32)(*buff++);
	return hash;
 }

 Char __llvmsharp_System_String_getChar(__llvmsharp_stringHeader* strHdr,Int32 pos){
	if(pos >=0 && pos < strHdr->length){
		Char* str = __LS_StringToBuffer(strHdr);
		return str[pos];
	}
	else
		exit(1000);
 }

 Int32 __llvmsharp_String_getLength(__llvmsharp_stringHeader* strHdr){
	return strHdr->length;
 }
    
 Bool __llvmsharp_String_compare(__llvmsharp_stringHeader* strHdr1,__llvmsharp_stringHeader* strHdr2){
	if(strHdr1->length != strHdr2->length)
		return FALSE;
	else
		return strcmp(__LS_StringToBuffer(strHdr1),__LS_StringToBuffer(strHdr2)) == 0;
 }

#endif
