using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class ElementoArreglo
    {

        Expresion valor;
        LinkedList<Expresion> posicion;

        public ElementoArreglo(Expresion valor, LinkedList<Expresion> posicion)
        {
            this.valor = valor;
            this.posicion = posicion;
        }

        public Expresion getValor()
        {
            return valor;
        }

        public LinkedList<Expresion> getPosicion()
        {
            return posicion;
        }
    }
}
