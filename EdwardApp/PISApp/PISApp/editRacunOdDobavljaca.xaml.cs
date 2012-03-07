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

namespace PISApp
{
    /// <summary>
    /// Interaction logic for editRacunOdDobavljaca.xaml
    /// </summary>
    public partial class editRacunOdDobavljaca : Window
    {
        public editRacunOdDobavljaca()
        {
            InitializeComponent();
            updateBox();
        }

        private void updateBox()
        {
            string i;
            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from s in cont.artiklis
                            select s;

                foreach (var st in query)
                {
                    i = st.id + "   (" + st.naziv + ")";
                    sifraCombo.Items.Add(i);
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string temp = sifraCombo.SelectedItem.ToString();
                string[] words = temp.Split('(');
                using (pisModelDataContext cont = new pisModelDataContext())
                {
                    var query = from c in cont.stavke_racuna_od_dobavljacas
                                where c.jel == Convert.ToInt32(idBox.Text)
                                select c;
                    foreach (var ord in query)
                    {
                        ord.kolicina = Convert.ToInt32(kolicinaBox.Text);
                        ord.id_artikla = Convert.ToInt32(words[0]);
                    }
                    cont.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dogodila se pogreška!!!" + ex.Message.ToString());
            }
            
            this.Close();
        }
    }
}