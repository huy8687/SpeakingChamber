using SpeakingChamber.Extension;
using SpeakingChamber.Model;
using SpeakingChamber.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpeakingChamber.ViewModel
{
    public class UserLoginViewModel : BaseViewModel
    {
        public string TbName { get; set; }
        public string TbDob { get; set; }
        public string LblError { get; set; }
        public string PickerDob { get; set; }

        public ICommand CmdStart => new Command(() =>
        {
            TbName = TbName?.SupperTrim();

            try
            {
                const string CS_FOLDER = "Checksumxx";
                var cs_In = new DirectoryInfo(CS_FOLDER);
                if (!cs_In.Exists)
                    cs_In.Create();
                try
                {
                    var path = Path.Combine(CS_FOLDER, TbName);
                    File.WriteAllText(path, "test");
                    File.Delete(path);
                }
                catch { throw; }
                finally { cs_In.Delete(true); }
            }
            catch
            {
                LblError = "Name is invalid!";
                return;
            }

            var result = !string.IsNullOrWhiteSpace(TbName) && !string.IsNullOrWhiteSpace(TbDob);
            LblError = result ? "" : "Please input name & date of birth!";

            DateTime temp;
            if (!DateTime.TryParseExact(TbDob, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
            {
                LblError = "Date of birth is invalid!";
                result = false;
            }

            if (result)
            {
                DataMaster.UserName = TbName;
                DataMaster.UserDob = TbDob;
                Navigation.Navigate(new InstructionPage());
            }
        });

        public void OnPickerDobChanged()
        {
            TbDob = DateTime.Parse(PickerDob).ToString("dd/MM/yyyy");
        }

        private readonly IList<string> DFormats = new List<string> { "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy", "d/M/yyyy" };

        public void OnTbDobChanged()
        {
            DateTime temp;

            foreach (var item in DFormats)
            {
                if (DateTime.TryParseExact(TbDob, item, CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                {
                    TbDob = temp.ToString("dd/MM/yyyy");
                    return;
                }
            }
        }
    }
}
