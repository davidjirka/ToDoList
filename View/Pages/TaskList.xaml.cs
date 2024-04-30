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
using Task = TodoList.Models.Task;

namespace TodoList.View.Pages
{
    /// <summary>
    /// Interaction logic for TaskList.xaml
    /// </summary>
    public partial class TaskList : Page
    {
        public TaskList()
        {
            InitializeComponent();
            
            

            // Generate tasks
            DbConnect sqlCon = new DbConnect();

            sqlCon.Open();
            string taskListQuery = "SELECT * from [Task]";
            SqlDataReader taskData = sqlCon.GetDataByQuery(taskListQuery);

            List<Task> taskList = new List<Task> { };

            while (taskData.Read())
            {
                taskList.Add(new Task { Id = (int)taskData["Id"], UserId = (int)taskData["UserId"], TaskStatusId = (int)taskData["TaskStatusId"], Title = (string)taskData["Title"], Description = (string)taskData["Description"], DueTo = (DateTime)taskData["DueTo"] });
            }
            sqlCon.Close();


            

            
            foreach (Task task in taskList)
            {
                
                Grid taskGrid = new Grid();
                taskGrid.Background = new SolidColorBrush(Colors.AliceBlue);
                taskGrid.Margin = new Thickness(10);
                taskGrid.ColumnDefinitions.Add(new ColumnDefinition());
                taskGrid.ColumnDefinitions.Add(new ColumnDefinition());
                taskGrid.ColumnDefinitions.Add(new ColumnDefinition());
                taskGrid.ColumnDefinitions.Add(new ColumnDefinition());

                for (int i = 0; i < 2; i++)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    rowDefinition.Height = new GridLength(60);
                    taskGrid.RowDefinitions.Add(rowDefinition);
                }

                int rowI = 0;
                int colI = 0;
                for (int i = 0; i < 8; i++)
                {
                    Grid secondaryGrid = new Grid();
                    taskGrid.Children.Add(secondaryGrid);

                    for (int j = 0; j < 2; j++)
                    {
                        RowDefinition rowDefinition = new RowDefinition();
                        rowDefinition.Height = new GridLength(30);
                        //if (j == 1)
                        //{
                        //    Grid.SetRowSpan(taskGrid, 3);
                        //}
                        secondaryGrid.RowDefinitions.Add(rowDefinition);
                    }

                    Grid.SetColumn(secondaryGrid, colI);
                    Grid.SetRow(secondaryGrid, rowI);
                    
                    

                    for (int j = 0; j < 2; j++)
                    {                        
                        if (j == 0)
                        {
                            Label label = new Label();
                            secondaryGrid.Children.Add(label);

                            string labelText;

                            switch (i)
                            {
                                case 0:
                                    labelText = "Id";
                                break;
                                case 1:
                                    labelText = "UserId";
                                break;
                                case 2:
                                    labelText = "TaskStatusId";
                                break;
                                case 3:
                                    labelText = "Title";
                                break;
                                case 4:
                                    labelText = "DueTo";
                                break;
                                case 5:
                                    labelText = "Description";
                                break;
                                case 6:
                                    labelText = "Edit";
                                break;
                                case 7:
                                    labelText = "Delete";
                                break;
                                default:
                                    labelText = "undefined";
                                break;
                            }

                            label.Content = labelText;
                            Grid.SetRow(label, j);
                        } else
                        {
                            TextBlock textBlock = new TextBlock();
                            secondaryGrid.Children.Add(textBlock);

                            string textBlockText;

                            switch (i)
                            {
                                case 0:
                                    textBlockText = task.Id.ToString();
                                break;
                                case 1:
                                    textBlockText = task.UserId.ToString();
                                break;
                                case 2:
                                    textBlockText = task.TaskStatusId.ToString();
                                break;
                                case 3:
                                    textBlockText = task.Title;
                                break;
                                case 4:
                                    textBlockText = task.DueTo.ToString();
                                break;
                                case 5:
                                    textBlockText = task.Description;
                                break;
                                case 6:
                                    textBlockText = "";
                                break;
                                case 7:
                                    textBlockText = "";
                                break;
                                default:
                                    textBlockText = "undefined";
                                break;
                            }

                            textBlock.Text = textBlockText;
                            Grid.SetRow(textBlock, j);
                        }
                    }

                    colI++;
                    if (colI == 4)
                    {
                        colI = 0;
                        rowI = 1;
                    }
                }

                

                stackPanel1.Children.Add(taskGrid);
            }



            

            
        }
    }
}
