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
    /// Interaction logic for editNalogZaSkladistenjePage.xaml
    /// </summary>
    public partial class editNalogZaSkladistenjePage : Window
    {
        public editNalogZaSkladistenjePage()
        {
            InitializeComponent();
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

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string temp = orgJedBox.SelectedItem.ToString();
                string[] words = temp.Split('(');
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    var query = from c in cont.nalog_za_skladistenjes
                                where c.broj_narudzbe == Convert.ToInt32(idBox.Text)
                                select c;
                    foreach (var ord in query)
                    {
                        ord.broj_narudzbe = Convert.ToInt32(idBox.Text);
                        ord.id_org_jed = Convert.ToInt32(words[0]);
                        ord.datum = Convert.ToDateTime(datumPicker.Text);
                        ord.Nalog_izdao_la = logIn.userName;
                        
                    }
                    cont.SubmitChanges();
                }

                stavkeNalogaZaSkladistenje stavke = new stavkeNalogaZaSkladistenje();
                stavke.idBoxx.Text = idBox.Text.ToString();
                iznos.nalogZaSkladistenjeID = Convert.ToInt32(idBox.Text);
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
