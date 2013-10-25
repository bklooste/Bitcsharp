using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LLVMSharp.VisualAst
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
//            try
//            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new VisualAstMainForm(args));
//            }
//            catch (Exception e)
//            {
//                MessageBox.Show(e.Message, "LLVM# Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
        }
    }
}
