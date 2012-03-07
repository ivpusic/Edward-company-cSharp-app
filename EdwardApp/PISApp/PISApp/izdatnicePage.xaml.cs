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
    /// Interaction logic for izdatnicePage.xaml
    /// </summary>
    public partial class izdatnicePage : Window
    {
        public izdatnicePage()
        {
            InitializeComponent();
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
                int id;
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    izdatnica art = new izdatnica
                    {
                        datum = Convert.ToDateTime(datumPicker.Text),
                        vrijeme = vrijemeBox.Text
                    };
                    cont.izdatnicas.InsertOnSubmit(art);
                    cont.SubmitChanges();
                    var entry = (from ee in cont.izdatnicas
                                 select ee).OrderByDescending(ee => ee.id).FirstOrDefault();
                    id = entry.id;
                    iznos.tempIzdatnicaID = id;
                }
                updateListBox();

                //MessageBox.Show(id.ToString());
                stavkeIzdatnicePage stavke = new stavkeIzdatnicePage();
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
                var query = from s in cont.izdatnicas
                            select s;
                izdatniceBox.ItemsSource = query;
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext con = new pisModelDataContext())
                {

                    var item = izdatniceBox.SelectedItems;
                    //MessageBox.Show(jedinica.ToString());

                    foreach (izdatnica j in item)
                    {
                        var _item = from _iteem in con.stavke_izdatnices
                                    where _iteem.id_izdatnice == j.id
                                    select _iteem;
                        con.stavke_izdatnices.DeleteAllOnSubmit(_item);
                        con.SubmitChanges();
                    }

                    foreach (izdatnica j in item)
                    {
                        var itm = (from it in con.izdatnicas
                                   where it.id == j.id
                                   select it).First();

                        con.izdatnicas.DeleteOnSubmit(itm);
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
            editIzdatnica edit = new editIzdatnica();
            var jedinica = izdatniceBox.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (izdatnica j in jedinica)
            {
                edit.idBox.Text = j.id.ToString();
                edit.datumPicker.Text = j.datum.ToString();
                edit.vrijemeBox.Text = j.vrijeme;
            }
            edit.ShowDialog();
            updateListBox();
        }
    }
}
