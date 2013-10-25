using System;
using System.Collections;
using System.Text;
using LLVMSharp.Compiler.Ast;
using DotNetMatrix;

namespace LLVMSharp.Compiler.Semantic
{
    public partial class ObjectHierarchy
    {
        private AstProgram _astProgram;
        private ErrorList _errors;
        private Hashtable _usingDeclarative;

        private bool _hasError;
        public bool hasError { get { return _hasError; } set { _hasError = value; } }

        public Hashtable ClassHTable;
        public Hashtable EnumHTable;
        public Hashtable StructHTable;
        public Hashtable RootClassHTable;
        public Hashtable MethodTable;

        public ClassHierarchyNode ClassHierarchy = null;

        public TypeChecker typeChecker = null;

        public ObjectHierarchy(AstProgram astProgram, ErrorList errors)
        {
            _astProgram = astProgram;
            _errors = errors;

            _hasError = false;

            _usingDeclarative = new Hashtable();

            ClassHTable = new Hashtable();
            EnumHTable = new Hashtable();
            StructHTable = new Hashtable();
            RootClassHTable = new Hashtable();
            MethodTable = new Hashtable();
        }

        #region Add To Hash Table Methods
        private void AddStructItem(AstStruct item, string key)
        {
            if (StructHTable.ContainsKey(key) || ClassHTable.ContainsKey(key) || EnumHTable.ContainsKey(key))
            {
                _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Already contains a definition of " + key, item.LineNumber, item.ColumnNumber, item.Path));
                _hasError = true;
                return;
            }
            item.FullQualifiedName = key;
            StructHTable.Add(key, item);
        }

        /// <summary>
        /// Added Enum object to HashTable only if the varible in Enum are not duplicated
        /// check the variable duplication in Enum also
        /// </summary>
        /// <param name="item"></param>
        /// <param name="key"></param>
        private void AddEnumItem(AstEnum item, string key)
        {
            //Check for the data inside Enum
            Hashtable _enumItem = new Hashtable();

            foreach (AstEnumMember _emember in item.AstEnumMemberCollection)
            {
                if (_enumItem.Contains(_emember.Name))
                {
                    _errors.Add(new ErrorInfo(ErrorType.SymenticError,
                        "The type " + key + " already contains a definition of " + _emember.Name, _emember.LineNumber, _emember.ColumnNumber, _emember.Path));
                    _hasError = true;
                    return;
                }
                _emember.FullQualifiedName = key + _emember.Name;
                _enumItem.Add(_emember.Name, _emember);
            }
            if (EnumHTable.ContainsKey(key) || ClassHTable.ContainsKey(key) || StructHTable.ContainsKey(key))
            {
                _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Already contains a definition of " + key, item.LineNumber, item.ColumnNumber, item.Path));
                _hasError = true;
                return;
            }
            item.FullQualifiedName = key;
            EnumHTable.Add(key, item);
        }

        private void AddClassItem(AstClass item, string key)
        {
            if (ClassHTable.ContainsKey(key) || StructHTable.ContainsKey(key) || EnumHTable.ContainsKey(key))
            {
                _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Already contains a definition of " + key, item.LineNumber, item.ColumnNumber, item.Path));
                _hasError = true;
                return;
            }

            item.FullQualifiedName = key;
            ClassHTable.Add(key, item);

        }
        #endregion

        #region Stage#01 : Collect Objects - Generate full qualified name for AstTypes

        /// <summary>
        /// This function started adding all the object(classes,struct and enum) into hashtable
        /// </summary>
        public void CollectObjects()
        {
            foreach (AstSourceFile SItem in _astProgram.SourceFiles)
            {
                //Add the using clauses to hashtable for later use
                _usingDeclarative.Clear();
                foreach (AstUsingDeclarative item in SItem.AstUsingDeclarativeCollection)
                {
                    if (!_usingDeclarative.Contains(item.Namespace))
                    {
                        _usingDeclarative.Add(item.Namespace, item);
                    }

                }

                foreach (AstType item in SItem.AstTypeCollection)
                    AddAstTypeNodes(item, "");

                foreach (AstNamespaceBlock item in SItem.AstNamespaceBlockCollection)
                    AddAstNamespaceBlockNodes(item, "");
            }
        }
        /// <summary>
        /// This class identify the type item and added to relative Hashtable
        /// </summary>
        /// <param name="item"></param>
        /// <param name="nsp"></param>
        private void AddAstTypeNodes(AstType item, string nsp)
        {
            string key;

            if (string.IsNullOrEmpty(nsp))
                key = item.Name;
            else
            {
                key = nsp + "." + item.Name;
            }

            if (item is AstClass)
            {
                AddClassItem((AstClass)item, key);
            }
            else if (item is AstEnum)
            {
                AddEnumItem((AstEnum)item, key);
            }
            else if (item is AstStruct)
            {
                AddStructItem((AstStruct)item, key);
            }
        }

        /// <summary>
        /// Checking inside the namespace block
        /// </summary>
        /// <param name="Nitem"></param>
        /// <param name="nsp"></param>
        private void AddAstNamespaceBlockNodes(AstNamespaceBlock Nitem, string nsp)
        {
            string strNameSpace;

            if (string.IsNullOrEmpty(nsp))
                strNameSpace = Nitem.Namespace;
            else
            {
                strNameSpace = nsp + "." + Nitem.Namespace;
            }

            foreach (AstType item in Nitem.AstTypeCollection)
                AddAstTypeNodes(item, strNameSpace);

            foreach (AstNamespaceBlock item in Nitem.AstNamespaceBlockCollection)
                AddAstNamespaceBlockNodes(item, strNameSpace);


        }
        #endregion


        #region Stage#01.5 - Generate full qualified name for Parent Class


        public void GenerateFullQualifiedNameForParentClass()
        {
            //this is similar to CollectObjects)
            foreach (AstSourceFile SItem in _astProgram.SourceFiles)
            {
                //Add the using clauses to hashtable for later use
                _usingDeclarative.Clear();
                foreach (AstUsingDeclarative item in SItem.AstUsingDeclarativeCollection)
                {
                    if (!_usingDeclarative.Contains(item.Namespace))
                        _usingDeclarative.Add(item.Namespace, item);
                }

                foreach (AstType item in SItem.AstTypeCollection)
                {
                    if (item is AstClass) // parent class is only for classes
                        GenerateFullQualifiedNameForParentClass((AstClass)item, "");
                }

                foreach (AstNamespaceBlock item in SItem.AstNamespaceBlockCollection)
                {
                    GenerateFullQualifiedNameForParentClass(item, item.Namespace);
                }
            }
        }

        private void GenerateFullQualifiedNameForParentClass(AstNamespaceBlock item, string ns)
        {
            foreach (AstType i in item.AstTypeCollection)
            {
                if (i is AstClass) // parent class is only for classes
                    GenerateFullQualifiedNameForParentClass((AstClass)i, ns);
            }

            foreach (AstNamespaceBlock i in item.AstNamespaceBlockCollection)
            {
                if (string.IsNullOrEmpty(ns))
                    ns = item.Namespace + "." + i.Namespace;
                else
                    ns += "." + i.Namespace;
                GenerateFullQualifiedNameForParentClass(i, ns);
            }
        }

