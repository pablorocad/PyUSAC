using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace PyUSAC.Analisis
{
    class Gramatica : Grammar
    {

        public Gramatica() : base (caseSensitive: false)
        {
            #region Expresiones Regulares
            //COMENTARIOS----------------------------------------------------------------
            CommentTerminal comentarioLinea = new CommentTerminal("comentarioLinea", "//", "\n", "\r\n");
            CommentTerminal comentarioMulti = new CommentTerminal("comentarioMulti", "/*", "*/");
            NonGrammarTerminals.Add(comentarioLinea);
            NonGrammarTerminals.Add(comentarioMulti);

            //Expresiones----------------------------------------------------------------
            NumberLiteral numero = new NumberLiteral("numero");
            StringLiteral cadena = new StringLiteral("cadena", "\"");
            IdentifierTerminal identificador = new IdentifierTerminal("identificador");
            #endregion

            #region Terminales
            //Palabras reservadas--------------------------------------------------------
            var rvar = ToTerm("var");


            //Simbolos-------------------------------------------------------------------
            var igual = ToTerm("=");
            var puntocoma = ToTerm(";");
            var coma = ToTerm(",");
            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var multi = ToTerm("*");
            var div = ToTerm("/");
            var potencia = ToTerm("pow");
            var parizq = ToTerm("(");
            var parder = ToTerm(")");
            #endregion

            #region Precedencia
            RegisterOperators(1, mas, menos);
            RegisterOperators(2, multi, div);
            #endregion

            #region No Terminales

            NonTerminal INICIO = new NonTerminal("INICIO"),
                S = new NonTerminal("S"),
                L_INS = new NonTerminal("L_INS"),
                INS = new NonTerminal("INS"),
                DECLARACION = new NonTerminal("DECLARACION"),
                L_ID = new NonTerminal("L_ID"),
                ZI = new NonTerminal("ZI"),
                E = new NonTerminal("E");
                ;

            #endregion

            #region Gramatica
            INICIO.Rule = S
                        |Empty //Epsilon
                        ;

            S.Rule = L_INS
                ;

            L_INS.Rule = L_INS + INS
                       | INS
                        ;

            INS.Rule = DECLARACION
                ;

            DECLARACION.Rule = rvar + L_ID + ZI + puntocoma
                ;

            L_ID.Rule = L_ID + coma + identificador
                      | identificador
                       ;

            ZI.Rule = igual + E
                    | Empty
                    ;

            E.Rule = E + mas + E
                    | E + menos + E
                    | E + multi + E
                    | E + div + E
                    | E + potencia + E
                    | parizq + E + parder
                    | numero
                    | cadena
                    | identificador
                    ;
            #endregion

            #region Especificaciones
            this.Root = INICIO;
            #endregion
        }

    }
}
