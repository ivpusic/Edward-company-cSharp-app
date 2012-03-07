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
    /// Interaction logic for artikls.xaml
    /// </summary>
    public partial class artikls : Window
    {
        public artikls()
        {
            InitializeComponent();
            updateBox();
            updateListBox();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            string temp = jedMjCombo.SelectedItem.ToString();
            string[] words = temp.Split('(');
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                artikli art = new artikli
                {
                    naziv = nazivBox.Text,
                    jedinicna_cijena = Convert.ToInt32(jedCijena.Text),
                    vrsta = vrstaBox.Text,
                    id_jedinice_mjere = Convert.ToInt32(words[0])
                };
                cont.artiklis.InsertOnSubmit(art);
                cont.SubmitChanges();
            }
            updateListBox();

        }

        void updateBox()
        {
            string i;
            using(pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from s in cont.jedinica_mjeres
                            select s;
                
                foreach(var st in query){
                    i = st.id + "   (" + st.naziv + ")";
                    jedMjCombo.Items.Add(i);
                    }
            }
        }

        void updateListBox()
        {
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from c in cont.artiklis select c;
                artikliListBox.ItemsSource = query;
            }
            
        }

        private void jedMjCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext con = new pisModelDataContext())
                {

                    var item = artikliListBox.SelectedItems;
                    //MessageBox.Show(jedinica.ToString());
                    foreach (artikli j in item)
                    {
                        var _itm = (from it in con.artiklis
                                    where it.id == j.id
                                    select it).First();
                        con.artiklis.DeleteOnSubmit(_itm);
                        con.SubmitChanges();
                    }
                    updateListBox();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se pogreška!!!\n" + ex.Message.ToString());
            }
            
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            editArtikli edit = new editArtikli();
            var jedinica = artikliListBox.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (artikli j in jedinica)
            {
                edit.idBox.Text = j.id.ToString();
                edit.nazivBox.Text = j.naziv;
                edit.vrstaBox.Text = j.vrsta;
                edit.jedCijenaBox.Text = j.jedinicna_cijena.ToString();
                //edit.idJedCijBox.Items.Add(j.id_jedinice_mjere.ToString());
                //edit.idJedCijBox.SelectedItem = 0;
            }
            edit.ShowDialog();
            updateListBox();
        }
    }
}
