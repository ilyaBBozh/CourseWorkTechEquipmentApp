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
    /// Логика взаимодействия для EquipmentPage.xaml
    /// </summary>
    public partial class EquipmentPage : Page
    {
        public EquipmentPage()
        {
            InitializeComponent();

            if (Manager.role != "Супервайзер")
            {
                btnAdd.Visibility = Visibility.Hidden;
            }

            var connectionString = "Host=localhost;Username=postgres;Password=admin;Database=EquipmentAccounting";
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);

            try
            {

                conn.Open();
                if (Manager.role == "Супервайзер")
                {
                    string command = "SELECT \"Equipment\".\"id\", \"Equipment\".\"name\", width, length, height, weight, CONCAT_WS(' ', \"Users\".\"secondName\", \"Users\".\"name\", \"Users\".\"thirdName\") AS FIO, \"Statuses\".\"name\" as status FROM public.\"Equipment\" INNER JOIN public.\"Users\" ON public.\"Equipment\".\"engineerId\" = public.\"Users\".\"id\" INNER JOIN public.\"Statuses\" ON public.\"Equipment\".\"statusId\" = public.\"Statuses\".\"id\"";

                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(command, conn);
                    System.Data.DataTable table = new System.Data.DataTable();
                    dataAdapter.Fill(table);

                    table.Columns[0].ColumnName = "Номер";
                    table.Columns[1].ColumnName = "Имя";
                    table.Columns[2].ColumnName = "Ширина";
                    table.Columns[3].ColumnName = "Длина";
                    table.Columns[4].ColumnName = "Высота";
                    table.Columns[5].ColumnName = "Вес";
                    table.Columns[6].ColumnName = "Ответственный инженер";
                    table.Columns[7].ColumnName = "Статус";

                    equipmentTable.ItemsSource = table.DefaultView;
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
            Manager.controlFrame.Navigate(new AddNewEquipmentPage());
        }
    }
}
