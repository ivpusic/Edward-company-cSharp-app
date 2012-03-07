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
    /// Interaction logic for racunOdDobavljacaPage.xaml
    /// </summary>
    public partial class racunOdDobavljacaPage : Window
    {
        public racunOdDobavljacaPage()
        {
            InitializeComponent();
            updateListBox();
        }
        public int id;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    racun_od_dobavljaca racun = new racun_od_dobavljaca
                    {
                        datum = Convert.ToDateTime(datumBox.Text),
                        adresa = adresaBox.Text
                    };
                    cont.racun_od_dobavljacas.InsertOnSubmit(racun);
                    cont.SubmitChanges();
                    var entry = (from ee in cont.racun_od_dobavljacas
                                 select ee).OrderByDescending(ee => ee.br_racuna).FirstOrDefault();
                    id = entry.br_racuna;
                    iznos.tempID = entry.br_racuna;
                }
                updateListBox();

                //MessageBox.Show(id.ToString());
                stavkeRacunaOdDobPage stavke = new stavkeRacunaOdDobPage();
                stavke.idBoxx.Text = id.ToString();
                //MessageBox.Show(iznos.tempID.ToString());
                stavke.ShowDialog();

            //}

            //catch (Exception ex)
            //{
               // MessageBox.Show("Dogodila se pogreška!!!\n" + ex.Message.ToString());
            //}
        }

        public void updateListBox()
        {
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from c in cont.racun_od_dobavljacas select c;
                racunOdDobBox.ItemsSource = query;
                
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext con = new pisModelDataContext())
                {

                    var item = racunOdDobBox.SelectedItems;
                    //MessageBox.Show(jedinica.ToString());

                    foreach (racun_od_dobavljaca j in item)
                    {
                        var _item = from _iteem in con.stavke_racuna_od_dobavljacas
                                    where _iteem.id_racuna_od_dobavljaca == j.br_racuna
                                    select _iteem;
                        con.stavke_racuna_od_dobavljacas.DeleteAllOnSubmit(_item);
                        con.SubmitChanges();
                    }

                    foreach (racun_od_dobavljaca j in item)
                    {
                        var itm = (from it in con.racun_od_dobavljacas
                                   where it.br_racuna == j.br_racuna
                                   select it).First();

                        con.racun_od_dobavljacas.DeleteOnSubmit(itm);
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
            editRacunOdDobPageee edit = new editRacunOdDobPageee();
            var jedinica = racunOdDobBox.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (racun_od_dobavljaca j in jedinica)
            {
                edit.idBox.Text = j.br_racuna.ToString();
                edit.adresaBox.Text = j.adresa;
                edit.datumBox.Text = j.datum.ToString();
                //edit.idJedCijBox.Items.Add(j.id_jedinice_mjere.ToString());
                //edit.idJedCijBox.SelectedItem = 0;
            }
            edit.ShowDialog();
            updateListBox();
        }

    }
}
