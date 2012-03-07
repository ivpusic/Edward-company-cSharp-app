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
using System.Windows.Threading;

namespace PISApp
{
    /// <summary>
    /// Interaction logic for NarudzbenicaPage.xaml
    /// </summary>
    public partial class NarudzbenicaPage : Window
    {
        public NarudzbenicaPage()
        {
            InitializeComponent();
            updateBox();
            updateListBox();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                this.timeLabel.Content = DateTime.Now.ToString();
            }, this.Dispatcher);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string temp = orgJedBox.SelectedItem.ToString();
                string[] words = temp.Split('(');
                int id;
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    narudzbenica art = new narudzbenica
                    {
                        datum = Convert.ToDateTime(datumPicker.Text),
                        vrijeme = vrijemeBox.Text,
                        id_org_jed = Convert.ToInt32(words[0])
                    };
                    cont.narudzbenicas.InsertOnSubmit(art);
                    cont.SubmitChanges();
                    var entry = (from ee in cont.narudzbenicas
                                 select ee).OrderByDescending(ee => ee.id).FirstOrDefault();
                    id = entry.id;
                    iznos.tempNarudzbenicaID = id;
                }
                updateListBox();

                //MessageBox.Show(id.ToString());
                stavkeNarudzbenice stavke = new stavkeNarudzbenice();
                stavke.idBoxx.Text = id.ToString();
                //MessageBox.Show(iznos.tempID.ToString());
                stavke.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se pogreška!!!\n" + ex.Message.ToString());
            }
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
                var query = from c in cont.narudzbenicas select c;
                narudzbeniceListBox.ItemsSource = query;
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext con = new pisModelDataContext())
                {

                    var item = narudzbeniceListBox.SelectedItems;
                    //MessageBox.Show(jedinica.ToString());

                    foreach (narudzbenica j in item)
                    {
                        var _item = from _iteem in con.stavke_narudzbenices
                                    where _iteem.id_narudzbencie == j.id
                                    select _iteem;
                        con.stavke_narudzbenices.DeleteAllOnSubmit(_item);
                        con.SubmitChanges();
                    }

                    foreach (narudzbenica j in item)
                    {
                        var itm = (from it in con.narudzbenicas
                                   where it.id == j.id
                                   select it).First();

                        con.narudzbenicas.DeleteOnSubmit(itm);
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
            editNarudzbenica edit = new editNarudzbenica();
            var jedinica = narudzbeniceListBox.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (narudzbenica j in jedinica)
            {

                edit.idBox.Text = j.id.ToString();
                edit.vrijemeBox.Text = j.vrijeme;
                edit.datumPicker.Text = j.datum.ToString();
                edit.orgJedBox.Text = j.id_org_jed.ToString();
                //edit.idJedCijBox.Items.Add(j.id_jedinice_mjere.ToString());
                //edit.idJedCijBox.SelectedItem = 0;
                iznos.tempNarudzbenicaID = j.id;
            }
            
            edit.ShowDialog();
            updateListBox();
        }
    }
}
