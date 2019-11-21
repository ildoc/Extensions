using System.Collections.Generic;

namespace Utils
{
    public static class Process
    {
        public class RunResult
        {
            public bool Success { get; set; }
            public List<string> Output = new List<string>();
            public List<string> Error = new List<string>();
        }

        public static RunResult RunWaiting(string fileName, string arguments = null, string workingDirectory = "")
        {
            var res = new RunResult();
            var p = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    WorkingDirectory = workingDirectory,
                    FileName = System.IO.Path.Combine(workingDirectory, fileName),
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                }
            };
            p.OutputDataReceived += (s, a) => { if (a.Data == null) return; res.Output.Add(a.Data); };
            p.ErrorDataReceived += (s, a) => { if (a.Data == null) return; res.Error.Add(a.Data); };
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.WaitForExit();
            p.CancelOutputRead();
            p.CancelErrorRead();
            res.Success = p.ExitCode == 0;
            return res;
        }
    }
}
