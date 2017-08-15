using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;

namespace SpeakingChamber.Model
{
    public static class DataMaster
    {
        public static IList<Test> Tests { get; private set; }
        public static SpeakingSetting Setting { get; private set; }

        public static string UserName { get; set; }
        public static string UserNamePath => UserName?.Replace(" ", "");
        public static string Date { get; set; }

        private const string DB_FILE = "db.xml";
        private const string SETTING_FILE = "setting.xml";
        private const string DB_ROOT = "tests";

        public static Test CurrentTest;

        public static void LoadData()
        {
            Tests = ReadTest(DB_FILE);
            if (File.Exists(SETTING_FILE))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SpeakingSetting));
                using (TextReader reader = new StreamReader(SETTING_FILE))
                {
                    try
                    {
                        Setting = serializer.Deserialize(reader) as SpeakingSetting;
                    }
                    catch (Exception) { }
                }
            }
            if (Tests == null)
            {
                Tests = new List<Test>();
            }
            if (Setting == null)
            {
                Setting = new SpeakingSetting();
            }
        }

        public static IList<Test> ImportTest(string filePath)
        {
            var result = ReadTest(filePath);
            if (result != null)
            {
                foreach (var test in result)
                {
                    var oldTest = Tests.FirstOrDefault(t => t.Code == test.Code);
                    if (oldTest != null)
                    {
                        Tests.Remove(oldTest);
                    }
                    Tests.Add(test);
                }
                SaveTestDB();
            }
            return result;
        }

        private static IList<Test> ReadTest(string filePath)
        {
            IList<Test> result = null;
            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Test>), root: new XmlRootAttribute(DB_ROOT));
                using (TextReader reader = new StreamReader(filePath))
                {
                    try
                    {
                        result = serializer.Deserialize(reader) as List<Test>;
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            return result;
        }

        private static void SaveTestDB()
        {
            File.Delete(DB_FILE);
            XmlSerializer serializer = new XmlSerializer(typeof(List<Test>), root: new XmlRootAttribute(DB_ROOT));
            using (TextWriter writer = new StreamWriter(DB_FILE))
            {
                try
                {
                    serializer.Serialize(writer, Tests);
                }
                catch (Exception) { }
            }
        }

        public static bool SaveSetting(string onlineURL, string localPath, string networkPath)
        {
            Setting.OnlineUrl = onlineURL;
            Setting.LocalPath = localPath;
            Setting.NetworkPath = networkPath;
            return SaveSetting();
        }

        public static bool SaveSetting()
        {
            var result = false;
            try
            {
                File.Delete(SETTING_FILE);

                XmlSerializer serializer = new XmlSerializer(typeof(SpeakingSetting));
                using (TextWriter writer = new StreamWriter(SETTING_FILE))
                {
                    serializer.Serialize(writer, Setting);
                }
                result = true;
            }
            catch (Exception) { }
            return result;
        }

        public static void SaveCurrentTestFile(string dirPath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Test), root: new XmlRootAttribute("test"));
                using (TextWriter reader = new StreamWriter(Path.Combine(dirPath, $"questions_{DataMaster.CurrentTest.Code}.txt")))
                {
                    serializer.Serialize(reader, DataMaster.CurrentTest);
                }
            }
            catch { }
        }
    }
}
