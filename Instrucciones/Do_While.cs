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
    class Do_While : Instruccion
    {
        ParseTreeNode condicion;
        Instruccion bloque;

        public Do_While(ParseTreeNode condicion, Instruccion bloque)
        {
            this.condicion = condicion;
            this.bloque = bloque;
        }

        public Instruccion Ejecutar(Entorno ent)
        {

            Resolve resolve = new Resolve();
            Expresion cond = resolve.resolverExpresion(condicion, ent);//Resolvemso la condicion
            Instruccion ins = null;

            if (cond.getTipo().Equals(Tipo.Valor.booleano))
            {
                do
                {
                    ins = bloque.Ejecutar(ent);
                    if (ins != null)
                    {
                        if (ins.getTipo().Equals(Tipo.Instruccion.Break))
                        {
                            break;
                        }
                        else if (ins.getTipo().Equals(Tipo.Instruccion.Continue))
                        {
                            break;
                        }
                        else if (ins.getTipo().Equals(Tipo.Instruccion.Return))
                        {
                            return ins;
                        }
                    }
                    else
                    {
                        cond = resolve.resolverExpresion(condicion, ent);
                    }
                } while (cond.getValor().ToString().ToLower().Equals("true"));
            }
            else
            {
                int linea = condicion.Span.Location.Line;
                int columna = condicion.Span.Location.Column;
                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La Expresion debe ser booleana", linea, columna));
            }
            return null;
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.Do_While;
        }
    }
}
