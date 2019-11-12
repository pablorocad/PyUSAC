using Irony.Parsing;
using PyUSAC.Clases;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.CMF
{
    class Clase : Bloques
    {
        String identificador;
        ParseTreeNode esqueleto;

        public Clase(string identificador, ParseTreeNode esqueleto)
        {
            this.identificador = identificador;
            this.esqueleto = esqueleto;
        }

        public ParseTreeNode getEsqueleto()
        {
            return esqueleto;
        }

        public string getName()
        {
            return identificador;
        }

        public Tipo.Simbolo getTipo()
        {
            return Tipo.Simbolo.clase;
        }
    }
}
