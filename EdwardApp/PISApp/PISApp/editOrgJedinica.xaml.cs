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
    /// Interaction logic for editOrgJedinica.xaml
    /// </summary>
    public partial class editOrgJedinica : Window
    {
        public editOrgJedinica()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    var query = from c in cont.organizacijska_jedinicas
                                where c.id == Convert.ToInt32(idBox.Text)
                                select c;
                    foreach (var ord in query)
                    {
                        ord.id = Convert.ToInt32(idBox.Text);
                        ord.naziv = nazivBox.Text;
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
