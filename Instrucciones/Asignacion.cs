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

        ParseTreeNode name;
        ParseTreeNode l_dim;
        ParseTreeNode exp;

        public Asignacion(ParseTreeNode name, ParseTreeNode exp)
        {
            this.name = name;
            this.exp = exp;
        }

        public Asignacion(ParseTreeNode name, ParseTreeNode lista, ParseTreeNode exp)
        {
            this.name = name;
            this.l_dim = lista;
            this.exp = exp;
        }

        public Instruccion Ejecutar(Entorno ent)
        {
            Resolve resolve = new Resolve();

            Simbolo sim;

            if (name.ChildNodes.Count == 1)
            {
                String nombre = name.ChildNodes.ElementAt(0).ToString().Split(' ')[0];
                sim = ent.search(nombre, 0, 0, true);
                if (l_dim != null)
                {
                    //Expresion exp = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                    
                        Expresion expAs = resolve.resolverExpresion(exp, ent);
                        ArbolArreglo arbolAux = (ArbolArreglo)((Expresion)sim.getContenido()).getValor();

                        LinkedList<Expresion> dimensiones = resolve.L_DIM(l_dim, ent);

                        arbolAux.setValor(expAs, dimensiones);
                        Simbolo simAs = new Simbolo(Tipo.Simbolo.variable, new Expresion(Tipo.Valor.arreglo, arbolAux));

                        ent.edit(nombre, simAs);

                }
                else if (l_dim == null)
                {
                    Expresion expAs = resolve.resolverExpresion(exp, ent);
                    sim = new Simbolo(Tipo.Simbolo.variable, expAs);

                    ent.edit(nombre, sim);
                }

                //ent.edit(name.ChildNodes.ElementAt(0).ToString().Split(' ')[0], sim);
            }
            else if (name.ChildNodes.Count == 3)
            {
                String nombre = name.ChildNodes.ElementAt(2).ChildNodes.ElementAt(0).ToString().Split(' ')[0];
                Expresion expAs = resolve.resolverExpresion(exp, ent);
                sim = resolve.asignacion(name.ChildNodes.ElementAt(0), ent);
                Entorno entAux = (Entorno)((Expresion)sim.getContenido()).getValor();

                sim = new Simbolo(Tipo.Simbolo.variable, expAs);

                entAux.edit(nombre, sim);
            }

            //ent.edit(name, sim);
            //Console.WriteLine();
            return null;
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.asignacion;
        }
    }
}
