using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class Declaracion
    {
        Tipo.Simbolo tipo;
        String identificador;
        Expresion exp = null;
        ArbolArreglo arreglo = null;

        public Declaracion(string identificador, Expresion exp)
        {
            this.identificador = identificador;
            this.exp = exp;
        }

        public Declaracion(string identificador, ArbolArreglo arbol)
        {
            this.identificador = identificador;
            this.arreglo = arbol; 
        }

        public String getIdentificador()
        {
            return identificador;
        }

        public Expresion getExpresion()
        {
            return exp;
        }


        public ArbolArreglo getArbol()
        {
            return arreglo;
        }

        public void setExpresion(Expresion exp)
        {
            this.exp = exp;
        }
    }
}
