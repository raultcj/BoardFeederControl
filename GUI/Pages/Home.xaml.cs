using System.Diagnostics;
using System.Windows.Controls;

namespace GUI.Pages {

    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl {

        public Home() {
            InitializeComponent();
        }

        private void startButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            string programPath = path.Text;

            Process program = new Process();
            program.StartInfo.UseShellExecute = false;
            program.StartInfo.CreateNoWindow = true;
            program.StartInfo.RedirectStandardOutput = true;
            program.StartInfo.RedirectStandardError = true;
            program.StartInfo.FileName = programPath;
            program.StartInfo.Arguments = "";

            program.Start();

            string output = program.StandardOutput.ReadToEnd();
            string error = program.StandardError.ReadToEnd();

            program.WaitForExit();

            outText.Text = "";

            outText.Text += output;
            outText.Text += error;
        }
    }
}