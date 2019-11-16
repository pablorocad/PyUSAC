using PyUSAC.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Interfaces
{
    interface Instruccion
    {

        Tipo.Instruccion getTipo();

        Instruccion Ejecutar(Entorno ent);

    }
}
