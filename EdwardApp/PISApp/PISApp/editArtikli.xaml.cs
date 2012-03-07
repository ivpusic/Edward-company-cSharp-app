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
    /// Interaction logic for editArtikli.xaml
    /// </summary>
    public partial class editArtikli : Window
    {
        public editArtikli()
        {
            InitializeComponent();
            updateBox();
        }


        void updateBox()
        {
            string i;
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from s in cont.jedinica_mjeres
                            select s;

                foreach (var st in query)
                {
                    i = st.id + "   (" + st.naziv + ")";
                    idJedCijBox.Items.Add(i);
                }
            }
        }


        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string temp = idJedCijBox.SelectedItem.ToString();
                string[] words = temp.Split('(');
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    var query = from c in cont.artiklis
                                where c.id == Convert.ToInt32(idBox.Text)
                                select c;
                    foreach (var ord in query)
                    {
                        ord.id = Convert.ToInt32(idBox.Text);
                        ord.naziv = nazivBox.Text;
                        ord.vrsta = vrstaBox.Text;
                        ord.id_jedinice_mjere = Convert.ToInt32(words[0]);
                        ord.jedinicna_cijena = Convert.ToInt32(jedCijenaBox.Text);
                    }
                    cont.SubmitChanges();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se pogreška!!!\n" + ex.Message.ToString());
            }
        }

    }
}
