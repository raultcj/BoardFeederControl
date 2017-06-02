using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace GUI.Pages {

    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : ModernDialog {
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SMTBoardFeeder");
        private SQLiteConnection dbConnection;

        public Login() {
            InitializeComponent();
            checkForDB();

            CloseButton.Visibility = Visibility.Collapsed;
        }

        private void checkForDB() {
            if (Directory.Exists(dbPath)) {
                if (File.Exists(Path.Combine(dbPath, "db.sqlite"))) {
                    //Continue with Login
                }
                else {
                    SQLiteConnection.CreateFile(Path.Combine(dbPath, "db.sqlite"));

                    //Creating SQL DB and adding Admin user and pass
                    dbConnection = new SQLiteConnection("Data Source=" + Path.Combine(dbPath, "db.sqlite") + ";Version=3");
                    dbConnection.Open();

                    //Create TABLE USERS
                    string createTable = "CREATE TABLE USERS(USERNAME VARCHAR(20) NOT NULL, PASS VARCHAR(20) NOT NULL, NAME VARCHAR(20) NOT NULL,ID   INT NOT NULL,POSITION    VARCHAR(20), SUPERVISOR  VARCHAR(20), PRIMARY KEY(ID)) ";
                    SQLiteCommand command = new SQLiteCommand(createTable, dbConnection);
                    command.ExecuteNonQuery();

                    //ADD ADMIN
                    string addAdmin = "INSERT INTO USERS(USERNAME, PASS, NAME, ID, POSITION, SUPERVISOR) VALUES ('ADMIN', 'ADMIN', 'ADMIN', 0, 'ADMIN', 'ADMIN')";
                    command = new SQLiteCommand(addAdmin, dbConnection);
                    command.ExecuteNonQuery();

                    dbConnection.Close();
                }
            }
            else {
                Directory.CreateDirectory(dbPath);
                SQLiteConnection.CreateFile(Path.Combine(dbPath, "db.sqlite"));

                //Creating SQL DB and adding Admin user and pass
                dbConnection = new SQLiteConnection("Data Source=" + Path.Combine(dbPath, "db.sqlite") + ";Version=3");
                dbConnection.Open();

                //Create TABLE USERS
                string createTable = "CREATE TABLE USERS(USERNAME VARCHAR(20) NOT NULL, PASS VARCHAR(20) NOT NULL, NAME VARCHAR(20) NOT NULL,ID   INT NOT NULL,POSITION    VARCHAR(20), SUPERVISOR  VARCHAR(20), PRIMARY KEY(ID)) ";
                SQLiteCommand command = new SQLiteCommand(createTable, dbConnection);
                command.ExecuteNonQuery();

                //ADD ADMIN
                string addAdmin = "INSERT INTO USERS(USERNAME, PASS, NAME, ID, POSITION, SUPERVISOR) VALUES ('ADMIN', 'ADMIN', 'ADMIN', 0, 'ADMIN', 'ADMIN')";
                command = new SQLiteCommand(addAdmin, dbConnection);
                command.ExecuteNonQuery();

                dbConnection.Close();
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e) {
            string sql = "SELECT * FROM USERS ORDER BY ID DESC";
            dbConnection = new SQLiteConnection("Data Source=" + Path.Combine(dbPath, "db.sqlite") + ";Version=3");

            dbConnection.Open();

            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                if (reader["USERNAME"].ToString() == username.Text && reader["PASS"].ToString() == pass.Password) {
                    ModernDialog.ShowMessage("Login successful.", "", MessageBoxButton.OK);

                    MainWindow mWindow = new MainWindow(int.Parse(reader["ID"].ToString()));
                    mWindow.Show();

                    dbConnection.Close();
                    this.Close();
                    return;
                }
            }

            ModernDialog.ShowMessage("Failed to login.", "", MessageBoxButton.OK);
        }

        private void exitButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}