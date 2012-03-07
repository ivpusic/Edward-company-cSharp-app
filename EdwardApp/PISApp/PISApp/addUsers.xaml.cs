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
    /// Interaction logic for addUsers.xaml
    /// </summary>
    public partial class addUsers : Window
    {
        
        public addUsers()
        {
            InitializeComponent();
            updateListBox();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            string r;
            if (roleBox.Text == "Administrator") r = "admin";
            else r = "user";
            try
            {
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    korisnici art = new korisnici
                    {
                        korisnicko_ime = userName.Text,
                        lozinka = Password.Text,
                        ovlast = r
                    };
                    cont.korisnicis.InsertOnSubmit(art);
                    cont.SubmitChanges();
                }
                updateListBox();
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
                var query = from c in cont.korisnicis select c;
                korisniciListBox.ItemsSource = query;
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (pisModelDataContext con = new pisModelDataContext())
            {

                var item = korisniciListBox.SelectedItems;
                //MessageBox.Show(jedinica.ToString());
                foreach (korisnici j in item)
                {
                    var _itm = (from it in con.korisnicis
                                where it.id == j.id
                                select it).First();
                    con.korisnicis.DeleteOnSubmit(_itm);
                    con.SubmitChanges();
                }
                updateListBox();
            }
        }
    }
}