        private void GenerateFullQualifiedNameForParentClass(AstClass item, string ns)
        {
            AstParentClass p = item.AstParentClass;

            if (p == null) // if no parent class then System.Object is parent by default
            {
                if (item.FullQualifiedName != "System.Object") // System.Object doesnt haven any parent class, tats the max it goes
                {
                    item.AstParentClass = new AstParentClass(item.Path, item.LineNumber, item.ColumnNumber);
                    item.AstParentClass.FullQualifiedName = "System.Object";
                }
                return;
            }

            if (string.IsNullOrEmpty(ns)) // if no ns, then check the using
            {
                bool found = false;
                foreach (string i in _usingDeclarative.Keys)
                {
                    if (ClassHTable.ContainsKey(i + "." + p.Name))
                    {
                        item.AstParentClass.FullQualifiedName = i + "." + p.Name;
                        found = true;
                    }
                }
                if (!found)
                {
                    found = ClassHTable.ContainsKey(p.Name); //check if it was qualified full name
                    if (found)
                        item.AstParentClass.FullQualifiedName = p.Name;
                    else
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Class " + p.Name + " not found (are you missing using declaratives)", p.LineNumber, p.ColumnNumber, p.Path));
                        _hasError = true;
                    }
                }
            }
            else
            {
                string toFind = ns + "." + p.Name;
                if (ClassHTable.ContainsKey(toFind))
                {
                    item.AstParentClass.FullQualifiedName = toFind;
                    return;
                }

                int cutIndex = toFind.LastIndexOf("." + p.Name);
                string temp = toFind.Substring(0, cutIndex);
                if (!temp.Contains("."))
                    GenerateFullQualifiedNameForParentClass(item, "");
                else
                    GenerateFullQualifiedNameForParentClass(item, temp.Substring(0, temp.LastIndexOf('.')));
            }
        }

        #endregion

        #region Stage#01.6 - Check circular class inheritance

        int ClassInheritCount = 0;
        //method to count number of classes that have inheiritance
        public int ClassInheiritanceCount(Hashtable CTable)
        {
            foreach (AstClass value in CTable.Values)
            {
                if (value.AstParentClass != null)
                {
                    if (value.AstParentClass.FullQualifiedName != "System.Object")
                    {
                        ClassInheritCount++;
                    }
                }
            }
            return ClassInheritCount;
        }

        public void CheckCircularInheiritance(Hashtable CTable, int ClassInheritCount, LLVMSharpCompiler Compiler)
        {
            if (ClassInheritCount == 0)
            { return; }
            //int i = 0; int j = 0;
            int count = 0;
            String[] classes = new String[ClassInheritCount];
            BooleanMatrix A1 = new BooleanMatrix(ClassInheritCount, ClassInheritCount);
            GeneralMatrix G1 = new GeneralMatrix(ClassInheritCount, ClassInheritCount);
            String[] parentclasses = new String[ClassInheritCount];
            Hashtable ClassInherit = new Hashtable();
            foreach (AstClass item in CTable.Values)
            {
                if (item.AstParentClass != null)
                {
                    if (item.AstParentClass.FullQualifiedName != "System.Object")
                    {
                        classes[count] = item.FullQualifiedName;
                        parentclasses[count] = item.AstParentClass.FullQualifiedName;
                        ClassInherit.Add(item.FullQualifiedName, item);
                        count++;
                    }
                }

            }
            for (int i = 0; i < ClassInheritCount; i++)
            {
                AstClass IClass = (AstClass)ClassInherit[classes[i]];

                for (int j = 0; j < ClassInheritCount; j++)
                {
                    AstClass JClass = (AstClass)ClassInherit[classes[j]];
                    if (IClass.AstParentClass != null)
                    {
                        if (IClass.AstParentClass.FullQualifiedName == JClass.FullQualifiedName)
                        {
                            A1[i, j] = true;
                            G1.SetElement(i, j, 1);
                        }
                    }

                }
            }



            //foreach (AstClass item in ClassInherit.Values)//rows for each class name
            //{
            //    j = 0;
            //    foreach (AstClass value in ClassInherit.Values)//columns// check if there's inheiritance from other classes
            //    {
            //        if (i < ClassInheritCount && item.AstParentClass.FullQualifiedName == value.FullQualifiedName)
            //        {
            //            A1[i, j] = true;
            //            G1.SetElement(i, j, 1);
            //            i++;
            //        }
            //        j++;
            //    }
            //}
            // Console.WriteLine(A1.ToString());
            int Mrank = G1.Rank();
            BooleanMatrix temp = new BooleanMatrix(ClassInheritCount, ClassInheritCount);
            BooleanMatrix C = new BooleanMatrix(ClassInheritCount, ClassInheritCount);
            temp = A1.Clone();
            C = A1.Clone();// Console.WriteLine("Matrix rank is "+Mrank);
            while (Mrank != 0)
            {  //C=C+C^2+C^3+..+C^rank(C)

                A1 = A1.Product(temp);
                C = C.Join(A1);
                Console.WriteLine(Mrank);
                Console.WriteLine(C.ToString());
                Mrank -= 1;
            }

            for (int x = 0; x < C.Height; x++)
            {
                for (int y = 0; y < C.Width; y++)
                {
                    if (x == y)
                    {
                        if (C[x, y] == true)
                        {
                            ErrorInfo e = new ErrorInfo();
                            AstClass TempNode;
                            //detected circular inheiritance for class, add error info 
                            e.message = "Classes " + classes[x] + ", " + parentclasses[x] + " involved in Circular Class Inheiritance";
                            TempNode = (AstClass)CTable[classes[x]];
                            e.line = TempNode.LineNumber;
                            e.col = TempNode.ColumnNumber;
                            e.type = ErrorType.SymenticError;
                            e.fileName = TempNode.Path;
                            Compiler.Errors.Add(e);//((AstNode)(CTable[classes[x]])).ToErrorInfo(e.message));
                            _hasError = true;
                        }
                    }
                }

            }
        }

        #endregion

        #region Stage#02 : Create Class Hierarchy

        public void CreateClassHierarchy()
        {
            ClassHierarchy = new ClassHierarchyNode();
            ClassHierarchy.AstClass = (AstClass)ClassHTable["System.Object"];

            foreach (AstClass item in ClassHTable.Values)
            {
                if (item.FullQualifiedName != "System.Object" && item.AstParentClass.FullQualifiedName == "System.Object")
                {
                    ClassHierarchyNode n = new ClassHierarchyNode();
                    n.AstClass = item;
                    ClassHierarchy.ClassHierarchyNodeCollection.Add(n);
                }
            }

            CreateClassHierarchy(ClassHierarchy.ClassHierarchyNodeCollection);

            //bool hasSystemObject = ClassHTable.ContainsKey("System.Object");

            //if (!hasSystemObject && ClassHTable.Count > 0)
            //{
            //    _hasError = true;
            //    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "System.Object not found", 0, 0, ""));
            //    return;
            //}
        }

        private void CreateClassHierarchy(ClassHierarchyNodeCollection classHierarchyNodeCollection)
        {
            if (classHierarchyNodeCollection != null && classHierarchyNodeCollection.Count > 0)
            {
                foreach (ClassHierarchyNode item in classHierarchyNodeCollection)
                {
                    foreach (AstClass ch in ClassHTable.Values)
                    {
                        if (ch.AstParentClass != null && ch.AstParentClass.FullQualifiedName == item.AstClass.FullQualifiedName)
                        {
                            ClassHierarchyNode n = new ClassHierarchyNode();
                            n.AstClass = ch;
                            item.ClassHierarchyNodeCollection.Add(n);
                        }
                    }
                    CreateClassHierarchy(item.ClassHierarchyNodeCollection);
                }
            }
        }

