using Irony.Parsing;
using PyUSAC.Clases;
using PyUSAC.CMF;
using PyUSAC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PyUSAC.Analisis
{
    class Resolve
    {
        //public List<Error> listaErrores = new List<Error>();


        public LinkedList<Expresion> L_DIM(ParseTreeNode temp, Entorno ent)
        {
            LinkedList<Expresion> lista = null;
            if (temp.ChildNodes.Count == 4)
            {
                lista = L_DIM(temp.ChildNodes.ElementAt(0), ent);
                Expresion exp = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                lista.AddLast(exp);
            }
            else if (temp.ChildNodes.Count == 3)
            {
                lista = new LinkedList<Expresion>();
                Expresion exp = resolverExpresion(temp.ChildNodes.ElementAt(1), ent);
                lista.AddLast(exp);
            }
            return lista;
        }

        public Simbolo LLAMADAP(ParseTreeNode temp, Entorno ent)
        {
            Simbolo sim = null;
            if (temp.ChildNodes.Count == 1)
            {
                String name = temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0];
                sim = ent.search(name, temp.Span.Location.Line, temp.Span.Location.Column, true);
            }
            else if (temp.ChildNodes.Count == 3)
            {
                sim = LLAMADAP(temp.ChildNodes.ElementAt(0), ent);
                Expresion exp = (Expresion)sim.getContenido();
                if (exp.getTipo().Equals(Tipo.Valor.objeto))
                {
                    Entorno Eaux = (Entorno)exp.getValor();
                    sim = LLAMADAP(temp.ChildNodes.ElementAt(2), Eaux);
                }
                else
                {
                    Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "No es entorno",
                        temp.Span.Location.Line, temp.Span.Location.Column));
                }
            }
            return sim;
        }
        public LinkedList<NodoArreglo> VAL_ARR(ParseTreeNode temp, Entorno ent)
        {
            LinkedList<NodoArreglo> lista = new LinkedList<NodoArreglo>();

            if (temp.ChildNodes.Count == 3)//Si vienen { }
            {
                NodoArreglo nuevo = new NodoArreglo(null);
                LinkedList<NodoArreglo> listaH = null;
                listaH = VAL_ARR(temp.ChildNodes.ElementAt(1), ent);

                nuevo.setListaHijos(listaH);
                lista.AddLast(nuevo);
            }
            else if (temp.ChildNodes.Count == 5)//viene {},{}
            {

                lista = VAL_ARR(temp.ChildNodes.ElementAt(0), ent);//Guardar todo como nodos del arreglo xd
                LinkedList<NodoArreglo> listaH = VAL_ARR(temp.ChildNodes.ElementAt(3), ent);

                NodoArreglo nuevo = new NodoArreglo(null);
                nuevo.setListaHijos(listaH);

                lista.AddLast(nuevo);

            }
            else if (temp.ChildNodes.Count == 1)//vviene { L_EXP }
            {
                lista = L_EXP(temp.ChildNodes.ElementAt(0), ent);
            }

            return lista;
        }

        public LinkedList<NodoArreglo> L_EXP(ParseTreeNode temp, Entorno ent)
        {
            LinkedList<NodoArreglo> l_exp = null;
            Expresion exp = null;

            if (temp.ChildNodes.Count == 3)
            {
                l_exp = L_EXP(temp.ChildNodes.ElementAt(0), ent);
                exp = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                l_exp.AddLast(new NodoArreglo(exp));
            }
            else if (temp.ChildNodes.Count == 1)
            {
                l_exp = new LinkedList<NodoArreglo>();
                exp = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                l_exp.AddLast(new NodoArreglo(exp));
            }

            return l_exp;
        }

        public Simbolo asignacion(ParseTreeNode temp, Entorno ent)
        {
            Simbolo simAux = null;

            if (temp.ChildNodes.Count == 1)
            {

                String name = temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0];
                simAux = ent.search(name, 0, 0, true);
                //Expresion expAux = (Expresion)simAux.getContenido();
                //if (!expAux.getTipo().Equals(Tipo.Valor.objeto))
                //{
                //if (l_dim != null)
                //{
                //    //Expresion exp = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                //    if (expAux != null)
                //    {
                //        Expresion expAs = resolverExpresion(exp, ent);
                //        ArbolArreglo arbolAux = (ArbolArreglo)((Expresion)simAux.getContenido()).getValor();

                //        LinkedList<Expresion> dimensiones = L_DIM(l_dim, ent);

                //        arbolAux.setValor(expAs, dimensiones);
                //        Simbolo simAs = new Simbolo(Tipo.Simbolo.variable, new Expresion(Tipo.Valor.arreglo, arbolAux));

                //        //ent.edit(name, simAs);
                //    }
                //}
                //else if (l_dim == null)
                //{
                //    Expresion expAs = resolverExpresion(exp, ent);
                //    simAux = new Simbolo(Tipo.Simbolo.variable, expAs);

                //    //ent.edit(name, simAux);
                //}
                //}
                //else
                //{
                return simAux;
                //}

            }
            else if (temp.ChildNodes.Count == 3)
            {
                simAux = asignacion(temp.ChildNodes.ElementAt(0), ent);

                if (simAux != null)
                {
                    Simbolo nuevo = asignacion(temp.ChildNodes.ElementAt(2), (Entorno)((Expresion)simAux.getContenido()).getValor());
                    simAux = nuevo;
                }
            }

                return simAux;
        }

        public Expresion resolverExpresion(ParseTreeNode temp, Entorno ent)
        {
            if (temp.ChildNodes.Count == 3)//Si vienen 3 hijos es (E) o E op E
            {
                if (temp.ChildNodes.ElementAt(1).Term.ToString().Equals("E"))//Si es (E)
                {
                    return resolverExpresion(temp.ChildNodes.ElementAt(1), ent);
                }
                else
                {
                    String operador = temp.ChildNodes.ElementAt(1).ToString().Split(' ')[0];
                    Expresion hijo1, hijo2;

                    //try
                    //{
                    //    hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                    //}
                    //catch (Exception e)
                    //{
                    //    hijo1 = new Expresion(Tipo.Valor.rnull, "");
                    //}

                    //try
                    //{
                    //    hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                    //}
                    //catch (Exception e)
                    //{
                    //    hijo2 = new Expresion(Tipo.Valor.rnull, "");
                    //}

                    Object nuevoValor = null;
                    switch (operador)
                    {
                        case "+":

                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            try
                            {
                                hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                            }
                            catch (Exception e)
                            {
                                hijo2 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            //CADENAS
                            if (hijo1.getTipo().Equals(Tipo.Valor.cadena) || hijo2.getTipo().Equals(Tipo.Valor.cadena))
                            {
                                nuevoValor = hijo1.getValor().ToString() + "" + hijo2.getValor().ToString();
                                return new Expresion(Tipo.Valor.cadena, nuevoValor);
                            }
                            //NUMEROS
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) + Double.Parse(hijo2.getValor().ToString());
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) + car;
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = Double.Parse(hijo2.getValor().ToString()) + car;
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car1 = Convert.ToChar(hijo1.getValor().ToString());
                                double car2 = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = car1 + car2;
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.rnull) || hijo2.getTipo().Equals(Tipo.Valor.rnull))
                            {
                                nuevoValor = hijo1.getValor().ToString() + "" + hijo2.getValor().ToString();
                                return new Expresion(Tipo.Valor.cadena, nuevoValor);
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede sumarse con la expresion: " + hijo2.getValor().ToString(), linea, columna));

                                //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                //    + ". No puede sumarse con la expresion: " + hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.rnull, "");
                            }
                            break;

                        case "-":

                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            try
                            {
                                hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                            }
                            catch (Exception e)
                            {
                                hijo2 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            //NUMEROS
                            if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) - Double.Parse(hijo2.getValor().ToString());
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) - car;
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = car - Double.Parse(hijo2.getValor().ToString());
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car1 = Convert.ToChar(hijo1.getValor().ToString());
                                double car2 = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = car1 - car2;
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede restarse con la expresion: " + hijo2.getValor().ToString(), linea, columna));

                                //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                //    + ". No puede restarse con la expresion: " + hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.rnull, "");
                            }
                            break;

                        case "*":

                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            try
                            {
                                hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                            }
                            catch (Exception e)
                            {
                                hijo2 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            //NUMEROS
                            if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) * Double.Parse(hijo2.getValor().ToString());
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) * car;
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = Double.Parse(hijo2.getValor().ToString()) * car;
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car1 = Convert.ToChar(hijo1.getValor().ToString());
                                double car2 = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = car1 * car2;
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede multiplicarse con la expresion: " + hijo2.getValor().ToString(), linea, columna));

                                //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                //    + ". No puede multiplicarse con la expresion: " + hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.rnull, "");
                            }
                            break;

                        case "/":

                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            try
                            {
                                hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                            }
                            catch (Exception e)
                            {
                                hijo2 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            //NUMEROS
                            if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                if (!hijo2.getValor().ToString().Equals("0"))
                                {
                                    nuevoValor = Double.Parse(hijo1.getValor().ToString()) / Double.Parse(hijo2.getValor().ToString());
                                    nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                    return new Expresion(Tipo.Valor.numero, nuevoValor);
                                }
                                else
                                {
                                    int linea = temp.Span.Location.Line;
                                    int columna = temp.Span.Location.Column;

                                    Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                        + ". No puede dividirse en 0", linea, columna));

                                    //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    //    + ". No puede dividirse en 0");
                                    return new Expresion(Tipo.Valor.rnull, "");
                                }
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) / car;
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                if (!hijo2.getValor().ToString().Equals("0"))
                                {
                                    double car = Convert.ToChar(hijo1.getValor().ToString());

                                    nuevoValor = car / Double.Parse(hijo2.getValor().ToString());
                                    nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                    return new Expresion(Tipo.Valor.numero, nuevoValor);
                                }
                                else
                                {
                                    int linea = temp.Span.Location.Line;
                                    int columna = temp.Span.Location.Column;

                                    Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                        + ". No puede dividirse en 0", linea, columna));

                                    //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    //    + ". No puede dividirse en 0");
                                    return new Expresion(Tipo.Valor.rnull, "");
                                }
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car1 = Convert.ToChar(hijo1.getValor().ToString());
                                double car2 = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = car1 / car2;
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede dividirse con la expresion: " + hijo2.getValor().ToString(), linea, columna));

                                //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                //    + ". No puede dividirse con la expresion: " + hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.rnull, "");
                            }
                            break;

                        case "pow":

                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            try
                            {
                                hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                            }
                            catch (Exception e)
                            {
                                hijo2 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            //NUMEROS
                            if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Math.Pow(Double.Parse(hijo1.getValor().ToString()), Double.Parse(hijo2.getValor().ToString()));
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Math.Pow(Double.Parse(hijo1.getValor().ToString()), car);
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = Math.Pow(car, Double.Parse(hijo2.getValor().ToString()));
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car1 = Convert.ToChar(hijo1.getValor().ToString());
                                double car2 = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Math.Pow(car1, car2);

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede elevarse a la expresion: " + hijo2.getValor().ToString(), linea, columna));

                                //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                //    + ". No puede elevarse a la expresion: " + hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.rnull, "");
                            }
                            break;

                        case ">":

                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            try
                            {
                                hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                            }
                            catch (Exception e)
                            {
                                hijo2 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) > Double.Parse(hijo2.getValor().ToString());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) > car;

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = car > Double.Parse(hijo2.getValor().ToString());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car1 = Convert.ToChar(hijo1.getValor().ToString());
                                double car2 = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = car1 > car2;

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (>) con la expresion: " + hijo2.getValor().ToString(), linea, columna));

                                //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                //    + ". No puede compararse (>) con la expresion: " + hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.rnull, "");
                            }
                            break;

                        case "<":

                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            try
                            {
                                hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                            }
                            catch (Exception e)
                            {
                                hijo2 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) < Double.Parse(hijo2.getValor().ToString());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) < car;

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = car < Double.Parse(hijo2.getValor().ToString());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car1 = Convert.ToChar(hijo1.getValor().ToString());
                                double car2 = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = car1 < car2;

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (<) con la expresion: " + hijo2.getValor().ToString(), linea, columna));

                                //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                //    + ". No puede compararse (<) con la expresion: " + hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.rnull, "");
                            }
                            break;

                        case ">=":

                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            try
                            {
                                hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                            }
                            catch (Exception e)
                            {
                                hijo2 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) >= Double.Parse(hijo2.getValor().ToString());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) >= car;

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = car >= Double.Parse(hijo2.getValor().ToString());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car1 = Convert.ToChar(hijo1.getValor().ToString());
                                double car2 = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = car1 >= car2;

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (>=) con la expresion: " + hijo2.getValor().ToString(), linea, columna));

                                //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                //    + ". No puede compararse (>=) con la expresion: " + hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.rnull, "");
                            }
                            break;

                        case "<=":

                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            try
                            {
                                hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                            }
                            catch (Exception e)
                            {
                                hijo2 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) <= Double.Parse(hijo2.getValor().ToString());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) <= car;

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = car <= Double.Parse(hijo2.getValor().ToString());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car1 = Convert.ToChar(hijo1.getValor().ToString());
                                double car2 = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = car1 <= car2;

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (<=) con la expresion: " + hijo2.getValor().ToString(), linea, columna));

                                //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                //    + ". No puede compararse (<=) con la expresion: " + hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.rnull, "");
                            }
                            break;

                        case "==":

                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            try
                            {
                                hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                            }
                            catch (Exception e)
                            {
                                hijo2 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            if (hijo1.getTipo().Equals(Tipo.Valor.cadena) || hijo2.getTipo().Equals(Tipo.Valor.cadena))
                            {
                                nuevoValor = hijo1.getValor().ToString() == hijo2.getValor().ToString();

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) == Double.Parse(hijo2.getValor().ToString());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) == car;

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = car == Double.Parse(hijo2.getValor().ToString());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car1 = Convert.ToChar(hijo1.getValor().ToString());
                                double car2 = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = car1 == car2;

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.rnull) || hijo2.getTipo().Equals(Tipo.Valor.rnull))
                            {
                                nuevoValor = hijo1.getValor().ToString() == hijo2.getValor().ToString();

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (==) con la expresion: " + hijo2.getValor().ToString(), linea, columna));

                                //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                //    + ". No puede compararse (==) con la expresion: " + hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.rnull, "");
                            }
                            break;

                        case "<>":

                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            try
                            {
                                hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                            }
                            catch (Exception e)
                            {
                                hijo2 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            if (hijo1.getTipo().Equals(Tipo.Valor.cadena) || hijo2.getTipo().Equals(Tipo.Valor.cadena))
                            {
                                nuevoValor = hijo1.getValor().ToString() != hijo2.getValor().ToString();

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) != Double.Parse(hijo2.getValor().ToString());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.numero) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Double.Parse(hijo1.getValor().ToString()) != car;

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = car != Double.Parse(hijo2.getValor().ToString());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car1 = Convert.ToChar(hijo1.getValor().ToString());
                                double car2 = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = car1 != car2;

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            if (hijo1.getTipo().Equals(Tipo.Valor.rnull) || hijo2.getTipo().Equals(Tipo.Valor.rnull))
                            {
                                nuevoValor = hijo1.getValor().ToString() != hijo2.getValor().ToString();

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (<>) con la expresion: " + hijo2.getValor().ToString(), linea, columna));

                                //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                //    + ". No puede compararse (<>) con la expresion: " + hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.rnull, "");
                            }
                            break;

                        case "&&":

                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            if (hijo1.getTipo().Equals(Tipo.Valor.booleano))
                            {

                                if (hijo1.getValor().ToString().ToLower().Equals("false"))
                                {
                                    return new Expresion(Tipo.Valor.booleano, false);
                                }
                                else
                                {
                                    try
                                    {
                                        hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                                    }
                                    catch (Exception e)
                                    {
                                        hijo2 = new Expresion(Tipo.Valor.rnull, "");
                                    }

                                    if (hijo2.getTipo().Equals(Tipo.Valor.booleano))
                                    {
                                        nuevoValor = ((Boolean)hijo1.getValor()) && ((Boolean)hijo2.getValor());
                                    }
                                    else
                                    {
                                        int linea = temp.Span.Location.Line;
                                        int columna = temp.Span.Location.Column;

                                        Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo2.getValor().ToString()
                                            + ". Debe ser booleana para ser usada con el operador (&&)", linea, columna));

                                        //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                        //    + ". No puede compararse (&&) con la expresion: " + hijo2.getValor().ToString());
                                        return new Expresion(Tipo.Valor.rnull, "");
                                    }
                                }

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". Debe ser booleana para ser usada con el operador (&&)", linea, columna));

                                //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                //    + ". No puede compararse (&&) con la expresion: " + hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.rnull, "");
                            }
                            break;

                        case "||":
                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            if (hijo1.getTipo().Equals(Tipo.Valor.booleano))
                            {

                                if (hijo1.getValor().ToString().ToLower().Equals("true"))
                                {
                                    return new Expresion(Tipo.Valor.booleano, true);
                                }
                                else
                                {
                                    try
                                    {
                                        hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                                    }
                                    catch (Exception e)
                                    {
                                        hijo2 = new Expresion(Tipo.Valor.rnull, "");
                                    }

                                    if (hijo2.getTipo().Equals(Tipo.Valor.booleano))
                                    {
                                        nuevoValor = ((Boolean)hijo1.getValor()) || ((Boolean)hijo2.getValor());
                                    }
                                    else
                                    {
                                        int linea = temp.Span.Location.Line;
                                        int columna = temp.Span.Location.Column;

                                        Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo2.getValor().ToString()
                                            + ". Debe ser booleana para ser usada con el operador (||)", linea, columna));

                                        //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                        //    + ". No puede compararse (&&) con la expresion: " + hijo2.getValor().ToString());
                                        return new Expresion(Tipo.Valor.rnull, "");
                                    }
                                }

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". Debe ser booleana para ser usada con el operador (||)", linea, columna));

                                //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                //    + ". No puede compararse (&&) con la expresion: " + hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.rnull, "");
                            }
                            break;

                        case "^":
                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            try
                            {
                                hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                            }
                            catch (Exception e)
                            {
                                hijo2 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            if (hijo1.getTipo().Equals(Tipo.Valor.booleano) && hijo2.getTipo().Equals(Tipo.Valor.booleano))
                            {
                                nuevoValor = ((Boolean)hijo1.getValor()) ^ ((Boolean)hijo2.getValor());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (^) con la expresion: " + hijo2.getValor().ToString(), linea, columna));

                                //MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                //    + ". No puede compararse (^) con la expresion: " + hijo2.getValor().ToString());
                                return new Expresion(Tipo.Valor.rnull, "");
                            }
                            break;

                        case ".":
                            Entorno entorno = null;
                            try
                            {
                                hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                            }
                            catch (Exception e)
                            {
                                hijo1 = new Expresion(Tipo.Valor.rnull, "");
                            }

                            if (hijo1.getTipo().Equals(Tipo.Valor.objeto))
                            {
                                entorno = (Entorno)hijo1.getValor();
                                hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), entorno);
                                return hijo2;
                            }
                            else
                            {
                                int linea = temp.Span.Location.Line;
                                int columna = temp.Span.Location.Column;
                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico,hijo1.getValor().ToString()
                                    + ". No es una clase ",linea, columna));
                            }
                            
                            break;
                    }
                }
            }
            else if (temp.ChildNodes.Count == 2)
            {
                if (temp.ChildNodes.ElementAt(1).ToString().Equals("ZI2"))
                {
                    if (temp.ChildNodes.ElementAt(1).ChildNodes.Count != 0)
                    {
                        int linea = temp.Span.Location.Line;
                        int columna = temp.Span.Location.Column;
                        Simbolo sim = ent.search(temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0], linea, columna, true);

                        if (!temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ToString().Equals("RICARDO"))
                        {
                            LinkedList<Expresion> l_dim = L_DIM(temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0), ent);
                            ArbolArreglo arbolAux = (ArbolArreglo)((Expresion)sim.getContenido()).getValor();

                            return arbolAux.getValor(l_dim, temp.Span.Location.Line, temp.Span.Location.Column);
                        }
                        else{
                            ParseTreeNode n = temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).ChildNodes.ElementAt(1);
                            LinkedList<NodoArreglo> l_exp = new LinkedList<NodoArreglo>();

                            if (n.ChildNodes.Count != 0)
                            {
                                l_exp = L_EXP(n.ChildNodes.ElementAt(0), ent);
                            }

                            if (sim.getTipo().Equals(Tipo.Simbolo.funcion))
                            {
                                Funcion fun = (Funcion)sim.getContenido();
                                fun.setParametrosFun(l_exp);
                                fun.Ejecutar(ent);
                                Expresion exp = fun.getRetorno();
                                return exp;
                            }
                            else
                            {

                                Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico,
                                    ". No hay nada que retornar ", linea, columna));
                            }
                        }
                    }
                    else//viene solo identificador
                    {
                        int linea = temp.Span.Location.Line;
                        int columna = temp.Span.Location.Column;

                        Simbolo sim = ent.search(temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0], linea, columna, true);

                        if (sim != null)
                        {
                            return new Expresion(((Expresion)sim.getContenido()).getTipo(), ((Expresion)sim.getContenido()).getValor());
                        }
                    }

                }
                else if (temp.ChildNodes.ElementAt(1).ToString().Split(' ')[0].Equals("++"))
                {
                    if (temp.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Term.ToString().Equals("identificador"))
                    {
                        Expresion hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                        double num1;



                        if (hijo1.getTipo().Equals(Tipo.Valor.numero))
                        {
                            num1 = double.Parse(hijo1.getValor().ToString());
                            num1++;

                            ent.edit(temp.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ToString().Split(' ')[0],
                                new Simbolo(Tipo.Simbolo.variable, new Expresion(Tipo.Valor.numero, num1)));
                        }
                        else if (hijo1.getTipo().Equals(Tipo.Valor.caracter))
                        {
                            num1 = Convert.ToChar(hijo1.getValor().ToString());
                            num1++;

                            ent.edit(temp.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ToString().Split(' ')[0],
                                new Simbolo(Tipo.Simbolo.variable, new Expresion(Tipo.Valor.caracter, (char)num1)));
                        }

                        return hijo1;
                    }
                    else
                    {
                        Expresion hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);

                        if (hijo1.getTipo().Equals(Tipo.Valor.numero))
                        {
                            int num = int.Parse(hijo1.getValor().ToString());
                            num++;
                            return new Expresion(Tipo.Valor.numero, num);
                        }
                        else if (hijo1.getTipo().Equals(Tipo.Valor.caracter))
                        {
                            double car = Convert.ToChar(hijo1.getValor().ToString());
                            car++;
                            return new Expresion(Tipo.Valor.caracter, (char)car);
                        }
                    }
                }
                else if (temp.ChildNodes.ElementAt(1).ToString().Split(' ')[0].Equals("--"))
                {
                    if (temp.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Term.ToString().Equals("identificador"))
                    {
                        Expresion hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                        double num1 = double.Parse(hijo1.getValor().ToString());

                        num1--;

                        if (hijo1.getTipo().Equals(Tipo.Valor.numero))
                        {
                            ent.edit(temp.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ToString().Split(' ')[0],
                                new Simbolo(Tipo.Simbolo.variable, new Expresion(Tipo.Valor.numero, num1)));
                        }
                        else if (hijo1.getTipo().Equals(Tipo.Valor.caracter))
                        {
                            ent.edit(temp.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).ToString().Split(' ')[0],
                                new Simbolo(Tipo.Simbolo.variable, new Expresion(Tipo.Valor.caracter, (char)num1)));
                        }

                        return hijo1;
                    }
                    else
                    {
                        Expresion hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);

                        if (hijo1.getTipo().Equals(Tipo.Valor.numero))
                        {
                            int num = int.Parse(hijo1.getValor().ToString());
                            num--;
                            return new Expresion(Tipo.Valor.numero, num);
                        }
                        else if (hijo1.getTipo().Equals(Tipo.Valor.caracter))
                        {
                            double car = Convert.ToChar(hijo1.getValor().ToString());
                            car--;
                            return new Expresion(Tipo.Valor.caracter, (char)car);
                        }
                    }
                }
                else
                {
                    Expresion hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(1), ent);
                    String n = temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0];

                    if (n.Equals("-"))
                    {
                        if (hijo2.getTipo().Equals(Tipo.Valor.numero))
                        {
                            return new Expresion(Tipo.Valor.numero, -1 * Double.Parse(hijo2.getValor().ToString()));
                        }
                        else if (hijo2.getTipo().Equals(Tipo.Valor.cadena))
                        {
                            return new Expresion(Tipo.Valor.cadena, "-" + hijo2.getValor().ToString());
                        }
                        else if (hijo2.getTipo().Equals(Tipo.Valor.caracter))
                        {
                            double car = Convert.ToChar(hijo2.getValor().ToString());

                            return new Expresion(Tipo.Valor.cadena, -1 * car);
                        }
                    }
                    else if (n.Equals("!"))
                    {
                        if (hijo2.getTipo().Equals(Tipo.Valor.booleano))
                        {
                            return new Expresion(Tipo.Valor.booleano, !((Boolean)hijo2.getValor()));
                        }
                        else
                        {
                            int linea = temp.Span.Location.Line;
                            int columna = temp.Span.Location.Column;

                            Sintactico.listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo2.getValor().ToString()
                                        + ". No puede negarse", linea, columna));

                            //MessageBox.Show("La expresion: " + hijo2.getValor().ToString()
                            //            + ". No puede negarse");
                            return new Expresion(Tipo.Valor.rnull, "");
                        }
                    }
                }
            }
            else if(temp.ChildNodes.Count == 4)
            {

                String name = temp.ChildNodes.ElementAt(1).ToString().Split(' ')[0];
                int linea = temp.Span.Location.Line;
                int columna = temp.Span.Location.Column;
                Simbolo sim = ent.search(name, linea, columna, true);

                Expresion exp = new Expresion(Tipo.Valor.rnull, "");

                if (sim != null)
                {
                    Clase clase = (Clase)sim.getContenido();
                    Entorno nuevo = clase.Ejecutar(ent);

                    List<String> names = nuevo.getListKeys();
                    List<Simbolo> valores = nuevo.getListValues();

                    foreach (Simbolo simTemp in valores)
                    {
                        if (simTemp.getTipo().Equals(Tipo.Simbolo.funcion))
                        {
                            ((Funcion)simTemp.getContenido()).setExterno(nuevo);
                        }
                        else if (simTemp.getTipo().Equals(Tipo.Simbolo.metodo))
                        {
                            ((Metodo)simTemp.getContenido()).setExterno(nuevo);
                        }
                    }

                    exp = new Expresion(Tipo.Valor.objeto, nuevo);
                }
                return exp;
            }
            else if (temp.ChildNodes.Count == 1)
            {
                String type = temp.ChildNodes.ElementAt(0).Term.ToString().ToLower();

                switch (type)
                {
                    case "numero":
                        return new Expresion(Tipo.Valor.numero, int.Parse(temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0]));

                    case "caracter":
                        return new Expresion(Tipo.Valor.caracter, temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0]);

                    case "cadena":

                        return new Expresion(Tipo.Valor.cadena, temp.ChildNodes.ElementAt(0).ToString().Replace(" (cadena)", ""));

                    case "true":
                        return new Expresion(Tipo.Valor.booleano, true);

                    case "false":
                        return new Expresion(Tipo.Valor.booleano, false);

                    case "identificador":
                        Simbolo sim = ent.search(temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0], 0, 0, true);

                        if (sim != null)
                        {
                            return new Expresion(((Expresion)sim.getContenido()).getTipo(), (sim.getContenido() as Expresion).getValor());
                        }
                        break;

                    case "null":
                        return new Expresion(Tipo.Valor.rnull, "");
                }
            }
            return new Expresion(Tipo.Valor.rnull, "");
        }


    }
}
