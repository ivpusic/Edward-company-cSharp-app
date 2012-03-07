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
    /// Interaction logic for editZahtjevnica.xaml
    /// </summary>
    public partial class editZahtjevnica : Window
    {
        public editZahtjevnica()
        {
            InitializeComponent();
            updateBox();
        }

        void updateBox()
        {
            string i;
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from s in cont.izdatnicas
                            select s;

                foreach (var st in query)
                {
                    i = st.id + "   (" + st.datum + ")";
                    izdatnicaBox.Items.Add(i);
                }
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string temp = izdatnicaBox.SelectedItem.ToString();
                string[] words = temp.Split('(');
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    var query = from c in cont.zahtjevnicas
                                where c.id == Convert.ToInt32(idBox.Text)
                                select c;
                    foreach (var ord in query)
                    {
                        ord.id = Convert.ToInt32(idBox.Text);
                        ord.datum = Convert.ToDateTime(datumPicker.Text);
                        ord.id_izdatnice = Convert.ToInt32(words[0]);
                        ord.odgOsoba = logIn.userName;
                        
                    }
                    cont.SubmitChanges();
                }

                stavkeZahtjevnicePage stavke = new stavkeZahtjevnicePage();
                stavke.idBoxx.Text = idBox.Text.ToString();
                iznos.tempPrimkaID = Convert.ToInt32(idBox.Text);
                stavke.ShowDialog();
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se pogreška!!!" + ex.Message.ToString());
            }
        }
    }
}
