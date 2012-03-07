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
    /// Interaction logic for editSkladisnica.xaml
    /// </summary>
    public partial class editSkladisnica : Window
    {
        public editSkladisnica()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    var query = from c in cont.skladisnicas
                                where c.id == Convert.ToInt32(idBox.Text)
                                select c;
                    foreach (var ord in query)
                    {
                        ord.datum = Convert.ToDateTime(datumPicker.Text);
                        ord.odgOsoba = logIn.userName;
                    }
                    cont.SubmitChanges();
                }

                stavkeSkladisnicePage stavke = new stavkeSkladisnicePage();
                stavke.idBoxx.Text = idBox.Text.ToString();
                iznos.tempSkladisnicaID = Convert.ToInt32(idBox.Text);
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
