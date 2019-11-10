using Irony.Parsing;
using PyUSAC.Analisis;
using PyUSAC.Clases;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Instrucciones
{
    class Asignacion : Instruccion
    {

        String name;
        ParseTreeNode l_dim;
        ParseTreeNode exp;

        public Asignacion(string name, ParseTreeNode exp)
        {
            this.name = name;
            this.exp = exp;
        }

        public Asignacion(string name, ParseTreeNode lista, ParseTreeNode exp)
        {
            this.name = name;
            this.l_dim = lista;
            this.exp = exp;
        }

        public void Ejecutar(Entorno ent)
        {
            Resolve resolve = new Resolve();
            
            if (l_dim != null)
            {
                Simbolo aux = ent.search(name, 0, 0);

                if (aux != null)
                {
                    Expresion expAs = resolve.resolverExpresion(exp, ent);
                    ArbolArreglo arbolAux = ((ArbolArreglo)aux.getContenido());

                    LinkedList<Expresion> dimensiones = resolve.L_DIM(l_dim, ent);

                    arbolAux.setValor(expAs, dimensiones);
                    Simbolo simAs = new Simbolo(Tipo.Simbolo.arreglo, arbolAux);

                    ent.edit(name, simAs);
                }
            }
            else
            {
                Expresion expAs = resolve.resolverExpresion(exp, ent);
                Simbolo simAs = new Simbolo(Tipo.Simbolo.variable, expAs);

                ent.edit(name, simAs);
            }
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.asignacion;
        }
    }
}
