using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GUI.Pages {

    /// <summary>
    /// Interaction logic for Recipe.xaml
    /// </summary>
    public partial class Recipe : UserControl {
        public static MainWindow mWindow;

        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SMTBoardFeeder");
        private SQLiteConnection dbConnection;

        public Recipe() {
            InitializeComponent();

            loadRecipes();
        }

        private void loadRecipes() {
            //This method loads the recipes from the SQL Database.
            ObservableCollection<string> list = new ObservableCollection<string>();

            string sql = "SELECT * FROM RECIPES";
            dbConnection = new SQLiteConnection("Data Source=" + Path.Combine(dbPath, "db.sqlite") + ";Version=3");
            dbConnection.Open();

            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                list.Add(reader["ID"].ToString());
            }

            dbConnection.Close();

            recipeList.ItemsSource = list;
        }

        private void recipeList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            //This method loads the recipe's attributes from the SQL database.
            string sql = "SELECT * FROM RECIPES";
            dbConnection = new SQLiteConnection("Data Source=" + Path.Combine(dbPath, "db.sqlite") + ";Version=3");
            dbConnection.Open();

            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                if (reader["ID"].ToString() == recipeList.SelectedItem.ToString()) {
                    recipeID.Text = reader["ID"].ToString();
                    recipeWidth.Text = reader["WIDTH"].ToString();
                    recipeBirth.Text = reader["DATE"].ToString();

                    byte[] imgBytes = (Byte[])reader["IMG"];
                    recipeImage.Source = ByteToImage(imgBytes);

                    break;
                }
            }

            dbConnection.Close();
            loadRecipes();
            mWindow.recipeSelected = true;
        }

        private BitmapImage ByteToImage(byte[] imageBytes) {
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);

            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.StreamSource = ms;
            img.EndInit();

            SaveImage(img);

            return img;
        }

        private void SaveImage(BitmapImage img) {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(img));

            using (var fs = new FileStream(Path.Combine(dbPath, "tempImg.png"), FileMode.Create)) {
                encoder.Save(fs);
            }
        }
    }
}