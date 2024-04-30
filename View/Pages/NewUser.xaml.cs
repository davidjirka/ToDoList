using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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
using TodoList.Services.DbConnect;
using TodoList.View.UserControls;

namespace TodoList.View.Pages
{
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : Page
    {
        public NewUser()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string userName = userNameTB.Text;

            DbConnect conn = new DbConnect();
            int userId = conn.GetLastIdByTable("User");

            conn.Open();

            SqlConnection connection = conn.GetConnection();
            string query = "INSERT INTO [User] (Id, Name) VALUES (@Value1, @Value2)";

            SqlCommand sql = new SqlCommand(query, connection);
            sql.Parameters.AddWithValue("@Value1", ++userId);
            sql.Parameters.AddWithValue("@Value2", userName);
            sql.ExecuteNonQuery();

            conn.Close();

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Content = new TaskList();
        }
    }
}
