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
    /// Interaction logic for nalogZaSkladistenjePage.xaml
    /// </summary>
    public partial class nalogZaSkladistenjePage : Window
    {
        public nalogZaSkladistenjePage()
        {
            InitializeComponent();
            updateBox();
            odgOsobaBox.Text = logIn.userName;
            updateListBox();
        }

        void updateBox()
        {
            string i;
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from s in cont.organizacijska_jedinicas
                            select s;

                foreach (var st in query)
                {
                    i = st.id + "   (" + st.naziv + ")";
                    orgJedBox.Items.Add(i);
                }
            }
        }

        void updateListBox()
        {
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from c in cont.nalog_za_skladistenjes select c;
                naloziListBox.ItemsSource = query;
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string temp = orgJedBox.SelectedItem.ToString();
                string[] words = temp.Split('(');
                int id;
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    nalog_za_skladistenje art = new nalog_za_skladistenje
                    {
                        datum = Convert.ToDateTime(datumPicker.Text),
                        Nalog_izdao_la = logIn.userName,
                        id_org_jed = Convert.ToInt32(words[0])
                    };
                    cont.nalog_za_skladistenjes.InsertOnSubmit(art);
                    cont.SubmitChanges();
                    var entry = (from ee in cont.nalog_za_skladistenjes
                                 select ee).OrderByDescending(ee => ee.broj_narudzbe).FirstOrDefault();
                    id = entry.broj_narudzbe;
                    iznos.nalogZaSkladistenjeID = id;
                }
                updateListBox();

                //MessageBox.Show(id.ToString());
                stavkeNalogaZaSkladistenje stavke = new stavkeNalogaZaSkladistenje();
                stavke.idBoxx.Text = id.ToString();
                //MessageBox.Show(iznos.tempID.ToString());
                stavke.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se pogreška!!!\n" + ex.Message.ToString());
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            editNalogZaSkladistenjePage edit = new editNalogZaSkladistenjePage();
            var jedinica = naloziListBox.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (nalog_za_skladistenje j in jedinica)
            {

                edit.idBox.Text = j.broj_narudzbe.ToString();
                edit.datumPicker.Text = j.datum.ToString();
                edit.orgJedBox.Text = j.id_org_jed.ToString();
                edit.odgOsobaBox.Text = logIn.userName;
                //edit.idJedCijBox.Items.Add(j.id_jedinice_mjere.ToString());
                //edit.idJedCijBox.SelectedItem = 0;
                iznos.nalogZaSkladistenjeID = j.broj_narudzbe;
            }

            edit.ShowDialog();
            updateListBox();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext con = new pisModelDataContext())
                {

                    var item = naloziListBox.SelectedItems;
                    //MessageBox.Show(jedinica.ToString());

                    foreach (nalog_za_skladistenje j in item)
                    {
                        var _item = from _iteem in con.stavke_naloga_za_skladistenjes
                                    where _iteem.id_naloga_za_skladistenje == j.broj_narudzbe
                                    select _iteem;
                        con.stavke_naloga_za_skladistenjes.DeleteAllOnSubmit(_item);
                        con.SubmitChanges();
                    }

                    foreach (nalog_za_skladistenje j in item)
                    {
                        var itm = (from it in con.nalog_za_skladistenjes
                                   where it.broj_narudzbe == j.broj_narudzbe
                                   select it).First();

                        con.nalog_za_skladistenjes.DeleteOnSubmit(itm);
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
    }
}
