using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Host_de_servicio_Servicio_Wpf_
{
    /// <summary>
    /// Lógica de interacción para Win.xaml
    /// </summary>
    public partial class Win : Window
    {
        public Win()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PlaySound();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void PlaySound()
        {
            MediaPlayer MyPlayer = new MediaPlayer();
            Uri path = new Uri(@"C:\Users\Nacho\Documents\GitHub\Clave-de-registro\Host de servicio Servicio(Wpf)\Niños.mp3");
            MyPlayer.Open(path);
            MyPlayer.Play();
        }
    }
}
