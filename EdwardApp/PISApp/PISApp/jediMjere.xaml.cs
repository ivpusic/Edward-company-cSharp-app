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
    /// Interaction logic for jediMjere.xaml
    /// </summary>
    public partial class jediMjere : Window
    {
        public jediMjere()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                jedinica_mjere jedMj = new jedinica_mjere
                {
                    naziv = nameJedMjere.Text.ToString()
                };
                cont.jedinica_mjeres.InsertOnSubmit(jedMj);
                cont.SubmitChanges();
                MessageBox.Show("Spremljeno");
                updateListBox();
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            updateListBox();
        }

        void updateListBox()
        {
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var jedinice = from jed in cont.jedinica_mjeres
                               select jed;
                jediniceBox.ItemsSource = jedinice;
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext con = new pisModelDataContext())
                {

                    var jedinica = jediniceBox.SelectedItems;
                    //MessageBox.Show(jedinica.ToString());
                    foreach (jedinica_mjere j in jedinica)
                    {
                        var _jmj = (from jed in con.jedinica_mjeres
                                    where jed.id == j.id
                                    select jed).First();
                        con.jedinica_mjeres.DeleteOnSubmit(_jmj);
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
            editJmj edit = new editJmj();
            var jedinica = jediniceBox.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (jedinica_mjere j in jedinica)
            {
                edit.IDjmj.Text = j.id.ToString();
                edit.newName.Text = j.naziv;
            }
            edit.ShowDialog();
            updateListBox();
        }
    }
}
