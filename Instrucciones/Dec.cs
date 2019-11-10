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
    class Dec : Instruccion
    {

        LinkedList<Declaracion> ID;

        public Dec(LinkedList<Declaracion> iD)
        {
            ID = iD;
        }

        public void Ejecutar(Entorno ent)
        {
            Resolve resolve = new Resolve();
            foreach (Declaracion dec in ID)
            {

                Simbolo sim = null;

                if (dec.getArbol() == null)//Variable
                {
                    if (dec.getExpresion() != null)//Trae valor
                    {
                        Expresion ExpDec = resolve.resolverExpresion(dec.getExpresion(), ent);
                        sim = new Simbolo(Tipo.Simbolo.variable, ExpDec);
                    }
                    else
                    {
                        Expresion ExpDec = new Expresion(Tipo.Valor.rnull, null);
                        sim = new Simbolo(Tipo.Simbolo.variable, ExpDec);
                    }
                }
                else if (dec.getArbol() != null)//Arreglo
                {
                    ArbolArreglo arbol = new ArbolArreglo(dec.getIdentificador().ToString());

                    if (dec.getExpresion() != null)
                    {
                        LinkedList<NodoArreglo> val = resolve.VAL_ARR(dec.getExpresion(), ent);
                        arbol.setValores(val.ElementAt(0).getListaHijos());
                    }
                    else
                    {
                        LinkedList<Expresion> dim = resolve.L_DIM(dec.getArbol(), ent);
                        foreach(Expresion exp in dim)
                        {
                            arbol.crearNodo(int.Parse(exp.getValor().ToString()));
                        }
                    }

                    sim = new Simbolo(Tipo.Simbolo.arreglo, arbol);
                }
                ent.add(dec.getIdentificador(), sim, 0, 0);//Guardamos en el entorno
            }
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.declaracion;
        }
    }
}
