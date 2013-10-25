using System;
using System.Text;
using LLVMSharp.Compiler.CocoR;
using System.Collections;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Walkers;

namespace LLVMSharp.Compiler.Ast
{
    public class AstStruct : AstType
    {
        // nested class or struct or enum
        public AstTypeCollection AstTypeCollection = new AstTypeCollection();
        public AstConstructorCollection AstConstructorCollection = new AstConstructorCollection();
        public AstFieldCollection AstFieldCollection = new AstFieldCollection();
        public AstMethodCollection AstMethodCollection = new AstMethodCollection();
        public AstAccessorCollection AstAccessorCollection = new AstAccessorCollection();
        public AstTypeConverterCollection AstTypeConverterCollection = new AstTypeConverterCollection();
        public AstOperatorOverloadCollection AstOperatorOverloadCollection = new AstOperatorOverloadCollection();

        public Hashtable methodTable = new Hashtable();
        public Hashtable ObjectLayout = new Hashtable();
        public int SizeOfObj;

        public AstStruct(
            string path, int lineNumber, int columnNumber)
            : base(path, lineNumber, columnNumber) { }

        public AstStruct(IParser parser) : base(parser) { }
        public AstStruct(IParser parser, bool useLookAhead) : base(parser, useLookAhead) { }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--AstStruct--{0}{0}");

            sb.Append("Src: {1}{0}");
            sb.Append("Ln: {2}{0}");
            sb.Append("Col: {3}{0}{0}");

            sb.Append("Struct Name: {4}");

            return string.Format(sb.ToString(), Environment.NewLine, base.Path, base.LineNumber, base.ColumnNumber,
                base.Name);
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


