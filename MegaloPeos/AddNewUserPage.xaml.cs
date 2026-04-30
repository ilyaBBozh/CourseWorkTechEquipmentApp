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
    /// Логика взаимодействия для AddNewUserPage.xaml
    /// </summary>
    public partial class AddNewUserPage : Page
    {
        public AddNewUserPage()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.controlFrame.Navigate(new CoworkersPage());
        }

        private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            string errors = "";
            string[] txtFields = new string[6];
            int role = 0;

            txtFields[0] = txtName.Text;
            txtFields[1] = txtSecondName.Text;
            txtFields[2] = txtThirdName.Text;
            txtFields[3] = txtPassword.Password;
            txtFields[4] = txtLogin.Text;
            txtFields[5] = cbxRole.SelectedValue == null ? "" : cbxRole.SelectedValue.ToString();
            Console.WriteLine(txtFields[5]);

            if (txtPassword.Password != txtPassword1.Password)
            {
                errors = "Пароли не совпадают";
            }

            foreach (string txtField in txtFields)
            {
                if (txtField == "")
                {
                    errors = "Не все поля заполнены";
                }
            }

            if (errors != "")
            {
                MessageBox.Show(errors, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
            }

            else
            {
                switch (txtFields[5])
                {
                    case "Инженер":
                        role = 3;
                        break;
                    case "Супервайзер":
                        role = 2;
                        break;
                    case "Перевозчик":
                        role = 5;
                        break;
                }

                var connectionString = "Host=localhost;Username=postgres;Password=admin;Database=EquipmentAccounting";
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                try
                {
                    conn.Open();

                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = conn;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = $"INSERT INTO public.\"Users\"(\"name\", \"secondName\", \"thirdName\", \"login\", \"password\", \"roleId\") VALUES ('{txtFields[0]}', '{txtFields[1]}', '{txtFields[2]}', '{txtFields[4]}', '{txtFields[3]}', '{role}');";

                    command.ExecuteNonQuery();

                    Console.WriteLine("Пользователь добавлен");
                }
                catch (NpgsqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                    Manager.controlFrame.Navigate(new CoworkersPage());
                }
            }
        }
    }
}
