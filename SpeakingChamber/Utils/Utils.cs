﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SpeakingChamber
{
    public class Utils
    {
        private const string TEMP_FILE = "temp.xxx";

        public static bool CheckUrl(string url)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.AllowAutoRedirect = false;
                request.Method = "HEAD";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    response.Close();
                    return (response.StatusCode == HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }
        
        public static bool CheckPath(string dirPath)
        {
            var result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(dirPath) && Directory.Exists(dirPath))
                {
                    var path = Path.Combine(dirPath, TEMP_FILE);
                    File.WriteAllText(path, "test");
                    File.Delete(path);
                    result = true;
                }
            }
            catch { }
            return result;
        }
    }
}
