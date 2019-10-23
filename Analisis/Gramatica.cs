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
            CommentTerminal comentarioLinea = new CommentTerminal("comentarioLinea", "//", "\r\n");
            CommentTerminal comentarioMulti = new CommentTerminal("comentarioMulti", "/*", "*/");
            NonGrammarTerminals.Add(comentarioLinea);
            NonGrammarTerminals.Add(comentarioMulti);

            //Expresiones----------------------------------------------------------------
            NumberLiteral numero = new NumberLiteral("numero");
            StringLiteral cadena = new StringLiteral("cadena", "\"");
            StringLiteral caracter = TerminalFactory.CreateCSharpChar("caracter");//Chars
            IdentifierTerminal identificador = new IdentifierTerminal("identificador");
            #endregion

            #region Terminales
            //Palabras reservadas--------------------------------------------------------
            var rvar = ToTerm("var");
            var rtrue = ToTerm("true");
            var rfalse = ToTerm("false");
            var rlog = ToTerm("log");
            var ralert = ToTerm("alert");


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
                LOG = new NonTerminal("LOG"),
                ALERT = new NonTerminal("ALERT"),

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
                      | LOG
                      | ALERT
                ;

            DECLARACION.Rule = rvar + L_ID + ZI + puntocoma
                ;

            L_ID.Rule = L_ID + coma + identificador
                      | identificador
                       ;

            ZI.Rule = igual + E
                    | Empty
                    ;

            LOG.Rule = rlog + parizq + E + parder + puntocoma
                ;

            ALERT.Rule = ralert + parizq + E + parder + puntocoma
                ;

            E.Rule = E + mas + E
                    | E + menos + E
                    | E + multi + E
                    | E + div + E
                    | E + potencia + E
                    | parizq + E + parder
                    | menos + E
                    //| E + mas + mas
                    | numero
                    | cadena
                    | identificador
                    | caracter
                    | rtrue
                    | rfalse
                    ;
            #endregion

            #region Especificaciones
            this.Root = INICIO;
            #endregion
        }

    }
}
