﻿using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Data.SQLite;
using System.IO;

namespace GUI {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow {
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SMTBoardFeeder");
        private SQLiteConnection dbConnection;

        private string _username;
        private int _id;
        public string _name;
        public string _position;
        public string _supervisor;

        public MainWindow(int ID) {
            InitializeComponent();

            Pages.UserInfo.mWindow = this;

            this._id = ID;
            getUserInfo();

            welcomeText.DisplayName = "Welcome, " + _name;
        }

        private void getUserInfo() {
            string sql = "SELECT * FROM USERS ORDER BY ID DESC";
            dbConnection = new SQLiteConnection("Data Source=" + Path.Combine(dbPath, "db.sqlite") + ";Version=3");
            dbConnection.Open();

            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
                if (reader["ID"].ToString() == _id.ToString()) {
                    _username = reader["USERNAME"].ToString();
                    _name = reader["NAME"].ToString();
                    _position = reader["POSITION"].ToString();
                    _supervisor = reader["SUPERVISOR"].ToString();
                }
            }

            dbConnection.Close();
        }
    }
}