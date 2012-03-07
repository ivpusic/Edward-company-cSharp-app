using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PISApp
{
    /// <summary>
    /// Interaction logic for zahtjevnicaPage.xaml
    /// </summary>
    public partial class zahtjevnicaPage : Window
    {
        public zahtjevnicaPage()
        {
            InitializeComponent();
            updateBox();
            updateListBox();
            odgOsobaBox.Text = logIn.userName;
        }

        void updateBox()
        {
            string i;
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from s in cont.izdatnicas
                            select s;

                foreach (var st in query)
                {
                    i = st.id + "   (" + st.datum + ")";
                    izdatnicaBox.Items.Add(i);
                }
            }
        }

        public void updateListBox()
        {
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from s in cont.zahtjevnicas
                            select s;
                zahtjevniceBox.ItemsSource = query;
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string temp = izdatnicaBox.SelectedItem.ToString();
                string[] words = temp.Split('(');
                int id;
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    zahtjevnica art = new zahtjevnica
                    {
                        datum = Convert.ToDateTime(datumPicker.Text),
                        id_izdatnice = Convert.ToInt32(words[0])
                    };
                    cont.zahtjevnicas.InsertOnSubmit(art);
                    cont.SubmitChanges();
                    var entry = (from ee in cont.zahtjevnicas
                                 select ee).OrderByDescending(ee => ee.id).FirstOrDefault();
                    id = entry.id;
                    iznos.tempZahtjevnicaID = id;
                }
                updateListBox();

                //MessageBox.Show(id.ToString());
                stavkeZahtjevnicePage stavke = new stavkeZahtjevnicePage();
                stavke.idBoxx.Text = id.ToString();
                //MessageBox.Show(iznos.tempID.ToString());
                stavke.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se pogreška!!!\n" + ex.Message.ToString());
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext con = new pisModelDataContext())
                {

                    var item = zahtjevniceBox.SelectedItems;
                    //MessageBox.Show(jedinica.ToString());

                    foreach (zahtjevnica j in item)
                    {
                        var _item = from _iteem in con.stavke_zahtjevnices
                                    where _iteem.id_zahtjevnice == j.id
                                    select _iteem;
                        con.stavke_zahtjevnices.DeleteAllOnSubmit(_item);
                        con.SubmitChanges();
                    }

                    foreach (zahtjevnica j in item)
                    {
                        var itm = (from it in con.zahtjevnicas
                                   where it.id == j.id
                                   select it).First();

                        con.zahtjevnicas.DeleteOnSubmit(itm);
                        //con.stavke_racuna_od_dobavljacas.DeleteOnSubmit(_itm);
                        con.SubmitChanges();
                    }
                    updateListBox();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se pogreška!!!" + ex.Message.ToString());
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            editZahtjevnica edit = new editZahtjevnica();
            var jedinica = zahtjevniceBox.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (zahtjevnica j in jedinica)
            {
                edit.datumPicker.Text = j.datum.ToString();
                edit.idBox.Text = j.id.ToString();
                edit.odgOsobaBox.Text = logIn.userName;
                edit.izdatnicaBox.Text = j.id_izdatnice.ToString();
                iznos.tempZahtjevnicaID = j.id;
            }
            
            edit.ShowDialog();
            updateListBox();
        }
    }
}
