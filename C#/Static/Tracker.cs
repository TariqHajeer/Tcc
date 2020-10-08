using System;
using System.IO;

namespace Static
{
    static public class Tracker
    {
        static string FileName = "TccLog.Txt";
        public   static void WriteException(Exception ex)
        {
            var MyDocuments= Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}