        public void GenerateMethodTable(LLVMSharpCompiler Compiler)
        {
            Mangler mangleConstruct;
            string mangledName; ErrorList _errors = new ErrorList();          
            mangleConstruct = new Mangler(); ErrorInfo e = new ErrorInfo();

            foreach (AstMethod method in AstMethodCollection)
            {

                if (!method.AstMemberModifierCollection.Validate(Compiler.Errors))
                { continue; }

                if (method.AstMemberModifierCollection.IsProtected)
                {
                    e.col = method.ColumnNumber;
                    e.fileName = method.Path;
                    e.line = method.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "The modifier 'protected' is not valid for this item";
                    Compiler.Errors.Add(e);
                }
                if (method.AstMemberModifierCollection.IsOverriden)
                {
                    e.col = method.ColumnNumber;
                    e.fileName = method.Path;
                    e.line = method.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "The modifier 'override' is not valid for this item";
                    Compiler.Errors.Add(e);
                }
                if (method.AstMemberModifierCollection.IsVirtual)
                {
                    e.col = method.ColumnNumber;
                    e.fileName = method.Path;
                    e.line = method.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "The modifier 'virtual' is not valid for this item";
                    Compiler.Errors.Add(e);
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
                if (!method.AstMemberModifierCollection.IsExtern && method.AstBlock == null)
                {
                    e.col = method.ColumnNumber;
                    e.fileName = method.Path;
                    e.line = method.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "Method " + method.Name + " must declare a body because it is not marked abstract or extern";
                    Compiler.Errors.Add(e);
                }
                mangledName = mangleConstruct.MangleMethod(method, false);
                if (methodTable.Contains(mangledName))
                {
                    e.col = method.ColumnNumber;
                    e.fileName = method.Path;
                    e.line = method.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "Struct "+this.FullQualifiedName+ " already defines a method " + method.Name + " with the same parameter types";
                    Compiler.Errors.Add(e);
                }
                else
                {
                    methodTable.Add(mangledName, method);
                    method.key = mangledName;
                    method.KeyTypeInfo = this.FullQualifiedName;
                }
            }
            foreach (AstConstructor construct in this.AstConstructorCollection)
            {
                //static constructor cannot have access modifiers
                if (!construct.AstMemberModifierCollection.Validate(Compiler.Errors))
                { continue; }
                if (construct.AstMemberModifierCollection.IsStatic)
                {
                    if (construct.AstConstructorCall != null)
                    {
                        e.col = construct.ColumnNumber;
                        e.line = construct.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.fileName = construct.Path;
                        e.message = "In struct "+this.FullQualifiedName + ": Static constructor cannot have an explicit this or base constructor call ";
                        Compiler.Errors.Add(e);
                    }
                    if (construct.AstMemberModifierCollection.IsPublic || construct.AstMemberModifierCollection.IsProtected
                       || construct.AstMemberModifierCollection.IsPrivate)
                    {
                        e.col = construct.ColumnNumber;
                        e.line = construct.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.fileName = construct.Path;
                        e.message = "In struct "+this.FullQualifiedName +" Access modifiers are not allowed on static constructors ";
                        Compiler.Errors.Add(e);
                    }
                    if (construct.Parameters.Count > 0)
                    {
                        e.col = construct.ColumnNumber;
                        e.line = construct.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.fileName = construct.Path;
                        e.message = "In struct "+this.FullQualifiedName + ": A static constructor must be parameterless";
                        Compiler.Errors.Add(e);
                    }
                }
                if (construct.Parameters.Count == 0 && !construct.AstMemberModifierCollection.IsStatic)
                {
                    e.col = construct.ColumnNumber;
                    e.line = construct.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.fileName = construct.Path;
                    e.message = "In struct "+this.FullQualifiedName +": A Struct cannot contain an explicit parameterless constructor";
                    Compiler.Errors.Add(e);

                }
                if (construct.AstConstructorCall != null)
                {
                    if (construct.AstConstructorCall is AstBaseConstructorCall)
                    {
                        e.col = construct.ColumnNumber;
                        e.line = construct.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.fileName = construct.Path;
                        e.message = "In struct "+this.FullQualifiedName + ": Structs cannot call base class constructors";
                        Compiler.Errors.Add(e);

                    }
                }
                foreach (AstField astField in this.AstFieldCollection)
                {
                    if (astField.Initialization == null && !construct.AstMemberModifierCollection.IsStatic)
                    {
                        e.col = construct.ColumnNumber;
                        e.line = construct.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.fileName = construct.Path;
                        e.message = "Field " + this.Name + "." + astField.Name + " must be fully assigned before control is returned to caller";
                        Compiler.Errors.Add(e);
                    }
                }
                mangledName = mangleConstruct.MangleMethod(construct.FullQName, construct, false);
                if (methodTable.Contains(mangledName))
                {
                    e.col = construct.ColumnNumber;
                    e.line = construct.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.fileName = construct.Path;
                    e.message = "In struct "+this.FullQualifiedName+" a constructor already exists containing the same parameter types";
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
                if (accessor.AstMemberModifierCollection.IsOverriden)
                {
                    e.col = accessor.ColumnNumber;
                    e.fileName = accessor.Path;
                    e.line = accessor.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "The modifier 'override' is not valid for this item";
                    Compiler.Errors.Add(e);
                }
                if (accessor.AstMemberModifierCollection.IsVirtual)
                {
                    e.col = accessor.ColumnNumber;
                    e.fileName = accessor.Path;
                    e.line = accessor.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "The modifier 'virtual' is not valid for this item";
                    Compiler.Errors.Add(e);
                }
                mangledName = mangleConstruct.MangleAccessor(accessor, false);
                if (methodTable.Contains(mangledName))
                {
                    e.col = accessor.ColumnNumber;
                    e.fileName = accessor.Path;
                    e.line = accessor.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "Accessor for " + accessor.Name + " already exists in, "+this.FullQualifiedName+" a duplicate has been detected";
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
                if (methodTable.Contains(mangle1) || methodTable.Contains(mangle2))
                {
                    string temp = "";
                    if (astTypeConverter is AstExplicitTypeConverter)
                        temp = "Explicit";
                    else if (astTypeConverter is AstImplicitTypeConverter)
                        temp = "Implicit";
                    e.col = astTypeConverter.ColumnNumber;
                    e.fileName = astTypeConverter.Path;
                    e.line = astTypeConverter.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "Duplicate user defined conversion in type '" + this.FullQualifiedName + "': "+temp+" type conversion from type '" +astTypeConverter.AstParameter.FullQualifiedType+ "' to type '" +astTypeConverter.ReturnType+"' already exists";
                    Compiler.Errors.Add(e);

                }
                else
                {
                    methodTable.Add(mangledName, astTypeConverter);
                    astTypeConverter.Key = mangledName;
                    astTypeConverter.KeyTypeInfo = this.FullQualifiedName;
                }

            }
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
                    //check for field initializaiton in struct, not allowed!

                }
            }
            foreach (AstField item in this.AstFieldCollection)
            {
                if (item.Initialization != null && !item.IsConstant)
                {
                    e.col = item.ColumnNumber;
                    e.fileName = item.Path;
                    e.line = item.LineNumber;
                    e.type = ErrorType.SymenticError;
                    e.message = "Type " + this.FullQualifiedName + " does not allow initialization of field " + item.Name;
                    Compiler.Errors.Add(e);
                }
                foreach (AstMemberModifier astMemberModifier in item.AstMemberModifierCollection)
                {
                    if (item.IsConstant && astMemberModifier is AstStaticMemberModifier)
                    {
                        e.col = item.ColumnNumber;
                        e.fileName = item.Path;
                        e.line = item.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "The constant " + this.FullQualifiedName + "." + item.Name + "cannot be marked static";
                        Compiler.Errors.Add(e);
                    }
                }
                if (item.IsConstant)
                {
                    if (item.Initialization == null)
                    {
                        e.col = item.ColumnNumber;
                        e.fileName = item.Path;
                        e.line = item.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "Constant "+item.Name +" in struct " +this.FullQualifiedName+ " must be initialized";
                        Compiler.Errors.Add(e);
                    }
                }

            }
        }

        public void CheckOverloads(LLVMSharpCompiler Compiler)
        {
            bool eq = false; bool gt = false; bool lt = false; bool gte = false; bool lte = false;
            bool neq = false; int i = 0; int tempchk1 = 0; int tempchk2 = 0; int tempchk3 = 0;
            bool optrue = false;
            bool optfalse = false; int tempchk4 = 0;
            ErrorInfo e = new ErrorInfo();
            foreach (AstOperatorOverload opOverload in this.AstOperatorOverloadCollection)
            {
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

                if (eq && !neq)//can seperate in to two cases
                {
                    if (this.AstOperatorOverloadCollection.Count == i)
                    {
                        AstOperatorOverload tempOp = (AstOperatorOverload)AstOperatorOverloadCollection[tempchk1];
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
                        AstOperatorOverload tempOp = (AstOperatorOverload)AstOperatorOverloadCollection[tempchk1];
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
                        AstOperatorOverload tempOp = (AstOperatorOverload)AstOperatorOverloadCollection[tempchk2];
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
                        AstOperatorOverload tempOp = (AstOperatorOverload)AstOperatorOverloadCollection[tempchk2];
                        e.fileName = tempOp.Path;
                        e.col = tempOp.ColumnNumber;
                        e.line = tempOp.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "The operator " + this.FullQualifiedName + " operator > requires a matching operator < to also be defined";
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
                        e.message =  temp+" type conversion method in struct "+this.FullQualifiedName+": User-defined conversion must convert to or from the enclosing type";
                        e.type = ErrorType.SymenticError;
                        Compiler.Errors.Add(e);
                    }
                }
                if (astConver.FullQReturnType == astConver.AstParameter.FullQualifiedType)
                {
                    e.col = astConver.ColumnNumber;
                    e.fileName = astConver.Path;
                    e.line = astConver.LineNumber;
                    e.message = temp + " type conversion method in struct " + this.FullQualifiedName + ": User-defined operator cannot take an object of the enclosing type and convert to an object of the enclosing type";
                    e.type = ErrorType.SymenticError;
                    Compiler.Errors.Add(e);
                }

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
                e.message = "Nested type in struct: " + this.FullQualifiedName + " not recognized. Nested types are not allowed";
                Compiler.Errors.Add(e);


            }
        }

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
        public string GetMethodKey(AstMethod astMethod)
        {
            StringBuilder sbtemp = new StringBuilder(); string firstPortion; string SecondPortion;
            StringBuilder temp1 = new StringBuilder();

            sbtemp.Append("_mt_");
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
        }

    }
}
