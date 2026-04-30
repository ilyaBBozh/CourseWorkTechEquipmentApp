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
    /// Логика взаимодействия для CoworkersPage.xaml
    /// </summary>
    public partial class CoworkersPage : Page
    {
        public CoworkersPage()
        {
            InitializeComponent();

            if(Manager.role != "Администратор")
            {
                btnAdd.Visibility = Visibility.Hidden;
            }

            var connectionString = "Host=localhost;Username=postgres;Password=admin;Database=EquipmentAccounting";
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);

            try
            {
                
                conn.Open();
                if(Manager.role == "Администратор")
                {
                    string command = $"SELECT \"Users\".id, \"Users\".name, \"Users\".\"secondName\", \"Users\".\"thirdName\", \"Users\".login, \"Users\".password, \"Roles\".name FROM public.\"Users\" INNER JOIN public.\"Roles\" ON \"Roles\".id = \"Users\".\"roleId\"";

                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command, conn);
                    System.Data.DataTable table = new System.Data.DataTable();
                    dataAdapter.Fill(table);

                    table.Columns[0].ColumnName = "Номер";
                    table.Columns[1].ColumnName = "Имя";
                    table.Columns[2].ColumnName = "Фамилия";
                    table.Columns[3].ColumnName = "Отчество";
                    table.Columns[4].ColumnName = "Логин";
                    table.Columns[5].ColumnName = "Пароль";
                    table.Columns[6].ColumnName = "Роль";

                    coworkersTable.ItemsSource = table.DefaultView;
                }
                else
                {
                    string command = $"SELECT \"Users\".id, \"Users\".name, \"Users\".\"secondName\", \"Users\".\"thirdName\", \"Roles\".name FROM public.\"Users\" INNER JOIN public.\"Roles\" ON \"Roles\".id = \"Users\".\"roleId\"";

                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command, conn);
                    System.Data.DataTable table = new System.Data.DataTable();
                    dataAdapter.Fill(table);

                    table.Columns[0].ColumnName = "Номер";
                    table.Columns[1].ColumnName = "Имя";
                    table.Columns[2].ColumnName = "Фамилия";
                    table.Columns[3].ColumnName = "Отчество";
                    table.Columns[4].ColumnName = "Роль";

                    coworkersTable.ItemsSource = table.DefaultView;
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.controlFrame.Navigate(new AddNewUserPage());
        }
    }
}
