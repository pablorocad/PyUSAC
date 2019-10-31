using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class ArbolArreglo
    {

        NodoArreglo raiz;
        String name;

        public ArbolArreglo(String name)
        {
            this.name = name;
            raiz = new NodoArreglo(new Expresion(Tipo.Valor.cadena, name));
        }

        public void setValores(LinkedList<NodoArreglo> aux)
        {
            raiz.setListaHijos(aux);
        }

        public void crearNodo(int n)
        {
            crearNodo(n, raiz);
        }

        public void crearNodo(int n, NodoArreglo temp)
        {
            if (!temp.Count().Equals(0))//Si  ya tiene hijos, nos vamos a esos hiijos para setar nuevos nodos
            {
                foreach (NodoArreglo nodo in temp.getListaHijos())
                {
                    crearNodo(n,nodo);
                }
            }
            else
            {
                for (int v = n; v > 0; v--)
                {
                    temp.addHijo(new NodoArreglo(new Expresion(Tipo.Valor.rnull, null)));//si no tene hijos, aqui seran los nuevos nodos
                }
            }
        }

        public void setValor(Expresion valor, LinkedList<Expresion> posicion)
        {
            setValor(valor, posicion, raiz, 0);
        }

        public void setValor(Expresion valor, LinkedList<Expresion> posicion, NodoArreglo temp, int v)
        {
            if (temp.Count() == 0)
            {
                temp.setValor(valor);
            }
            else
            {
                int num = int.Parse(posicion.ElementAt(v).getValor().ToString());
                if (num < temp.Count())
                {
                    setValor(valor, posicion, temp.getHijo(num), v + 1);
                }
            }
        }

        public Expresion getValor(LinkedList<Expresion> posicion)
        {
            return getValor(posicion, raiz, 0);
        }

        public Expresion getValor(LinkedList<Expresion> posicion, NodoArreglo temp, int v)
        {
            if (!(temp.Count() == 0))
            {
                int num = int.Parse(posicion.ElementAt(v).getValor().ToString());
                if (num < temp.Count())
                {
                    return getValor(posicion, temp.getHijo(num), v + 1);
                }
                
            }
            return temp.getValor();
        }
    }
}
