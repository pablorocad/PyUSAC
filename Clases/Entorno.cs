using PyUSAC.Analisis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PyUSAC.Clases
{
    class Entorno
    {

        public Dictionary<String, Simbolo> tabla;
        public LinkedList<Entorno> import;
        Entorno anterior;

        public Entorno(Entorno ant)
        {
            anterior = ant;
            tabla = new Dictionary<string, Simbolo>();
            import = new LinkedList<Entorno>();
            tabla.Add("null", new Simbolo(Tipo.Simbolo.variable, new Expresion(Tipo.Valor.rnull, "")));
        }

        public bool addImport(Entorno ent, Boolean h)
        {
            if (h)
            {
                import.AddLast(ent);
            }
            else
            {
                import.AddFirst(ent);
            }
            return true;
        }

        public List<Simbolo> getListValues()
        {
            List<Simbolo> lista = tabla.Values.ToList();
            return lista;
        }

        public List<String> getListKeys()
        {
            List<String> lista = tabla.Keys.ToList();
            return lista;
        }

        public bool add(String name, Simbolo valor, int fila, int columna)
        {
            if (tabla.ContainsKey(name.ToLower()))
            {
                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico,
                    "La variable " + name + " ya esxiste", fila, columna));

                //MessageBox.Show("La variable " + name + " ya esxiste");
                return false;
            }

            tabla.Add(name.ToLower(), valor);
            return true;
        }

        public void edit(String name, Simbolo sm)
        {
            Boolean aux = true;
            for (Entorno en = this; en != null; en = en.anterior)
            {
                if (en.tabla.ContainsKey(name.ToLower()))
                {
                    en.tabla[name.ToLower()] = sm;
                    aux = false;
                    return;
                }
            }

            if (aux)
            {
                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico,
                "La variable " + name + " no existe", 0, 0));
                //MessageBox.Show("La variable " + name + " no esxiste");
            }
        }

        public Simbolo search(String name, int fila, int columna, Boolean h)
        {
            /*comienza en el mismo y mientras sea diferente de nulo va hacia atras 
            hasta que encuentre la variable mas cercana*/
            for (Entorno en = this; en != null; en = en.anterior)
            {
                if (en.tabla.ContainsKey(name.ToLower()))
                {
                    return en.tabla[name.ToLower()];
                }
            }

            if (h)
            {
                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico,
                "La variable " + name + " no existe", fila, columna));
                //MessageBox.Show("La variable " + name + " no esxiste");
            }
            return null;
        }

    }
}
