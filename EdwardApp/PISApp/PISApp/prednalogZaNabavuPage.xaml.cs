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
    /// Interaction logic for prednalogZaNabavuPage.xaml
    /// </summary>
    public partial class prednalogZaNabavuPage : Window
    {
        public prednalogZaNabavuPage()
        {
            InitializeComponent();
            updateListBox();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                this.timeLabel.Content = DateTime.Now.ToString();
            }, this.Dispatcher);
        }

        void updateListBox()
        {
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from c in cont.prednalog_za_nabavus select c;
                nalogListBox.ItemsSource = query;
            }

        }
        

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id;
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    prednalog_za_nabavu art = new prednalog_za_nabavu
                    {
                        datum = Convert.ToDateTime(datumPicker.Text),
                        vrijeme = vrijemeBox.Text
                    };
                    cont.prednalog_za_nabavus.InsertOnSubmit(art);
                    cont.SubmitChanges();
                    var entry = (from ee in cont.prednalog_za_nabavus
                                 select ee).OrderByDescending(ee => ee.id).FirstOrDefault();
                    id = entry.id;
                    iznos.prednalogZaNabavuID = id;
                }
                updateListBox();

                //MessageBox.Show(id.ToString());
                stavkePrednalogaZaNabavuPage stavke = new stavkePrednalogaZaNabavuPage();
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
            editPrednalogZaNabavu edit = new editPrednalogZaNabavu();
            var jedinica = nalogListBox.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (prednalog_za_nabavu j in jedinica)
            {
                edit.datumPicker.Text = j.datum.ToString();
                edit.vrijemeBox.Text = j.vrijeme;
                edit.idBox.Text = j.id.ToString();
                iznos.prednalogZaNabavuID = j.id;
            }
            //MessageBox.Show(iznos.tempNalogZaNabavuID.ToString());
            edit.ShowDialog();
            updateListBox();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext con = new pisModelDataContext())
                {

                    var item = nalogListBox.SelectedItems;
                    //MessageBox.Show(jedinica.ToString());

                    foreach (prednalog_za_nabavu j in item)
                    {
                        var _item = from _iteem in con.stavke_prednaloga_za_nabavus
                                    where _iteem.id_prednaloga == j.id
                                    select _iteem;
                        con.stavke_prednaloga_za_nabavus.DeleteAllOnSubmit(_item);
                        con.SubmitChanges();
                    }

                    foreach (prednalog_za_nabavu j in item)
                    {
                        var itm = (from it in con.prednalog_za_nabavus
                                   where it.id == j.id
                                   select it).First();

                        con.prednalog_za_nabavus.DeleteOnSubmit(itm);
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
