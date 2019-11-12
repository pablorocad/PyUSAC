using PyUSAC.Clases;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Instrucciones
{
    class Break : Instruccion
    {
        public void Ejecutar(Entorno ent)
        {
        }

        public Tipo.Instruccion getTipo()
        {
            return Tipo.Instruccion.Break;
        }
    }
}
