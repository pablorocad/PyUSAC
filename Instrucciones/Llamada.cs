using Irony.Parsing;
using PyUSAC.Analisis;
using PyUSAC.Clases;
using PyUSAC.CMF;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Instrucciones
{
    class Llamada : Instruccion
    {
        ParseTreeNode lla;
        ParseTreeNode parametros;

        public Llamada(ParseTreeNode lla, ParseTreeNode parametros)
        {
            this.lla = lla;
            this.parametros = parametros;
        }

        public Instruccion Ejecutar(Entorno ent)
        {
            Resolve resolve = new Resolve();

            Simbolo sim = resolve.LLAMADAP(lla, ent);
            ParseTreeNode parametrosFun = null;

            if (parametros.ChildNodes.ElementAt(1).ChildNodes.Count != 0)
            {
                parametrosFun = parametros.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0);
            }

            if (sim.getTipo().Equals(Tipo.Simbolo.metodo))
            {
                Metodo met = (Metodo)sim.getContenido();
                met.setParametrosFun(parametrosFun);
                met.Ejecutar(ent);
            }

            return null;
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.llamada;
        }
    }
}
