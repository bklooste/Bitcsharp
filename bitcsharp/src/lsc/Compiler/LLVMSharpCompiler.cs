using System;
using System.Collections;
using System.Reflection;
using LLVMSharp.Compiler.Ast;
using LLVMSharp.Compiler.CodeGenerators;
using LLVMSharp.Compiler.Semantic;
using System.IO;
using LLVMSharp.Compiler.Walkers;
using lsc.Compiler.Semantic;

namespace LLVMSharp.Compiler
{
    public partial class LLVMSharpCompiler
    {
        public string WorkingDirectory;

        private readonly Options _options;
        public Options Options { get { return _options; } }

        private CompilerPhases _compilerPhase;
        public CompilerPhases CompilerPhase { get { return _compilerPhase; } }

        public CodeGenerator CodeGenerator;

        private AstProgram _astProgram;
        public AstProgram AstProgram
        {
            get
            {
                if ((int)_compilerPhase < 2
                    || ((int)_compilerPhase == 2 && !_canGoToNextStep))
                    throw new LLVMSharpException("Compiler has not yet successfully Lexed and Parsed.");
                return _astProgram;
            }
        }

        public AstMethod EntryPoint;
        public string EntryPointFQType;
        public AstType EntryPointAstType;

        private ErrorList _errors;
        public ErrorList Errors { get { return _errors; } }

        private bool _canGoToNextStep;
        public bool CanGoToNextStep { get { return _canGoToNextStep; } }

        public LLVMSharpCompiler(string[] args)
            : this(new Options(args))
        {

        }

        public LLVMSharpCompiler(Options options)
        {
            WorkingDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            _errors = new ErrorList();
            _options = options;
            Reset();
        }

        public void Reset()
        {
            _astProgram = null;
            _errors.Clear();
            _compilerPhase = CompilerPhases.Initialized;
            _canGoToNextStep = true;
            EntryPointFQType = string.Empty;
            EntryPoint = null;
        }

        public void StartNextStep()
        {
            if (!_canGoToNextStep)
                return;

            switch (_compilerPhase)
            {
                case CompilerPhases.Initialized: // have to start parsing all the files, by building AstNodes
                    _astProgram = new AstProgram();
                    _compilerPhase = CompilerPhases.LexingAndParsing;
                    Phase1();
                    _compilerPhase = CompilerPhases.LexingAndParsingCompleted;
                    break;
                case CompilerPhases.LexingAndParsingCompleted:
                    _compilerPhase = CompilerPhases.GeneratingAst;
                    // Phase2();// commented out to save the function overhead.. :-)
                    _compilerPhase = CompilerPhases.GeneratingAstCompleted;
                    break;
                case CompilerPhases.GeneratingAstCompleted:
                    _compilerPhase = CompilerPhases.GeneratingObjectHierarchy;
                    Phase3();
                    _compilerPhase = CompilerPhases.GeneratingObjectHierarchyCompleted;
                    break;

                case CompilerPhases.GeneratingObjectHierarchyCompleted: // this is just dummy, more things to do before generating code
                    _compilerPhase = CompilerPhases.CheckingTypes;
                    Phase4();
                    _compilerPhase = CompilerPhases.TypesCheckedCompleted;
                    break;
                case CompilerPhases.TypesCheckedCompleted:
                    _compilerPhase = CompilerPhases.GeneratingCode;
                    GenerateCodePhase();
                    _compilerPhase = CompilerPhases.GeneratingCodeCompleted;
                    break;
                default:
                    break;
            }
        }


