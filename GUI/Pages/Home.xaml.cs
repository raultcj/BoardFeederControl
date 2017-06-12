using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Windows.Controls;

namespace GUI.Pages {

    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl {
        public static MainWindow mWindow;
        private SerialPort arduino = new SerialPort();

        public Home() {
            InitializeComponent();

            //Defining Serial Connection
            try {
                arduino.BaudRate = 115200;
                arduino.PortName = "COM3";
                arduino.Open();
            }
            catch {
                mWindow.displayErrorMessage("Failure connecting to COM.");
            }
        }

        private void startButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            if (!mWindow.recipeSelected) {
                mWindow.displayErrorMessage("No recipe selected");
                return;
            }

            string programPath = "C:/Users/Raul/Source/Repos/BoardFeederControl/BoardFeeder/x64/Release/BoardFeeder.exe";

            Process program = new Process();
            program.StartInfo.UseShellExecute = false;
            program.StartInfo.CreateNoWindow = true;
            program.StartInfo.RedirectStandardOutput = true;
            program.StartInfo.RedirectStandardError = true;
            program.StartInfo.FileName = programPath;
            program.StartInfo.Arguments = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SMTBoardFeeder", "tempImg.png");

            program.Start();

            string output = program.StandardOutput.ReadToEnd();
            string error = program.StandardError.ReadToEnd();

            program.WaitForExit();

            outText.Text = "";

            outText.Text += output;
            outText.Text += error;

            //Serial Connection
            try {
                if (!arduino.IsOpen) {
                    try {
                        arduino.Open();
                    }
                    catch (Exception ext) {
                        mWindow.displayErrorMessage("Failure trying to connect to COM.");
                        Console.WriteLine(ext.Message);
                        return;
                    }
                    arduino.Write(outText.Text);
                }
                else {
                    arduino.Write(outText.Text);
                }
            }
            catch (InvalidOperationException ex) {
                ModernDialog.ShowMessage("Error writing message to COM.", "Error", System.Windows.MessageBoxButton.OK);
                Console.WriteLine(ex.Message);
            }
        }
    }
}