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
    /// Interaction logic for skladisnicaPage.xaml
    /// </summary>
    public partial class skladisnicaPage : Window
    {
        public skladisnicaPage()
        {
            InitializeComponent();
            updateListBox();
            odgOsoba.Text = logIn.userName;
        }

        public void updateListBox()
        {
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from s in cont.skladisnicas
                            select s;
                skladisniceBox.ItemsSource = query;
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id;
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    skladisnica art = new skladisnica
                    {
                        datum = Convert.ToDateTime(datumPicker.Text),
                        
                    };
                    cont.skladisnicas.InsertOnSubmit(art);
                    cont.SubmitChanges();
                    var entry = (from ee in cont.skladisnicas
                                 select ee).OrderByDescending(ee => ee.id).FirstOrDefault();
                    id = entry.id;
                    iznos.tempSkladisnicaID = id;
                }
                updateListBox();

                //MessageBox.Show(id.ToString());
                stavkeSkladisnicePage stavke = new stavkeSkladisnicePage();
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

                    var item = skladisniceBox.SelectedItems;
                    //MessageBox.Show(jedinica.ToString());

                    foreach (skladisnica j in item)
                    {
                        var _item = from _iteem in con.stavke_skladisnices
                                    where _iteem.id_skladisnice == j.id
                                    select _iteem;
                        con.stavke_skladisnices.DeleteAllOnSubmit(_item);
                        con.SubmitChanges();
                    }

                    foreach (skladisnica j in item)
                    {
                        var itm = (from it in con.skladisnicas
                                   where it.id == j.id
                                   select it).First();

                        con.skladisnicas.DeleteOnSubmit(itm);
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
            editSkladisnica edit = new editSkladisnica();
            var jedinica = skladisniceBox.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (skladisnica j in jedinica)
            {
                edit.datumPicker.Text = j.datum.ToString();
                edit.idBox.Text = j.id.ToString();
                edit.odgOsoba.Text = logIn.userName;
                iznos.tempSkladisnicaID = j.id;
            }
            edit.ShowDialog();
            updateListBox();
        }
    }
}
