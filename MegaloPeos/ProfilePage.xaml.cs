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
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();

            var connectionString = "Host=localhost;Username=postgres;Password=admin;Database=EquipmentAccounting";
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);

            try
            {
                conn.Open();

                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = $"SELECT \"Users\".name, \"Users\".\"secondName\", \"Users\".\"thirdName\", \"Users\".login, \"Roles\".name FROM public.\"Users\" INNER JOIN public.\"Roles\" ON \"Roles\".id = \"Users\".\"roleId\" Where \"Users\".id = \'{Manager.userId}\'";
                NpgsqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    textLogin.Text += dataReader.GetString(3);
                    textName.Text += dataReader.GetString(0);
                    textSecondName.Text += dataReader.GetString(1);
                    textThirdName.Text += dataReader.GetString(2);
                    textRole.Text += dataReader.GetString(4);
                    Manager.role = dataReader.GetString(4);
                }

            } catch(NpgsqlException ex)
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
