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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PISApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //glavniMeni.IsEnabled = false;
            userName.Text = "Korisničko ime";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void jedMjere(object sender, RoutedEventArgs e)
        {
            jediMjere jedMj = new jediMjere();
            jedMj.ShowDialog();
        }

        private void artikli(object sender, RoutedEventArgs e)
        {
            artikls art = new artikls();
            art.ShowDialog();
        }

        private void primka(object sender, RoutedEventArgs e)
        {
            primkaPage primka = new primkaPage();
            primka.Show();
        }

        private void izdatnicaA(object sender, RoutedEventArgs e)
        {
            izdatnicePage izd = new izdatnicePage();
            izd.ShowDialog();
        }

        private void nalogZaNabavu(object sender, RoutedEventArgs e)
        {
            nalogZaNabavuPage nalog = new nalogZaNabavuPage();
            nalog.ShowDialog();
        }

        private void otpremnicaEvent(object sender, RoutedEventArgs e)
        {
            otpremnicaPage otpremnica = new otpremnicaPage();
            otpremnica.ShowDialog();
        }

        private void orgJedEvent(object sender, RoutedEventArgs e)
        {
            organizacijskaJedinicaPage orgJed = new organizacijskaJedinicaPage();
            orgJed.ShowDialog();
        }

        private void narudzbenicaEventt(object sender, RoutedEventArgs e)
        {
            NarudzbenicaPage nar = new NarudzbenicaPage();
            nar.ShowDialog();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (loginButton.Content == "Odjava")
            {
                glavniMeni.IsEnabled = false;
                loginButton.Content = "Prijava";
                userName.IsEnabled = true;
                userName.Text = "Korisničko ime";
                passwordBox.IsEnabled = true;
                
                return;
            }

            bool _logIn = false;

            using (pisModelDataContext cont = new pisModelDataContext())
            {
                var query = from s in cont.korisnicis
                            select s;

                foreach (var st in query)
                {
                    if (st.korisnicko_ime == userName.Text && st.lozinka == passwordBox.Password.ToString())
                    {
                        _logIn = true;
                        glavniMeni.IsEnabled = true;
                        loginButton.Content = "Odjava";
                        userName.IsEnabled = false;
                        passwordBox.IsEnabled = false;
                        logIn.userName = userName.Text;
                        logIn.password = passwordBox.Password.ToString();
                        logIn.ovlast = st.ovlast.ToString();
                        MessageBox.Show(logIn.userName + " dobrodošli!!!");
                    }
                }
            }
            if (_logIn == false) MessageBox.Show("Pogrešno ime ili lozinka!!!");
            
        }

        private void dodavanjeKorisnikaEvent(object sender, RoutedEventArgs e)
        {
            if (logIn.ovlast != "admin") MessageBox.Show("Nemate ovlasti za uređivanje liste korisnika!!!");
            else
            {
                addUsers kor = new addUsers();
                kor.ShowDialog();
            }
        }

        private void racunOdDobEvent(object sender, RoutedEventArgs e)
        {
            racunOdDobavljacaPage racun = new racunOdDobavljacaPage();
            racun.ShowDialog();
        }

        private void prednalogEvent(object sender, RoutedEventArgs e)
        {
            prednalogZaNabavuPage prednalog = new prednalogZaNabavuPage();
            prednalog.ShowDialog();
        }

        private void nalogZaSkladistenjeEvent(object sender, RoutedEventArgs e)
        {
            nalogZaSkladistenjePage nalog = new nalogZaSkladistenjePage();
            nalog.ShowDialog();
        }

        private void zahtjevnicaEvent(object sender, RoutedEventArgs e)
        {
            zahtjevnicaPage zahtjevnica = new zahtjevnicaPage();
            zahtjevnica.ShowDialog();
        }

        private void skladisnicaEvent(object sender, RoutedEventArgs e)
        {
            skladisnicaPage skladisnica = new skladisnicaPage();
            skladisnica.ShowDialog();
        }

        private void pozvrdaSitotisakEvent(object sender, RoutedEventArgs e)
        {
            potvrdaOuskladištenomMaterijaluZaSitotisakPage potvrda = new potvrdaOuskladištenomMaterijaluZaSitotisakPage();
            potvrda.ShowDialog();
        }

        private void pozvrdaOstaliPosloviEvent(object sender, RoutedEventArgs e)
        {
            potvrdaOUskladistenomMaterijaluZaOstaleProizvodaPage potvrda = new potvrdaOUskladistenomMaterijaluZaOstaleProizvodaPage();
            potvrda.ShowDialog();
        }

        private void reportEvent(object sender, RoutedEventArgs e)
        {
            Report report = new Report();
            report.ShowDialog();
        }
    }
}
