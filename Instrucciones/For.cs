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

        public void Ejecutar(Entorno ent)
        {
            Sintactico.pilaBreak.Push(this);//Indicamos que estamos ejecutando

            Resolve resolve = new Resolve();
            SegundaPasada second = new SegundaPasada();

            Instruccion ins;
            ins = second.instructions(inicilizacion, entornoFor);
            ins.Ejecutar(entornoFor);

            Expresion cond = resolve.resolverExpresion(condicion, entornoFor);//Resolvemos condicion

            if (!cond.getTipo().Equals(Tipo.Valor.rnull))
            {
                while (cond.getValor().ToString().ToLower().Equals("true"))
                {//Mientras la condicion se cumpla ejecutaremos el bloque
                    //Entorno entAux = new Entorno(entornoFor);
                    if (Sintactico.pilaBreak.Count == 0 || !Sintactico.pilaBreak.Peek().Equals(this))
                    {
                        break;//Si ya no se encuentra la instruccion es porque hubo un break
                    }

                    bloque.Ejecutar(entornoFor);
                    resolve.resolverExpresion(actualizacion, entornoFor);//actualizar
                    cond = resolve.resolverExpresion(condicion, entornoFor);

                    if (cond.getTipo().Equals(Tipo.Valor.rnull))
                    {
                        cond = new Expresion(Tipo.Valor.booleano, false);
                    }
                }
            }

            if (Sintactico.pilaBreak.Count != 0 && Sintactico.pilaBreak.Peek().Equals(this))
            {
                Sintactico.pilaBreak.Pop();//Si termino la interacion sin break, indicamos que salimos
            }
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.For;
        }
    }
}
