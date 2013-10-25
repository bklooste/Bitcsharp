//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;
//using LLVMSharp.Compiler.Assembler;
//using System.Collections;
//using LLVMSharp.Compiler;
//using System.IO;

//namespace lsc.Tests.Assembler
//{
//    [TestFixture]
//    public class AssemblerTests
//    {
//        [Test]
//        public void Assemble()
//        {
//            //LLVMAssembler asm = new LLVMAssembler(Helpers.LLVMAssemblerPath, "");
//            ////asm.Process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(Process_OutputDataReceived);
//            ////asm.Process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(Process_ErrorDataReceived);
//            //int i = asm.Assemble(
//            //     (object sender, StreamWriter inputWriter) =>
//            //     {
//            //         inputWriter.Write(";Hello World");
//            //     });
//            //Assert.AreEqual(0, i);
//        }

//        //StringBuilder sr = new StringBuilder();
//        //StringBuilder er = new StringBuilder();
//        //void Process_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
//        //{
//        //    er.Append(e.Data);
//        //}

//        //void Process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
//        //{
//        //    sr.Append(e.Data);
//        //}

//        //[Test]
//        //[TestCaseSource("Assemblers")]
//        ////[Ignore]
//        //public void HasAssembled_True(AssemblerBase asm)
//        //{
//        //    asm.Assemble(
//        //        (object sender, StreamWriter writer) =>
//        //        {
//        //            writer.Write("a");
//        //            writer.Flush();
//        //        }, null, null);
//        //    Assert.IsTrue(asm.HasAssembled);
//        //}

//        //    [Test]
//        //    [TestCaseSource("Assemblers")]
//        //    public void HasAssembled_False(AssemblerBase asm)
//        //    {
//        //        Assert.IsFalse(asm.HasAssembled);
//        //    }

//        //    [Test]
//        //    [ExpectedException(typeof(ApplicationException))]
//        //    [TestCaseSource("Assemblers")]
//        //    public void ExitCode_CallBeforeAssembling_ThrowsError(AssemblerBase asm)
//        //    {
//        //        int exitCode = asm.ExitCode;
//        //    }

//        //    public IEnumerable Assemblers
//        //    {
//        //        get
//        //        {
//        //            AssemblerBase[] assembelers = new AssemblerBase[]{
//        //                    //new FakeAssembler(),
//        //                    new LLVMAssembler(string.Empty,Helpers.LLVMAssemblerPath)
//        //            };
//        //            foreach (AssemblerBase assembeler in assembelers)
//        //            {
//        //                yield return new TestCaseData(assembeler)
//        //                    .SetDescription(assembeler.GetType().Name)
//        //                    .SetName(assembeler.GetType().Name);
//        //            }
//        //        }
//        //    }

//        //    public class FakeAssembler : AssemblerBase
//        //    {
//        //        public FakeAssembler()
//        //            : base(string.Empty)
//        //        {

//        //        }

//        //        public override string AssemblerPath
//        //        {
//        //            get { return string.Empty; }
//        //        }

//        //        protected override int AssembleCode(StandardInputDelegate standardInput, StandardOutputDelegate standardOutput, StandardErrorDelgate standardError)
//        //        {
//        //            return 0;
//        //        }
//        //    }
//    }
//}
