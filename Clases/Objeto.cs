using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class Objeto
    {

        Entorno entorno;

        public Objeto(Entorno ent)
        {
            entorno = ent;
        }

        public Entorno getEntorno()
        {
            return entorno;
        }

    }
}
