using System;
using System.Collections.Generic;
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
using TodoList.Models;
using TodoList.Services.DbConnect;
using TaskStatus = TodoList.Models.TaskStatus;

namespace TodoList.View.Pages
{
    /// <summary>
    /// Interaction logic for NewTask.xaml
    /// </summary>
    public partial class NewTask : Page
    {
        public NewTask()
        {
            InitializeComponent();

            // Generate Hours in ComboBox
            for (int i = 1; i <= 24; i++)
            {
                hoursCB.Items.Add(i);
            }

            // Generate Minutes in ComboBox
            for (int i = 0; i <= 60; i += 5)
            {
                minutesCB.Items.Add(i);
            }

            DbConnect sqlCon = new DbConnect();

            // Asigne items in task status ComboBox
            sqlCon.Open();
            string taskStatusQuery = "SELECT * from [TaskStatus]";
            SqlDataReader taskStatusData = sqlCon.GetDataByQuery(taskStatusQuery);

            List<TaskStatus> taskStatus = new List<TaskStatus> { };

            while (taskStatusData.Read())
            {
                taskStatus.Add(new TaskStatus { Id = (int)taskStatusData["Id"], Title = (string)taskStatusData["Title"] });
            }
            sqlCon.Close();

            taskStatusCB.ItemsSource = taskStatus;
            taskStatusCB.DisplayMemberPath = "Title";

            // Asigne items in user 
            sqlCon.Open();
            string userQuery = "SELECT * from [User]";
            SqlDataReader userData = sqlCon.GetDataByQuery(userQuery);

            List<User> users = new List<User> { };
            
            while (userData.Read())
            {
                users.Add(new User { Id = (int)userData["Id"], Name = (string)userData["Name"] });
            }
            sqlCon.Close();

            userCB.ItemsSource = users;
            userCB.DisplayMemberPath = "Name";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // get all data from inputs
            User selectedUser = userCB.SelectedItem as User;
            TaskStatus selectedTaskStatus = taskStatusCB.SelectedItem as TaskStatus;
            string taskTitle = taskTitleTB.Text;
            string date = dateDP.Text;
            int hour = int.Parse(hoursCB.Text);
            int minute = int.Parse(minutesCB.Text);
            string description = descriptionTB.Text;

            // create dateTime by datePicker, hour and minute
            DateTime dateTime;
            DateTime.TryParseExact(date, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out dateTime);
            dateTime = dateTime.AddHours(hour).AddMinutes(minute);

            DbConnect sqlCon = new DbConnect();
            int lastTaskId = sqlCon.GetLastIdByTable("Task");

            sqlCon.Open();

            SqlConnection connection = sqlCon.GetConnection();
            string query = "INSERT INTO [Task] (Id, UserId, TaskStatusId, Title, Description, DueTo) VALUES (@IdValue, @UserIdValue, @TaskStatusIdValue, @TitleValue, @DescriptionValue, @DueToValue)";

            SqlCommand sql = new SqlCommand(query, connection);
            sql.Parameters.AddWithValue("@IdValue", ++lastTaskId);
            sql.Parameters.AddWithValue("@UserIdValue", selectedUser.Id);
            sql.Parameters.AddWithValue("@TaskStatusIdValue", selectedTaskStatus.Id);
            sql.Parameters.AddWithValue("@TitleValue", taskTitle);
            sql.Parameters.AddWithValue("@DescriptionValue", description);
            sql.Parameters.AddWithValue("@DueToValue", dateTime);
            sql.ExecuteNonQuery();

            sqlCon.Close();

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Content = new TaskList();
        }
    }
}
