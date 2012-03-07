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
    /// Interaction logic for potvrdaOuskladištenomMaterijaluZaSitotisakPage.xaml
    /// </summary>
    public partial class potvrdaOuskladištenomMaterijaluZaSitotisakPage : Window
    {
        public potvrdaOuskladištenomMaterijaluZaSitotisakPage()
        {
            InitializeComponent();
            odgOsoba.Text = logIn.userName;
            updateListBox();
        }

        public void updateListBox()
        {
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from s in cont.potvrda_o_uskladistenom_materijalu_za_sitotisaks
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
                    potvrda_o_uskladistenom_materijalu_za_sitotisak art = new potvrda_o_uskladistenom_materijalu_za_sitotisak
                    {
                        datum = Convert.ToDateTime(datumPicker.Text),
                        odgOsoba = logIn.userName
                    };
                    cont.potvrda_o_uskladistenom_materijalu_za_sitotisaks.InsertOnSubmit(art);
                    cont.SubmitChanges();
                    var entry = (from ee in cont.potvrda_o_uskladistenom_materijalu_za_sitotisaks
                                 select ee).OrderByDescending(ee => ee.id).FirstOrDefault();
                    id = entry.id;
                    iznos.tempPotvrdaOUskladistenomMaterijaluZaSitotisakID = id;
                }
                updateListBox();

                //MessageBox.Show(id.ToString());
                stavkePotvrdeZaSitotisak stavke = new stavkePotvrdeZaSitotisak();
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

                    foreach (potvrda_o_uskladistenom_materijalu_za_sitotisak j in item)
                    {
                        var _item = from _iteem in con.stavke_potvrde_o_uskladistenom_materijalu_za_sitotisaks
                                    where _iteem.id_potvrde_o_uskl_materijalu_za_sitotisak == j.id
                                    select _iteem;
                        con.stavke_potvrde_o_uskladistenom_materijalu_za_sitotisaks.DeleteAllOnSubmit(_item);
                        con.SubmitChanges();
                    }

                    foreach (potvrda_o_uskladistenom_materijalu_za_sitotisak j in item)
                    {
                        var itm = (from it in con.potvrda_o_uskladistenom_materijalu_za_sitotisaks
                                   where it.id == j.id
                                   select it).First();

                        con.potvrda_o_uskladistenom_materijalu_za_sitotisaks.DeleteOnSubmit(itm);
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
            editPotvrdaSitotisak edit = new editPotvrdaSitotisak();
            var jedinica = postvrdaListBox.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (potvrda_o_uskladistenom_materijalu_za_sitotisak j in jedinica)
            {
                edit.datumPicker.Text = j.datum.ToString();
                edit.idBox.Text = j.id.ToString();
                edit.odgOsoba.Text = logIn.userName;
                iznos.tempPotvrdaOUskladistenomMaterijaluZaSitotisakID = j.id;
            }
            edit.ShowDialog();
            updateListBox();
        }
    }
}
