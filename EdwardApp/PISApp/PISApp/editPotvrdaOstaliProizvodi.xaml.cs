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
    /// Interaction logic for editPotvrdaOstaliProizvodi.xaml
    /// </summary>
    public partial class editPotvrdaOstaliProizvodi : Window
    {
        public editPotvrdaOstaliProizvodi()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    var query = from c in cont.potvrda_o_uskladistenom_materijalu_za_ostale_posloves
                                where c.id == Convert.ToInt32(idBox.Text)
                                select c;
                    foreach (var ord in query)
                    {
                        ord.datum = Convert.ToDateTime(datumPicker.Text);
                        ord.odgOsoba = logIn.userName;
                    }
                    cont.SubmitChanges();
                }

                stavkePotvrdeOstaliProizvodi stavke = new stavkePotvrdeOstaliProizvodi();
                stavke.idBoxx.Text = idBox.Text.ToString();
                iznos.tempPotvrdaOUskladistenomMaterijaluZaOstalePosloveID = Convert.ToInt32(idBox.Text);
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
