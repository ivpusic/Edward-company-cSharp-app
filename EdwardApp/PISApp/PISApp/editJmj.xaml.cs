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
    /// Interaction logic for editJmj.xaml
    /// </summary>
    public partial class editJmj : Window
    {
        public editJmj()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from c in cont.jedinica_mjeres
                            where c.id == Convert.ToInt32(IDjmj.Text)
                            select c;
                foreach (var ord in query)
                {
                    ord.id = Convert.ToInt32(IDjmj.Text);
                    ord.naziv = newName.Text;
                    // Insert any additional changes to column values.
                }
                cont.SubmitChanges();
            }
            this.Close();
        }
    }
}
