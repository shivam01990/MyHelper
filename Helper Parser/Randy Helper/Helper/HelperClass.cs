using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Randy_Helper.Helper
{
    public class HelperClass
    {
        public static void GenerateLog(string strLogText)
        {
            try
            {

                // Create a writer and open the file:
                StreamWriter log;

                string logfilepath = GetRootPath() + @"\GeneratedFile\logfile.aspx";

                if (!File.Exists(logfilepath))
                {
                    log = new StreamWriter(logfilepath);
                }
                else
                {
                    log = File.AppendText(logfilepath);
                }

                // Write to the file:
                log.WriteLine(strLogText);
                log.WriteLine();

                // Close the stream:
                log.Close();
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        public static string GetRootPath()
        {
            try
            {
                string currentDir = Environment.CurrentDirectory;
                DirectoryInfo directory = new DirectoryInfo(currentDir);
                string fullDirectory = directory.FullName;
                string rootdirectory = fullDirectory.Replace(@"\bin\Debug", "");
                return rootdirectory;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }
    }
}
