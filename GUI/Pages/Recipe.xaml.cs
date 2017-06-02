using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using System.IO;
using System.Collections.ObjectModel;

namespace GUI.Pages {

    /// <summary>
    /// Interaction logic for Recipe.xaml
    /// </summary>
    public partial class Recipe : UserControl {
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SMTBoardFeeder");
        private SQLiteConnection dbConnection;

        private bool recipeLoaded = false;

        public Recipe() {
            InitializeComponent();

            loadRecipes();
        }

        private void loadRecipes() {
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

        private void Button_Click(object sender, RoutedEventArgs e) {
        }

        private void recipeList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            string sql = "SELECT * FROM RECIPES";
            dbConnection = new SQLiteConnection("Data Source=" + Path.Combine(dbPath, "db.sqlite") + ";Version=3");
            dbConnection.Open();

            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                if (reader["ID"].ToString() == recipeList.SelectedItem.ToString()) {
                    recipeID.Text = reader["ID"].ToString();
                    recipeWidth.Text = reader["WIDTH"].ToString();
                    recipeLot.Text = reader["LOT"].ToString();
                    recipeBirth.Text = reader["DATE"].ToString();
                }
            }

            dbConnection.Close();
        }
    }
}