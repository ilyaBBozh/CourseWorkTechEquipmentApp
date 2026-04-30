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
    /// Логика взаимодействия для AddNewEquipmentPage.xaml
    /// </summary>
    public partial class AddNewEquipmentPage : Page
    {
        public AddNewEquipmentPage()
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
                command.CommandText = "SELECT * FROM public.\"Statuses\"";
                NpgsqlDataReader dataReader = command.ExecuteReader();

                List<ComboBoxItem> options = new List<ComboBoxItem>();

                while (dataReader.Read())
                {
                    ComboBoxItem option = new ComboBoxItem();
                    option.Tag = dataReader.GetInt32(0);
                    option.Content = dataReader.GetString(1);
                    options.Add(option);
                }

                dataReader.Close();
                cbxStatus.ItemsSource = options;
                options = new List<ComboBoxItem>();

                command.CommandText = "SELECT id, name, \"secondName\", \"thirdName\" FROM public.\"Users\" WHERE \"roleId\" = \'3\'";
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    ComboBoxItem option = new ComboBoxItem();
                    option.Tag = dataReader.GetInt32(0);
                    option.Content = $"{dataReader.GetInt32(0)} {dataReader.GetString(1)}  {dataReader.GetString(2)} {dataReader.GetString(3)}";
                    options.Add(option);
                }

                cbxEngineer.ItemsSource = options;
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.controlFrame.Navigate(new EquipmentPage());
        }

        private void btnAddEquipment_Click(object sender, RoutedEventArgs e)
        {
            string errors = "";
            string[] txtFields = new string[7];

            txtFields[0] = txtName.Text;
            txtFields[1] = txtLength.Text;
            txtFields[2] = txtWidth.Text;
            txtFields[3] = txtHeight.Text;
            txtFields[4] = txtWeight.Text;
            txtFields[5] = cbxEngineer.SelectedValue == null ? "" : ((ComboBoxItem)cbxEngineer.SelectedItem).Tag.ToString();
            txtFields[6] = cbxStatus.SelectedValue == null ? "" : ((ComboBoxItem)cbxStatus.SelectedItem).Tag.ToString();

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
            } else
            {
                var connectionString = "Host=localhost;Username=postgres;Password=admin;Database=EquipmentAccounting";
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);

                try
                {
                    conn.Open();

                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = conn;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = $"INSERT INTO public.\"Equipment\"(\"name\", \"width\", \"length\", \"height\", \"weight\", \"engineerId\", \"statusId\") VALUES ('{txtFields[0]}', '{txtFields[2]}', '{txtFields[1]}', '{txtFields[3]}', '{txtFields[4]}', '{txtFields[5]}', '{txtFields[6]}');";

                    Console.WriteLine(txtFields[5]);
                    Console.WriteLine(txtFields[6]);

                    command.ExecuteNonQuery();

                    Manager.controlFrame.Navigate(new EquipmentPage());
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
