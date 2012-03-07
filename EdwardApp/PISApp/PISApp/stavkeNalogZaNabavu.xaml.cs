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
    /// Interaction logic for stavkeNalogZaNabavu.xaml
    /// </summary>
    public partial class stavkeNalogZaNabavu : Window
    {
        public stavkeNalogZaNabavu()
        {
            InitializeComponent();
            updateBox();
            updateListBox();
    
        }

        void updateBox()
        {
            string i;
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from s in cont.artiklis
                            select s;

                foreach (var st in query)
                {
                    i = st.id + "   (" + st.naziv + ")";
                    artikliBox.Items.Add(i);
                }
            }
        }

        public void updateListBox()
        {
            //MessageBox.Show(iznos.tempNalogZaNabavuID.ToString());
            //int iznos;
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from c in cont.stavke_naloga_za_nabavus from bb in cont.artiklis from kk in cont.jedinica_mjeres where c.id_artikla == bb.id && c.id_naloga == Convert.ToInt32(iznos.tempNalogZaNabavuID) && kk.id == bb.id_jedinice_mjere select new stavkee { id = c.id, id_artikla = Convert.ToInt32(c.id_artikla), id_racuna_od_dobavljača = Convert.ToInt32(c.id_naloga), kolicina = Convert.ToInt32(c.kolicina), iznos = Convert.ToInt32(c.kolicina * bb.jedinicna_cijena), naziv = bb.naziv, jedinicna_cijena = Convert.ToInt32(bb.jedinicna_cijena), jedinica_mjere = kk.naziv };  //{c.kolicina,bb.naziv, bb.jedinicna_cijena, iznos=c.kolicina*bb.jedinicna_cijena};
                stavkeNaloga.ItemsSource = query;
                iznos.iznDob = 0;
                foreach (var v in query)
                {

                    iznos.iznDob += Convert.ToInt32(v.iznos);
                }
                iznosBox.Text = iznos.iznDob.ToString();
            }
        }


        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string temp = artikliBox.SelectedItem.ToString();
                string[] words = temp.Split('(');
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    stavke_naloga_za_nabavu racun = new stavke_naloga_za_nabavu
                    {
                        id_naloga = Convert.ToInt32(idBoxx.Text.ToString()),
                        kolicina = Convert.ToInt32(kolicinaBox.Text),
                        id_artikla = Convert.ToInt32(words[0])
                    };
                    cont.stavke_naloga_za_nabavus.InsertOnSubmit(racun);
                    cont.SubmitChanges();
                }
                updateListBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se pogreška!!!" + ex.Message.ToString());
            }
        }

        public class stavkee
        {
            public int id { get; set; }
            public int id_racuna_od_dobavljača { get; set; }
            public int id_artikla { get; set; }
            public int kolicina { get; set; }
            public string naziv { get; set; }
            public int jedinicna_cijena { get; set; }
            public int iznos { get; set; }
            public string jedinica_mjere { get; set; }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext con = new pisModelDataContext())
                {

                    var item = stavkeNaloga.SelectedItems;
                    //MessageBox.Show(jedinica.ToString());
                    foreach (stavkee j in item)
                    {
                        //stavke_racuna_od_dobavljaca sta = (stavke_racuna_od_dobavljaca)j;
                        var itm = (from it in con.stavke_naloga_za_nabavus
                                   where it.id == j.id
                                   select it).First();
                        con.stavke_naloga_za_nabavus.DeleteOnSubmit(itm);
                        con.SubmitChanges();
                    }
                    updateListBox();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se pogreška!!!\n" + ex.Message.ToString());
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            editStavkeNalogaZaNabavuPage edit = new editStavkeNalogaZaNabavuPage();
            var jedinica = stavkeNaloga.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (stavkee j in jedinica)
            {
                edit.idBox.Text = j.id.ToString();
                edit.kolicinaBox.Text = j.kolicina.ToString();
                edit.sifraCombo.Text = j.id_artikla.ToString();
                //edit.idJedCijBox.Items.Add(j.id_jedinice_mjere.ToString());
                //edit.idJedCijBox.SelectedItem = 0;
            }
            edit.ShowDialog();
            updateListBox();
        }

    }
}
