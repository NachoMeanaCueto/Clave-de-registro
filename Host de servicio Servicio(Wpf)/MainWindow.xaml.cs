using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;

namespace Host_de_servicio_Servicio_Wpf_
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer Timer;
        TimeSpan Time, interval;
        bool altF4;

        ArrayList Question, Answer;
        Process myProcess;
        string respuestaCorrecta;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            inicializateRegistryKey();

            readQuestion();

            ReadAnswers();

            generateQuestion();

            inicializateProcess();

            inicializateTimer();

            tbxRespuesta.Focus();

        }

        
       

        //tick del timer
        private void Timer_Tick(object sender, object e)
        {
            labTiempo.Content = (TimeSpan)labTiempo.Content - interval;

            if ((TimeSpan)labTiempo.Content == TimeSpan.Zero)
            {
                Timer.Stop();
                myProcess.Start();
            }
        }


        private void BtnResponder_Click(object sender, RoutedEventArgs e)
        {
            string respuesta;

            if(Comprobaciacion(tbxRespuesta, "No puedes dejar la respesta en blanco"))
            {
                respuesta = QuitAccents(tbxRespuesta.Text.ToLower());

                if (respuesta == "configurate question")
                {
                    Timer.Stop();
                    Configuracion miventana = new Configuracion();
                    miventana.ShowDialog();

                    readQuestion();
                    inicializateTimer();
                    tbxRespuesta.Focus();
                }
                else
                {
                    if(respuesta == respuestaCorrecta)
                    {
                        Timer.Stop();
                        Win window = new Win();                       
                        window.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("¡Ohh! Has fallado. Vuelve a intentarlo", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
           
                
        }


        /*---------------------------------------------------------------------------------
         * 
         * Control para evitar que se cierre la aplicación pulsando alt + f4
         * 
         ----------------------------------------------------------------------------------*/

         //Evita el cierre
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (altF4)
            {
                e.Cancel = true;
                altF4 = false;
            }
        }

        //gestiona el campo altF4
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.SystemKey == Key.F4 && (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)))
            {
                altF4 = true;
            }
        }


        /*---------------------------------------------------------------------------------
         * 
         * Otros métodos
         * 
         ----------------------------------------------------------------------------------*/

        //Clave para autoejecutarse
        private void inicializateRegistryKey()
        {
            RegistryKey key = Registry.CurrentUser;

            key = key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            key.SetValue("Host de servicio(WPF)", System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        //Inicializa el proceso de apagado 
        private void inicializateProcess()
        {
            myProcess = new Process();
            myProcess.StartInfo = new ProcessStartInfo("shutdown.exe");
            myProcess.StartInfo.Arguments = "/s /t 0";
        }

        //lee las preguntas de un fichero externo
        private void readQuestion()
        {
            Question = new ArrayList();
            string path = @"..\archive.dat";
            
            FileStream fsQst = null;
            BinaryReader BrQst = null;
            
            try
            {
                //Abre los flujos
                fsQst = new FileStream(path, FileMode.Open, FileAccess.Read);
                BrQst = new BinaryReader(fsQst);
              

                while (true)
                {
                    Question.Add(BrQst.ReadString());
                }


            }
            catch (EndOfStreamException)
            {
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (BrQst != null) BrQst.Close();
                if (fsQst != null) fsQst.Close();
            }

        }

        //lee las respuestas de un fichero externo
        private void ReadAnswers()
        {
            Answer = new ArrayList();
            string path = @"..\datas.dat";

            FileStream fsAns = null;
            BinaryReader BrAns = null;

            try
            {
                fsAns = new FileStream(path, FileMode.Open, FileAccess.Read);
                BrAns = new BinaryReader(fsAns);

                while (true)
                {
                    Answer.Add(BrAns.ReadString());
                }
            }
            catch (EndOfStreamException)
            {
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (BrAns != null) BrAns.Close();
                if (fsAns != null) fsAns.Close();
            }
     
        }

        //Inicializa el timer
        private void inicializateTimer()
        {
            interval = new TimeSpan(0,0,1);
            Time = new TimeSpan(0, 1, 30);

            Timer = new DispatcherTimer();
            Timer.Interval = interval;
            Timer.Tick += Timer_Tick;

            labTiempo.Content = Time;

            Timer.Start();
        }

        //Elimina los acentos de una cadena
        public string QuitAccents(string inputString)
        {
            Regex a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            inputString = a.Replace(inputString, "a");
            inputString = e.Replace(inputString, "e");
            inputString = i.Replace(inputString, "i");
            inputString = o.Replace(inputString, "o");
            inputString = u.Replace(inputString, "u");
            return inputString;
        }

        //Comprueba que el textbox tenga contenido si lo tiene devuelve verdadero y sino falso y pone el foco
        //sobre el tbx
        private bool Comprobaciacion(TextBox tbx, string mensaje)
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
        
        //Saca una pregunta aleatoria entre las disponibles
        private void generateQuestion()
        {
            Random rand = new Random();

            int position = rand.Next(0, Question.Count);

            try
            {
                LabPregunta.Content = Question[position].ToString();
                respuestaCorrecta = QuitAccents(Answer[position].ToString().ToLower());
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

    }
}
