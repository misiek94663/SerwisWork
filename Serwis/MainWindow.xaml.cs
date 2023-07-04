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
using MySqlConnector;

namespace Serwis
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public struct MyData
        {
            public int ID { set; get; }
            public string Tytul { set; get; }
            public string Stan { set; get; }
            public string Opis { set; get; }
            public string Priorytet { set; get; }
            public string Klient { set; get; }

        }

        public List<MyData> MyList = new List<MyData>();

        public MainWindow()
        {
            InitializeComponent();
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            connectAsync();
        }


        private async void connectAsync()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "mws02.mikr.us",
                Port = 40161,
                UserID = "root",
                Password = "Ke4rXP63Mk",
                Database = "serwis",
            };

            var connection = new MySqlConnection(builder.ConnectionString);
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = @"Select zd.id                    as ID
                                   ,zd.Tytul                        as Tytul
                                   ,stid.Opis                       as Stan
                                   ,zd.Opis                         as Opis
                                   ,CONCAT(k.Imię,' ',k.Nazwisko)   as Klient
                                   ,prid.Opis                       as Priorytet

                                    From Zadania as zd
                                    Left Join StanId as stid on stid.id=zd.stan
                                    Left Join PriotytetID as prid on prid.id=zd.Priorytet
                                    Left Join Kontakty as k on k.ID = zd.Klient
                                                ;";

            //command.Parameters.AddWithValue("@OrderId", "1");
            var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                MyData tempData = new MyData();
                tempData.ID = reader.GetInt32("ID");
                tempData.Tytul = reader.GetString("Tytul");
                try { tempData.Stan = reader.GetString("Stan");             }catch { }
                try { tempData.Opis = reader.GetString("Opis");             }catch { }
                try { tempData.Klient = reader.GetString("Klient");         }catch { }
                try { tempData.Priorytet = reader.GetString("Priorytet");   }catch { }
                MyList.Add(tempData);
            }

           dgDrawings.ItemsSource = MyList;

        }
    }
}
