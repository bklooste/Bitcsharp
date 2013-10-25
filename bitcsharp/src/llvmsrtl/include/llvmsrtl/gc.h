#ifndef LLVMSRT_GC
    #define LLVMSRT_GC

	#include <stdlib.h>
	#include <stdio.h>
	#include <string.h>
	#include <llvmsrtl/types.h>

	//#define LOG_GC

	#ifdef LOG_GC
		FILE *fpGc;

		void gc_log(char* str){
			fputs(str,fpGc);
		}

		void gc_logn(){
			fputs("\n", fpGc);
		}

		void gc_logi(int i){
			char str[CHAR_BUFF_SIZE];
			snprintf(str,sizeof(str),"%d",i);
			fputs(str,fpGc);
		}

		void gc_loga(void* adr){
			char str[CHAR_BUFF_SIZE];
			snprintf(str,sizeof(str),"%p",adr);
			fputs(str,fpGc);
		}

	#endif

	typedef struct __llvmsharp_gc_root{
		void** ptrRoot;					
		struct __llvmsharp_gc_root *next;
		struct __llvmsharp_gc_root *prev;
	}__llvmsharp_gc_root;

	void* __llvmsharp_gc_alloc(Int32 size,Int32 ptrCount);
	void  __llvmsharp_gc_free(void* ptr);
	void  __llvmsharp_gc_markRoot(void* ptrRoot);
	__llvmsharp_gc_root* __llvmsharp_gc_markRootAndReturn(void* ptrRoot);
	void __llvmsharp_gc_unmarkRoot(__llvmsharp_gc_root* gcRoot);
	void  __llmvsharp_gc_collect(void);
	void  __llvmsharp_gc_collectWalk(__llvmsharp_gc_object *object);

	void __llvmsharp_gc_init();

	static __llvmsharp_gc_object *__llvmsharp_gc_objects; // linked list of managed objects
	static __llvmsharp_gc_root   *__llvmsharp_gc_roots;	 // linked list of gc roots

	size_t __llvmsharp_gc_maxHeapSize = 8*1024*1024; // 8mb
	size_t __llvmsharp_gc_heapUsed = 0;

	
	void __llvmsharp_gc_init(){
		#ifdef LOG_GC
			if((fpGc=fopen("gc.log", "w"))==NULL) {
			    printf("Cannot GC_LOG_FILE file.\n");
			    exit(1000);
			}
			gc_log("__llvmsharp_gc_init()\n");
			gc_log("__llvmsharp_gc_maxHeapSize = ");
			gc_logi(__llvmsharp_gc_maxHeapSize);
			gc_logn();
		#endif
	}

	// change max heap size
	void __llvmsharp_gc_setMaxHeapSize(size_t maxHeapSize){
		__llvmsharp_gc_maxHeapSize = maxHeapSize;
	}
		
	void* __llvmsharp_gc_alloc(Int32 size, Int32 ptrCount){
		__llvmsharp_gc_object* object;
		size_t sizeRequired = sizeof(__llvmsharp_gc_object) + size;

		#ifdef LOG_GC
			gc_log("gc_alloc = sizeWithHdr:");
			gc_logi(sizeRequired);
			gc_log(", ptrCount:");
			gc_logi(ptrCount);
			gc_log("\n\theap used before alloc: ");
			gc_logi(__llvmsharp_gc_heapUsed);
			gc_logn();
		#endif
		
		if(__llvmsharp_gc_heapUsed + sizeRequired > __llvmsharp_gc_maxHeapSize){
			#ifdef LOG_GC
				gc_log("\theap full. starting auto collect\n");
			#endif
			__llmvsharp_gc_collect();
			if(__llvmsharp_gc_heapUsed + sizeRequired > __llvmsharp_gc_maxHeapSize){
				printf("Couldn't allocate gc object. HEAP full. aborting...");
				#ifdef LOG_GC
				gc_log("\tCouldn't allocate gc object. HEAP full. aborting...\n");
				gc_log("\tCurrent heap size: ");
				gc_logi(__llvmsharp_gc_heapUsed);
				gc_log("\n\tRequested new object size: ");
				gc_logi(sizeRequired);
				gc_log("\n\tFailed by: ");
				gc_logi(__llvmsharp_gc_heapUsed + sizeRequired);
				gc_log(" - ");
				gc_logi(__llvmsharp_gc_maxHeapSize);
				gc_log(" = ");
				gc_logi((__llvmsharp_gc_heapUsed + sizeRequired)-__llvmsharp_gc_maxHeapSize);
				#endif
				exit(1000);
				return NULL;
			}
		}

		object = (__llvmsharp_gc_object*)malloc(sizeRequired); 
		#ifdef LOG_GC
			gc_log("\taddr: ");
			gc_loga(object);
		#endif
		__llvmsharp_gc_heapUsed += sizeRequired;

		if(object == NULL){
			printf("Couldn't allocate gc object. aborting...");
			exit(1000);
			return NULL;
		}

		memset(object, 0, sizeof(__llvmsharp_gc_object) + size);
		
		object->mark = 0;
		object->ptrCount = ptrCount;
		object->index = 0;
		object->size = size;

		// insert object in front of the linked list
		object->next = __llvmsharp_gc_objects;
		object->prev = NULL;
		if(__llvmsharp_gc_objects)
			__llvmsharp_gc_objects->prev = object;
		__llvmsharp_gc_objects = object;
		
		#ifdef LOG_GC
			gc_log("\n\theap used after alloc: ");
			gc_logi(__llvmsharp_gc_heapUsed);
			gc_logn();
		#endif
		return object + 1;
	}

	void  __llvmsharp_gc_free(void* ptr){
		__llvmsharp_gc_object* object=(__llvmsharp_gc_object*)ptr-1; //get the obj header

		// rm obj from managed linked list
		if(object->prev == NULL)
			__llvmsharp_gc_objects = object->next;
		else
			object->prev->next = object->next;

		__llvmsharp_gc_heapUsed -= object->size+ sizeof(__llvmsharp_gc_object);
		free(object);
		object=NULL;
	}

	__llvmsharp_gc_root* __llvmsharp_gc_markRootAndReturn(void* ptrRoot){
		__llvmsharp_gc_root* newRoot=(__llvmsharp_gc_root*)malloc(sizeof(__llvmsharp_gc_root));

		if(newRoot==NULL){
			printf("Couldn't allcoate GC Root object. Aborting...");
			exit(1000);
		}

		memset(newRoot, 0, sizeof(__llvmsharp_gc_root));
		
		newRoot->ptrRoot = (void**)ptrRoot;
		newRoot->next = __llvmsharp_gc_roots;
		newRoot->prev = NULL;

		if(__llvmsharp_gc_roots)
			__llvmsharp_gc_roots->prev = newRoot;
		__llvmsharp_gc_roots = newRoot;
		return newRoot;
	}

	void inline __llvmsharp_gc_markRoot(void* ptrRoot){
		__llvmsharp_gc_markRootAndReturn(ptrRoot);
	}

	void __llvmsharp_gc_unmarkRoot(__llvmsharp_gc_root* gcRoot){
		if(gcRoot == NULL)
			return;

		__llvmsharp_gc_root* tmp;

		if(gcRoot == __llvmsharp_gc_roots){ // if head
			tmp = __llvmsharp_gc_roots;
			__llvmsharp_gc_roots = __llvmsharp_gc_roots->next;
			free(tmp);
			tmp = NULL;
		}else if(gcRoot->next == NULL){ // if tail
			gcRoot->prev->next == NULL;
			free(gcRoot);
			gcRoot = NULL;
		}else{ // if middle
			gcRoot->prev->next = gcRoot->next;
			free(gcRoot);
		}
		//// rm gcRoot from roots linked list
		//if(gcRoot->prev == NULL)
		//	__llvmsharp_gc_roots = gcRoot->next;
		//else
		//	gcRoot->prev->next = gcRoot->next;

		//free(actual);
		//actual=NULL;
	}

	void  __llmvsharp_gc_collect(void){
		__llvmsharp_gc_object *object, *next,*prev;
		__llvmsharp_gc_root *currRoot;

		#ifdef LOG_GC
			gc_log("gc_collect() heapUsedBeforeCollect:");
			gc_logi(__llvmsharp_gc_heapUsed);
			gc_log("\n\t");
		#endif


		// MARK PHASE
		// mark all refrenced objects
		currRoot = __llvmsharp_gc_roots;
		while(currRoot){
			if(*currRoot->ptrRoot){
				object=(__llvmsharp_gc_object*)*currRoot->ptrRoot-1; // back to object header
				__llvmsharp_gc_collectWalk(object);
			}
			currRoot = currRoot->next;
		}

		// SWEEP PHASE
		for(object = __llvmsharp_gc_objects; object; object = next){
		
			#ifdef LOG_GC
				gc_loga(object);
			#endif
			next = object->next;
			if(object->mark == 0){
				#ifdef LOG_GC
					gc_log("~ ");
				#endif
				__llvmsharp_gc_free(object + 1); // pass the actual data not the object header
				
			}
			else{
				object->prev = prev;
				object->mark = 0;
				object->index = 0;
				prev = object;
				#ifdef LOG_GC
					gc_log(" ");
				#endif
			}
		}

		#ifdef LOG_GC
			gc_log("\n\theapUsedAfterCollect:");
			gc_logi(__llvmsharp_gc_heapUsed);
			gc_logn();
		#endif
	}

	void  __llvmsharp_gc_collectWalk(__llvmsharp_gc_object *object){
		__llvmsharp_gc_object **refs, *obj;

		if(object->mark)
			return;
		object->mark = 1;
		object->prev = NULL;

	

		int i;
		while(object){
			refs = (__llvmsharp_gc_object**)(object +1); // point to top of obj
			for(i = object->index; i < object->ptrCount; ++i){
				++object->index;
				if(refs[i] != NULL){
					obj = refs[i]-1;
					if(obj->mark == 0){
						obj->prev = object;
						object = obj;
						object->mark = 1;
						break;
					}
				}
			}
			if(object->index >= object->ptrCount)
				object = object->prev;
		}
	}

#endif