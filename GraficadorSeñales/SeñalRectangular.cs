using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class SeñalRectangular : Señal
    {
        
        public SeñalRectangular()
        {
            
            Muestras = new List<Muestra>();
            AmplitudMaxima = 0.0;
            TiempoFinal = 0.0;
            TiempoInicial = 0.0;

        }

        public SeñalRectangular(double TiempoFinal, double TiempoInicial, double umbral)
        {
            
            Muestras = new List<Muestra>();
            AmplitudMaxima = 0.0;
            TiempoFinal = 0.0;
            TiempoInicial = 0.0;

        }

        override public double evaluar(double tiempo)
        {
            double resultado;
            if (Math.Abs(tiempo) > 0.5)
            {
                resultado = 0;
            }

            else if (Math.Abs(tiempo) == 0.5)
            {
                resultado = 0.5;
            }

            else
            {
                resultado = 1;
            }

            return resultado;
        }

    }
}