        #endregion

        #region Stage #02.01 GenerateFQN for Return Type
        public void GenerateFQReturnType()
        {
            foreach (AstSourceFile SItem in _astProgram.SourceFiles)
            {
                //Add the using clauses to hashtable for later use
                _usingDeclarative.Clear();
                foreach (AstUsingDeclarative item in SItem.AstUsingDeclarativeCollection)
                {
                    if (!_usingDeclarative.Contains(item.Namespace))
                        _usingDeclarative.Add(item.Namespace, item);
                }

                foreach (AstType item in SItem.AstTypeCollection)
                {
                    if (item is AstClass) // parent class is only for classes
                        GenerateFQReturnType((AstClass)item, "");
                    else if (item is AstStruct)
                        GenerateFQReturnType((AstStruct)item, "");
                }

                foreach (AstNamespaceBlock item in SItem.AstNamespaceBlockCollection)
                {
                    GenerateFQReturnType(item, item.Namespace);
                }
            }
        }

        private void GenerateFQReturnType(AstNamespaceBlock item, string ns)
        {
            foreach (AstType i in item.AstTypeCollection)
            {
                if (i is AstClass) // parent class is only for classes
                    GenerateFQReturnType((AstClass)i, ns);
                else if (i is AstStruct)
                    GenerateFQReturnType((AstStruct)i, ns);
            }

            foreach (AstNamespaceBlock i in item.AstNamespaceBlockCollection)
            {
                if (string.IsNullOrEmpty(ns))
                    ns = item.Namespace + "." + i.Namespace;
                else
                    ns += "." + i.Namespace;
                GenerateFQReturnType(i, ns);
            }
        }

        private void GenerateFQReturnType(AstStruct astStruct, string ns)
        {
            GenerateFQReturnType(astStruct.AstMethodCollection, ns, astStruct.FullQualifiedName, false);
            GenerateFQReturnType(astStruct.AstFieldCollection, ns, astStruct.FullQualifiedName);
        }

        private void GenerateFQReturnType(AstClass astClass, string ns)
        {
            GenerateFQReturnType(astClass.AstMethodCollection, ns, astClass.FullQualifiedName, true);
            GenerateFQReturnType(astClass.AstConstructorCollection, ns, astClass.FullQualifiedName, true);
            GenerateFQReturnType(astClass.AstFieldCollection, ns, astClass.FullQualifiedName);
            GenerateFQReturnType(astClass.AstTypeConverterCollection, ns, astClass.FullQualifiedName, true);
            GenerateFQReturnType(astClass.AstOperatorOverloadCollection, ns, astClass.FullQualifiedName, true);
        }

