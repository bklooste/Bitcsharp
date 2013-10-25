using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace lsc.Tests.cgen
{
    public class CodegenTests
    {
        [Theory]
        [CSVData(@"data\cgen\_data.csv")]
        public void Cgen(string file, string description)
        {
            Console.Write("[" + file + "]");
            Console.WriteLine(description);
            file = Helpers.GetPathRelativeToExecutable(@"data\cgen\" + file);
            LscCommandLine lsc = new LscCommandLine(Helpers.LSCPath, " -s -output-bin -o=test " + file);
            lsc.Process.StartInfo.RedirectStandardInput = false;
            lsc.Process.StartInfo.RedirectStandardOutput = true;
            lsc.Process.StartInfo.RedirectStandardError = true;
            lsc.Process.ErrorDataReceived += Process_ErrorDataReceived;
            sb = new StringBuilder();

            int retVal = lsc.Run((object sender, StreamReader reader) =>
                        {
                            //Console.WriteLine(reader.ReadToEnd());
                            //Console.WriteLine(sb);

                            //Console.WriteLine(sb.ToString());
                        });
            if (retVal == 0)
                CheckOutput(file);
            else
                Console.WriteLine(sb.ToString());
            
            Assert.Equal(0, lsc.Process.ExitCode);
            
            if (File.Exists(exeLii))
                File.Delete(exeLii);
            if (File.Exists(exeLii + ".bc"))
                File.Delete(exeLii + ".bc");
            Console.WriteLine();
        }


        private void CheckOutput(string file)
        {
            Process p = new Process()
            {
                StartInfo = new ProcessStartInfo(exeLii)
                                {
                                    UseShellExecute = false,
                                    ErrorDialog = false,
                                    CreateNoWindow = true,
                                    RedirectStandardError = true,
                                    RedirectStandardInput = false,
                                    RedirectStandardOutput = true
                                },
            };
            p.Start();

            StreamReader reader = p.StandardOutput;
            StreamReader errReader = p.StandardError;

            p.WaitForExit();

            string o = reader.ReadToEnd();

            using (StreamReader data = new StreamReader(file.Replace(".cs", ".data")))
            {
                string dataContents = data.ReadToEnd();

                Assert.Equal(dataContents, o);
            }


        }

        private static string exeLii = Helpers.GetPathRelativeToExecutable("test-lli.exe");

        private StringBuilder sb;
        void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            sb.Append(e.Data);
        }
    }
}

