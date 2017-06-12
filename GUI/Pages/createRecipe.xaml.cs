using System;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace GUI.Pages {

    /// <summary>
    /// Interaction logic for createRecipe.xaml
    /// </summary>
    public partial class createRecipe : UserControl {
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SMTBoardFeeder");
        private SQLiteConnection dbConnection;

        public createRecipe() {
            InitializeComponent();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e) {
            //SQLiteCommand command = dbConnection.CreateCommand();
            //command.CommandText = String.Format("INSERT INTO RECIPES (ID, WIDTH, DATE, IMG) VALUES (@ID, @WIDTH, @DATE, @IMG);");

            string upload = "INSERT INTO RECIPES (ID, WIDTH, DATE, IMG) VALUES (" + recipeID.Text + ","
                + Double.Parse(recipeWidth.Text) + "," + DateTime.Now.ToString() + ","
                + ImageToByte(System.Drawing.Image.FromFile(recipeTemp.Text), System.Drawing.Imaging.ImageFormat.Png) + ")";

            dbConnection = new SQLiteConnection("Data Source=" + Path.Combine(dbPath, "db.sqlite") + ";Version=3");
            SQLiteCommand command = new SQLiteCommand(upload, dbConnection);

            /*SQLiteParameter ID = new SQLiteParameter("@ID", System.Data.DbType.Double);
            ID.Value = recipeID.Text;

            SQLiteParameter WIDTH = new SQLiteParameter("@WIDTH", System.Data.DbType.Double);
            WIDTH.Value = Double.Parse(recipeWidth.Text);

            SQLiteParameter DATE = new SQLiteParameter("@DATE", System.Data.DbType.String);
            DATE.Value = DateTime.Now.ToString();

            SQLiteParameter IMG = new SQLiteParameter("@IMG", System.Data.DbType.Binary);
            IMG.Value = ImageToByte(System.Drawing.Image.FromFile(recipeTemp.Text), System.Drawing.Imaging.ImageFormat.Png);

            command.Parameters.Add(ID);
            command.Parameters.Add(WIDTH);
            command.Parameters.Add(DATE);
            command.Parameters.Add(IMG);*/

            dbConnection.Open();

            command.ExecuteNonQuery();

            dbConnection.Close();
        }

        private void browseFile_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.DefaultExt = ".png";
            Nullable<bool> result = dialog.ShowDialog();

            if (result == true) {
                string filename = dialog.FileName;
                recipeTemp.Text = filename;
            }
        }

        private byte[] ImageToByte(System.Drawing.Image img, System.Drawing.Imaging.ImageFormat format) {
            using (MemoryStream ms = new MemoryStream()) {
                img.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                return imageBytes;
            }
        }
    }
}