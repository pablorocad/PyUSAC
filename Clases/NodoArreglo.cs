using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class NodoArreglo
    {
        Expresion valor;
        LinkedList<NodoArreglo> hijo;

        public NodoArreglo(Expresion valor)
        {
            this.valor = valor;
            hijo = new LinkedList<NodoArreglo>();
        }

        public Expresion getValor()
        {
            return valor;
        }

        public int Count()
        {
            return hijo.Count;
        }

        public void setValor(Expresion valor)
        {
            this.valor = valor;
        }

        public NodoArreglo getHijo(int n)
        {
            return hijo.ElementAt(n);
        }

        public LinkedList<NodoArreglo> getListaHijos()
        {
            return hijo;
        }

        public void setListaHijos(LinkedList<NodoArreglo> h)
        {
            hijo = h;
        }

        public void addHijo(NodoArreglo n)
        {
            hijo.AddLast(n);
        }
    }
}
