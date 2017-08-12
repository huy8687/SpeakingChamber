using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace SpeakingChamber
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            File.AppendAllText("ex_rr.txt", e.Exception.StackTrace);
        }
    }
}
