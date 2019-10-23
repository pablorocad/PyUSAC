using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyUSAC.Clases
{
    class Tipo
    {

        public enum Valor
        {
            numero, cadena, identificador, caracter, booleano, rnull,
            clase, metodo, funcion
        };

        public enum Error
        {
            lexico, sintactico, semantico
        };

        public enum Instruccion
        {
            declaracion, log
        };

    }
}
