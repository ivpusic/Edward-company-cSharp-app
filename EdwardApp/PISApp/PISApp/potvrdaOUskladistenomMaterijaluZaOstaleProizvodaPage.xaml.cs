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
    /// Interaction logic for potvrdaOUskladistenomMaterijaluZaOstaleProizvodaPage.xaml
    /// </summary>
    public partial class potvrdaOUskladistenomMaterijaluZaOstaleProizvodaPage : Window
    {
        public potvrdaOUskladistenomMaterijaluZaOstaleProizvodaPage()
        {
            InitializeComponent();
            updateListBox();
            odgOsoba.Text = logIn.userName;
        }

        public void updateListBox()
        {
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from s in cont.potvrda_o_uskladistenom_materijalu_za_ostale_posloves
                            select s;
                postvrdaListBox.ItemsSource = query;
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id;
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    potvrda_o_uskladistenom_materijalu_za_ostale_poslove art = new potvrda_o_uskladistenom_materijalu_za_ostale_poslove
                    {
                        datum = Convert.ToDateTime(datumPicker.Text),
                        odgOsoba = logIn.userName
                    };
                    cont.potvrda_o_uskladistenom_materijalu_za_ostale_posloves.InsertOnSubmit(art);
                    cont.SubmitChanges();
                    var entry = (from ee in cont.potvrda_o_uskladistenom_materijalu_za_ostale_posloves
                                 select ee).OrderByDescending(ee => ee.id).FirstOrDefault();
                    id = entry.id;
                    iznos.tempPotvrdaOUskladistenomMaterijaluZaOstalePosloveID = id;
                }
                updateListBox();

                //MessageBox.Show(id.ToString());
                stavkePotvrdeOstaliProizvodi stavke = new stavkePotvrdeOstaliProizvodi();
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

                    var item = postvrdaListBox.SelectedItems;
                    //MessageBox.Show(jedinica.ToString());

                    foreach (potvrda_o_uskladistenom_materijalu_za_ostale_poslove j in item)
                    {
                        var _item = from _iteem in con.stavke_potvrde_o_uskladistenom_materijalu_za_ostale_posloves
                                    where _iteem.id_potvrde_za_ostale_poslove == j.id
                                    select _iteem;
                        con.stavke_potvrde_o_uskladistenom_materijalu_za_ostale_posloves.DeleteAllOnSubmit(_item);
                        con.SubmitChanges();
                    }

                    foreach (potvrda_o_uskladistenom_materijalu_za_ostale_poslove j in item)
                    {
                        var itm = (from it in con.potvrda_o_uskladistenom_materijalu_za_ostale_posloves
                                   where it.id == j.id
                                   select it).First();

                        con.potvrda_o_uskladistenom_materijalu_za_ostale_posloves.DeleteOnSubmit(itm);
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
            editPotvrdaOstaliProizvodi edit = new editPotvrdaOstaliProizvodi();
            var jedinica = postvrdaListBox.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (potvrda_o_uskladistenom_materijalu_za_ostale_poslove j in jedinica)
            {
                edit.datumPicker.Text = j.datum.ToString();
                edit.idBox.Text = j.id.ToString();
                edit.odgOsoba.Text = logIn.userName;
                iznos.tempPotvrdaOUskladistenomMaterijaluZaOstalePosloveID = j.id;
            }
            edit.ShowDialog();
            updateListBox();
        }
    }
}
