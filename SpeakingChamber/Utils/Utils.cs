using System;
using System.IO;
using System.Net;

namespace SpeakingChamber
{
    public class Utils
    {
        private static string TEMP_FILE => "temp.xxx" + DateTime.Now.Ticks + new Random().Next(10000);

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
