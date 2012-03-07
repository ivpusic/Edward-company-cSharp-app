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
    /// Interaction logic for editRacunOdDobPageee.xaml
    /// </summary>
    public partial class editRacunOdDobPageee : Window
    {
        public editRacunOdDobPageee()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    var query = from c in cont.racun_od_dobavljacas
                                where c.br_racuna == Convert.ToInt32(idBox.Text)
                                select c;
                    foreach (var ord in query)
                    {
                        ord.datum = Convert.ToDateTime(datumBox.Text);
                        ord.adresa = adresaBox.Text;
                    }
                    cont.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se pogreška!!!" + ex.Message.ToString());
            }
            stavkeRacunaOdDobPage stavke = new stavkeRacunaOdDobPage();
            stavke.idBoxx.Text = idBox.Text.ToString();
            iznos.tempID = Convert.ToInt32(idBox.Text);
            stavke.ShowDialog();
            this.Close();
        }
    }
}
