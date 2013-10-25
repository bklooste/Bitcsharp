; ModuleID = 'a.cpp'
target datalayout = "e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:32:64-f32:32:32-f64:32:64-v64:64:64-v128:128:128-a0:0:64-f80:32:32"
target triple = "i386-mingw32"
	%struct.Test = type { i8, i32, i8* }
@_LSEi32_ColorBlue = external constant i32		; <i32*> [#uses=1]

define i32 @main() nounwind {
entry:
	%retval = alloca i32		; <i32*> [#uses=2]
	%t = alloca %struct.Test		; <%struct.Test*> [#uses=4]
	%0 = alloca i32		; <i32*> [#uses=2]
	%"alloca point" = bitcast i32 0 to i32		; <i32> [#uses=0]
	%1 = getelementptr %struct.Test* %t, i32 0, i32 1		; <i32*> [#uses=1]
	store i32 10, i32* %1, align 4
	call void @_ZN4Test5printEv(%struct.Test* %t) nounwind
	%2 = getelementptr %struct.Test* %t, i32 0, i32 1		; <i32*> [#uses=1]
	store i32 3, i32* %2, align 4
	call void @_ZN4Test2x2Ev(%struct.Test* %t) nounwind
	%3 = load i32* @_LSEi32_ColorBlue, align 4		; <i32> [#uses=1]
	store i32 %3, i32* %0, align 4
	%4 = load i32* %0, align 4		; <i32> [#uses=1]
	store i32 %4, i32* %retval, align 4
	br label %return

return:		; preds = %entry
	%retval1 = load i32* %retval		; <i32> [#uses=1]
	ret i32 %retval1
}

define linkonce void @_ZN4Test5printEv(%struct.Test* %this) nounwind {
entry:
	%this_addr = alloca %struct.Test*		; <%struct.Test**> [#uses=1]
	%"alloca point" = bitcast i32 0 to i32		; <i32> [#uses=0]
	store %struct.Test* %this, %struct.Test** %this_addr
	br label %return

return:		; preds = %entry
	ret void
}

define linkonce void @_ZN4Test2x2Ev(%struct.Test* %this) nounwind {
entry:
	%this_addr = alloca %struct.Test*		; <%struct.Test**> [#uses=1]
	%"alloca point" = bitcast i32 0 to i32		; <i32> [#uses=0]
	store %struct.Test* %this, %struct.Test** %this_addr
	br label %return

return:		; preds = %entry
	ret void
}
