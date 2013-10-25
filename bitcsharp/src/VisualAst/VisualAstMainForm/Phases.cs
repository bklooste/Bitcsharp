using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Threading;
using LLVMSharp.Compiler;

namespace LLVMSharp.VisualAst
{
    public partial class VisualAstMainForm
    {
        private void buildAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(
               () =>
               {
                   resetToolStripMenuItem_Click(this, new EventArgs()); // Reset compiler
                   Phase1(); // Phase 1 - Lex and Parse
                   if (compiler.CanGoToNextStep)
                   {
                       Phase2(); // Phase 2 - Build Ast
                       if (compiler.CanGoToNextStep)
                       {
                           Phase3(); // Phase 3 - Build Object Heirerachy
                           if (compiler.CanGoToNextStep)
                           {
                               Phase4();
                               //GenerateCodeToNewWindow(); // Generate code to new window by default
                           }
                       }
                   }
               })
            );
            t.Start();
        }

        private void Phase1()
        {
            compiler.StartNextStep();

            // if can go to next step
            if (compiler.CanGoToNextStep)
            {
                menuStrip1.Invoke(new MethodInvoker(() =>
                {
                    phase2BuildASTToolStripMenuItem.Enabled = true;
                }));
            }

            menuStrip1.Invoke(new MethodInvoker(() =>
            {
                resetToolStripMenuItem.Enabled = true;
            }));
        }

        private void phase1LexAndParseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(
                () =>
                {
                    Phase1();
                })
            );
            t.Start();
        }

        private void Phase2()
        {
            compiler.StartNextStep();

            BuildAstTreeNodes builder = new BuildAstTreeNodes(tvAstNode, compiler.AstProgram);
            builder.BuildTreeView();

            if (compiler.CanGoToNextStep)
            {
                this.Invoke(new MethodInvoker(() =>
                    {
                        phase1LexAndParseToolStripMenuItem.Enabled = false;
                        phase2BuildASTToolStripMenuItem.Enabled = false;
                        phase3BuildObjectHierarchyToolStripMenuItem.Enabled = true;
                    }
                ));
            }
        }

        private void phase2BuildASTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Phase2();
        }


        private void phase3BuildObjectHierarchyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Phase3();
        }

        private void phase4TypeCheckingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Phase4();
        }

        private void Phase3()
        {
            compiler.StartNextStep();

            // todo

            if (compiler.CanGoToNextStep) // enable the object browser. only partial view will be supported though.
            {
                this.Invoke(new MethodInvoker(() =>
                    {
                        phase3BuildObjectHierarchyToolStripMenuItem.Enabled = false;
                        //enable the next menu out here`
                        objectBrowserToolStripMenuItem.Enabled = true;

                        if (compiler.CanGoToNextStep)
                        {
                            phase4TypeCheckingToolStripMenuItem.Enabled = true;
                            //mnuGenerateCode.Enabled = true;
                        }
                    }
                ));
            }
        }

        private void Phase4()
        {

            compiler.StartNextStep();

            this.Invoke(new MethodInvoker(() =>
            {
                phase4TypeCheckingToolStripMenuItem.Enabled = false;

                if (compiler.CanGoToNextStep)
                {
                    mnuGenerateCode.Enabled = true;
                    mnuGenerateCodeToNewWindow.Enabled = true;
                }
            }));
        }

        private void mnuGenerateCodeToNewWindow_Click(object sender, EventArgs e)
        {
            GenerateCodeToNewWindow();
        }

        private void GenerateCodeToNewWindow()
        {
            // Generate output in MemoryStream

            GeneratedCode window = null;

            using (MemoryStream ms = new MemoryStream())
            {
                compiler.CodeGenerator =
                    new Compiler.CodeGenerators.LLVMSharpCodeGenerator(new StreamWriter(ms));

                //                compiler.CodeGenerator = new LLVMSharp.Compiler.LLVM.LLVMCodeGenerator(new StreamWriter(ms));

                compiler.StartNextStep();

                if (compiler.CanGoToNextStep)
                {
                    window = new GeneratedCode(compiler.CodeGenerator.Writer);
                    window.Show();
                }

            }


            // yeah compiler completed.
        }
    }
}
