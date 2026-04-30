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
using Npgsql;

namespace MegaloPeos
{
    /// <summary>
    /// Логика взаимодействия для LogInPage.xaml
    /// </summary>
    public partial class LogInPage : Page
    {
        public LogInPage()
        {
            InitializeComponent();
        }

        private void btnAutor_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new AutorizationPage());
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            if(txtLogin.Text == "" || txtPassword.Password == "")
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
            }
            else
            {
                var connectionString = "Host=localhost;Username=postgres;Password=admin;Database=EquipmentAccounting";
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                try
                {
                    conn.Open();

                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = conn;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = $"SELECT * FROM public.\"Users\" WHERE login = '{txtLogin.Text}'";
                    NpgsqlDataReader dataReader = command.ExecuteReader();

                    string password = "";
                    while (dataReader.Read())
                    {
                        password = dataReader.GetString(5);
                        Manager.userId = dataReader.GetInt32(0);
                    }
                    
                    if(password != "" && password == txtPassword.Password)
                    {
                        Manager.mainFrame.Navigate(new MainPage());
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
                    }
                }
                catch (NpgsqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
