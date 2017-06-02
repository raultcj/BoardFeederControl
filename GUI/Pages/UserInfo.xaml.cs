using System.Windows.Controls;

namespace GUI.Pages {

    /// <summary>
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : UserControl {
        public static MainWindow mWindow;

        public UserInfo() {
            InitializeComponent();

            _name.Text = mWindow._name;
            _position.Text = mWindow._position;
            _supervisor.Text = mWindow._supervisor;
        }
    }
}