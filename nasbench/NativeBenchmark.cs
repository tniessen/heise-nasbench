using System;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;

namespace nasbench
{
    public class NativeBenchmark
    {
        public class LogEntryEventArgs : EventArgs
        {
            public LogFile.Entry Entry;
        }

        public delegate void LogEntryEventHandler(object sender, LogEntryEventArgs args);

        public event LogEntryEventHandler LogEntryParsed;

        public bool ParseLog;
        public string LogFilePath;

        public readonly string NativeBinaryPath;

        public NativeBenchmark(string nativeBinaryPath)
        {
            this.NativeBinaryPath = nativeBinaryPath;
        }

        private string EncodeArgument(string arg)
        {
            arg = Regex.Replace(arg, @"(\\*)" + "\"", @"$1$1\" + "\"");
            arg = "\"" + Regex.Replace(arg, @"(\\+)$", @"$1$1") + "\"";
            return arg;
        }

        public int Generate(string dir, string[] fileGroups, bool singleTargetPerGroup, bool deleteTarget)
        {
            StringBuilder sb = new StringBuilder("Generate ");

            sb.Append(EncodeArgument(dir));

            if (singleTargetPerGroup)
            {
                sb.Append(" --single-target-per-group");
            }

            if (deleteTarget)
            {
                sb.Append(" --delete-target");
            }

            sb.Append(' ');
            sb.Append(String.Join(" ", fileGroups.Select(EncodeArgument).ToArray()));

            return InvokeNative(sb.ToString());
        }

        public int Copy(string sourceDir, string targetDir, string[] fileGroups, bool singleSourcePerGroup, bool singleTargetPerGroup, bool deleteTarget, bool systemBuffer)
        {
            StringBuilder sb = new StringBuilder("Copy ");

            sb.Append(EncodeArgument(sourceDir));

            sb.Append(' ');
            sb.Append(EncodeArgument(targetDir));

            if (singleSourcePerGroup)
            {
                sb.Append(" --single-source-per-group");
            }

            if (singleTargetPerGroup)
            {
                sb.Append(" --single-target-per-group");
            }

            if (deleteTarget)
            {
                sb.Append(" --delete-target");
            }

            if (systemBuffer)
            {
                sb.Append(" --system-buffer");
            }

            sb.Append(' ');
            sb.Append(String.Join(" ", fileGroups.Select(EncodeArgument).ToArray()));

            return InvokeNative(sb.ToString());
        }

        public int Verify(string dir, string[] fileGroups, bool singleTargetPerGroup)
        {
            StringBuilder sb = new StringBuilder("Verify ");

            sb.Append(EncodeArgument(dir));

            if (singleTargetPerGroup)
            {
                sb.Append(" --single-target-per-group");
            }

            sb.Append(' ');
            sb.Append(String.Join(" ", fileGroups.Select(EncodeArgument).ToArray()));

            return InvokeNative(sb.ToString());
        }

        public int InvokeNative(string args)
        {
            // Create ProcessStartInfo
            ProcessStartInfo psi = new ProcessStartInfo(NativeBinaryPath, args);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.RedirectStandardError = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;

            // Prepare process
            Process proc = new Process();
            proc.StartInfo = psi;

            // Parse stdout as log entries
            if(ParseLog)
            {
                proc.OutputDataReceived += new DataReceivedEventHandler(ParseLogDataReceived);
            }

            // Handle stderr (always error messages, everything else will be in the log)
            proc.ErrorDataReceived += new DataReceivedEventHandler(StderrDataReceived);

            // Open log file for writing
            StreamWriter logFile = null;
            if (LogFilePath != null)
            {
                logFile = new StreamWriter(LogFilePath, false, Encoding.UTF8);
                proc.OutputDataReceived += new DataReceivedEventHandler((sender, a) =>
                {
                    logFile.WriteLine(a.Data);
                    logFile.Flush();
                });
            }

            try
            {
                // Execute and wait for exit
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();

                // Return exit code
                return proc.ExitCode;
            }
            finally
            {
                // Always close the log file, even if we fail to start the process
                if (logFile != null) logFile.Close();
            }
        }

        private void ParseLogDataReceived(object sender, DataReceivedEventArgs args)
        {
            string line = args.Data;
            if (String.IsNullOrEmpty(line)) return;

            // TODO: Catch exceptions when parsing, although they should never occur
            LogFile.Entry entry = LogFile.ParseEntry(line);
            LogEntryParsed(this, new LogEntryEventArgs() { Entry = entry });
        }

        private void StderrDataReceived(object sender, DataReceivedEventArgs args)
        {
            string line = args.Data;
            if (String.IsNullOrEmpty(line)) return;

            // TODO: Implement a better way to report errors
            System.Windows.Forms.MessageBox.Show("Error: " + line);
        }
    }
}
