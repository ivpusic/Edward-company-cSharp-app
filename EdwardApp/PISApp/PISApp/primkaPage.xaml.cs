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
using System.Threading;
using System.Windows.Threading;

namespace PISApp
{
    /// <summary>
    /// Interaction logic for primkaPage.xaml
    /// </summary>
    /// 

    public partial class primkaPage : Window
    {
        public primkaPage()
        {
            InitializeComponent();
            updateListBox();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                {
                    this.timeLabel.Content = DateTime.Now.ToString();
                }, this.Dispatcher);
        }

        public void updateListBox()
        {
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from s in cont.primkas
                            select s;
                primkeBox.ItemsSource = query;
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext con = new pisModelDataContext())
                {

                    var item = primkeBox.SelectedItems;
                    //MessageBox.Show(jedinica.ToString());

                    foreach (primka j in item)
                    {
                        var _item = from _iteem in con.stavke_primkes
                                    where _iteem.id_primke == j.id
                                    select _iteem;
                        con.stavke_primkes.DeleteAllOnSubmit(_item);
                        con.SubmitChanges();
                    }

                    foreach (primka j in item)
                    {
                        var itm = (from it in con.primkas
                                   where it.id == j.id
                                   select it).First();

                        con.primkas.DeleteOnSubmit(itm);
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
            editPrimka edit = new editPrimka();
            var jedinica = primkeBox.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (primka j in jedinica)
            {
                edit.datumPicker.Text = j.datum.ToString();
                edit.vrijemeBox.Text = j.vrijeme;
                edit.idBox.Text = j.id.ToString();
            }
            edit.ShowDialog();
            updateListBox();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id;
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    primka art = new primka
                    {
                        datum = Convert.ToDateTime(datumPicker.Text),
                        vrijeme = vrijemeBox.Text
                    };
                    cont.primkas.InsertOnSubmit(art);
                    cont.SubmitChanges();
                    var entry = (from ee in cont.primkas
                                 select ee).OrderByDescending(ee => ee.id).FirstOrDefault();
                    id = entry.id;
                    iznos.tempPrimkaID = id;
                }
                updateListBox();

                //MessageBox.Show(id.ToString());
                stavkePrimkePage stavke = new stavkePrimkePage();
                stavke.idBoxx.Text = id.ToString();
                //MessageBox.Show(iznos.tempID.ToString());
                stavke.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se pogreška!!!\n" + ex.Message.ToString());
            }

        }
    }
}
