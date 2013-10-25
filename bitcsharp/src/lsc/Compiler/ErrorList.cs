//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace LLVMSharp.Compiler
//{
//    public enum ErrorType
//    {
//        SyntaxError,
//        SymenticError,
//        Warning
//    }

//    public class ErrorInfo
//    {

//        public ErrorType type;
//        public string message;
//        public int line;
//        public int col;
//        public string fileName;

//        public ErrorInfo() { }

//        public ErrorInfo(ErrorType type, string message, int line, int col)
//        {
//            this.type = type;
//            this.message = message;
//            this.line = line;
//            this.col = col;
//        }

//        public ErrorInfo(ErrorType type, string message, int line, int col, string fileName)
//            : this(type, message, line, col)
//        {
//            this.fileName = fileName;
//        }
//    }

//    public delegate void ErrorHandler(object o, ErrorArgs e);

//    public class ErrorArgs : EventArgs
//    {
//        public readonly ErrorInfo errorInfo;
//        public ErrorArgs(ErrorInfo errorInfo)
//        {
//            this.errorInfo = errorInfo;
//        }
//    }

//    public class ErrorList : System.Collections.ArrayList
//    {
//        public override int Add(object value)
//        {
//            if (!(value is ErrorInfo))
//                throw new FatalError("value added to errorlist must be of type ErrorInfo.");
//            int rt = base.Add(value);
//            if (ErrorAdded != null)
//                ErrorAdded(this, new ErrorArgs((ErrorInfo)value));
//            return rt;
//        }

//        public event ErrorHandler ErrorAdded;

//        public ErrorList() { }
//    }

//}
