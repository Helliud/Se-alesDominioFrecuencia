using System;
using System.Windows.Forms;
using System.Windows;

namespace GraficadorSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double amplitudMaxima = 1;

        Señal señal;
        Señal segundaSeñal;
        Señal señalResultado;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGraficar_Click(object sender, RoutedEventArgs e)
        {
           
            double tiempoInicial = double.Parse(txtTiempoInicial.Text);
            double tiempoFinal = double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo = double.Parse(txtFrecuenciaMuestreo.Text);


            double umbral = double.Parse(txtUmbral.Text);
            double umbral_segundaSeñal = double.Parse(txtUmbral_SegundaSeñal.Text);

            //PRIMERA SEÑAL
            switch (cbTipoSeñal.SelectedIndex)
            {
                //Señal Senoidal
                case 0:
                    double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)
                        panelConfiguracion.Children[0]).txtAmplitud.Text);

                    double fase = double.Parse(((ConfiguracionSeñalSenoidal)
                        panelConfiguracion.Children[0]).txtFase.Text);

                    double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)
                        panelConfiguracion.Children[0]).txtFrecuencia.Text);

                    señal = new SeñalSenoidal(amplitud, fase, frecuencia, umbral); //constructor

                    break;

                //Rampa
                case 1: señal = new SeñalRampa();

                    break;

                //Exponencial
                case 2:
                    double alpha = double.Parse(((ConfiguracionSeñalExponencial)
                        panelConfiguracion.Children[0]).txtAlpha.Text);

                    señal = new SeñalExponencial(alpha, umbral);
                    break;

                case 3:
                    señal = new SeñalRectangular();
                    break;

                default:

                    señal = null;

                    break;

            }

            //SEGUNDA SEÑAL
            switch (cbTipoSeñal_SegundaSeñal.SelectedIndex)
            {
                //Señal Senoidal
                case 0:
                    double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)
                        panelConfiguracion_SegundaSeñal.Children[0]).txtAmplitud.Text);

                    double fase = double.Parse(((ConfiguracionSeñalSenoidal)
                        panelConfiguracion_SegundaSeñal.Children[0]).txtFase.Text);

                    double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)
                        panelConfiguracion_SegundaSeñal.Children[0]).txtFrecuencia.Text);

                    segundaSeñal = new SeñalSenoidal(amplitud, fase, frecuencia, umbral); //constructor

                    break;

                //Rampa
                case 1:
                    segundaSeñal = new SeñalRampa();

                    break;

                //Exponencial
                case 2:
                    double alpha = double.Parse(((ConfiguracionSeñalExponencial)
                        panelConfiguracion_SegundaSeñal.Children[0]).txtAlpha.Text);

                    segundaSeñal = new SeñalExponencial(alpha, umbral);
                    break;

                case 3:
                    segundaSeñal = new SeñalRectangular();
                    break;

                default:

                    segundaSeñal = null;

                    break;

            }

            
            //---------------------------------PRIMERA SEÑAL------------------------------------------------------//
            señal.TiempoInicial = tiempoInicial;
            señal.TiempoFinal = tiempoFinal;
            señal.FrecuenciaMuestreo = frecuenciaMuestreo;
            señal.construirSeñalDigital();

            //Escalar
            double factorEscala = double.Parse(txtFactorEscalaAmplitud.Text);
            señal.escalar(factorEscala);
            
            //Desplazamiento 
            double desplazar = double.Parse(txtDesplazamientoY.Text);
            señal.desplazarY(desplazar);

            //Truncar
            //señal.truncar(umbral);



            //------------------------------------SEGUNDA SEÑAL--------------------------------------------------//
            segundaSeñal.TiempoInicial = tiempoInicial;
            segundaSeñal.TiempoFinal = tiempoFinal;
            segundaSeñal.FrecuenciaMuestreo = frecuenciaMuestreo;
            segundaSeñal.construirSeñalDigital();

            //Escalar
            double factorEscala_segundaSeñal = double.Parse(txtFactorEscalaAmplitud_SegundaSeñal.Text);
            segundaSeñal.escalar(factorEscala_segundaSeñal);

            //Desplazamiento 
            double desplazar_segundaSeñal = double.Parse(txtDesplazamientoY_SegundaSeñal.Text);
            segundaSeñal.desplazarY(desplazar_segundaSeñal);

            //Truncar
            //segundaSeñal.truncar(umbral_segundaSeñal);
           


            señal.actualizarAmplitudMaxima();
            segundaSeñal.actualizarAmplitudMaxima();

            amplitudMaxima = señal.AmplitudMaxima;
            if(segundaSeñal.AmplitudMaxima > amplitudMaxima)
            {
                amplitudMaxima = segundaSeñal.AmplitudMaxima;

            }

            plnGrafica.Points.Clear();
            plnGrafica2.Points.Clear();

            lblAmplitudMaximaY.Text = amplitudMaxima.ToString("F");
            lblAmplitudMaximaNegativaY.Text = "-" + amplitudMaxima.ToString("F");

            //PRIMERA SEÑAL
            if (señal != null)
            {
                //Recorre todos los elementos de una coleccion o arreglo
                foreach (Muestra muestra in señal.Muestras)
                {
                    plnGrafica.Points.Add(new Point((muestra.X - tiempoInicial) * scrContenedor.Width, (muestra.Y /
                        amplitudMaxima * ((scrContenedor.Height / 2.0) - 30) * -1) + 
                        (scrContenedor.Height / 2)));

                }
                
            }

            //SEGUNDA SEÑAL
            if (segundaSeñal != null)
            {
                //Recorre todos los elementos de una coleccion o arreglo
                foreach (Muestra muestra in segundaSeñal.Muestras)
                {
                    plnGrafica2.Points.Add(new Point((muestra.X - tiempoInicial) * scrContenedor.Width, (muestra.Y /
                        amplitudMaxima * ((scrContenedor.Height / 2.0) - 30) * -1) +
                        (scrContenedor.Height / 2)));

                }

            }

            plnEjeX.Points.Clear();
            //Punto del principio
            plnEjeX.Points.Add(new Point(0, (scrContenedor.Height / 2)));
            //Punto del final
            plnEjeX.Points.Add(new Point((tiempoFinal - tiempoInicial) * scrContenedor.Width, 
                (scrContenedor.Height / 2)));

            plnEjeY.Points.Clear();
            //Punto del principio
            plnEjeY.Points.Add(new Point((0 - tiempoInicial) * scrContenedor.Width , (señal.AmplitudMaxima * 
                ((scrContenedor.Height / 2.0) - 30) * -1) + (scrContenedor.Height / 2)));
            //Punto del final
            plnEjeY.Points.Add(new Point((0 - tiempoInicial) * scrContenedor.Width, (-señal.AmplitudMaxima * 
                ((scrContenedor.Height / 2.0) - 30) * -1) + (scrContenedor.Height / 2)));
                        
        }

        private void btnGraficarRampa_Click(object sender, RoutedEventArgs e)
        {
            double tiempoInicial = double.Parse(txtTiempoInicial.Text);
            double tiempoFinal = double.Parse(txtTiempoFinal.Text);
            double umbral = double.Parse(txtUmbral.Text);

            SeñalRampa rampa = new SeñalRampa(tiempoInicial, tiempoFinal, umbral);

            plnGrafica.Points.Clear();

            for (double i = tiempoInicial; i <= tiempoFinal; i++)
            {
                double valorMuestra = rampa.evaluar(i);
                rampa.Muestras.Add(new Muestra(i, valorMuestra));
                

            }

            foreach (Muestra muestra in rampa.Muestras)
            {
                plnGrafica.Points.Add(new Point(muestra.X * scrContenedor.Width, (muestra.Y * 
                    ((scrContenedor.Height / 2.0) - 30) * -1) + (scrContenedor.Height / 2)));

            }
           
        }

        private void cbTipoSeñal_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (panelConfiguracion != null)
            {
                panelConfiguracion.Children.Clear();

                switch (cbTipoSeñal.SelectedIndex)
                {
                    case 0:  //Senoidal
                        panelConfiguracion.Children.Add(new ConfiguracionSeñalSenoidal());
                        break;

                    case 1: //Rampa

                        break;

                    case 2://Exponencial
                        panelConfiguracion.Children.Add(new ConfiguracionSeñalExponencial());
                        break;

                    case 3: //Rectangular
                        break;
                    default:
                        break;

                }

            }
           
        }

        private void cbTipoSeñal_SegundaSeñal_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            panelConfiguracion_SegundaSeñal.Children.Clear();
            switch (cbTipoSeñal_SegundaSeñal.SelectedIndex)
            {
                case 0: //Senoidal
                    panelConfiguracion_SegundaSeñal.Children.Add(new ConfiguracionSeñalSenoidal());
                    break;

                case 1://Rampa
                    
                    break;

                case 2: //Exponencial
                    panelConfiguracion_SegundaSeñal.Children.Add(new ConfiguracionSeñalExponencial());
                    break;
                    
                default:

                    break;

            }
        }

        //CHECKBOX'S
        private void cbEscalaAmplitud_Checked(object sender, RoutedEventArgs e)
        {
            if (cbEscalaAmplitud.IsChecked == true)
            {
                txtFactorEscalaAmplitud.IsEnabled = true;
            }
            else
            {
                txtFactorEscalaAmplitud.IsEnabled = false;
            }
        }

        private void cbDesplazamientoY_Checked(object sender, RoutedEventArgs e)
        {
            if (cbDesplazamientoY.IsChecked == true)
            {
                txtDesplazamientoY.IsEnabled = true;
            }
            else
            {
                txtDesplazamientoY.IsEnabled = false;
            }
        }
        
        private void cbUmbral_Checked(object sender, RoutedEventArgs e)
        {
            if (cbUmbral.IsChecked == true)
            {
                txtUmbral.IsEnabled = true;

            } 
            else
            {
                txtUmbral.IsEnabled = false;
            }
        }

        private void btnRealizarOperacion_Click(object sender, RoutedEventArgs e)
        {
            señalResultado = null;
            switch (cbTipoOperacion.SelectedIndex)
            {
                case 0: //Suma
                    señalResultado = Señal.sumar(señal, segundaSeñal);
                    break;

                case 1: //Multiplicacion
                    señalResultado = Señal.multiplicar(señal, segundaSeñal);
                    break;

                case 2: //Convolcion
                    señalResultado = Señal.convolucionar(señal, segundaSeñal);
                    break;

                default:
                    break;
            }

            señalResultado.actualizarAmplitudMaxima();

            plnGraficaResultado.Points.Clear();

            lblAmplitudMaximaY_Resultado.Text = señalResultado.AmplitudMaxima.ToString("F");
            lblAmplitudMaximaNegativaY_Resultado.Text = "-" + señalResultado.AmplitudMaxima.ToString("F");

            //PRIMERA SEÑAL
            if (señalResultado != null)
            {
                //Recorre todos los elementos de una coleccion o arreglo
                foreach (Muestra muestra in señalResultado.Muestras)
                {
                    plnGraficaResultado.Points.Add(new Point((muestra.X - señalResultado.TiempoInicial) * scrContenedor_Resultado.Width, (muestra.Y /
                        señalResultado.AmplitudMaxima * ((scrContenedor_Resultado.Height / 2.0) - 30) * -1) +
                        (scrContenedor.Height / 2)));

                }

            }


            plnEjeXResultado.Points.Clear();
            //Punto del principio
            plnEjeXResultado.Points.Add(new Point(0, (scrContenedor_Resultado.Height / 2)));
            //Punto del final
            plnEjeXResultado.Points.Add(new Point((señalResultado.TiempoFinal - señalResultado.TiempoInicial) * scrContenedor_Resultado.Width,
                (scrContenedor_Resultado.Height / 2)));

            plnEjeYResultado.Points.Clear();
            //Punto del principio
            plnEjeYResultado.Points.Add(new Point((0 - señalResultado.TiempoInicial) * scrContenedor_Resultado.Width, (señal.AmplitudMaxima *
                ((scrContenedor_Resultado.Height / 2.0) - 30) * -1) + (scrContenedor_Resultado.Height / 2)));
            //Punto del final-
            plnEjeYResultado.Points.Add(new Point((0 - señalResultado.TiempoInicial) * scrContenedor_Resultado.Width, (-señal.AmplitudMaxima *
                ((scrContenedor_Resultado.Height / 2.0) - 30) * -1) + (scrContenedor_Resultado.Height / 2)));

        
        }
    }

}
