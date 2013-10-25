namespace LLVMSharp.Compiler
{
    public enum CompilerPhases
    {
        Initialized = 0,
        LexingAndParsing,
        LexingAndParsingCompleted,
        GeneratingAst,
        GeneratingAstCompleted,
        GeneratingObjectHierarchy,
        GeneratingObjectHierarchyCompleted,
        CheckingTypes,
        TypesCheckedCompleted,
        GeneratingCode,
        GeneratingCodeCompleted,
        Successfull,
    }
}