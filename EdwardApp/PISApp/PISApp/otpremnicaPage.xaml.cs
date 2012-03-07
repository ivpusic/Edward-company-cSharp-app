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
    /// Interaction logic for otpremnicaPage.xaml
    /// </summary>
    public partial class otpremnicaPage : Window
    {
        public otpremnicaPage()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                this.timeLabel.Content = DateTime.Now.ToString();
            }, this.Dispatcher);
            updateListBox();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id;
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    otpremnica art = new otpremnica
                    {
                        datum = Convert.ToDateTime(datumPicker.Text),
                        vrijeme = vrijemeBox.Text
                    };
                    cont.otpremnicas.InsertOnSubmit(art);
                    cont.SubmitChanges();
                    var entry = (from ee in cont.otpremnicas
                                 select ee).OrderByDescending(ee => ee.broj_narudzbe).FirstOrDefault();
                    id = entry.broj_narudzbe;
                    iznos.tempOtpremnicaID = id;
                }
                updateListBox();

                //MessageBox.Show(id.ToString());
                stavkeOtpremnicePage stavke = new stavkeOtpremnicePage();
                stavke.idBoxx.Text = id.ToString();
                //MessageBox.Show(iznos.tempID.ToString());
                stavke.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se pogreška!!!\n" + ex.Message.ToString());
            }
        }

        public void updateListBox()
        {
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from s in cont.otpremnicas
                            select s;
                otpremniceListBox.ItemsSource = query;
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext con = new pisModelDataContext())
                {

                    var item = otpremniceListBox.SelectedItems;
                    //MessageBox.Show(jedinica.ToString());

                    foreach (otpremnica j in item)
                    {
                        var _item = from _iteem in con.stavke_otpremnices
                                    where _iteem.id_otpremnice == j.broj_narudzbe
                                    select _iteem;
                        con.stavke_otpremnices.DeleteAllOnSubmit(_item);
                        con.SubmitChanges();
                    }

                    foreach (otpremnica j in item)
                    {
                        var itm = (from it in con.otpremnicas
                                   where it.broj_narudzbe == j.broj_narudzbe
                                   select it).First();

                        con.otpremnicas.DeleteOnSubmit(itm);
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
            editOtpremnica edit = new editOtpremnica();
            var jedinica = otpremniceListBox.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (otpremnica j in jedinica)
            {
                edit.idBox.Text = j.broj_narudzbe.ToString();
                edit.datumPicker.Text = j.datum.ToString();
                edit.vrijemeBox.Text = j.vrijeme;
            }
            edit.ShowDialog();
            updateListBox();
        }
    }
}
