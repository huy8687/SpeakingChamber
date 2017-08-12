﻿using SpeakingChamber.Model;
using System.Windows.Input;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Xml.Serialization;
using SpeakingChamber.Extension;

namespace SpeakingChamber.ViewModel
{
    public class TestFinishingViewModel : BaseViewModel
    {
        public string UserName => DataMaster.UserName;
        public string ExamCode => DataMaster.CurrentTest.Code;
        public string ExamDate => DataMaster.Date;

        public ICommand CmdOK => new Command(() =>
        {
            Navigation.PopToRoot();
        });

        public override async Task Appearing()
        {
            await base.Appearing();
            await Task.Run(() => PushFileToNetwork());
        }

        private void PushFileToNetwork()
        {
            var di = new DirectoryInfo(DataMaster.Setting.UserLocalPath);
            var diNetwork = new DirectoryInfo(DataMaster.Setting.NetworkPath);
            if (di.Exists && diNetwork.Exists)
            {
                var today = DateTime.Now.ToString("ddMMyyyy");
                var name = DataMaster.UserNamePath;
                diNetwork = new DirectoryInfo(Path.Combine(DataMaster.Setting.NetworkPath, today));
                if (!diNetwork.Exists) diNetwork.Create();
                diNetwork = new DirectoryInfo(Path.Combine(DataMaster.Setting.NetworkPath, today, name));
                if (!diNetwork.Exists) diNetwork.Create();

                foreach (var file in di.GetFiles())
                {
                    File.Copy(file.FullName, Path.Combine(diNetwork.FullName, file.Name));
                }

                XmlSerializer serializer = new XmlSerializer(typeof(Test), root: new XmlRootAttribute("test"));
                using (TextWriter reader = new StreamWriter(Path.Combine(diNetwork.FullName, $"questions_{DataMaster.CurrentTest.Code}.txt")))
                {
                    try
                    {
                        serializer.Serialize(reader, DataMaster.CurrentTest);
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