        private void GenerateFQReturnType(AstOperatorOverloadCollection astOperatorOverloadCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstOperatorOverload item in astOperatorOverloadCollection)
            {
                GenerateFQReturnType(item, ns, fullType, isClass);
            }
        }

        private void GenerateFQReturnType(AstOperatorOverload astOpOverload, string ns, string fullType, bool isClass)
        {
            bool builtin = false;

            string primitiveType = Mangler.Instance.MapPrimitivesToFullQualifiedName(astOpOverload.ReturnType);
            if (!string.IsNullOrEmpty(primitiveType))
            {
                builtin = true;
                astOpOverload.FullQReturnType = primitiveType;
            }

            if (string.IsNullOrEmpty(ns))
            {
                bool found = false;
                foreach (string i in _usingDeclarative.Keys)
                {
                    string temp_full = i + "." + astOpOverload.ReturnType;
                    //using declarative
                    if (!builtin)
                    {
                        if (ClassHTable.ContainsKey(temp_full) || StructHTable.ContainsKey(temp_full) || EnumHTable.ContainsKey(temp_full))
                        {
                            astOpOverload.FullQReturnType = temp_full;
                            found = true;
                        }
                    }
                    else
                    {
                        if (ClassHTable.ContainsKey(astOpOverload.FullQReturnType) || StructHTable.ContainsKey(astOpOverload.FullQReturnType) || EnumHTable.ContainsKey(astOpOverload.FullQReturnType))
                        {
                            // method.FullQReturnType already contains the full name
                            found = true;
                        }
                    }
                }
                //check if using fully qualified name
                if (!found)
                {
                    string returnTypetoCheck = astOpOverload.FullQReturnType ?? astOpOverload.ReturnType;
                    if (ClassHTable.ContainsKey(returnTypetoCheck) || StructHTable.ContainsKey(returnTypetoCheck) || EnumHTable.ContainsKey(returnTypetoCheck))
                    {
                        astOpOverload.FullQReturnType = returnTypetoCheck;
                        found = true;
                    }
                    else
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Type " + astOpOverload.ReturnType + " not found (are you missing using declaratives)", astOpOverload.LineNumber, astOpOverload.ColumnNumber, astOpOverload.Path));
                        _hasError = true;
                    }
                }

            }
            else
            {
                string toFind = ns + "." + astOpOverload.ReturnType;
                if (ClassHTable.ContainsKey(toFind) || StructHTable.ContainsKey(toFind) || EnumHTable.ContainsKey(toFind))
                {
                    astOpOverload.FullQReturnType = toFind;
                    return;
                }
                int cutIndex = toFind.LastIndexOf("." + astOpOverload.ReturnType);
                string temp = toFind.Substring(0, cutIndex);
                if (!temp.Contains("."))
                    GenerateFQReturnType(astOpOverload, "", fullType, isClass);
                else
                    GenerateFQReturnType(astOpOverload, temp.Substring(0, temp.LastIndexOf('.')), fullType, isClass);
            }
        }

        private void GenerateFQReturnType(AstTypeConverterCollection astTypeConverterCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstTypeConverter astTypeConverter in astTypeConverterCollection)
            {
                GenerateFQReturnType(astTypeConverter, ns, fullType, isClass);
            }
        }

        private void GenerateFQReturnType(AstTypeConverter astTypeConverter, string ns, string fullType, bool isClass)
        {
            bool builtin = false;

            string primitiveType = Mangler.Instance.MapPrimitivesToFullQualifiedName(astTypeConverter.ReturnType);
            if (!string.IsNullOrEmpty(primitiveType))
            {
                builtin = true;
                astTypeConverter.FullQReturnType = primitiveType;
            }

            if (string.IsNullOrEmpty(ns))
            {
                bool found = false;
                foreach (string i in _usingDeclarative.Keys)
                {
                    string temp_full = i + "." + astTypeConverter.ReturnType;
                    //using declarative
                    if (!builtin)
                    {
                        if (ClassHTable.ContainsKey(temp_full) || StructHTable.ContainsKey(temp_full) || EnumHTable.ContainsKey(temp_full))
                        {
                            astTypeConverter.FullQReturnType = temp_full;
                            found = true;
                        }
                    }
                    else
                    {
                        if (ClassHTable.ContainsKey(astTypeConverter.FullQReturnType) || StructHTable.ContainsKey(astTypeConverter.FullQReturnType) || EnumHTable.ContainsKey(astTypeConverter.FullQReturnType))
                        {
                            // method.FullQReturnType already contains the full name
                            found = true;
                        }
                    }
                }
                //check if using fully qualified name
                if (!found)
                {
                    string returnTypetoCheck = astTypeConverter.FullQReturnType ?? astTypeConverter.ReturnType;
                    if (ClassHTable.ContainsKey(returnTypetoCheck) || StructHTable.ContainsKey(returnTypetoCheck) || EnumHTable.ContainsKey(returnTypetoCheck))
                    {
                        astTypeConverter.FullQReturnType = returnTypetoCheck;
                        found = true;
                    }
                    else
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Type " + astTypeConverter.ReturnType + " not found (are you missing using declaratives)", astTypeConverter.LineNumber, astTypeConverter.ColumnNumber, astTypeConverter.Path));
                        _hasError = true;
                    }
                }

            }
            else
            {
                string toFind = ns + "." + astTypeConverter.ReturnType;
                if (ClassHTable.ContainsKey(toFind) || StructHTable.ContainsKey(toFind) || EnumHTable.ContainsKey(toFind))
                {
                    astTypeConverter.FullQReturnType = toFind;
                    return;
                }
                int cutIndex = toFind.LastIndexOf("." + astTypeConverter.ReturnType);
                string temp = toFind.Substring(0, cutIndex);
                if (!temp.Contains("."))
                    GenerateFQReturnType(astTypeConverter, "", fullType, isClass);
                else
                    GenerateFQReturnType(astTypeConverter, temp.Substring(0, temp.LastIndexOf('.')), fullType, isClass);
            }

        }

        private void GenerateFQReturnType(AstConstructorCollection astConstructorCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstConstructor item in astConstructorCollection)
            {
                item.FullQName = fullType;
            }
        }


        private void GenerateFQReturnType(AstMethodCollection astMethodCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstMethod item in astMethodCollection)
            {
                GenerateFQReturnType(item, ns, fullType, isClass);
            }
        }

        private void GenerateFQReturnType(AstFieldCollection astFieldCollection, string ns, string fullType)
        {
            foreach (AstField astField in astFieldCollection)
            {
                GenerateFQReturnType(astField, ns, fullType);
            }
        }

        private void GenerateFQReturnType(AstField astField, string ns, string fullType)
        {
            string primitiveType = Mangler.Instance.MapPrimitivesToFullQualifiedName(astField.Type);

            bool builtin = false;
            if (!string.IsNullOrEmpty(primitiveType))
            {
                builtin = true;
                astField.FullQualifiedType = primitiveType;
            }

            if (string.IsNullOrEmpty(ns))
            {
                bool found = false;
                foreach (string i in _usingDeclarative.Keys)
                {
                    string tempFull = i + "." + astField.Type;

                    if (!builtin)
                    {
                        if (ClassHTable.ContainsKey(tempFull) || StructHTable.ContainsKey(tempFull) ||
                            EnumHTable.ContainsKey(tempFull))
                        {
                            astField.FullQualifiedType = tempFull;
                            found = true;
                        }
                    }
                    else
                    {
                        if (ClassHTable.ContainsKey(astField.FullQualifiedType) ||
                            StructHTable.ContainsKey(astField.FullQualifiedType) ||
                            EnumHTable.ContainsKey(astField.FullQualifiedType))
                        {
                            // astField.FullQReturnType already contains the full name
                            found = true;
                        }
                    }
                }
                //check if using fully qualified name
                if (!found)
                {
                    string returnTypeToCheck = astField.FullQualifiedType ?? astField.Type;
                    if (ClassHTable.ContainsKey(returnTypeToCheck) || StructHTable.ContainsKey(returnTypeToCheck) || EnumHTable.ContainsKey(returnTypeToCheck))
                    {
                        astField.FullQualifiedType = returnTypeToCheck;
                        found = true;
                    }
                    else
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Type " + astField.Type + " not found (are you missing using declaratives)", astField.LineNumber, astField.ColumnNumber, astField.Path));
                        _hasError = true;
                    }
                }
            }
            else
            {
                string toFind = ns + "." + astField.Type;
                if (ClassHTable.ContainsKey(toFind) || StructHTable.ContainsKey(toFind) || EnumHTable.ContainsKey(toFind))
                {
                    astField.FullQualifiedType = toFind;
                    return;
                }
                int cutIndex = toFind.LastIndexOf("." + astField.Type);
                string temp = toFind.Substring(0, cutIndex);
                if (!temp.Contains("."))
                    GenerateFQReturnType(astField, "", fullType);
                else
                    GenerateFQReturnType(astField, temp.Substring(0, temp.LastIndexOf('.')), fullType);
            }
        }

        private void GenerateFQReturnType(AstMethod method, string ns, string fullType, bool isClass)
        {
            bool builtin = false;

            string primitiveType = Mangler.Instance.MapPrimitivesToFullQualifiedName(method.ReturnType);
            if (!string.IsNullOrEmpty(primitiveType))
            {
                builtin = true;
                method.FullQReturnType = primitiveType;
            }
            if (string.IsNullOrEmpty(ns))
            {
                bool found = false;
                foreach (string i in _usingDeclarative.Keys)
                {
                    string temp_full = i + "." + method.ReturnType;
                    //using declarative
                    if (!builtin)
                    {
                        if (ClassHTable.ContainsKey(temp_full) || StructHTable.ContainsKey(temp_full) || EnumHTable.ContainsKey(temp_full))
                        {
                            method.FullQReturnType = temp_full;
                            found = true;
                        }
                    }
                    else
                    {
                        if (ClassHTable.ContainsKey(method.FullQReturnType) || StructHTable.ContainsKey(method.FullQReturnType) || EnumHTable.ContainsKey(method.FullQReturnType))
                        {
                            // method.FullQReturnType already contains the full name
                            found = true;
                        }
                    }
                }
                //check if using fully qualified name
                if (!found)
                {
                    string returnTypetoCheck = method.FullQReturnType ?? method.ReturnType;
                    if (ClassHTable.ContainsKey(returnTypetoCheck) || StructHTable.ContainsKey(returnTypetoCheck) || EnumHTable.ContainsKey(returnTypetoCheck))
                    {
                        method.FullQReturnType = returnTypetoCheck;
                        found = true;
                    }
                    else
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Type " + method.ReturnType + " not found (are you missing using declaratives)", method.LineNumber, method.ColumnNumber, method.Path));
                        _hasError = true;
                    }
                }

            }
            else
            {
                string toFind = ns + "." + method.ReturnType;
                if (ClassHTable.ContainsKey(toFind) || StructHTable.ContainsKey(toFind) || EnumHTable.ContainsKey(toFind))
                {
                    method.FullQReturnType = toFind;
                    return;
                }
                int cutIndex = toFind.LastIndexOf("." + method.ReturnType);
                string temp = toFind.Substring(0, cutIndex);
                if (!temp.Contains("."))
                    GenerateFQReturnType(method, "", fullType, isClass);
                else
                    GenerateFQReturnType(method, temp.Substring(0, temp.LastIndexOf('.')), fullType, isClass);
            }

        }

        #endregion

        #region Generate FQT for Parameters
        public void GenerateFQTParameters()
        {
            foreach (AstSourceFile SItem in _astProgram.SourceFiles)
            {
                _usingDeclarative.Clear();
                foreach (AstUsingDeclarative item in SItem.AstUsingDeclarativeCollection)
                {
                    if (!_usingDeclarative.Contains(item.Namespace))
                        _usingDeclarative.Add(item.Namespace, item);
                }
                foreach (AstType item in SItem.AstTypeCollection)
                {
                    if (item is AstClass) // parent class is only for classes
                        GenerateFQTParameters((AstClass)item, "");
                    else if (item is AstStruct)
                        GenerateFQTParameters((AstStruct)item, "");
                }

                foreach (AstNamespaceBlock item in SItem.AstNamespaceBlockCollection)
                {
                    GenerateFQTParameters(item, item.Namespace);
                }
            }
        }

        private void GenerateFQTParameters(AstNamespaceBlock item, string ns)
        {
            foreach (AstType i in item.AstTypeCollection)
            {
                if (i is AstClass) // parent class is only for classes
                    GenerateFQTParameters((AstClass)i, ns);
                else if (i is AstStruct)
                    GenerateFQTParameters((AstStruct)i, ns);
            }

            foreach (AstNamespaceBlock i in item.AstNamespaceBlockCollection)
            {
                if (string.IsNullOrEmpty(ns))
                    ns = item.Namespace + "." + i.Namespace;
                else
                    ns += "." + i.Namespace;
                GenerateFQTParameters(i, ns);
            }
        }

        private void GenerateFQTParameters(AstStruct astStruct, string ns)
        {
            GenerateFQTParameters(astStruct.AstMethodCollection, ns, astStruct.FullQualifiedName, false);
            GenerateFQTAccessors(astStruct.AstAccessorCollection, ns, astStruct.FullQualifiedName, false);
            GenerateFQTParameters(astStruct.AstConstructorCollection, ns, astStruct.FullQualifiedName, false);
            GenerateFQTParameters(astStruct.AstOperatorOverloadCollection, ns, astStruct.FullQualifiedName, false);
            GenerateFQTParameters(astStruct.AstTypeConverterCollection, ns, astStruct.FullQualifiedName, false);
        }

        private void GenerateFQTParameters(AstClass astClass, string ns)
        {
            GenerateFQTParameters(astClass.AstConstructorCollection, ns, astClass.FullQualifiedName, true);
            GenerateFQTParameters(astClass.AstMethodCollection, ns, astClass.FullQualifiedName, true);
            GenerateFQTAccessors(astClass.AstAccessorCollection, ns, astClass.FullQualifiedName, true);
            GenerateFQTParameters(astClass.AstTypeConverterCollection, ns, astClass.FullQualifiedName, true);
            GenerateFQTParameters(astClass.AstOperatorOverloadCollection, ns, astClass.FullQualifiedName, true);
        }


        private void GenerateFQTParameters(AstConstructorCollection astConstructorCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstConstructor astConstructor in astConstructorCollection)
            {
                GenerateFQTParameters(astConstructor, ns, fullType, isClass);
            }
        }

        private void GenerateFQTParameters(AstConstructor astConstructor, string ns, string fullType, bool isClass)
        {
            foreach (AstParameter astParam in astConstructor.Parameters)
            {
                GenerateFQTParameters(astParam, ns, fullType, isClass);
            }
        }

        private void GenerateFQTParameters(AstMethodCollection astMethodCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstMethod item in astMethodCollection)
            {
                GenerateFQTParameters(item, ns, fullType, isClass);
            }
        }
        private void GenerateFQTParameters(AstOperatorOverloadCollection astOpOverloadCollect, string ns, string fullType, bool isClass)
        {
            foreach (AstOperatorOverload item in astOpOverloadCollect)
            {
                if (item.AstParameter1 != null)
                {
                    GenerateFQTParameters(item.AstParameter1, ns, fullType, isClass);
                }
                if (item.AstParameter2 != null)
                {
                    GenerateFQTParameters(item.AstParameter2, ns, fullType, isClass);
                }

            }
        }
        private void GenerateFQTParameters(AstTypeConverterCollection astTypeConvCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstTypeConverter item in astTypeConvCollection)
            {
                GenerateFQTParameters(item.AstParameter, ns, fullType, isClass);
            }
        }
        private void GenerateFQTParameters(AstMethod astMethod, string ns, string fullType, bool isClass)
        {
            foreach (AstParameter item in astMethod.Parameters)
            {
                GenerateFQTParameters(item, ns, fullType, isClass);
            }

        }

        private void GenerateFQTAccessors(AstAccessorCollection astAccessorCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstAccessor astAccessor in astAccessorCollection)
            {
                GenerateFQTAccessors(astAccessor, ns, fullType, isClass);
            }

        }
        private void GenerateFQTAccessors(AstAccessor astAccessor, string ns, string fullType, bool isClass)
        {
            bool builtin = false;

            string primitiveType = Mangler.Instance.MapPrimitivesToFullQualifiedName(astAccessor.ReturnType);
            if (!string.IsNullOrEmpty(primitiveType))
            {
                builtin = true;
                astAccessor.FullyQualifiedType = primitiveType;
            }

            if (string.IsNullOrEmpty(ns))
            {
                bool found = false;

                foreach (string i in _usingDeclarative.Keys)
                {
                    string temp_full = i + "." + astAccessor.ReturnType;
                    if (!builtin)
                    {
                        if (ClassHTable.ContainsKey(temp_full) || StructHTable.ContainsKey(temp_full) || EnumHTable.ContainsKey(temp_full))
                        {
                            astAccessor.FullyQualifiedType = temp_full;
                            found = true;
                        }
                    }
                    else
                    {
                        if (ClassHTable.ContainsKey(astAccessor.FullyQualifiedType) || StructHTable.ContainsKey(astAccessor.FullyQualifiedType) || EnumHTable.ContainsKey(astAccessor.FullyQualifiedType))
                        {
                            // parameter.FullQualifiedType already contains the full name
                            found = true;
                        }
                    }
                }
                if (!found) // check if using fully qualified name
                {
                    string returnTypetoCheck = astAccessor.FullyQualifiedType ?? astAccessor.ReturnType;
                    if (ClassHTable.ContainsKey(returnTypetoCheck) || StructHTable.ContainsKey(returnTypetoCheck) || EnumHTable.ContainsKey(returnTypetoCheck))
                    {
                        astAccessor.FullyQualifiedType = returnTypetoCheck;
                        found = true;
                    }
                    else
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Type " + astAccessor.ReturnType + " not found (are you missing using declaratives)", astAccessor.LineNumber, astAccessor.ColumnNumber, astAccessor.Path));
                        _hasError = true;
                    }
                }
            }
            else
            {
                string toFind = ns + "." + astAccessor.ReturnType;
                if (ClassHTable.ContainsKey(toFind) || StructHTable.ContainsKey(toFind) || EnumHTable.ContainsKey(toFind))
                {
                    astAccessor.FullyQualifiedType = toFind;
                    return;
                }
                int cutIndex = toFind.LastIndexOf("." + astAccessor.ReturnType);
                string temp = toFind.Substring(0, cutIndex);
                if (!temp.Contains("."))
                    GenerateFQTAccessors(astAccessor, "", fullType, isClass);
                else
                    GenerateFQTAccessors(astAccessor, temp.Substring(0, temp.LastIndexOf('.')), fullType, isClass);
            }
        }
        private void GenerateFQTParameters(AstParameter parameter, string ns, string fullType, bool isClass)
        {
            bool builtin = false;

            string primitiveType = Mangler.Instance.MapPrimitivesToFullQualifiedName(parameter.Type);
            if (!string.IsNullOrEmpty(primitiveType))
            {
                builtin = true;
                parameter.FullQualifiedType = primitiveType;
            }

            if (string.IsNullOrEmpty(ns))
            {
                bool found = false;

                foreach (string i in _usingDeclarative.Keys)
                {
                    string temp_full = i + "." + parameter.Type;

                    if (!builtin)
                    {
                        if (ClassHTable.ContainsKey(temp_full) || StructHTable.ContainsKey(temp_full) || EnumHTable.ContainsKey(temp_full))
                        {
                            parameter.FullQualifiedType = temp_full;
                            found = true;
                        }
                    }
                    else
                    {
                        if (ClassHTable.ContainsKey(parameter.FullQualifiedType) || StructHTable.ContainsKey(parameter.FullQualifiedType) || EnumHTable.ContainsKey(parameter.FullQualifiedType))
                        {
                            // parameter.FullQualifiedType already contains the full name
                            found = true;
                        }
                    }
                }
                if (!found) // check if using fully qualified name
                {
                    string returnTypetoCheck = parameter.FullQualifiedType ?? parameter.Type;
                    if (ClassHTable.ContainsKey(returnTypetoCheck) || StructHTable.ContainsKey(returnTypetoCheck) || EnumHTable.ContainsKey(returnTypetoCheck))
                    {
                        parameter.FullQualifiedType = returnTypetoCheck;
                        found = true;
                    }
                    else
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Type " + parameter.Type + " not found (are you missing using declaratives)", parameter.LineNumber, parameter.ColumnNumber, parameter.Path));
                        _hasError = true;
                    }
                }
            }
            else
            {
                string toFind = ns + "." + parameter.Type;
                if (ClassHTable.ContainsKey(toFind) || StructHTable.ContainsKey(toFind) || EnumHTable.ContainsKey(toFind))
                {
                    parameter.FullQualifiedType = toFind;
                    return;
                }
                int cutIndex = toFind.LastIndexOf("." + parameter.Type);
                string temp = toFind.Substring(0, cutIndex);
                if (!temp.Contains("."))
                    GenerateFQTParameters(parameter, "", fullType, isClass);
                else
                    GenerateFQTParameters(parameter, temp.Substring(0, temp.LastIndexOf('.')), fullType, isClass);
            }

        }
        #endregion

        #region

        public void GenerateFQTLocalVars()
        {
            foreach (AstSourceFile SItem in _astProgram.SourceFiles)
            {
                _usingDeclarative.Clear();
                foreach (AstUsingDeclarative item in SItem.AstUsingDeclarativeCollection)
                {
                    if (!_usingDeclarative.Contains(item.Namespace))
                        _usingDeclarative.Add(item.Namespace, item);
                }
                foreach (AstType item in SItem.AstTypeCollection)
                {
                    if (item is AstClass) // parent class is only for classes
                        GenerateFQTLocalVars((AstClass)item, "");
                    else if (item is AstStruct)
                        GenerateFQTLocalVars((AstStruct)item, "");
                }

                foreach (AstNamespaceBlock item in SItem.AstNamespaceBlockCollection)
                {
                    GenerateFQTLocalVars(item, item.Namespace);
                }
            }
        }

        private void GenerateFQTLocalVars(AstNamespaceBlock item, string ns)
        {
            foreach (AstType i in item.AstTypeCollection)
            {
                if (i is AstClass) // parent class is only for classes
                    GenerateFQTLocalVars((AstClass)i, ns);
                else if (i is AstStruct)
                    GenerateFQTLocalVars((AstStruct)i, ns);
            }

            foreach (AstNamespaceBlock i in item.AstNamespaceBlockCollection)
            {
                if (string.IsNullOrEmpty(ns))
                    ns = item.Namespace + "." + i.Namespace;
                else
                    ns += "." + i.Namespace;
                GenerateFQTLocalVars(i, ns);
            }
        }

        private void GenerateFQTLocalVars(AstStruct astStruct, string ns)
        {
            GenerateFQTLocalVars(astStruct.AstMethodCollection, ns, astStruct.FullQualifiedName, false);
        }
        private void GenerateFQTLocalVars(AstClass astClass, string ns)
        {
            GenerateFQTLocalVars(astClass.AstConstructorCollection, ns, astClass.FullQualifiedName, true);
            GenerateFQTLocalVars(astClass.AstMethodCollection, ns, astClass.FullQualifiedName, true);
            GenerateFQTLocalVars(astClass.AstAccessorCollection, ns, astClass.FullQualifiedName, true);
        }

        private void GenerateFQTLocalVars(AstAccessorCollection astAccessorCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstAccessor astAccessor in astAccessorCollection)
            {
                GenerateFQTLocalVars(astAccessor, ns, fullType, isClass);
            }
        }

        private void GenerateFQTLocalVars(AstConstructorCollection astConstructorCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstConstructor astConstructor in astConstructorCollection)
            {
                GenerateFQTLocalVars(astConstructor, ns, fullType, isClass);
            }
        }

        private void GenerateFQTLocalVars(AstMethodCollection astMethodCollection, string ns, string fullType, bool isClass)
        {
            foreach (AstMethod item in astMethodCollection)
            {
                GenerateFQTLocalVars(item, ns, fullType, isClass);
            }
        }

        private void GenerateFQTLocalVars(AstMethod astMethod, string ns, string fullType, bool isClass)
        {
            GenerateFQTLocalVars(astMethod.AstBlock, ns, fullType, isClass);
        }
        private void GenerateFQTLocalVars(AstBlock astBlock, string ns, string fullType, bool isClass)
        {
            if (astBlock != null)
            {
                foreach (AstStatement astStatement in astBlock.AstStatementCollection)
                {
                    if (astStatement is AstLocalVariableDeclaration)
                    {
                        AstLocalVariableDeclaration tempLocal = (AstLocalVariableDeclaration)astStatement;
                        GenerateFQTLocalVars(tempLocal, ns, fullType, isClass);
                    }
                    else if (astStatement is AstLoop)
                    {
                        if (astStatement is AstDoLoop)//check body of dowhile loop
                        {
                            AstDoLoop tempDoLoop = (AstDoLoop)astStatement;
                            if (tempDoLoop.AstStatement is AstLocalVariableDeclaration)
                            {
                                AstLocalVariableDeclaration tempLocal = (AstLocalVariableDeclaration)astStatement;
                                GenerateFQTLocalVars(tempLocal, ns, fullType, isClass);
                            }
                            else if (tempDoLoop.AstStatement is AstBlock)
                            {
                                AstBlock astBlockTemp = (AstBlock)tempDoLoop.AstStatement;
                                GenerateFQTLocalVars(astBlockTemp, ns, fullType, isClass);
                            }
                        }//end of checking dowhile loop
                        else if (astStatement is AstForLoop)//checking forloop local variables body
                        {
                            AstForLoop tempForLoop = (AstForLoop)astStatement;
                            foreach (AstStatement item in tempForLoop.Initializers)
                            {
                                if (item is AstLocalVariableDeclaration)
                                {
                                    AstLocalVariableDeclaration tempLocal = (AstLocalVariableDeclaration)item;
                                    GenerateFQTLocalVars(tempLocal, ns, fullType, isClass);
                                }
                                if (item is AstBlock)
                                {
                                    AstBlock astBlocktemp = (AstBlock)item;
                                    GenerateFQTLocalVars(astBlocktemp, ns, fullType, isClass);
                                }
                            }
                            if (tempForLoop.Body is AstLocalVariableDeclaration)
                            {
                                AstLocalVariableDeclaration tempLocal = (AstLocalVariableDeclaration)tempForLoop.Body;
                                GenerateFQTLocalVars(tempLocal, ns, fullType, isClass);
                            }
                            else if (tempForLoop.Body is AstBlock)
                            {
                                AstBlock astBlocktemp = (AstBlock)tempForLoop.Body;
                                GenerateFQTLocalVars(astBlocktemp, ns, fullType, isClass);
                            }
                        }//end check of for loop body
                        else if (astStatement is AstWhileLoop)//check astwhile loop
                        {
                            AstWhileLoop astTempWhile = (AstWhileLoop)astStatement;
                            if (astTempWhile.AstStatement is AstBlock)
                            {
                                AstBlock astBlocktemp = (AstBlock)astTempWhile.AstStatement;
                                GenerateFQTLocalVars(astBlocktemp, ns, fullType, isClass);
                            }
                            else if (astTempWhile.AstStatement is AstLocalVariableDeclaration)
                            {
                                AstLocalVariableDeclaration tempLocal = (AstLocalVariableDeclaration)astTempWhile.AstStatement;
                                GenerateFQTLocalVars(tempLocal, ns, fullType, isClass);
                            }
                        }//end of check
                    }
                    else if (astStatement is AstConditional)
                    {
                        if (astStatement is AstIfCondition)
                        {
                            AstIfCondition tempIfAst = (AstIfCondition)astStatement;
                            GenerateFQTLocalVars(tempIfAst, ns, fullType, isClass);
                        }
                    }
                }
            }

        }
        private void GenerateFQTLocalVars(AstIfCondition astIfCondition, string ns, string fullType, bool isClass)
        {
            if (astIfCondition.AstStatement is AstBlock)
            {
                AstBlock astBlockTemp = (AstBlock)astIfCondition.AstStatement;
                GenerateFQTLocalVars(astBlockTemp, ns, fullType, isClass);
            }
            if (astIfCondition.AstStatementElse is AstBlock)
            {
                AstBlock astBlockTemp = (AstBlock)astIfCondition.AstStatementElse;
                GenerateFQTLocalVars(astBlockTemp, ns, fullType, isClass);
            }
            else if (astIfCondition.AstStatementElse is AstIfCondition)
            {
                AstIfCondition astTempIFCondition = (AstIfCondition)astIfCondition.AstStatementElse;
                GenerateFQTLocalVars(astTempIFCondition, ns, fullType, isClass);
            }
        }
        private void GenerateFQTLocalVars(AstLocalVariableDeclaration localvar, string ns, string fullType, bool isClass)
        {
            bool builtin = false;

            string primitiveType = Mangler.Instance.MapPrimitivesToFullQualifiedName(localvar.Type);
            if (!string.IsNullOrEmpty(primitiveType))
            {
                builtin = true;
                localvar.FullQualifiedType = primitiveType;
            }

            if (string.IsNullOrEmpty(ns))
            {
                bool found = false;

                foreach (string i in _usingDeclarative.Keys)
                {
                    string temp_full = i + "." + localvar.Type;

                    if (!builtin)
                    {
                        if (ClassHTable.ContainsKey(temp_full) || StructHTable.ContainsKey(temp_full) || EnumHTable.ContainsKey(temp_full))
                        {
                            localvar.FullQualifiedType = temp_full;
                            found = true;
                        }
                    }
                    else
                    {
                        if (ClassHTable.ContainsKey(localvar.FullQualifiedType) || StructHTable.ContainsKey(localvar.FullQualifiedType) || EnumHTable.ContainsKey(localvar.FullQualifiedType))
                        {
                            // parameter.FullQualifiedType already contains the full name
                            found = true;
                        }
                    }
                }
                if (!found) // check if using fully qualified name
                {
                    string returnTypetoCheck = localvar.FullQualifiedType ?? localvar.Type;
                    if (ClassHTable.ContainsKey(returnTypetoCheck) || StructHTable.ContainsKey(returnTypetoCheck) || EnumHTable.ContainsKey(returnTypetoCheck))
                    {
                        localvar.FullQualifiedType = returnTypetoCheck;
                        found = true;
                    }
                    else
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError, "Type " + localvar.Type + " not found (are you missing using declaratives)", localvar.LineNumber, localvar.ColumnNumber, localvar.Path));
                        _hasError = true;
                    }
                }
            }
            else
            {
                string toFind = ns + "." + localvar.Type;
                if (ClassHTable.ContainsKey(toFind) || StructHTable.ContainsKey(toFind) || EnumHTable.ContainsKey(toFind))
                {
                    localvar.FullQualifiedType = toFind;
                    return;
                }
                int cutIndex = toFind.LastIndexOf("." + localvar.Type);
                string temp = toFind.Substring(0, cutIndex);
                if (!temp.Contains("."))
                    GenerateFQTNewType(localvar, "", fullType, isClass);
                else
                    GenerateFQTNewType(localvar, temp.Substring(0, temp.LastIndexOf('.')), fullType, isClass);
            }

        }
        private void GenerateFQTLocalVars(AstConstructor astConstructor, string ns, string fullType, bool isClass)
        {
            GenerateFQTLocalVars(astConstructor.AstBlock, ns, fullType, isClass);
        }
        private void GenerateFQTLocalVars(AstAccessor astAccessor, string ns, string fullType, bool isClass)
        {
            if (astAccessor.AstGetAccessor != null)
            {
                GenerateFQTLocalVars(astAccessor.AstGetAccessor, ns, fullType, isClass);
            }
            if (astAccessor.AstSetAccessor != null)
            {
                GenerateFQTLocalVars(astAccessor.AstSetAccessor, ns, fullType, isClass);
            }

        }
        private void GenerateFQTLocalVars(AstGetAccessor astGetAccess, string ns, string fullType, bool isClass)
        {
            if (astGetAccess.AstBlock != null)
            {
                GenerateFQTLocalVars(astGetAccess.AstBlock, ns, fullType, isClass);
            }
        }
        private void GenerateFQTLocalVars(AstSetAccessor astSetAccess, string ns, string fullType, bool isClass)
        {
            if (astSetAccess.AstBlock != null)
            {
                GenerateFQTLocalVars(astSetAccess.AstBlock, ns, fullType, isClass);
            }
        }
        #endregion

        #region Stage#02.1 : Add method table info

        public void AddMethodTableInfo()
        {
            // _methodTable = new MethodTable();
            //_methodTable.generateMethodTable();           
            /* _c_ ctors, 
             * _m_ methods and 
             * _get_ _set_ properties get_ set_
             */
        }

        #region Manglers

        public static string Mangle(string ns)
        {
            if (string.IsNullOrEmpty(ns))
                return string.Empty;

            string[] s = ns.Split('.');
            StringBuilder sb = new StringBuilder();
            foreach (string item in s)
            {
                sb.Append(item.Length);
                sb.Append(item);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Mangles normal methods
        /// </summary>
        /// <param name="fullTypeName">Full name for class or struct</param>
        /// <param name="astMethod"></param>
        /// <returns></returns>
        public static string MangleMethodName(string fullTypeName, AstMethod astMethod)
        {
            return null;
        }

        public static string MangleCtors(string fullTypeName, AstConstructor astConstructor)
        {
            return null;
        }

        public static string MangleProperty(string fullTypeName, AstAccessor astAccessor, bool isGet)
        {
            return null;
        }

        public static string MangleField(string fullTypename, AstField astField)
        {
            return null;
        }

        #endregion

        #endregion

        #region Stage#02.2 : Add method table info

        private void CreateObjectLayout()
        {
            //ObjectLayoutNode o = new ObjectLayoutNode(null);
            //o.ClassHierarchyNode.AstClass.AstMethodCollection 
        }

        #endregion

        #region Stage#02.5 : Add Object Layout

        public void GenerateObjectLayoutStructs(LLVMSharpCompiler Compiler) //For Structs
        {
            ErrorInfo e = new ErrorInfo();
            foreach (AstStruct astStruct in Compiler.StructHashtable.Values)
            {
                foreach (AstField astField in astStruct.AstFieldCollection)
                {
                    if (!astField.AstMemberModifierCollection.Validate(Compiler.Errors))
                    { continue; }


                    if (astStruct.ObjectLayout.Contains(Mangler.Instance.MangleField(astField, false)))
                    {
                        e.col = astField.ColumnNumber;
                        e.fileName = astField.Path;
                        e.line = astField.LineNumber;
                        e.type = ErrorType.SymenticError;
                        e.message = "Type '" + astStruct.Name + "' already contains a defined field '" + astField.Name + "'";
                        Compiler.Errors.Add(e);
                        continue;
                    }
                    if (astField.AstMemberModifierCollection.IsProtected)
                    {
                        e.col = astField.ColumnNumber;
                        e.fileName = astField.Path;
                        e.line = astField.LineNumber;
                        e.message = "Protected modifier is invalid for struct types";
                        e.type = ErrorType.SymenticError;
                        Compiler.Errors.Add(e);
                        continue;
                    }
                    if (astField.AstMemberModifierCollection.IsVirtual || astField.AstMemberModifierCollection.IsOverriden)
                    {
                        e.col = astField.ColumnNumber;
                        e.fileName = astField.Path;
                        e.line = astField.LineNumber;
                        e.message = "Virtual or Override modifier is not valid for this item";
                        e.type = ErrorType.SymenticError;
                        Compiler.Errors.Add(e); continue;
                    }
                    if (astField.AstMemberModifierCollection.IsSealed)
                    {
                        e.col = astField.ColumnNumber;
                        e.fileName = astField.Path;
                        e.line = astField.LineNumber;
                        e.message = "Sealed modifier is not valid for this item";
                        e.type = ErrorType.SymenticError;
                        Compiler.Errors.Add(e); continue;

                    }
                    if (astField.AstMemberModifierCollection.IsExtern)
                    {
                        e.col = astField.ColumnNumber;
                        e.fileName = astField.Path;
                        e.line = astField.LineNumber;
                        e.message = "Extern modifier is not valid for this item";
                        e.type = ErrorType.SymenticError;
                        Compiler.Errors.Add(e); continue;
                    }
                    else
                    {
                        astStruct.ObjectLayout.Add(Mangler.Instance.MangleField(astField, false), astField);
                    }
                }
            }
        }
        public void GenerateObjectLayout()
        {
            GenerateObjectLayout(ClassHierarchy);
        }

        private void GenerateObjectLayout(ClassHierarchyNode classHierarchy)
        {
            AstClass astClass = classHierarchy.AstClass;

            Hashtable parentObjectLayout = new Hashtable();
            if (astClass.FullQualifiedName != "System.Object")
                InheritFields(parentObjectLayout, astClass.AstParentClass);

            astClass.ObjectLayout = parentObjectLayout;

            // Generate Object Layout for current class
            GenerateObjectLayout(astClass.ObjectLayout, astClass.AstFieldCollection, astClass);

            foreach (ClassHierarchyNode chn in classHierarchy.ClassHierarchyNodeCollection)
                GenerateObjectLayout(chn);
        }

        private void InheritFields(Hashtable ht, AstParentClass astParentClass)
        {
            AstClass astClass = (AstClass)ClassHTable[astParentClass.FullQualifiedName];
            foreach (AstField astField in astClass.AstFieldCollection)
            {
                if (!astField.AstMemberModifierCollection.Validate(_errors))
                {
                    _hasError = true;
                    continue;
                }
                if (astField.AstMemberModifierCollection.IsVirtual)
                {
                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "The modifier 'virtual' is not valid for this item", astField.LineNumber, astField.ColumnNumber));
                }
                string key = Mangler.Instance.MangleField(astField, true);
                if (astField.AstMemberModifierCollection.IsPublic || astField.AstMemberModifierCollection.IsProtected)
                {
                    if (ht.ContainsKey(key))
                    {
                        _errors.Add(new ErrorInfo(ErrorType.SymenticError,
                                                  "The type '" + astClass.FullQualifiedName +
                                                  "' already contains the definition of " + astField.Name,
                                                  astField.LineNumber, astField.ColumnNumber, astField.Path));
                        _hasError = true;
                        continue;
                    }

                    ht.Add(key, astField);
                }
            }
            if (astClass.AstParentClass != null)
                InheritFields(ht, astClass.AstParentClass);
        }

        private void GenerateObjectLayout(Hashtable ht, AstFieldCollection astFieldCollection, AstClass astClass)
        {
            foreach (AstField astField in astFieldCollection)
            {
                if (!astField.AstMemberModifierCollection.Validate(_errors))
                {
                    _hasError = true;
                    continue;
                }
                if (astField.AstMemberModifierCollection.IsVirtual)
                {
                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, "The modifier 'virtual' is not valid for this item", astField.LineNumber, astField.ColumnNumber));
                }
                string key = Mangler.Instance.MangleField(astField, false);

                if (ht.ContainsKey(key))
                {
                    _errors.Add(new ErrorInfo(ErrorType.SymenticError,
                        "The type '" + astClass.FullQualifiedName + "' already contains the definition of " + astField.Name,
                         astField.LineNumber, astField.ColumnNumber, astField.Path));
                    _hasError = true;
                    continue;
                }

                ht.Add(key, astField);
            }
        }

        private void InheritParentFields(Hashtable ht, AstClass astClass)
        {
            if (astClass.FullQualifiedName == "System.Object")
                return;

            AstClass parentClass = (AstClass)ClassHTable[astClass.AstParentClass.FullQualifiedName];

            foreach (AstField astField in parentClass.AstFieldCollection)
            {
                if (!astField.AstMemberModifierCollection.Validate(_errors))
                {
                    _hasError = true;
                    continue;
                }
                if (astField.AstMemberModifierCollection.ProtectionModifierType != ProtectionModifierType.Private)
                {
                    string key = Mangler.Instance.MangleField(astField, true);
                    if (ht.ContainsKey(key))
                    {
                        //_errors.Add(new ErrorInfo(ErrorType.SymenticError,
                        //    "The type '" + astClass.FullQualifiedName + "' already contains the definition of " + astField.Name,
                        //     astField.LineNumber, astField.ColumnNumber, astField.Path));
                        _hasError = true;
                        continue;
                    }

                    ht.Add(Mangler.Instance.MangleField(astField, true), astField);
                }
            }

            if (parentClass.AstParentClass != null)
                InheritParentFields(ht, (AstClass)ClassHTable[parentClass.AstParentClass.FullQualifiedName]);
        }

        #endregion

        #region Stage#03.01 : Add information into Symbol Table
        /*
        public void CreateSymbolTableNode()
        {
            if (ClassHierarchy != null)
            {
                foreach (ClassHierarchyNode item in ClassHierarchy.ClassHierarchyNodeCollection)
                {
                    item.CreateSymbolTable(_errors);
                }
            }
        }
        */
        #endregion


        #region Stage#04 : Create Class Hierarchy and Check semantic
        /// <summary>
        /// This method will exmaine each root classes and their child classes 
        /// and will check the semantic error along the class hierarchy path
        /// </summary>


        #endregion



    }
}
