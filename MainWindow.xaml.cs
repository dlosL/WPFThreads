using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace santana.luz._4i.WPFThreads
{
    public partial class MainWindow : Window
    {
        //Definizione della costante "GIRI"
        const int GIRI1 = 5000;
        const int GIRI2 = 500;
        const int GIRI3 = 50;
        //Definizione della costante "TEMPO"
        const int TEMPO1 = 1;
        const int TEMPO2 = 10;
        const int TEMPO3 = 100;
        //Definizione della variabile "_counter"
        int _counter1 = 0;
        int _counter2 = 0;
        int _counter3 = 0;
        static readonly object _locker = new object(); //Definizione di una variabile di sola lettura "_locker" di tipo "object"

        CountdownEvent semaforo;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) //Quando viene cliccato, questo metodo viene eseguito
        {
            // Spegne momentaneamente il pulsante
            btnGo.IsEnabled = false;
            prbarCounter1.Maximum += (GIRI1 + GIRI2);

            Thread thread1 = new Thread(incrementa1);
            thread1.Start();

            Thread thread2 = new Thread(incrementa2);
            thread2.Start();

            Thread thread3 = new Thread(incrementa3);//Avvia il thread che eseguirà il metodo "incrementa3"
            thread3.Start();

            semaforo = new CountdownEvent(3); //Definisce una nuova istanza di "CountdownEvent" con un parametro di valore 3

            Thread thread4 = new Thread(() =>
            {
                semaforo.Wait();
                Dispatcher.Invoke(() =>
                {
                    lblCounter1.Text = _counter1.ToString();
                    lblCounter2.Text = _counter2.ToString();
                    lblCounter3.Text = _counter3.ToString();

                    prbarCounter1.Value = _counter1 + _counter2 + _counter3;
                    btnGo.IsEnabled = true; 
                }
                );
            }
            );

            thread4.Start();

        }
        //Il metodo viene utilizzato per l'incremento di una variabile contatore.
        private void incrementa1()
        {
            for (int x = 0; x < GIRI1; x++)
            {
                Interlocked.Increment(ref _counter1);

                Dispatcher.Invoke(() =>
                {
                    lblCounter1.Text = _counter1.ToString();
                    prbarCounter1.Value = _counter1 + _counter2 + _counter3;
                    lblCounterTOT.Text = (_counter1 + _counter2 + _counter3).ToString();
                });

                Thread.SpinWait(TEMPO1);
            }
            semaforo.Signal();
        }

        private void incrementa2()
        {
            for (int x = 0; x < GIRI2; x++)
            {
                Interlocked.Increment(ref _counter2);

                Dispatcher.Invoke(() =>
                {
                    lblCounter2.Text = _counter2.ToString();
                    prbarCounter1.Value = _counter1 + _counter2 + _counter3;
                    lblCounterTOT.Text = (_counter1 + _counter2 + _counter3).ToString();
                });

                Thread.SpinWait(TEMPO2);

            }
            semaforo.Signal();
        }
        private void incrementa3()
        {
            for (int x = 0; x < GIRI3; x++)
            {
                Interlocked.Increment(ref _counter3);

                Dispatcher.Invoke(() =>
                {
                    lblCounter3.Text = _counter3.ToString();
                    prbarCounter1.Value = _counter1 + _counter2 + _counter3;
                    lblCounterTOT.Text = (_counter1 + _counter2 + _counter3).ToString();
                });

                Thread.SpinWait(TEMPO3);

            }
            semaforo.Signal();
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            _counter1 = 0;
            _counter2 = 0;
            _counter3 = 0;
            lblCounter1.Text = _counter1.ToString();
            lblCounter2.Text = _counter2.ToString();
            lblCounter3.Text = _counter3.ToString();

            prbarCounter1.Value = 0;
            prbarCounter1.Maximum = GIRI1 + GIRI2 + GIRI3;

        }


    }
}