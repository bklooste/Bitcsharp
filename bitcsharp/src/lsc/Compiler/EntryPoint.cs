using System.Collections;
using LLVMSharp.Compiler.Ast;

namespace LLVMSharp.Compiler
{
    public partial class LLVMSharpCompiler
    {
        internal void CheckEntryPoint(Hashtable methodTable, AstType astType)
        {
            const string mainFunction = "_mt_4Main";

            foreach (string methodKey in methodTable.Keys)
            {
                if (methodKey.EndsWith(mainFunction)) // possible candidate for entry point
                {
                    AstMethod astMethod = (AstMethod)methodTable[methodKey];
                    if (astMethod.AstMemberModifierCollection.IsStatic)
                    {
                        if (astMethod.FullQReturnType == "System.Void" || astMethod.FullQReturnType == "System.Int32")
                        {
                            if (astMethod.Parameters.Count == 0) // rite now we dont support string[] args
                            {
                                if (EntryPoint == null)
                                {
                                    EntryPointFQType = astMethod.FullQReturnType;
                                    EntryPoint = astMethod;
                                    EntryPointAstType = astType;
                                }
                                else // there already exist an entry point before
                                {
                                    Errors.Add(new ErrorInfo(
                                        ErrorType.SymenticError, "Program has more than one entry point defined",
                                        astMethod.LineNumber, astMethod.ColumnNumber, astMethod.Path));
                                }
                            }
                        }
                    }
                }
            }
        }

        //public AstMethod CheckEntryPoint(LLVMSharpCompiler Compiler)
        //{
        //    return null;
        //    /*  string MainFunction1 = "_mt_4Main";
        //      string MainFunction2 = "_mt_4Main_14_6System6String";
        //      AstMethod MainFunc = null;
        //      bool MainEntry = false;
        //      foreach (AstSourceFile astSource in Compiler.AstProgram.SourceFiles)
        //      {
        //          foreach (AstNamespaceBlock astNameSp in astSource.AstNamespaceBlockCollection)
        //          {
        //              foreach (AstType astType in astNameSp.AstTypeCollection)
        //              {
        //                  #region CheckClasses
        //                  if (astType is AstClass)
        //                  {
        //                      AstClass astClass = (AstClass)astType;
        //                      if (astClass.methodTable.Contains(MainFunction1))
        //                      {
        //                          MainFunc = (AstMethod)astClass.methodTable[MainFunction1];
        //                          if (MainFunc.AstMemberModifierCollection.IsStatic)
        //                          {
        //                              if (MainFunc.FullQReturnType == "System.Void" || MainFunc.FullQReturnType == "System.Int32")
        //                              {
        //                                  MainEntry = true;
        //                                  EntryPointFQType = astType.FullQualifiedName;
        //                              }
        //                          }
        //                      }
        //                      if (astClass.methodTable.Contains(MainFunction2) && MainEntry)
        //                      {
        //                          ErrorInfo e = new ErrorInfo();
        //                          e.message = "Program has more than one defined entry point";
        //                          e.type = ErrorType.SymenticError;
        //                          Compiler.Errors.Add(e);
        //                      }
        //                      if (astClass.methodTable.Contains(MainFunction2) && !MainEntry)
        //                      {
        //                          MainFunc = (AstMethod)astClass.methodTable[MainFunction2];
        //                          if (MainFunc.AstMemberModifierCollection.IsStatic)
        //                          {
        //                              if (MainFunc.FullQReturnType == "System.Void" || MainFunc.FullQReturnType == "System.Int32")
        //                              {
        //                                  MainEntry = true;
        //                                  EntryPointFQType = astType.FullQualifiedName;
        //                              }
        //                          }
        //                      }

        //                  }
        //                  #endregion
        //                  #region CheckStructs
        //                  if (astType is AstStruct)
        //                  {
        //                      AstStruct astStruct = (AstStruct)astType;
        //                      if (astStruct.methodTable.Contains(MainFunction1) && MainEntry || astStruct.methodTable.Contains(MainFunction2) && MainEntry)
        //                      {
        //                          ErrorInfo e = new ErrorInfo();
        //                          e.type = ErrorType.SymenticError;
        //                          e.message = "Program has more than one defined entry point";
        //                          Compiler.Errors.Add(e);
        //                      }
        //                      if (astStruct.methodTable.Contains(MainFunction1) && !MainEntry)
        //                      {
        //                          MainFunc = (AstMethod)astStruct.methodTable[MainFunction1];
        //                          if (MainFunc.AstMemberModifierCollection.IsStatic)
        //                          {
        //                              if (MainFunc.FullQReturnType == "System.Void" || MainFunc.FullQReturnType == "System.Int32")
        //                              {
        //                                  MainEntry = true;
        //                                  EntryPointFQType = astType.FullQualifiedName;
        //                              }
        //                          }
        //                      }
        //                      if (astStruct.methodTable.Contains(MainFunction2) && !MainEntry)
        //                      {
        //                          MainFunc = (AstMethod)astStruct.methodTable[MainFunction2];
        //                          if (MainFunc.AstMemberModifierCollection.IsStatic)
        //                          {
        //                              if (MainFunc.FullQReturnType == "System.Void" || MainFunc.FullQReturnType == "System.Int32")
        //                              {
        //                                  MainEntry = true;
        //                                  EntryPointFQType = astType.FullQualifiedName;
        //                              }
        //                          }
        //                      }
        //                  }
        //                  #endregion

