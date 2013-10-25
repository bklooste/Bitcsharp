////using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using LLVMSharp.Compiler.Assembler;
//using System.IO;

//namespace lsc.Tests
//{
//    [TestFixture]
//    // ReSharper disable InconsistentNaming
//    public class LLVMAssemblerTests
//    // ReSharper restore InconsistentNaming
//    {
//        private LLVMAssembler _assembler;

//        // ReSharper disable InconsistentNaming
//        private string _llvm_asPath;
//        // ReSharper restore InconsistentNaming
//        private string _llvmCodesPath;

//        [TestFixtureSetUp]
//        public void SetupFixture()
//        {
//            _llvm_asPath = Helpers.LLVMPath + Path.DirectorySeparatorChar + "llvm-as.exe";
//            _llvmCodesPath = Helpers.DataPath + Path.DirectorySeparatorChar + "llvm-as" + Path.DirectorySeparatorChar;
//        }

//        [Test]
//        public void CheckAssembler_Path()
//        {

//            //_assembler = new LLVMAssembler(_llvm_asPath, Path.GetFullPath(_llvmCodesPath + "main.ll"));

//            //int actual = _assembler.Assemble();

//            //Console.WriteLine("Standard Error:");
//            //Console.WriteLine(_assembler.StandardError);
//            //Console.WriteLine("Standard Output:");
//            //Console.WriteLine(_assembler.LLVMBitCode.ReadToEnd());

//            //Assert.AreEqual(0, actual);
//        }

//    }
//}
