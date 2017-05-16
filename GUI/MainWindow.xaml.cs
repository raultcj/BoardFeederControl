using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace GUI {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e) {
            Process myProcess = new Process();

            try {
                myProcess.StartInfo.FileName = "C:\\Users\\Raul Juarez\\Source\\Repos\\BoardFeederControl\\BoardFeeder\\x64\\Release\\BoardFeeder.exe";
                myProcess.StartInfo.Arguments = "0 17";
                myProcess.Start();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}