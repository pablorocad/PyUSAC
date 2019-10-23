using PyUSAC.Analisis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class Entorno
    {

        public Dictionary<String, Simbolo> tabla;

        Entorno anterior;

        public Entorno(Entorno ant)
        {
            anterior = ant;
            tabla = new Dictionary<string, Simbolo>();
        }

        public bool add(String name, Simbolo valor, int fila, int columna)
        {
            if (tabla.ContainsKey(name))
            {
                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico,
                    "La variable " + name + " ya esxiste", fila, columna));
                return false;
            }

            tabla.Add(name, valor);
            return true;
        }

        public Simbolo search(String name, int fila, int columna)
        {
            /*comienza en el mismo y mientras sea diferente de nulo va hacia atras 
            hasta que encuentre la variable mas cercana*/
            for (Entorno en = this; en != null; en = en.anterior)
            {
                if (en.tabla.ContainsKey(name))
                {
                    return en.tabla[name];
                }
            }
            Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico,
                "La variable " + name + " no existe", fila, columna));

            return null;
        }

    }
}
