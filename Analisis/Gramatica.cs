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
            var rgraph = ToTerm("graph");
            var rif = ToTerm("if");
            var relse = ToTerm("else");
            var rwhile = ToTerm("while");
            var rdo = ToTerm("do");
            var rfor = ToTerm("for");
            var rswitch = ToTerm("switch");
            var rcase = ToTerm("case");
            var rbreak = ToTerm("break");
            var rdefault = ToTerm("default");

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
            var llaveizq = ToTerm("{");
            var llaveder = ToTerm("}");
            var corcheteizq = ToTerm("[");
            var corcheteder = ToTerm("]");
            var mayor = ToTerm(">");
            var menor = ToTerm("<");
            var igualigual = ToTerm("==");
            var desigual = ToTerm("<>");
            var mayorigual = ToTerm(">=");
            var menorigual = ToTerm("<=");
            var and = ToTerm("&&");
            var or = ToTerm("||");
            var xor = ToTerm("^");
            var not = ToTerm("!");
            var aumento = ToTerm("++");
            var decremento = ToTerm("--");
            var dospuntos = ToTerm(":");
            #endregion

            #region Precedencia
            RegisterOperators(1, xor);
            RegisterOperators(2, or);
            RegisterOperators(3, and);
            RegisterOperators(4, igualigual, desigual);
            RegisterOperators(5, mayor, menor, mayorigual, menorigual);
            RegisterOperators(6, mas, menos);
            RegisterOperators(7, multi, div);
            RegisterOperators(8, potencia);
            RegisterOperators(9, not);
            RegisterOperators(10, aumento, decremento);
            #endregion

            #region No Terminales

            NonTerminal INICIO = new NonTerminal("INICIO"),
                S = new NonTerminal("S"),
                L_INS = new NonTerminal("L_INS"),
                INS = new NonTerminal("INS"),
                SUBINS = new NonTerminal("SUBINS"),


                DECLARACION = new NonTerminal("DECLARACION"),
                L_DIM = new NonTerminal("L_DIM"),
                VAL_ARR = new NonTerminal("VAL_ARR"),
                VAL_ARR2 = new NonTerminal("VAL_ARR2"),
                L_EXP = new NonTerminal("L_EXP"),

                LOG = new NonTerminal("LOG"),
                ALERT = new NonTerminal("ALERT"),
                ASIGNACION = new NonTerminal("ASIGNACION"),
                GRAPH = new NonTerminal("GRAPH"),
                AUM_DEC = new NonTerminal("AUM_DEC"),

                IF = new NonTerminal("IF"),
                ELSE = new NonTerminal("ELSE"),
                WHILE = new NonTerminal("WHILE"),
                DO_WHILE = new NonTerminal("DO_WHILE"),
                FOR = new NonTerminal("FOR"),
                DEC_ASIG = new NonTerminal("DEC_ASIG"),
                SWITCH = new NonTerminal("SWITCH"),
                BLOQUE_SW = new NonTerminal("BLOQUE_SW"),
                DEFAULT = new NonTerminal("DEFAULT"),
                BREAK = new NonTerminal("BREAK"),

                BLOQUE = new NonTerminal("BLOQUE"),
                L_ID = new NonTerminal("L_ID"),
                ZI = new NonTerminal("ZI"),
                ZI2 = new NonTerminal("ZI2"),
                E = new NonTerminal("E")
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
                      | ASIGNACION
                      | GRAPH
                      | AUM_DEC
                      | IF
                      | WHILE
                      | DO_WHILE
                      | FOR
                      | SWITCH
                ;

            SUBINS.Rule = L_INS
                        |Empty
                ;

            DECLARACION.Rule = rvar + L_ID + puntocoma
                ;

            L_ID.Rule = L_ID + coma + identificador + ZI
                      | identificador + ZI
                       ;

            ZI.Rule = igual + E
                    |L_DIM + VAL_ARR
                    | Empty
                    ;

            ZI2.Rule = L_DIM
                     | Empty
                     ;

            L_DIM.Rule = L_DIM + corcheteizq + E + corcheteder
                        | corcheteizq + E + corcheteder
                ;

            VAL_ARR.Rule = igual + VAL_ARR2
                         |Empty
                ;

            VAL_ARR2.Rule = VAL_ARR2 + coma + llaveizq + VAL_ARR2 + llaveder
                          | llaveizq + VAL_ARR2 + llaveder
                          | L_EXP
                ;

            L_EXP.Rule = L_EXP + coma + E
                       | E
                       ;

            LOG.Rule = rlog + parizq + E + parder + puntocoma
                ;

            ALERT.Rule = ralert + parizq + E + parder + puntocoma
                ;

            ASIGNACION.Rule = identificador + ZI2 + igual + E + puntocoma
                ;

            GRAPH.Rule = rgraph + parizq + E + coma + E + parder + puntocoma
                ;

            AUM_DEC.Rule = E + aumento + puntocoma
                      | E + decremento + puntocoma
                      ;


            IF.Rule = rif + parizq + E + parder + BLOQUE + ELSE
                ;

            BLOQUE.Rule = llaveizq + SUBINS + llaveder
                ;

            ELSE.Rule = relse + IF
                      | relse + BLOQUE
                      |Empty
                    ;

            WHILE.Rule = rwhile +parizq + E + parder + BLOQUE
                ;

            DO_WHILE.Rule = rdo + BLOQUE + rwhile + parizq + E + parder + puntocoma
                ;

            FOR.Rule = rfor + parizq + DEC_ASIG + puntocoma + E + puntocoma + E + parder + BLOQUE
                ;

            DEC_ASIG.Rule = rvar + identificador + igual + E
                          | identificador + igual + E
                          ;

            SWITCH.Rule = rswitch + parizq + identificador + parder + llaveizq + BLOQUE_SW + DEFAULT + llaveder
                ;

            BLOQUE_SW.Rule = BLOQUE_SW + rcase + E + dospuntos + SUBINS + BREAK
                            | rcase + E + dospuntos + SUBINS + BREAK
                            ;

            BREAK.Rule = rbreak + puntocoma
                        | Empty
                        ;

            DEFAULT.Rule = rdefault + dospuntos + SUBINS + BREAK
                          | Empty
                          ;

            E.Rule = E + mas + E
                    | E + menos + E
                    | E + multi + E
                    | E + div + E
                    | E + potencia + E
                    | E + menor + E
                    | E + mayor + E
                    | E + mayorigual + E
                    | E + menorigual + E
                    | E + igualigual + E
                    | E + desigual + E
                    | E + or + E
                    | E + and + E
                    | E + xor + E
                    |E + aumento
                    |E + decremento
                    | not + E
                    | parizq + E + parder
                    | menos + E
                    | numero
                    | cadena
                    | identificador + ZI2
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
