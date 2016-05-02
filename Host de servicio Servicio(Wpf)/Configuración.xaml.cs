using System;
using System.Collections;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace Host_de_servicio_Servicio_Wpf_
{
    /// <summary>
    /// Lógica de interacción para Configuracion.xaml
    /// </summary>
    public partial class Configuracion : Window
    {
        ArrayList Questions;
        ArrayList Answers;

        public Configuracion()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
             Questions = new ArrayList();
             Answers = new ArrayList();
             labAyuda.Content = 
                "En esta ventana puede configurar las preguntas que se mostrarán al inicio."+
                "\nPuede introducir el número de preguntas que desee.";
        }

        /*------------------------------------------------------------------------------------
         * eventos de los botones.
         * ----------------------------------------------------------------------------------*/
        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if(Comprobaciacion(tbxPregunta,"No puede dejar la pregunta vacía")
                && Comprobaciacion(tbxRespuesta, "No puede dejar la respuesta vacía"))
            {
                Questions.Add(tbxPregunta.Text);
                Answers.Add(tbxRespuesta.Text);

                tbxPregunta.Clear();
                tbxRespuesta.Clear();

                tbxPregunta.Focus();
            }
        }


        //Guarda las preguntas y respuestas en sus ficheros correspondientes
        private void WriteData()
        {
            string path = @"..\archive.dat";
            string pathData = @"..\datas.dat";
            FileStream fsQst = null;
            BinaryWriter BWrQst = null;
            FileStream fsAns = null;
            BinaryWriter BWrAns = null;


            try
            {
               
                fsQst = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                BWrQst = new BinaryWriter(fsQst);

                fsAns = new FileStream(pathData, FileMode.OpenOrCreate, FileAccess.Write);
                BWrAns = new BinaryWriter(fsAns);

                foreach(string question in Questions)
                {
                    BWrQst.Write(question);
                }
                
                foreach(string answer in Answers)
                {
                    BWrAns.Write(answer);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (BWrQst != null) BWrQst.Close();
                if (fsQst != null) fsQst.Close();
                if (BWrAns != null) BWrAns.Close();
                if (fsAns != null) fsAns.Close();


                MessageBox.Show("¡Configuración correcta!", "Correcto", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
           if (MessageBox.Show("Si continua se borrarán los datos de los campos de texto",
               "Cancelar pregunta", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                tbxPregunta.Clear();
                tbxRespuesta.Clear();

                tbxPregunta.Focus();
            }
          
        }

        private void Btn_Salir_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Si continua se borrarán las preguntas no guardadas",
               "¿Desea salir sin guardar?", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        private void Btn_Guardar_Click(object sender, RoutedEventArgs e)
        {
            WriteData();
        }

        /*------------------------------------------------------------------------------------------------
         * Otros métodos
         -------------------------------------------------------------------------------------------------*/

        //Comprueba que el textbox tenga contenido si lo tiene devuelve verdadero y sino falso y pone el foco
        //sobre el tbx
        private bool Comprobaciacion(TextBox tbx , string mensaje)
        {
            bool correcto = true;

            if (String.IsNullOrWhiteSpace(tbx.Text))
            {
                correcto = false;

                MessageBox.Show(mensaje, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                tbx.Focus();
            }
            return correcto;
        }

     
    }
}