        private void GenerateCodePhase()
        {
            if (CodeGenerator == null)
            {
                _errors.Add(new ErrorInfo(ErrorType.SyntaxError, "CodeGenerator is unspecified", 0, 0));
                _canGoToNextStep = false;
                Environment.ExitCode = 3;
                return;
            }
            else if (CodeGenerator.Writer == null)
            {
                _errors.Add(new ErrorInfo(ErrorType.SyntaxError, "Code Generator output stream is unspecified", 0, 0));
                _canGoToNextStep = false;
                return;
            }

            if (Options.Target == Target.Exe)
            {
                int oldErrorCount = Errors.Count;

                AstProgram.CheckEntryPoint(this);

                if (EntryPoint == null)
                {
                    Errors.Add(new ErrorInfo(ErrorType.SymenticError,
                                             "The Program does not contain a static method 'Main' suitable for an entry point",
                                             0, 0));
                }

                if (EntryPoint == null || oldErrorCount != Errors.Count)
                {
                    _canGoToNextStep = false;
                    return;
                }
            }

            CodeGenerator.Compiler = this;
            CodeGenerator.EmitCode();
        }

        /// <summary>
        /// Generate Object hierarchy
        /// </summary>
        /// <remarks>
        /// Create the object hierarchy (need to support nested also)
        /// detect circular refrences (only for class, struct can inherit),
        /// detect multiple class/enum/struct declrations
        /// </remarks>
        private void Phase3()
        {
            ObjectHierarchy objH = new ObjectHierarchy(_astProgram, _errors);
            objH.CollectObjects();
            if (!objH.hasError)
            {
                objH.GenerateFullQualifiedNameForParentClass();
                _classHashtable = objH.ClassHTable;
                _enumHashtable = objH.EnumHTable;
                _structHashTable = objH.StructHTable;

                if (!objH.hasError)
                {
                    objH.CreateClassHierarchy();
                    _classHierarchy = objH.ClassHierarchy;
                    if (!objH.hasError)
                    {
                        objH.CheckCircularInheiritance(_classHashtable,
                                                       objH.ClassInheiritanceCount(_classHashtable), this);
                        if (!objH.hasError)
                        {
                            UsingDirectivesWalker usingDirectivesWalker = new UsingDirectivesWalker(this);
                            usingDirectivesWalker.Walk(); // add usingdirectives in ns, fqt for locavarsdecl

                            objH.GenerateFQReturnType();
                            objH.CheckNestedNS(this);
                            objH.GenerateFQTParameters();
                            //objH.GenerateFQTLocalVars();
                            objH.GenerateFQTNewType();
                            if (_errors.Count == 0)
                            {
                                _stringTable = objH.StringTable;
                                _integerTable = objH.IntTable;

                                //objH.GenerateMethTable(this, objH.MethodTable);
                                foreach (AstClass item in this.ClassHashtable.Values)
                                {
                                    item.GetParents(this);
                                    item.GenerateMethodTable(this);
                                    item.CheckDuplicateItems(this);
                                    item.CheckOverloads(this);
                                    item.CheckTypeConversion(this);
                                    item.CreateDefaults();
                                }
                                foreach (AstStruct item in this.StructHashtable.Values)
                                {
                                    item.GenerateMethodTable(this);
                                    item.CheckDuplicateItems(this);
                                    item.CheckOverloads(this);
                                    item.CheckTypeConversion(this);
                                    item.CreateDefaults();
                                }
                                // objH.CheckEntryPoint(this);
                                if (_errors.Count == 0)
                                {
                                    _stringTable = objH.StringTable;
                                    _integerTable = objH.IntTable;

                                    if (_errors.Count == 0)
                                    {
                                        objH.GenerateObjectLayout();
                                        objH.GenerateObjectLayoutStructs(this);
                                    }
                                    else
                                        _canGoToNextStep = false;
                                }
                                else
                                {
                                    _canGoToNextStep = false;
                                }
                            }
                            else
                            {
                                _canGoToNextStep = false;
                            }
                        }
                    }
                }
                _canGoToNextStep = (_errors.Count == 0);
            }
        }

