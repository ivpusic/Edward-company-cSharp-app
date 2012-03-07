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
    /// Interaction logic for editNarudzbenica.xaml
    /// </summary>
    public partial class editNarudzbenica : Window
    {
        public editNarudzbenica()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                this.timeLabel.Content = DateTime.Now.ToString();
            }, this.Dispatcher);
            updateBox();
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
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string temp = orgJedBox.SelectedItem.ToString();
                string[] words = temp.Split('(');
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    var query = from c in cont.narudzbenicas
                                where c.id == Convert.ToInt32(idBox.Text)
                                select c;
                    foreach (var ord in query)
                    {
                        ord.id = Convert.ToInt32(idBox.Text);
                        ord.id_org_jed = Convert.ToInt32(words[0]);
                        ord.datum = Convert.ToDateTime(datumPicker.Text);
                        ord.vrijeme = vrijemeBox.Text;
                    }
                    cont.SubmitChanges();
                }

                stavkeNarudzbenice stavke = new stavkeNarudzbenice();
                stavke.idBoxx.Text = idBox.Text.ToString();
                iznos.tempNarudzbenicaID = Convert.ToInt32(idBox.Text);
                stavke.ShowDialog();
                this.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se pogreška!!!\n" + ex.Message.ToString());
            }
            
        }
    }
}