        //              }

        //          }

        //          foreach (AstType astType in astSource.AstTypeCollection)
        //          {
        //              #region CheckClasses
        //              if (astType is AstClass)
        //              {
        //                  AstClass astClass = (AstClass)astType;
        //                  if (astClass.methodTable.Contains(MainFunction1))
        //                  {
        //                      MainFunc = (AstMethod)astClass.methodTable[MainFunction1];
        //                      if (MainFunc.AstMemberModifierCollection.IsStatic)
        //                      {
        //                          if (MainFunc.FullQReturnType == "System.Void" || MainFunc.FullQReturnType == "System.Int32")
        //                          {
        //                              MainEntry = true;
        //                              EntryPointFQType = astType.FullQualifiedName;
        //                          }
        //                      }
        //                  }
        //                  if (astClass.methodTable.Contains(MainFunction2) && MainEntry)
        //                  {
        //                      ErrorInfo e = new ErrorInfo();
        //                      e.message = "Program has more than one defined entry point";
        //                      e.type = ErrorType.SymenticError;
        //                      Compiler.Errors.Add(e);
        //                  }
        //                  if (astClass.methodTable.Contains(MainFunction2) && !MainEntry)
        //                  {
        //                      MainFunc = (AstMethod)astClass.methodTable[MainFunction2];
        //                      if (MainFunc.AstMemberModifierCollection.IsStatic)
        //                      {
        //                          if (MainFunc.FullQReturnType == "System.Void" || MainFunc.FullQReturnType == "System.Int32")
        //                          { MainEntry = true; EntryPointFQType = astType.FullQualifiedName; }
        //                      }
        //                  }

        //              }
        //              #endregion
        //              #region CheckStructs
        //              if (astType is AstStruct)
        //              {
        //                  AstStruct astStruct = (AstStruct)astType;
        //                  if (astStruct.methodTable.Contains(MainFunction1) && MainEntry || astStruct.methodTable.Contains(MainFunction2) && MainEntry)
        //                  {
        //                      ErrorInfo e = new ErrorInfo();
        //                      e.type = ErrorType.SymenticError;
        //                      e.message = "Program has more than one defined entry point";
        //                      Compiler.Errors.Add(e);
        //                  }
        //                  if (astStruct.methodTable.Contains(MainFunction1) && !MainEntry)
        //                  {
        //                      MainFunc = (AstMethod)astStruct.methodTable[MainFunction1];
        //                      if (MainFunc.AstMemberModifierCollection.IsStatic)
        //                      {
        //                          if (MainFunc.FullQReturnType == "System.Void" || MainFunc.FullQReturnType == "System.Int32")
        //                          { MainEntry = true; EntryPointFQType = astType.FullQualifiedName; }
        //                      }
        //                  }
        //                  if (astStruct.methodTable.Contains(MainFunction2) && !MainEntry)
        //                  {
        //                      MainFunc = (AstMethod)astStruct.methodTable[MainFunction2];
        //                      if (MainFunc.AstMemberModifierCollection.IsStatic)
        //                      {
        //                          if (MainFunc.FullQReturnType == "System.Void" || MainFunc.FullQReturnType == "System.Int32")
        //                          { MainEntry = true; EntryPointFQType = astType.FullQualifiedName; }
        //                      }
        //                  }
        //              }
        //              #endregion
        //          }
        //      }
        //      if (!MainEntry)
        //      {
        //          ErrorInfo e = new ErrorInfo();
        //          e.message = " The Program does not contain a static method 'Main' suitable for an entry point";
        //          e.type = ErrorType.SymenticError;
        //          Compiler.Errors.Add(e);
        //          return null;
        //      }
        //      else { return MainFunc; }*/

        //}


    }
}
