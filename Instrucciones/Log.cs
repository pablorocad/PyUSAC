using Irony.Parsing;
using PyUSAC.Analisis;
using PyUSAC.Clases;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PyUSAC.Instrucciones
{
    class Log : Instruccion
    {

        ParseTreeNode exp;

        public Log(ParseTreeNode exp)
        {
            this.exp = exp;
        }

        public void Ejecutar(Entorno ent)
        {
            if (exp != null)
            {
                Resolve resolve = new Resolve();
                Expresion aux = resolve.resolverExpresion(exp, ent);//Resolvemos la expresion
                if (aux.getValor() != null)
                {
                    Sintactico.listaImp.AddLast(aux.getValor().ToString());
                }
            }
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.log;
        }
    }
}
