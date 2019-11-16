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
    class Continue : Instruccion
    {
        public Instruccion Ejecutar(Entorno ent)
        {

            return this;
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.Continue;
        }
    }
}
