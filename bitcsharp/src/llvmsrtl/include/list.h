#ifndef LS_LIST
 #define LS_STARTUP

 typedef struct _node NODE;
 typedef struct _list LIST;

 struct _node {
	void* data;
	NODE *next;
 };

 struct _list{
	NODE* head;
 };

 // creates a new list;
 LIST* list_create();

 NODE* list_add_beg(LIST* list,void* data);
 void list_delete_node(LIST** list,NODE** node);

 void list_clear(LIST* list);
 void list_delete(LIST* list);

  LIST* list_create(){
	LIST* newList=(LIST*)malloc(sizeof(LIST));

	newList->head=NULL;

	fprintf(stderr,"created list\n");

	return newList;
 }

 NODE* list_add_beg(LIST* list,void* data){
	NODE* newNode;

	fprintf(stderr,"creating new node\n");

	if(list==NULL){
		fprintf(stderr,"LIST has not been initialized\n");
		return NULL;
	}

	newNode=(NODE*)malloc(sizeof(NODE));

	if(newNode==NULL){
		fprintf(stderr,"couldnt allcoate memory for new node\n");
		return NULL;
	}

	newNode->data = data;

	if(list->head==NULL){ // if list empty
		newNode->next=NULL;
		list->head = newNode;
		fprintf(stderr,"first new node\n");
	}
	else{
		newNode->next=list->head;
		list->head=newNode;
		fprintf(stderr,"new node\n");
	}

	return newNode;
 }

 void list_delete_node(LIST** list,NODE** node){	
	NODE* prev;
	NODE* curr;

	fprintf(stderr,"deleting node\n");

	curr=(*list)->head;

	while(curr!=NULL){
		if(curr==(*node)){
			if(curr==(*list)->head){
				(*list)->head=curr->next;
				free(curr);
				return;
			}else{
				prev->next=curr->next;
				free(curr);
				return;
			}
		}else{
			prev=curr;
			curr=curr->next;
		}
	}

	fprintf(stderr,"deleting node not found\n");

 }

 void list_clear(LIST* list){
	NODE* tmp;

	fprintf(stderr,"clearnig list\n");

	if(list==NULL){
		fprintf(stderr,"LIST has not been initialized\n");
		return;
	}

	while(list->head!=NULL){
		tmp=list->head->next;
		free(list->head);
		list->head=tmp;
	}
	
	fprintf(stderr,"clearing list complete\n");
 }

 void list_delete(LIST* list){
	list_clear(list);
	free(list);
	list=NULL;
 }

#endif