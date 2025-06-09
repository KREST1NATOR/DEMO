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
using WpfApp1.Windows;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        private StudentDiaryEntities db = new StudentDiaryEntities();
        public UsersPage()
        {
            InitializeComponent();
            LoadDataGrid();
        }
        //Метод заполнения таблицы
        private void LoadDataGrid()
        {
            db = new StudentDiaryEntities();
            UsersDataGrid.ItemsSource = db.Users.Include("Roles").ToList();
        }
        //Метод добавления
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addEditUser = new AddEditUserWindow();
            addEditUser.ShowDialog();
            LoadDataGrid();
        }
        //Метод редактирования
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is Users selectedUser)
            {
                var editWindow = new AddEditUserWindow(selectedUser);
                editWindow.ShowDialog();
                LoadDataGrid();
            }
            else 
            {
                MessageBox.Show("Выберите пользователя!");
            }
        }
        //Обновление таблицы
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }
        //Кнопка назад
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MenuPage menuPage = new MenuPage();
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.Title = menuPage.Title;
            mainWindow.MainFrame.Navigate(menuPage);
        }
        //Метод удаления
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem == null) return;

            try
            {
                int id = ((dynamic)UsersDataGrid.SelectedItem).UserID;
                using (var db = new StudentDiaryEntities())
                {
                    var user = new Users { UserID = id };
                    db.Users.Attach(user);
                    db.Users.Remove(user);
                    db.SaveChanges();
                }
                LoadDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
        }
    }
}
