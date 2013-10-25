using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using LLVMSharp.Compiler.Ast.Types;
using LLVMSharp.Compiler.Semantic;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstClass : AstType
    {
        public Hashtable methodTable = new Hashtable();
        public AstParentClass AstParentClass = null;

        public AstChildClassCollection AstChildClassCollection;

        public int PtrCount; // required for cgen

        // nested class or struct or enum
        public AstTypeCollection AstTypeCollection = new AstTypeCollection();
        public AstConstructorCollection AstConstructorCollection = new AstConstructorCollection();
        public AstFieldCollection AstFieldCollection = new AstFieldCollection();
        public AstMethodCollection AstMethodCollection = new AstMethodCollection();
        public AstAccessorCollection AstAccessorCollection = new AstAccessorCollection();
        public AstTypeConverterCollection AstTypeConverterCollection = new AstTypeConverterCollection();
        public AstOperatorOverloadCollection AstOperatorOverloadCollection = new AstOperatorOverloadCollection();

        public ArrayList Parents = new ArrayList();

        public Hashtable ObjectLayout = new Hashtable();
        public int SizeOfObj;

        public AstField LookupObjectLayout(string fieldName, bool isInheritedFromParent)
        {
            string toFind = Mangler.Instance.MangleField(fieldName, isInheritedFromParent);
            if (!ObjectLayout.ContainsKey(toFind))
                return null;
            return (AstField)ObjectLayout[toFind];
        }

        public AstField LookupObjectLayout(string fieldName)
        {
            bool tmp;
            return LookupObjectLayout(fieldName, out tmp);
        }

        public AstField LookupObjectLayout(string fieldName, out bool isInheritedFromParent)
        {
            AstField field = LookupObjectLayout(fieldName, false);
            isInheritedFromParent = false;

            if (field == null)
            {
                field = LookupObjectLayout(fieldName, true);
                if (field != null) isInheritedFromParent = true;
            }

            return field;
        }

        public AstClass(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstClass(IParser parser) : base(parser) { }
        public AstClass(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstClass--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Class Name: {4}{0}");
            sb.Append("Full Class Name:{5}{0}");

            if (AstParentClass != null)
            {
                sb.Append("{0}Parent Class: {6}{0}");
                sb.Append("Full Parent Class Name: {7}");

                return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                base.Name, FullQualifiedName, AstParentClass.Name, AstParentClass.FullQualifiedName);
            }
            else
            {
                return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                base.Name, FullQualifiedName);
            }

        }

        AstMemberModifierCollection _astMemberModifierCollection = new AstMemberModifierCollection();
        public override AstMemberModifierCollection AstMemberModifierCollection
        {
            get { return _astMemberModifierCollection; }
            set { _astMemberModifierCollection = value; }
        }

        public override void CheckEntryPoint(LLVMSharpCompiler compiler)
        {
            compiler.CheckEntryPoint(methodTable, this);
        }

        public override void Walk(Walker walker)
        {
            walker.Walk(this);
        }


        /*Method Table Functions*/
        public void GenerateMethodTable(LLVMSharpCompiler Compiler)
        {

            Mangler mangleConstruct;
            string mangledName; ErrorList _errors = new ErrorList();
            mangleConstruct = new Mangler(); ErrorInfo e = new ErrorInfo();
            #region Checks on this
            foreach (AstMethod method in AstMethodCollection)
            {
                if (!method.AstMemberModifierCollection.Validate(Compiler.Errors))
                { continue; }


                #region Checks on methods

                if (method.AstMemberModifierCollection.IsStatic && method.AstMemberModifierCollection.IsVirtual ||
                   method.AstMemberModifierCollection.IsStatic && method.AstMemberModifierCollection.IsOverriden)
                {
                    e.col = method.ColumnNumber;
                    e.fileName = method.Path;
                    e.line = method.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "Static method '" + method.Name + "' in class '" + this.FullQualifiedName + "' cannot be marked 'virtual' or 'override'";
                    Compiler.Errors.Add(e);
                }
                if (method.AstMemberModifierCollection.IsVirtual && this.AstTypeModifierCollection.IsSealed)
                {
                    e.col = method.ColumnNumber;
                    e.fileName = method.Path;
                    e.line = method.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "Virtual member '" + method.Name + "' in a sealed class is inconsistent";
                    Compiler.Errors.Add(e);
                }
                if (!method.AstMemberModifierCollection.IsExtern && method.AstBlock == null)
                {
                    e.col = method.ColumnNumber;
                    e.fileName = method.Path;
                    e.line = method.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "In class "+this.FullQualifiedName+": Method " + method.Name + " must declare a body because it is not marked abstract or extern";
                    Compiler.Errors.Add(e);
                }
                if (method.AstMemberModifierCollection.IsVirtual)
                {
                    if (method.AstMemberModifierCollection.IsPrivate)
                    {
                        e.col = method.ColumnNumber;
                        e.fileName = method.Path;
                        e.line = method.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = this.FullQualifiedName + "." + method.Name + "() : virtual members cannot be private";
                        Compiler.Errors.Add(e);
                    }
                    if (method.AstMemberModifierCollection.IsOverriden)
                    {
                        e.col = method.ColumnNumber;
                        e.fileName = method.Path;
                        e.line = method.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "In class "+this.FullQualifiedName+": A Method " + method.Name + " marked as override cannot be marked as new or virtual";
                        Compiler.Errors.Add(e);
                    }
                }
                if (method.AstMemberModifierCollection.IsExtern && method.AstBlock != null)
                {
                    e.col = method.ColumnNumber;
                    e.fileName = method.Path;
                    e.line = method.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "Method " + this.FullQualifiedName + "." + method.Name + "() cannot be extern and declare a body";
                    Compiler.Errors.Add(e);
                }

                #endregion



                mangledName = mangleConstruct.MangleMethod(method, false);

                if (methodTable.Contains(mangledName))
                {
                    e.col = method.ColumnNumber;
                    e.fileName = method.Path;
                    e.line = method.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "Class "+this.FullQualifiedName+ " already defines a method " + method.Name + " with the same parameter types";
                    Compiler.Errors.Add(e);
                }
                else
                {
                    methodTable.Add(mangledName, method);
                    method.key = mangledName;
                    method.KeyTypeInfo = this.FullQualifiedName;
                }

                if (method.AstMemberModifierCollection.IsExtern)
                {
                    mangledName = "_mt_" + method.Name;
                    if (!Compiler.GlobalsTable.Contains(mangledName))
                    {
                        Compiler.GlobalsTable.Add(mangledName, method);                        
                    }
                }

            }
            foreach (AstConstructor construct in AstConstructorCollection)
            {
                if (!construct.AstMemberModifierCollection.Validate(Compiler.Errors))
                { continue; }

                #region Checks on static constructors
                if (construct.AstMemberModifierCollection.IsStatic)
                {
                    if (construct.AstConstructorCall != null)
                    {
                        e.col = construct.ColumnNumber;
                        e.line = construct.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.fileName = construct.Path;
                        e.message = construct.FullQName + ": Static constructor cannot have an explicit this or base constructor call ";
                        Compiler.Errors.Add(e);
                    }
                    if (construct.AstMemberModifierCollection.IsPublic || construct.AstMemberModifierCollection.IsProtected
                       || construct.AstMemberModifierCollection.IsPrivate)
                    {
                        e.col = construct.ColumnNumber;
                        e.line = construct.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.fileName = construct.Path;
                        e.message = construct.FullQName + ": Access modifiers are not allowed on static constructors ";
                        Compiler.Errors.Add(e);
                    }
                    if (construct.Parameters.Count > 0)
                    {
                        e.col = construct.ColumnNumber;
                        e.line = construct.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.fileName = construct.Path;
                        e.message = construct.FullQName + ": A static constructor must be parameterless";
                        Compiler.Errors.Add(e);
                    }
                }
                #endregion

                mangledName = mangleConstruct.MangleMethod(construct.FullQName, construct, false);

                if (methodTable.Contains(mangledName))
                {
                    e.col = construct.ColumnNumber;
                    e.line = construct.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.fileName = construct.Path;
                    e.message = "Constructor already exists for " + construct.Name + ", Duplicate constructor detected";
                    Compiler.Errors.Add(e);
                }
                if (construct.Name != this.Name)
                {
                    e.col = construct.ColumnNumber;
                    e.fileName = construct.Path;
                    e.line = construct.LineNumber;
                    e.message = construct.Name + ": Constructor can only be of enclosed type";
                    e.type = ErrorType.SymenticError;
                    Compiler.Errors.Add(e);
                }
                else
                {
                    methodTable.Add(mangledName, construct);
                    construct.Key = mangledName;
                    construct.KeyTypeInfo = this.FullQualifiedName;
                }
            }
            foreach (AstAccessor accessor in AstAccessorCollection)
            {
                if (!accessor.AstMemberModifierCollection.Validate(Compiler.Errors))
                { continue; }

                mangledName = mangleConstruct.MangleAccessor(accessor, false);

                if (methodTable.Contains(mangledName))
                {
                    e.col = accessor.ColumnNumber;
                    e.fileName = accessor.Path;
                    e.line = accessor.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "Accessor for " + accessor.Name + " already exists, duplicate detected";
                    Compiler.Errors.Add(e);
                }
                else
                {
                    methodTable.Add(mangledName, accessor);
                    accessor.Key = mangledName;
                    accessor.KeyTypeInfo = this.FullQualifiedName;
                }
            }
            foreach (AstOperatorOverload astOperatorOverload in this.AstOperatorOverloadCollection)
            {//add mangling of parameters
                if (!astOperatorOverload.AstMemberModifierCollection.Validate(Compiler.Errors))
                { continue; }
                astOperatorOverload.AstMemberModifierCollection.ValidateOpOverload(Compiler.Errors);

                mangledName = mangleConstruct.MangleOpOverload(astOperatorOverload);
                if (methodTable.Contains(mangledName))
                {
                    e.col = astOperatorOverload.ColumnNumber;
                    e.fileName = astOperatorOverload.Path;
                    e.line = astOperatorOverload.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "Type '" + this.FullQualifiedName + "' already defines an overloaded operator for '" + AstOperatorOverload.ToString(astOperatorOverload.OverloadableOperand) + "' ";
                    Compiler.Errors.Add(e);
                }
                else
                {
                    methodTable.Add(mangledName, astOperatorOverload);
                    astOperatorOverload.Key = mangledName;
                    astOperatorOverload.KeyTypeInfo = this.FullQualifiedName;
                }
            }
            foreach (AstTypeConverter astTypeConverter in this.AstTypeConverterCollection)
            {
                if (!astTypeConverter.AstMemberModifierCollection.Validate(Compiler.Errors))
                { continue; }
                astTypeConverter.AstMemberModifierCollection.ValidateOpOverload(Compiler.Errors);

                string mangle1 = Mangler.Instance.MangleTypeConvert(true, astTypeConverter.ReturnType, astTypeConverter.AstParameter.FullQualifiedType);
                string mangle2 = Mangler.Instance.MangleTypeConvert(false, astTypeConverter.ReturnType, astTypeConverter.AstParameter.FullQualifiedType);
                mangledName = mangleConstruct.MangleTypeConvert(astTypeConverter);
                string temp = "";
                if (astTypeConverter is AstExplicitTypeConverter)
                    temp = "Explicit";
                else if (astTypeConverter is AstImplicitTypeConverter)
                    temp = "Implicit";
                if (methodTable.Contains(mangle1) || methodTable.Contains(mangle2))
                {
                    e.col = astTypeConverter.ColumnNumber;
                    e.fileName = astTypeConverter.Path;
                    e.line = astTypeConverter.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "Duplicate user defined conversion in type '" + this.FullQualifiedName + "': " + temp + " type conversion from type '" + astTypeConverter.AstParameter.FullQualifiedType + "' to type '" + astTypeConverter.ReturnType + "' already exists";
                    Compiler.Errors.Add(e);

                }
                else
                {
                    methodTable.Add(mangledName, astTypeConverter);
                    astTypeConverter.Key = mangledName;
                    astTypeConverter.KeyTypeInfo = this.FullQualifiedName;
                }

            }
            #endregion
            #region Checks on Base
            if (this.FullQualifiedName != "System.Object")
            {
                foreach (string ParentClass in Parents)
                {
                    if (Compiler.ClassHashtable.Contains(ParentClass))
                    {
                        AstClass tempClass = (AstClass)Compiler.ClassHashtable[ParentClass];


                        foreach (AstMethod method in tempClass.AstMethodCollection)
                        {
                            if (!method.AstMemberModifierCollection.Validate(Compiler.Errors))
                            { continue; }
                            if (method.AstMemberModifierCollection.IsProtected ||
                                method.AstMemberModifierCollection.IsPublic)
                            {
                                mangledName = mangleConstruct.MangleMethod(method, true);
                                method.key = mangledName;
                                if (!methodTable.Contains(mangledName))
                                {
                                    methodTable.Add(mangledName, method);                                    
                                    method.KeyTypeInfo = this.FullQualifiedName;
                                }
                            }
                        }

                        foreach (AstAccessor accessor in tempClass.AstAccessorCollection)
                        {
                            if (!accessor.AstMemberModifierCollection.Validate(Compiler.Errors))
                            { continue; }

                            if (accessor.AstMemberModifierCollection.ProtectionModifierType == ProtectionModifierType.Public ||
                                accessor.AstMemberModifierCollection.ProtectionModifierType ==
                                ProtectionModifierType.Protected)
                            {
                                mangledName = mangleConstruct.MangleAccessor(accessor, true);                                
                                if (!methodTable.Contains(mangledName))
                                {                                   
                                    methodTable.Add(mangledName, accessor);
                                    accessor.Key = mangledName;
                                    accessor.KeyTypeInfo = this.FullQualifiedName;
                                }                                
                            }
                        }
                    }
                }

            }
            #endregion

        }

        public void CheckDuplicateItems(LLVMSharpCompiler Compiler)
        {
            ErrorInfo e = new ErrorInfo();
            //string fieldname; string methodname; string classname;
            foreach (AstMethod astMethod in this.AstMethodCollection)
            {
                foreach (AstField astField in this.AstFieldCollection)
                {
                    if (astMethod.Name == astField.Name)
                    {
                        e.col = astField.ColumnNumber;
                        e.line = astField.LineNumber;
                        e.fileName = astField.Path;
                        e.type = ErrorType.SymenticError;
                        e.message = "The type '" + this.Name + "' already contains a definition for " + astField.Name + "'";
                        Compiler.Errors.Add(e);
                    }
                }
                foreach (AstAccessor astAccessor in this.AstAccessorCollection)
                {
                    if (astMethod.Name == astAccessor.Name)
                    {
                        e.col = astAccessor.ColumnNumber;
                        e.line = astAccessor.LineNumber;
                        e.fileName = astAccessor.Path;
                        e.type = ErrorType.SymenticError;
                        e.message = "The type '" + this.Name + "' already contains a definition for " + astAccessor.Name + "'";
                        Compiler.Errors.Add(e);

                    }
                }
                if (astMethod.Name == this.Name)
                {
                    e.col = astMethod.ColumnNumber;
                    e.fileName = astMethod.Path;
                    e.line = astMethod.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "Method '" + astMethod.Name + "' :member names cannot be the same as their enclosing type";
                    Compiler.Errors.Add(e);
                }
            }
            foreach (AstAccessor astAccessor in this.AstAccessorCollection)
            {
                foreach (AstField astField in this.AstFieldCollection)
                {
                    if (astAccessor.Name == astField.Name)
                    {
                        e.col = astAccessor.ColumnNumber;
                        e.line = astAccessor.LineNumber;
                        e.fileName = astAccessor.Path;
                        e.type = ErrorType.SymenticError;
                        e.message = "The type '" + this.Name + "' already contains a definition for " + astAccessor.Name + "'";
                        Compiler.Errors.Add(e);
                    }
                }
                if (astAccessor.Name == this.Name)
                {
                    e.col = astAccessor.ColumnNumber;
                    e.line = astAccessor.LineNumber;
                    e.fileName = astAccessor.Path;
                    e.type = ErrorType.SymenticError;
                    e.message = "Accessor '" + astAccessor.Name + "' :member names cannot be the same as their enclosing type";
                    Compiler.Errors.Add(e);
                }
            }
            ErrorList errList = new ErrorList();
            foreach (AstField item in this.AstFieldCollection)
            {
                item.AstMemberModifierCollection.Validate(Compiler.Errors);
                foreach (AstMemberModifier astMemberModifier in item.AstMemberModifierCollection)
                {
                    if (item.IsConstant && astMemberModifier is AstStaticMemberModifier)
                    {
                        e.col = item.ColumnNumber;
                        e.fileName = item.Path;
                        e.line = item.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "The constant " + this.FullQualifiedName + "." + item.Name + " cannot be marked static";
                        Compiler.Errors.Add(e);
                    }
                    if (item.AstMemberModifierCollection.IsExtern || item.AstMemberModifierCollection.IsOverriden
                        || item.AstMemberModifierCollection.IsSealed || item.AstMemberModifierCollection.IsVirtual)
                    {
                        e.col = item.ColumnNumber;
                        e.fileName = item.Path;
                        e.line = item.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "The modifier '" + astMemberModifier.Name + "' is not valid for this item";
                        Compiler.Errors.Add(e);
                    }
                }
                if (item.Name == this.Name)
                {
                    e.col = item.ColumnNumber;
                    e.fileName = item.Path;
                    e.line = item.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "Field '" + item.Name + "' :member names cannot be the same as their enclosing type";
                    Compiler.Errors.Add(e);
                }
            }
        }

        public void GetParents(LLVMSharpCompiler Compiler)
        {
            AstClass ClassTemp = this; ErrorInfo e = new ErrorInfo();
            while (ClassTemp.AstParentClass != null)
            {
                if (!ClassTemp.AstTypeModifierCollection.Validate(Compiler.Errors))
                { continue; }
                Parents.Add(ClassTemp.AstParentClass.FullQualifiedName);
                ClassTemp = (AstClass)Compiler.ClassHashtable[ClassTemp.AstParentClass.FullQualifiedName];
                if (ClassTemp.AstTypeModifierCollection.IsSealed)
                {
                    e.col = ClassTemp.ColumnNumber;
                    e.fileName = ClassTemp.Path;
                    e.line = ClassTemp.LineNumber;
                    e.message = "Cannot derive from sealed type " + ClassTemp.FullQualifiedName;
                    e.type = ErrorType.SymenticError;
                    Compiler.Errors.Add(e);
                }
            }
        }

        public string GetMethodKey(AstMethod astMethod)
        {
            StringBuilder sbtemp = new StringBuilder(); string firstPortion; string SecondPortion;
            StringBuilder temp1 = new StringBuilder();          
            
            sbtemp.Append("_mt_");
            sbtemp.Append(astMethod.Name.Length); 
            sbtemp.Append(astMethod.Name);
            if (astMethod.Parameters.Count>0)
            {
                for (int i = 0; i < astMethod.Parameters.Count; i++)
                {
                    AstParameter tempParam = (AstParameter)astMethod.Parameters[i];
                    string portion = tempParam.FullQualifiedType;//fqtArguments[i];//null reference exception error
                    if (astMethod.Parameters[i] != null && portion.Contains("."))
                    {
                        firstPortion = portion.Substring(0, portion.LastIndexOf('.'));
                        SecondPortion = portion.Substring((portion.LastIndexOf('.') + 1));

                        int paramLength1 = firstPortion.Length;
                        int paramLength2 = SecondPortion.Length;

                        temp1.Append(paramLength1); temp1.Append(firstPortion);
                        temp1.Append(paramLength2); temp1.Append(SecondPortion);

                        sbtemp.Append("_" + temp1.Length + "_");

                        sbtemp.Append(paramLength1);
                        sbtemp.Append(firstPortion);
                        sbtemp.Append(paramLength2);
                        sbtemp.Append(SecondPortion);
                    }
                    else
                    {
                        sbtemp.Append(portion.Length);
                        sbtemp.Append(astMethod.Parameters[i]);
                    }

                }
            }
            if (methodTable.ContainsKey(sbtemp.ToString()))
            {
                return sbtemp.ToString();
            }
            else
            {
                #region checkbase
                sbtemp = new StringBuilder();
                sbtemp.Append("_mb_");
                sbtemp.Append(astMethod.Name.Length); 
                sbtemp.Append(astMethod.Name);
                if (astMethod.Parameters.Count > 0)
                {
                    for (int i = 0; i < astMethod.Parameters.Count; i++)
                    {
                        AstParameter tempParam = (AstParameter)astMethod.Parameters[i];
                        string portion = tempParam.FullQualifiedType;//fqtArguments[i];//null reference exception error
                        if (astMethod.Parameters[i] != null && portion.Contains("."))
                        {
                            firstPortion = portion.Substring(0, portion.LastIndexOf('.'));
                            SecondPortion = portion.Substring((portion.LastIndexOf('.') + 1));

                            int paramLength1 = firstPortion.Length;
                            int paramLength2 = SecondPortion.Length;

                            temp1.Append(paramLength1); temp1.Append(firstPortion);
                            temp1.Append(paramLength2); temp1.Append(SecondPortion);

                            sbtemp.Append("_" + temp1.Length + "_");

                            sbtemp.Append(paramLength1);
                            sbtemp.Append(firstPortion);
                            sbtemp.Append(paramLength2);
                            sbtemp.Append(SecondPortion);
                        }
                        else
                        {
                            sbtemp.Append(portion.Length);
                            sbtemp.Append(astMethod.Parameters[i]);
                        }

                    }
                }
                #endregion

                if (methodTable.ContainsKey(sbtemp.ToString()))
                {
                    return sbtemp.ToString();
                }
            }
            return null;

        }
        public string GetMethodKey(string M_name, string[] fqtArguments, bool isInheirited)
        {
            StringBuilder sbtemp = new StringBuilder(); string firstPortion; string SecondPortion;
            StringBuilder temp1 = new StringBuilder();
            if (isInheirited == false)//from this
            {
                sbtemp.Append("_mt_");
            }
            else if (isInheirited == true)
            {
                sbtemp.Append("_mb_");
            }
            sbtemp.Append(M_name.Length);
            sbtemp.Append(M_name);
            if (fqtArguments.Length > 0)
            {
                for (int i = 0; i < fqtArguments.Length; i++)
                {
                    string portion = fqtArguments[i];//null reference exception error
                    if (fqtArguments[i] != null && fqtArguments[i].Contains("."))
                    {
                        firstPortion = portion.Substring(0, portion.LastIndexOf('.'));
                        SecondPortion = portion.Substring((portion.LastIndexOf('.') + 1));

                        int paramLength1 = firstPortion.Length;
                        int paramLength2 = SecondPortion.Length;

                        temp1.Append(paramLength1); temp1.Append(firstPortion);
                        temp1.Append(paramLength2); temp1.Append(SecondPortion);

                        sbtemp.Append("_" + temp1.Length + "_");

                        sbtemp.Append(paramLength1);
                        sbtemp.Append(firstPortion);
                        sbtemp.Append(paramLength2);
                        sbtemp.Append(SecondPortion);
                    }
                    else
                    {
                        sbtemp.Append(fqtArguments[i].Length);
                        sbtemp.Append(fqtArguments[i]);
                    }

                }

            }
            return sbtemp.ToString();
        }

        public string GetConstructorKey(string C_name, string[] fqtArguments, bool isInheirited)
        {
            StringBuilder sb = new StringBuilder(); string firstPortion; string SecondPortion;
            StringBuilder temp1 = new StringBuilder();
            if (isInheirited)
            {
                sb.Append("_cb_");
            }
            else
            {
                sb.Append("_ct_");
            }
            sb.Append(Mangler.Instance.MangleName(C_name));
            if (fqtArguments.Length > 0)
            {
                for (int i = 0; i < fqtArguments.Length; i++)
                {
                    string portion = fqtArguments[i];//null reference exception error
                    if (fqtArguments[i] != null && fqtArguments[i].Contains("."))
                    {
                        firstPortion = portion.Substring(0, portion.LastIndexOf('.'));
                        SecondPortion = portion.Substring((portion.LastIndexOf('.') + 1));

                        int paramLength1 = firstPortion.Length;
                        int paramLength2 = SecondPortion.Length;

                        temp1.Append(paramLength1); temp1.Append(firstPortion);
                        temp1.Append(paramLength2); temp1.Append(SecondPortion);

                        sb.Append("_" + temp1.Length + "_");

                        sb.Append(paramLength1);
                        sb.Append(firstPortion);
                        sb.Append(paramLength2);
                        sb.Append(SecondPortion);
                    }
                    else
                    {
                        sb.Append(fqtArguments[i].Length);
                        sb.Append(fqtArguments[i]);
                    }
                }
            }
            return sb.ToString();
        }
        public AstMethod MethodLookup(string M_name, string[] fqtArguments, bool isInheirited)
        {
            StringBuilder sbtemp = new StringBuilder(); string firstPortion; string SecondPortion;
            StringBuilder temp1 = new StringBuilder();

            if (isInheirited == false)//from this
            {
                sbtemp.Append("_mt_");
            }
            else if (isInheirited == true)
            {
                sbtemp.Append("_mb_");
            }
            sbtemp.Append(M_name.Length);
            sbtemp.Append(M_name);
            if (fqtArguments.Length > 0)
            {
                for (int i = 0; i < fqtArguments.Length; i++)
                {
                    string portion = fqtArguments[i];//null reference exception error
                    if (fqtArguments[i] != null && fqtArguments[i].Contains("."))
                    {
                        firstPortion = portion.Substring(0, portion.LastIndexOf('.'));
                        SecondPortion = portion.Substring((portion.LastIndexOf('.') + 1));

                        int paramLength1 = firstPortion.Length;
                        int paramLength2 = SecondPortion.Length;

                        temp1.Append(paramLength1); temp1.Append(firstPortion);
                        temp1.Append(paramLength2); temp1.Append(SecondPortion);

                        sbtemp.Append("_" + temp1.Length + "_");

                        sbtemp.Append(paramLength1);
                        sbtemp.Append(firstPortion);
                        sbtemp.Append(paramLength2);
                        sbtemp.Append(SecondPortion);
                    }
                    else
                    {
                        sbtemp.Append(fqtArguments[i].Length);
                        sbtemp.Append(fqtArguments[i]);
                    }

                }
                
            }
            if (this.methodTable.Contains(sbtemp.ToString()))
            {          
                return (AstMethod)this.methodTable[sbtemp.ToString()];
            }

            return null;
        }

        public AstAccessor AccessorLookup(string Access_name, bool isInherited)
        {
            StringBuilder sb = new StringBuilder();
            if (isInherited)
            { sb.Append("_ab_"); }
            else
            {
                sb.Append("_at_");
            }
            sb.Append(Access_name);
            if (this.methodTable.Contains(sb.ToString()))
            {
                return (AstAccessor)this.methodTable[sb.ToString()];
            }
            return null;
        }
        public AstGetAccessor GetAccessorLookUp(string Access_Name, bool isInherited)//passs in name from astaccessor
        {
            StringBuilder sb = new StringBuilder();
            if (isInherited)
            { sb.Append("_ab_"); }
            else
            {
                sb.Append("_at_");
            }
            sb.Append(Access_Name);
            if (this.methodTable.Contains(sb.ToString()))
            {
                AstAccessor tempAccessor = (AstAccessor)this.methodTable[sb.ToString()];
                return tempAccessor.AstGetAccessor;
            }
            return null;
        }
        public AstSetAccessor SetAccessorLookUp(string Access_Name, bool isInherited)
        {
            StringBuilder sb = new StringBuilder();
            if (isInherited)
            { sb.Append("_ab_"); }
            else
            {
                sb.Append("_at_");
            }
            sb.Append(Access_Name);
            if (this.methodTable.Contains(sb.ToString()))
            {
                AstAccessor tempAccessor = (AstAccessor)this.methodTable[sb.ToString()];
                return tempAccessor.AstSetAccessor;
            }
            return null;
        }
        public AstConstructor AstConstructorLookup(string C_name, string[] fqtArguments, bool isInherited)
        {
            StringBuilder sb = new StringBuilder(); string firstPortion; string SecondPortion;
            StringBuilder temp1 = new StringBuilder();
            if (isInherited)
            {
                sb.Append("_cb_");
            }
            else
            {
                sb.Append("_ct_");
            }
            sb.Append(Mangler.Instance.MangleName(C_name));
            if (fqtArguments.Length > 0)
            {
                for (int i = 0; i < fqtArguments.Length; i++)
                {
                    string portion = fqtArguments[i];//null reference exception error
                    if (fqtArguments[i] != null && fqtArguments[i].Contains("."))
                    {
                        firstPortion = portion.Substring(0, portion.LastIndexOf('.'));
                        SecondPortion = portion.Substring((portion.LastIndexOf('.') + 1));

                        int paramLength1 = firstPortion.Length;
                        int paramLength2 = SecondPortion.Length;

                        temp1.Append(paramLength1); temp1.Append(firstPortion);
                        temp1.Append(paramLength2); temp1.Append(SecondPortion);

                        sb.Append("_" + temp1.Length + "_");

                        sb.Append(paramLength1);
                        sb.Append(firstPortion);
                        sb.Append(paramLength2);
                        sb.Append(SecondPortion);
                    }
                    else
                    {
                        sb.Append(fqtArguments[i].Length);
                        sb.Append(fqtArguments[i]);
                    }
                }
            }
            if (this.methodTable.Contains(sb.ToString()))
            {
                return (AstConstructor)this.methodTable[sb.ToString()];
            }


            return null;
        }
        public AstOperatorOverload OpOverloadLookup(string[] fqtparam, IAstExpression IExpression)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("_op_");
            if (IExpression is AstAdditionExpression)
            {
                sb.Append("p");
            }
            else if (IExpression is AstSubtractionExpression)
            {
                sb.Append("m");
            }
            else if (IExpression is AstDivisionExpression)
            {
                sb.Append("div");
            }
            else if (IExpression is AstEqualityExpression) //if implemented, not equal must also be implemented
            {
                sb.Append("ee");
            }
            else if (IExpression is AstGreaterThanExpression)
            {
                sb.Append("gt");
            }
            else if (IExpression is AstGreaterThanOrEqualExpression)
            {
                sb.Append("gte");
            }
            else if (IExpression is AstInequalityExpression)
            {
                sb.Append("ne");
            }
            else if (IExpression is AstLesserThanExpression)
            {
                sb.Append("lt");
            }
            else if (IExpression is AstLesserThanOrEqualExpression)
            {
                sb.Append("lte");
            }
            else if (IExpression is AstMuliplicationExpression)
            {
                sb.Append("multi");
            }
            else if (IExpression is AstAddAssignmentExpression)
            {
                sb.Append("p");
            }
            else if (IExpression is AstDivisionAssignmentExpression)
            {
                sb.Append("div");
            }
            else if (IExpression is AstMuliplicationExpression)
            {
                sb.Append("multi");
            }
            else if (IExpression is AstSubtractAssignmentExpression)
            {
                sb.Append("m");
            }
            else if (IExpression is AstNot)
            {
                sb.Append("not");
            }
            else if (IExpression is AstPostDecrement)
            {
                sb.Append("mm");
            }
            else if (IExpression is AstPostIncrement)
            {
                sb.Append("pp");
            }
            else if (IExpression is AstPreDecrement)
            {
                sb.Append("mm");
            }
            else if (IExpression is AstPreIncrement)
            {
                sb.Append("pp");
            }
            else if (IExpression is AstUnaryMinus)
            {
                sb.Append("um");
            }
            else if (IExpression is AstUnaryPlus)
            {
                sb.Append("up");
            }


            if (fqtparam.Length > 0 && !string.IsNullOrEmpty(fqtparam[0]))
            {
                sb.Append("_");
                sb.Append(fqtparam[0].Length);
                sb.Append(fqtparam[0]);
                if (fqtparam.Length > 1 && !string.IsNullOrEmpty(fqtparam[1]))
                {
                    sb.Append("_");
                    sb.Append(fqtparam[1].Length);
                    sb.Append(fqtparam[1]);
                }
            }

            if (methodTable.Contains(sb.ToString()))
                return (AstOperatorOverload)methodTable[sb.ToString()];
            else
                return null;
        }
        public AstTypeConverter TypeConverLookup(string Rettype, string paramType, IAstExpression IExpression)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("_op_");
            if (IExpression is AstSimpleAssignmentExpression)
            {
                //check for implicit operator overloading
                sb.Append("imp_");
            }
            else if (IExpression is AstReturn)
            {
                AstReturn astRetTemp = (AstReturn)IExpression;
                if (astRetTemp.AstExpression != null)
                {
                    if (astRetTemp.AstExpression is AstTypeCast)
                        sb.Append("exp_");
                    else
                        sb.Append("imp_");//still need to add more cases
                }
            }
            else if (IExpression is AstAddAssignmentExpression)
            { }
            else if (IExpression is AstSubtractAssignmentExpression)
            { }
            else if (IExpression is AstMultiplyAssignmentExpression)
            { }
            else if (IExpression is AstTypeCast)
            {
                //check for explicit operator overloading
                sb.Append("exp_");
            }

            sb.Append(Rettype);
            sb.Append("_");
            sb.Append(paramType);
            if (methodTable.Contains(sb.ToString()))
            {
                return (AstTypeConverter)methodTable[sb.ToString()];
            }
            return null;
        }

        public void CheckOverloads(LLVMSharpCompiler Compiler)
        {
            bool eq = false; bool gt = false; bool lt = false; bool gte = false; bool lte = false; bool optrue = false;
            bool optfalse = false; int tempchk4 = 0;
            bool neq = false; int i = 0; int tempchk1 = 0; int tempchk2 = 0; int tempchk3 = 0;
            ErrorInfo e = new ErrorInfo();
            foreach (AstOperatorOverload opOverload in this.AstOperatorOverloadCollection)
            {
                #region Check Matching Overloaded operands
                i++;
                if (opOverload.OverloadableOperand == OverloadableOperand.Equality)
                {
                    eq = true;
                    tempchk1 = i;
                }

                else if (opOverload.OverloadableOperand == OverloadableOperand.NotEqual)
                {
                    neq = true;
                    tempchk1 = i;
                }

                else if (opOverload.OverloadableOperand == OverloadableOperand.LessThan)
                {
                    lt = true;
                    tempchk2 = i;
                }
                else if (opOverload.OverloadableOperand == OverloadableOperand.GreaterThan)
                {
                    gt = true;
                    tempchk2 = i;
                }
                else if (opOverload.OverloadableOperand == OverloadableOperand.LessThanEqual)
                {
                    lte = true;
                    tempchk3 = i;
                }
                else if (opOverload.OverloadableOperand == OverloadableOperand.GreaterThanEqual)
                {
                    gte = true;
                    tempchk3 = i;
                }
                else if (opOverload.OverloadableOperand == OverloadableOperand.True)
                {
                    optrue = true;
                    tempchk4 = i;
                }
                else if (opOverload.OverloadableOperand == OverloadableOperand.False)
                {
                    optfalse = true;
                    tempchk4 = i;
                }
                if (eq && !neq)//can seperate in to two cases
                {
                    if (this.AstOperatorOverloadCollection.Count == i)
                    {
                        AstOperatorOverload tempOp = (AstOperatorOverload)AstOperatorOverloadCollection[tempchk1 - 1];
                        e.fileName = tempOp.Path;
                        e.col = tempOp.ColumnNumber;
                        e.line = tempOp.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "The operator " + this.FullQualifiedName + " operator == requires a matching operator != to also be defined";
                        Compiler.Errors.Add(e);
                    }
                }
                else if (!eq && neq)
                {
                    if (this.AstOperatorOverloadCollection.Count == i)
                    {
                        AstOperatorOverload tempOp = (AstOperatorOverload)AstOperatorOverloadCollection[tempchk1 - 1];
                        e.fileName = tempOp.Path;
                        e.col = tempOp.ColumnNumber;
                        e.line = tempOp.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "The operator " + this.FullQualifiedName + " operator != requires a matching operator == to also be defined";
                        Compiler.Errors.Add(e);
                    }
                }
                if (lt && !gt)
                {
                    if (this.AstOperatorOverloadCollection.Count == i)
                    {
                        AstOperatorOverload tempOp = (AstOperatorOverload)AstOperatorOverloadCollection[tempchk2 - 1];
                        e.fileName = tempOp.Path;
                        e.col = tempOp.ColumnNumber;
                        e.line = tempOp.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "The operator " + this.FullQualifiedName + " operator < requires a matching operator > to also be defined";
                        Compiler.Errors.Add(e);
                    }
                }
                else if (!lt && gt)
                {
                    if (this.AstOperatorOverloadCollection.Count == i)
                    {
                        AstOperatorOverload tempOp = (AstOperatorOverload)AstOperatorOverloadCollection[tempchk2 - 1];
                        e.fileName = tempOp.Path;
                        e.col = tempOp.ColumnNumber;
                        e.line = tempOp.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "The operator " + this.FullQualifiedName + " operator > requires a matching operator < to also be defined";
                        Compiler.Errors.Add(e);
                    }
                }
                if (lte && !gte)
                {
                    if (this.AstOperatorOverloadCollection.Count == i)
                    {
                        AstOperatorOverload tempOp = (AstOperatorOverload)AstOperatorOverloadCollection[tempchk3 - 1];
                        e.fileName = tempOp.Path;
                        e.col = tempOp.ColumnNumber;
                        e.line = tempOp.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "The operator " + this.FullQualifiedName + " operator <= requires a matching operator >= to also be defined";
                        Compiler.Errors.Add(e);
                    }
                }
                else if (!lte && gte)
                {
                    if (this.AstOperatorOverloadCollection.Count == i)
                    {
                        AstOperatorOverload tempOp = (AstOperatorOverload)AstOperatorOverloadCollection[tempchk3 - 1];
                        e.fileName = tempOp.Path;
                        e.col = tempOp.ColumnNumber;
                        e.line = tempOp.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "The operator " + this.FullQualifiedName + " operator >= requires a matching operator <= to also be defined";
                        Compiler.Errors.Add(e);
                    }
                }
                if (!optfalse && optrue)
                {
                    if (this.AstOperatorOverloadCollection.Count == i)
                    {
                        AstOperatorOverload tempOp = (AstOperatorOverload)AstOperatorOverloadCollection[tempchk4 - 1];
                        e.fileName = tempOp.Path;
                        e.col = tempOp.ColumnNumber;
                        e.line = tempOp.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "The operator " + this.FullQualifiedName + " operator true requires a matching operator false to also be defined";
                        Compiler.Errors.Add(e);
                    }
                }
                else if (optfalse && !optrue)
                {
                    if (this.AstOperatorOverloadCollection.Count == i)
                    {
                        AstOperatorOverload tempOp = (AstOperatorOverload)AstOperatorOverloadCollection[tempchk4 - 1];
                        e.fileName = tempOp.Path;
                        e.col = tempOp.ColumnNumber;
                        e.line = tempOp.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "The operator " + this.FullQualifiedName + " operator false requires a matching operator true to also be defined";
                        Compiler.Errors.Add(e);
                    }
                }
                #endregion Checking Matching Overloads

                #region Check if one of the parameters is of the enclosing type for binary overloads

                if (opOverload.AstParameter1.FullQualifiedType != this.FullQualifiedName) //parameter is not enclosing type
                {
                    if (opOverload.AstParameter2 != null)//overload takes in a second parameter
                    {
                        if (opOverload.AstParameter2.FullQualifiedType != this.FullQualifiedName)//2nd parameter is not of enclosing type either
                        {//report error
                            e.col = opOverload.ColumnNumber;
                            e.fileName = opOverload.Path;
                            e.line = opOverload.LineNumber;
                            e.message = "One of the parameters of a binary operator must be the enclosing type";
                            e.type = ErrorType.SymenticError;
                            Compiler.Errors.Add(e);
                        }
                    }
                }

                #endregion

                #region Check for unary operator overloads, parameter must be of the enclosing type

                if (opOverload.AstParameter2 == null)
                {
                    if (opOverload.AstParameter1.FullQualifiedType != this.FullQualifiedName)
                    {
                        e.col = opOverload.ColumnNumber;
                        e.fileName = opOverload.Path;
                        e.line = opOverload.LineNumber;
                        e.message = "The parameter of a unary operator overload must be the containing type ";
                        e.type = ErrorType.SymenticError;
                        Compiler.Errors.Add(e);

                    }
                }

                #endregion
                #region Check return type and parameter for ++ and -- must be enclosing type
                if (opOverload.OverloadableOperand == OverloadableOperand.Increment || opOverload.OverloadableOperand == OverloadableOperand.Decrement)
                {
                    if (opOverload.FullQReturnType != this.FullQualifiedName)
                    {
                        e.col = opOverload.ColumnNumber;
                        e.fileName = opOverload.Path;
                        e.line = opOverload.LineNumber;
                        e.message = "The return type for ++ or -- operator must be the containing type ";
                        e.type = ErrorType.SymenticError;
                        Compiler.Errors.Add(e);
                    }
                }
                #endregion
                #region Check if one of the parameters is of the enclosing type for binary overloads

                if (opOverload.AstParameter1.FullQualifiedType != this.FullQualifiedName) //parameter is not enclosing type
                {
                    if (opOverload.AstParameter2 != null)//overload takes in a second parameter
                    {
                        if (opOverload.AstParameter2.FullQualifiedType != this.FullQualifiedName)//2nd parameter is not of enclosing type either
                        {//report error
                            e.col = opOverload.ColumnNumber;
                            e.fileName = opOverload.Path;
                            e.line = opOverload.LineNumber;
                            e.message = "One of the parameters of a binary operator must be the enclosing type";
                            e.type = ErrorType.SymenticError;
                            Compiler.Errors.Add(e);
                        }
                    }
                }

                #endregion

                #region Check for unary operator overloads, parameter must be of the enclosing type

                if (opOverload.AstParameter2 == null)
                {
                    if (opOverload.AstParameter1.FullQualifiedType != this.FullQualifiedName)
                    {
                        e.col = opOverload.ColumnNumber;
                        e.fileName = opOverload.Path;
                        e.line = opOverload.LineNumber;
                        e.message = "The parameter of a unary operator overload must be the containing type ";
                        e.type = ErrorType.SymenticError;
                        Compiler.Errors.Add(e);

                    }
                }

                #endregion
                #region Check return type and parameter for ++ and -- must be enclosing type
                if (opOverload.OverloadableOperand == OverloadableOperand.Increment || opOverload.OverloadableOperand == OverloadableOperand.Decrement)
                {
                    if (opOverload.FullQReturnType != this.FullQualifiedName)
                    {
                        e.col = opOverload.ColumnNumber;
                        e.fileName = opOverload.Path;
                        e.line = opOverload.LineNumber;
                        e.message = "The return type for ++ or -- operator must be the containing type ";
                        e.type = ErrorType.SymenticError;
                        Compiler.Errors.Add(e);
                    }
                }
                #endregion
            }

        }

        public void CreateDefaults()
        {
            if (this.AstConstructorCollection.Count == 0)
            {
                AstConstructor astConstruct = new AstConstructor(this.Path, this.LineNumber, this.ColumnNumber);
                astConstruct.FullQName = this.FullQualifiedName;
                astConstruct.Name = this.Name;
                AstPublicMemberModifier astModifier = new AstPublicMemberModifier(astConstruct.Path, astConstruct.LineNumber, astConstruct.ColumnNumber);
                astConstruct.AstMemberModifierCollection.Add(astModifier);
                methodTable.Add(Mangler.Instance.MangleMethod(this.FullQualifiedName, astConstruct, false), astConstruct);
            }

        }
        public void CheckTypeConversion(LLVMSharpCompiler Compiler)
        {//checks user type conversion must convert to or from the enclosing type. 
            ErrorInfo e = new ErrorInfo(); string temp = "";
            foreach (AstTypeConverter astConver in this.AstTypeConverterCollection)
            {
                if (astConver is AstImplicitTypeConverter)
                    temp = "Implicit";
                else if (astConver is AstExplicitTypeConverter)
                    temp = "Explicit";

                if (astConver.FullQReturnType != this.FullQualifiedName)
                {
                    if (astConver.AstParameter.FullQualifiedType != this.FullQualifiedName)
                    {

                        e.col = astConver.ColumnNumber;
                        e.fileName = astConver.Path;
                        e.line = astConver.LineNumber;
                        e.message = temp+" type conversion method in class "+this.FullQualifiedName+": User-defined conversion must convert to or from the enclosing type";
                        e.type = ErrorType.SymenticError;
                        Compiler.Errors.Add(e);
                    }
                }
                if (astConver.FullQReturnType == astConver.AstParameter.FullQualifiedType)
                {
                    
                    e.col = astConver.ColumnNumber;
                    e.fileName = astConver.Path;
                    e.line = astConver.LineNumber;
                    
                    e.message = temp+" type conversion method in class " +this.FullQualifiedName +": User-defined operator cannot take an object of the enclosing type and convert to an object of the enclosing type";
                    e.type = ErrorType.SymenticError;
                    Compiler.Errors.Add(e);
                }
                foreach (string item in this.Parents)
                {
                    if (astConver.AstParameter.FullQualifiedType == item || astConver.FullQReturnType == item)
                    {
                        e.col = astConver.ColumnNumber;
                        e.fileName = astConver.Path;
                        e.line = astConver.LineNumber;
                        e.message = temp+" type conversion method in class " + this.FullQualifiedName + ": User-defined conversion to or from a base class are not allowed";
                        e.type = ErrorType.SymenticError;
                        Compiler.Errors.Add(e);
                    }
                }

            }
        }

        public void CheckVirtualOverrides(LLVMSharpCompiler Compiler)
        {
            foreach (AstMethod astMethod in this.AstMethodCollection)
            {
                if (astMethod.AstMemberModifierCollection.IsOverriden)
                {
                    CheckVirtualOverrides(astMethod, Compiler);
                }
            }
            foreach (AstAccessor astAccess in this.AstAccessorCollection)
            {
                if (astAccess.AstMemberModifierCollection.IsOverriden)
                {
                    CheckVirtualOverrides(astAccess, Compiler);
                }
            }
        }

        private void CheckVirtualOverrides(AstAccessor astAccess, LLVMSharpCompiler Compiler)
        {
            bool foundmatch = false; bool accessChange = false;
            ErrorInfo e = new ErrorInfo();
            foreach (string parent in this.Parents)
            {
                if (Compiler.ClassHashtable.Contains(parent))
                {
                    AstClass TempClass = (AstClass)Compiler.ClassHashtable[parent];

                    foreach (AstAccessor Par_access in TempClass.AstAccessorCollection)
                    {
                        if (Par_access.Name == astAccess.Name && Par_access.AstMemberModifierCollection.IsVirtual)
                        {
                            foundmatch = true;
                            if (Par_access.AstMemberModifierCollection.ProtectionModifierType != astAccess.AstMemberModifierCollection.ProtectionModifierType)
                            {
                                accessChange = true;
                            }
                        }

                    }

                }

            }
            if (!foundmatch)
            {
                e.col = astAccess.ColumnNumber;
                e.fileName = astAccess.Path;
                e.line = astAccess.LineNumber;
                e.message = "Method " + astAccess.Name + ": no suitable method found to override";
                e.type = ErrorType.SymenticError;
                Compiler.Errors.Add(e);
            }
            if (accessChange)
            {
                e.col = astAccess.ColumnNumber;
                e.fileName = astAccess.Path;
                e.line = astAccess.LineNumber;
                e.type = ErrorType.SymenticError;
                e.message = astAccess.Name + ": cannot change access modifiers when overriding a virtual method";
                Compiler.Errors.Add(e);
            }
        }
        private void CheckVirtualOverrides(AstMethod astMethod, LLVMSharpCompiler Compiler)
        {
            bool foundmatch = false; bool accessChange = false;
            ErrorInfo e = new ErrorInfo();
            this.GetMethodKey(astMethod);
            foreach (string parent in this.Parents)
            {
                #region body
                if (Compiler.ClassHashtable.Contains(parent))
                {
                    AstClass TempClass = (AstClass)Compiler.ClassHashtable[parent];

                    foreach (AstMethod Pmethod in TempClass.AstMethodCollection)
                    {
                        if (Pmethod.Name == astMethod.Name && Pmethod.AstMemberModifierCollection.IsVirtual)
                        {
                            foundmatch = true;
                            if (Pmethod.AstMemberModifierCollection.ProtectionModifierType != astMethod.AstMemberModifierCollection.ProtectionModifierType)
                            {
                                accessChange = true;
                            }
                        }

                    }

                }
                #endregion

            }
            if (!foundmatch)
            {
                e.col = astMethod.ColumnNumber;
                e.fileName = astMethod.Path;
                e.line = astMethod.LineNumber;
                e.message = "Method " + astMethod.Name + ": no suitable method found to override";
                e.type = ErrorType.SymenticError;
                Compiler.Errors.Add(e);
            }
            if (accessChange)
            {
                e.col = astMethod.ColumnNumber;
                e.fileName = astMethod.Path;
                e.line = astMethod.LineNumber;
                e.type = ErrorType.SymenticError;
                e.message = astMethod.Name + ": cannot change access modifiers when overriding a virtual method";
                Compiler.Errors.Add(e);
            }
        }

        public void CheckNestedTypes(LLVMSharpCompiler Compiler)
        {
            if (this.AstTypeCollection.Count != 0)
            {
                ErrorInfo e = new ErrorInfo();
                e.col = this.ColumnNumber;
                e.fileName = this.Path;
                e.line = this.LineNumber;
                e.type = ErrorType.SymenticError;
                e.message = "Nested type in class: " + this.FullQualifiedName + " not recognized. Nested types are not allowed";
                Compiler.Errors.Add(e);
            }
        }

        public void GetSize()
        {
            foreach (AstField astField in this.AstFieldCollection)
            {
                Type type = astField.GetType();
                if (type.FullName == "System.Bool" || type.FullName == "System.Char")// 1 byte
                {
                    this.SizeOfObj += 1;
                }
                else if (type.FullName == "System.Single" || type.FullName == "System.Int32")//4 bytes
                {
                    this.SizeOfObj += 4;
                }
                else if (type.FullName == "System.String")//8 bytes
                {
                    this.SizeOfObj += 8;
                }
                else
                {
                    this.SizeOfObj += 4;
                }

            }
            if (AstParentClass != null)
                SizeOfObj += 4;
        }

    }

}
