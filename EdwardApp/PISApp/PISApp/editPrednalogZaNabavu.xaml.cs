﻿using System;
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
using System.Windows.Threading;

namespace PISApp
{
    /// <summary>
    /// Interaction logic for editPrednalogZaNabavu.xaml
    /// </summary>
    public partial class editPrednalogZaNabavu : Window
    {
        public editPrednalogZaNabavu()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                this.timeLabel.Content = DateTime.Now.ToString();
            }, this.Dispatcher);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    var query = from c in cont.prednalog_za_nabavus
                                where c.id == Convert.ToInt32(idBox.Text)
                                select c;
                    foreach (var ord in query)
                    {
                        ord.datum = Convert.ToDateTime(datumPicker.Text);
                        ord.vrijeme = vrijemeBox.Text;
                    }
                    cont.SubmitChanges();
                }
                stavkePrednalogaZaNabavuPage stavke = new stavkePrednalogaZaNabavuPage();
                stavke.idBoxx.Text = idBox.Text.ToString();
                iznos.prednalogZaNabavuID = Convert.ToInt32(idBox.Text);
                //MessageBox.Show(iznos.TempnalogZaNabavuID.ToString());
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
