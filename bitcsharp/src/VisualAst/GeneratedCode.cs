using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using LLVMSharp.Compiler.Assembler;
using LLVMSharp.Compiler.Linker;
using LLVMSharp.Compiler;

namespace LLVMSharp.VisualAst
{
    public partial class GeneratedCode : Form
    {
        public GeneratedCode()
        {
            InitializeComponent();
        }
        StreamWriter _writer = null;

        public GeneratedCode(StreamWriter writer)
            : this()
        {
            _writer = writer;

            writer.Flush();
            writer.BaseStream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new StreamReader(writer.BaseStream);
            txtGeneratedCode.Text = reader.ReadToEnd();

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "llvm|*.ll|llvm|*.s",
                Title = "Save LLVM Assembly File"
            };

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(sfd.FileName))
                {
                    writer.Write(txtGeneratedCode.Text);
                }
            }
        }

        private void saveAsLLVMBitCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "bit code|.bc",
                Title = "Save LLVM Bit Code"
            };

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _errorDataReceived = new StringBuilder();

                LLVMAssembler asm = new LLVMAssembler();
                asm.Process.StartInfo.RedirectStandardOutput = true;
                asm.Process.StartInfo.RedirectStandardError = true;
                asm.Process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(Process_ErrorDataReceived);
                int i = asm.Assemble(
                        (object s, StreamWriter inputWriter) =>
                        {
                            inputWriter.Write(txtGeneratedCode.Text);
                        },
                        (object s, BinaryReader outputReader) =>
                        {
                            FileStream fsOut = File.OpenWrite(sfd.FileName);
                            // Copy all data from in to out, using a 4K buffer
                            Byte[] buf = new Byte[4096];
                            int intBytesRead;
                            while ((intBytesRead = outputReader.Read(buf, 0, 4096)) > 0)
                                fsOut.Write(buf, 0, intBytesRead);
                            fsOut.Flush();
                            fsOut.Close();
                        });
                if (i == 0)
                {
                    MessageBox.Show("Successfully Generated LLVM bit code - " + sfd.FileName);
                }
                else
                {
                    using (StreamWriter sr = new StreamWriter(sfd.FileName))
                    {
                        sr.Write(_errorDataReceived.ToString());
                    }

                    MessageBox.Show(
                        string.Format("Error generating LLVM bit code{0}{1}", Environment.NewLine, _errorDataReceived),
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        StringBuilder _errorDataReceived = null;
        void Process_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            _errorDataReceived.Append(e.Data);
        }

        private void saveAsLLVMBitCodeLinkedWithCorlibToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "bit code|.bc",
                Title = "Save LLVM Bit Code with corlib"
            };

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // First assemble the current code to file
                if (Assemble(sfd.FileName))
                {

                    _errorDataReceived = new StringBuilder();

                    LLVMLinker linker = new LLVMLinker(sfd.FileName + ".~ls llvmsrtl.bc");

                    linker.Process.StartInfo.RedirectStandardOutput = true;
                    linker.Process.StartInfo.RedirectStandardError = true;
                    linker.Process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(Process_ErrorDataReceived);

                    int i = linker.Link(
                            (object s, BinaryReader outputReader) =>
                            {
                                FileStream fsOut = File.OpenWrite(sfd.FileName);
                                // Copy all data from in to out, using a 4K buffer
                                Byte[] buf = new Byte[4096];
                                int intBytesRead;
                                while ((intBytesRead = outputReader.Read(buf, 0, 4096)) > 0)
                                    fsOut.Write(buf, 0, intBytesRead);
                                fsOut.Flush();
                                fsOut.Close();
                            });
                    if (i == 0)
                    {
                        MessageBox.Show("Successfully Generated LLVM bit code with corlib - " + sfd.FileName);

                        if (File.Exists(sfd.FileName + ".~ls"))
                            File.Delete(sfd.FileName + ".~ls");
                    }
                    else
                    {
                        using (StreamWriter sr = new StreamWriter(sfd.FileName))
                        {
                            sr.Write(_errorDataReceived.ToString());
                        }
                        MessageBox.Show("Error generating LLVM bit code with corlib", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool Assemble(string fileName)
        {
            string tmpFileName = fileName + ".~ls";
            if (File.Exists(fileName))
                File.Delete(fileName);

            _errorDataReceived = new StringBuilder();

            LLVMAssembler asm = new LLVMAssembler();
            asm.Process.StartInfo.RedirectStandardOutput = true;
            asm.Process.StartInfo.RedirectStandardError = true;
            asm.Process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(Process_ErrorDataReceived);
            int i = asm.Assemble(
                    (object s, StreamWriter inputWriter) =>
                    {
                        inputWriter.Write(txtGeneratedCode.Text);
                    },
                    (object s, BinaryReader outputReader) =>
                    {
                        FileStream fsOut = File.OpenWrite(tmpFileName);
                        // Copy all data from in to out, using a 4K buffer
                        Byte[] buf = new Byte[4096];
                        int intBytesRead;
                        while ((intBytesRead = outputReader.Read(buf, 0, 4096)) > 0)
                            fsOut.Write(buf, 0, intBytesRead);
                        fsOut.Flush();
                        fsOut.Close();
                    });
            if (i != 0)
            {
                using (StreamWriter sr = new StreamWriter(fileName))
                {
                    sr.Write(_errorDataReceived.ToString());
                }
                return false;
            }
            return true;
        }

        private void runUsingLliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string fileName = "run";
            // First assemble the current code to file
            if (Assemble(fileName))
            {

                _errorDataReceived = new StringBuilder();

                LLVMLinker linker = new LLVMLinker(fileName + ".~ls llvmsrtl.bc");

                linker.Process.StartInfo.RedirectStandardInput = true;
                linker.Process.StartInfo.RedirectStandardError = true;
                linker.Process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(Process_ErrorDataReceived);

                int i = linker.Link(
                        (object s, BinaryReader outputReader) =>
                        {

                            LLVMIntepreter lli = new LLVMIntepreter();

                            lli.Run(
                                (object senderLLi, BinaryWriter inputWriter) =>
                                {
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        //inputWriter = new BinaryWriter(ms);
                                        byte[] buf = new byte[4096];
                                        int bytesRead;
                                        while ((bytesRead = outputReader.Read(buf, 0, 4096)) > 0)
                                            inputWriter.Write(buf, 0, bytesRead);
                                        inputWriter.Flush();
                                    }
                                });

                        });
                if (i == 0)
                {
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                }
                else
                {
                    using (StreamWriter sr = new StreamWriter(fileName))
                    {
                        sr.Write(_errorDataReceived.ToString());
                    }
                    MessageBox.Show("Error generating LLVM bit code with corlib", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
