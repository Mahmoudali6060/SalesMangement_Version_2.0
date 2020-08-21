using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shared.Classes
{
    public static class LogFile
    {
        public static bool WriteLog(string strMessage)
        {
            try
            {
                string fileName = @"E:\Logs\" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".txt";
                System.IO.File.WriteAllText(fileName, strMessage);
                return true;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
                return false;
            }
        }

    }
}
