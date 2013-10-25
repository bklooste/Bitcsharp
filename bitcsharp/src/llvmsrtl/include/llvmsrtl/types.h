#ifndef LLVMSRT_TYPES
 #define LLVMSRT_TYPES

 typedef signed char			Int8;
 typedef unsigned char			UInt8;
 typedef short					Int16;
 typedef unsigned short			UInt16;
 typedef int					Int32;
 typedef unsigned int			UInt32;
 
 typedef UInt8					Byte;
 typedef Int8					SByte;

 typedef float					Single;
 typedef double					Double;

 typedef char					Boolean;
 typedef Boolean				Bool;
 #ifndef TRUE
	 #define TRUE 	1
	 #define FALSE 	0
 #endif

 typedef char					Char;

 typedef void					Void;

 typedef struct{
	Int32						length;
 } Array;

 typedef struct{
	//Int32						capacity;
	Int32						length;
 } __llvmsharp_stringHeader;

 	/*-----------------------------------------------
	| String Header | Actual String Conent (char*)  |
	-------------------------------------------------*/

 typedef struct {
	Int32						typeId;
 }__llvmsharp_objectHeader;

 typedef struct __llvmsharp_gc_object{
	Bool 						 mark;		   // mark bit
	Int32						 ptrCount;     // no of gc pointers in object
	Int32						 index; 	   // no of gc pointers that have been followed
	Int32						 size;		   // actual data size
	__llvmsharp_objectHeader	 objectHeader;
	struct __llvmsharp_gc_object *next;
	struct __llvmsharp_gc_object *prev;
 }__llvmsharp_gc_object;
 	
	/*-----------------------------------
	| GC ObjectHeader | Actual Object |
	-----------------------------------
	     ^
	     |
		 |_________________________ GC Header with Object Header*/
	  
  #define CHAR_BUFF_SIZE 256

#endif