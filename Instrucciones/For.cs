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
    class For : Instruccion
    {

        ParseTreeNode inicilizacion;
        ParseTreeNode condicion;
        ParseTreeNode actualizacion;
        Entorno entornoFor;//Entorno entre el global y el bloque (Solo para iteracion)
        Instruccion bloque;

        public For(ParseTreeNode inicilizacion, ParseTreeNode condicion, ParseTreeNode actualizacion, Entorno ent, Instruccion bloque)
        {
            this.inicilizacion = inicilizacion;
            this.condicion = condicion;
            this.actualizacion = actualizacion;
            entornoFor = ent;
            this.bloque = bloque;
        }

        public Instruccion Ejecutar(Entorno ent)
        {
            Resolve resolve = new Resolve();
            SegundaPasada second = new SegundaPasada();

            Instruccion ins;
            ins = second.instructions(inicilizacion, entornoFor);//Resolvemos la variable de inicio
            ins.Ejecutar(entornoFor);

            Expresion cond = resolve.resolverExpresion(condicion, entornoFor);//Resolvemos condicion

            
             while (cond.getValor().ToString().ToLower().Equals("true"))
             {//Mientras la condicion se cumpla ejecutaremos el bloque
                     ins = bloque.Ejecutar(entornoFor);

                if (ins != null)
                {
                    if (ins.getTipo().Equals(Tipo.Instruccion.Break))
                    {
                        break;
                    }
                    else if (ins.getTipo().Equals(Tipo.Instruccion.Continue))
                    {
                        resolve.resolverExpresion(actualizacion, entornoFor);//actualizar
                        cond = resolve.resolverExpresion(condicion, entornoFor);
                    }
                    else if (ins.getTipo().Equals(Tipo.Instruccion.Return))
                    {
                        return ins;
                    }
                }
                else
                {
                    resolve.resolverExpresion(actualizacion, entornoFor);//actualizar
                    cond = resolve.resolverExpresion(condicion, entornoFor);
                }
             }
            return null;
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.For;
        }
    }
}
