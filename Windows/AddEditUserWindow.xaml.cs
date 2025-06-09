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
using System.Windows.Shapes;

namespace WpfApp1.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditUserWindow.xaml
    /// </summary>
    public partial class AddEditUserWindow : Window
    {
        private StudentDiaryEntities db = new StudentDiaryEntities();
        private Users editingUser = null;
        public AddEditUserWindow(Users user = null)
        {
            InitializeComponent();

            if (user != null)
            {
                editingUser = db.Users.FirstOrDefault(u => u.UserID == user.UserID);
                Title = "Редактирование пользователя";
                FullName.Text = editingUser.FullName;
                Login.Text = editingUser.Login;
                Password.Text = editingUser.PasswordHash;
            }
            else 
            {
                Title = "Добавление пользователя";
            }
        }
        //Метод сохранения для карточки
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullName.Text;
            string login = Login.Text;
            string password = Password.Text;

            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните все обязательные поля!");
                return;
            }

            if (editingUser == null)
            {
                if (db.Users.Any(u => u.Login == login))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!");
                    return;
                }

                var newUser = new Users
                {
                    FullName = fullName,
                    Login = login,
                    PasswordHash = password,
                };

                db.Users.Add(newUser);
            }
            else 
            {
                editingUser.FullName = fullName;
                editingUser.Login = login;
                editingUser.PasswordHash = password;
            }

            db.SaveChanges();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
