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
    /// Логика взаимодействия для AutorizationPage.xaml
    /// </summary>
    public partial class AutorizationPage : Page
    {
        public AutorizationPage()
        {
            InitializeComponent();
        }

        private void btnReturnToLogIn_Click(object sender, RoutedEventArgs e)
        {
            Manager.mainFrame.Navigate(new LogInPage());
        }

        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            string errors = "";
            string[] txtFields = new string[5];

            txtFields[0] = txtName.Text;
            txtFields[1] = txtSecondName.Text;
            txtFields[2] = txtThirdName.Text;
            txtFields[3] = txtPassword.Password;
            txtFields[4] = txtLogin.Text;

            if(txtPassword.Password != txtPassword1.Password)
            {
                errors = "Пароли не совпадают";
            }

            foreach(string txtField in txtFields)
            {
                if(txtField == "")
                {
                    errors = "Не все поля заполнены";
                }
            }

            if(errors != "")
            {
                MessageBox.Show(errors, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
            }
            else
            {
                var connectionString = "Host=localhost;Username=postgres;Password=admin;Database=EquipmentAccounting";
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                try {
                    conn.Open();

                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = conn;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = $"INSERT INTO public.\"Users\"(\"name\", \"secondName\", \"thirdName\", \"login\", \"password\", \"roleId\") VALUES ('{txtFields[0]}', '{txtFields[1]}', '{txtFields[2]}', '{txtFields[4]}', '{txtFields[3]}', 1);";

                    command.ExecuteNonQuery();

                    Console.WriteLine("Пользователь добавлен");
                    Manager.mainFrame.Navigate(new MainPage());
                } catch(NpgsqlException ex)
                {
                    Console.WriteLine(ex.Message);
                } finally
                {
                    conn.Close();
                }
            }
        }
    }
}