        private void Phase4()
        {
            if (_errors.Count > 0)
                _canGoToNextStep = false;
            foreach (AstClass astClass in this.ClassHashtable.Values)
            {
                //Do Semantic Check for class here
                SemanticAnalysis semantic = new SemanticAnalysis();
                semantic.CreateSymbolTableForAll(this, astClass);

                astClass.CheckVirtualOverrides(this);
                astClass.CheckNestedTypes(this);
                astClass.GetSize();
            }
            foreach (AstStruct astStruct in this.StructHashtable.Values)
            {
                //Do Semantic Check for struct here
                SemanticAnalysis semantic = new SemanticAnalysis();
                semantic.CreateSymbolTableForAll(this, astStruct);

                astStruct.CheckNestedTypes(this);
                astStruct.GetSize();
            }
            _canGoToNextStep = (_errors.Count == 0);
        }

        private Hashtable _classHashtable;
        public Hashtable ClassHashtable
        {
            get
            {
                return _classHashtable ?? (_classHashtable = new Hashtable());
                //if (_classHashtable == null)
                //    //throw new ApplicationException("Compiler hasn't generated Object Hierarchy yet.");
                //return _classHashtable;
            }
        }

        private Hashtable _structHashTable;
        public Hashtable StructHashtable
        {
            get
            {
                return _structHashTable ?? (_structHashTable = new Hashtable());
                //if (_structHashTable == null)
                //    throw new ApplicationException("Compile hasn't generated Object Hierarchy yet.");
                //return _structHashTable;
            }
        }

        private Hashtable _enumHashtable;
        public Hashtable EnumHashtable
        {
            get
            {
                return _enumHashtable ?? (_enumHashtable = new Hashtable());
                //if (_enumHashtable == null)
                //    throw new ApplicationException("Compile hasn't generated Object Hierarchy yet.");
                //return _enumHashtable;
            }
        }

        private ClassHierarchyNode _classHierarchy = null;
        public ClassHierarchyNode ClassHierarchy
        {
            get
            {
                if (_classHierarchy == null)
                    throw new ApplicationException("Compile hasn't generated Object Hierarchy yet.");
                return _classHierarchy;
            }
        }
        private Hashtable _methodTable = null;
        public Hashtable MethodTable
        {
            get
            {
                //if (_methodTable == null)
                //    throw new ApplicationException("Compiler hasn't generated Object Hierarchy yet.");
                return _methodTable;
            }

        }

        private TypeChecker _typeChecker = null;
        public TypeChecker TypeChecker
        {
            get
            {
                if (_typeChecker == null)
                    _typeChecker = new TypeChecker(this);
                return _typeChecker;
            }
        }

        private Hashtable _stringTable = null;
        public Hashtable StringTable
        {
            get { return _stringTable; }

        }

        private Hashtable _integerTable = null;
        public Hashtable IntegerTable
        {
            get { return _integerTable; }

        }

        public Hashtable GlobalsTable = new Hashtable();

        /*
        /// <summary>
        /// Build Ast
        /// </summary>
        /// <remarks>
        /// no need to build Ast
        /// this step is already done during parsing.
        /// just to allow users to get the AstProgram Node.
        /// </remarks>
        private void Phase2() { }
        */


        private void Phase1()
        {

            foreach (string fileName in _options.SourceFiles)
            {
                if (!File.Exists(fileName))
                {
                    _errors.Add(new ErrorInfo(ErrorType.SymenticError, fileName + " not found", 0, 0));
                    break;
                }
                Parser p = new Parser(new Scanner(fileName));
                p.errors.errorList.ErrorAdded += new ErrorHandler(LexParse_ErrorAdded);
                p.Parse();
                _astProgram.SourceFiles.Add(p.RootNode);

            }
            _canGoToNextStep = (_errors.Count == 0) ? true : false;
        }



        private void LexParse_ErrorAdded(object sender, ErrorArgs e)
        {
            _errors.Add(e.errorInfo);
        }


        public bool TypeExist(string typeName)
        {
            return ClassHashtable.ContainsKey(typeName) || StructHashtable.ContainsKey(typeName) ||
                   EnumHashtable.ContainsKey(typeName);
        }
    }
}
