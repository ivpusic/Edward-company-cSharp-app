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
    /// Interaction logic for organizacijskaJedinicaPage.xaml
    /// </summary>
    public partial class organizacijskaJedinicaPage : Window
    {
        public organizacijskaJedinicaPage()
        {
            InitializeComponent();
            updateListBox();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    organizacijska_jedinica art = new organizacijska_jedinica
                    {
                        naziv = nazivBox.Text
                    };
                    cont.organizacijska_jedinicas.InsertOnSubmit(art);
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
                var query = from s in cont.organizacijska_jedinicas
                            select s;
                orgJedBox.ItemsSource = query;
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext con = new pisModelDataContext())
                {
                    var jedinica = orgJedBox.SelectedItems;
                    //MessageBox.Show(jedinica.ToString());
                    foreach (organizacijska_jedinica j in jedinica)
                    {
                        var _jmj = (from jed in con.organizacijska_jedinicas
                                    where jed.id == j.id
                                    select jed).First();
                        con.organizacijska_jedinicas.DeleteOnSubmit(_jmj);
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
            editOrgJedinica edit = new editOrgJedinica();
            var jedinica = orgJedBox.SelectedItems;
            //MessageBox.Show(jedinica.ToString());
            foreach (organizacijska_jedinica j in jedinica)
            {
                edit.idBox.Text = j.id.ToString();
                edit.nazivBox.Text = j.naziv.ToString();
            }
            edit.ShowDialog();
            updateListBox();
        }
    }
}
