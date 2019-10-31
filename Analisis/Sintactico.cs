using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Irony.Ast;
using Irony.Parsing;
using PyUSAC.Clases;
using PyUSAC.Interfaces;
using System.Windows.Forms;
using PyUSAC.Instrucciones;

namespace PyUSAC.Analisis
{
    class Sintactico : Grammar
    {
        public static List<Error> listaErrores = new List<Error>();
        LinkedList<Instruccion> listaIns;
        public static LinkedList<String> listaImp;

        public Sintactico()
        {
            listaIns = new LinkedList<Instruccion>();
            listaImp = new LinkedList<String>();
        }

        public LinkedList<Instruccion> getIns()
        {
            return listaIns;
        }

        public bool Analizar(String cadena, int n)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbolAST = parser.Parse(cadena);
            ParseTreeNode raiz = arbolAST.Root;

            if (n == 0)
            {
                generarArbol(raiz);
            }
            else if(n == 8200)
            {
                try
                {
                    ejecutarArbol(raiz);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            if (raiz != null)
            {
                return true;
            }

            return false;
        }

        public static void generarArbol(ParseTreeNode temp)
        {
            try
            {
                String DOT = graficarArbol.textoGraphviz(temp);
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\Carpetas\\Arbol\\AST.txt", DOT);

                generarImagen(Directory.GetCurrentDirectory() + "\\Carpetas\\Arbol");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public static void generarImagen(String path)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("echo off");
            cmd.StandardInput.WriteLine("cd " + path);
            cmd.StandardInput.WriteLine($"dot -Tpng AST.txt -o AST.png");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }


        public void ejecutarArbol(ParseTreeNode temp)
        {
            Entorno global = new Entorno(null);
            listaIns = second(temp, global);

            foreach (Instruccion lista in listaIns)
            {
                lista.Ejecutar(global);
            }

        }

        /*Se encarga de ejecutar lo que esta fuera de clases, metodos, funciones y leugo ejecuta el main()*/
        public LinkedList<Instruccion> second(ParseTreeNode temp, Entorno ent)
        {
            LinkedList<Instruccion> Ins = null;
            switch (temp.Term.ToString())
            {
                case "INICIO":
                    Ins = second(temp.ChildNodes.ElementAt(0), ent);
                    return Ins;

                case "S":
                    Ins = second(temp.ChildNodes.ElementAt(0), ent);
                    return Ins;

                case "L_INS":
                    
                    if (temp.ChildNodes.Count == 2)//Si trae mas instrucciones
                    {
                        Ins = second(temp.ChildNodes.ElementAt(0), ent);//Tomamos la lista de instrucciones

                        Instruccion s = instructions(temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0), ent);//Guardamos la instrucciones
                        if (s != null)//Para excluir declaracion y asignacion
                        {
                            Ins.AddLast(s);
                        }
                    }
                    else if (temp.ChildNodes.Count == 1)//La primera instruccion
                    {
                        Ins = new LinkedList<Instruccion>();//Creamso la lista de instrucciones
                        Instruccion s = instructions(temp.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0), ent);//Guardamos la instrucciones

                        if (s != null)//Para excluir declaracion y asignacion
                        {
                            Ins.AddLast(s);
                        }

                    }
                    return Ins;
            }
            return Ins;
        }

        public Instruccion instructions(ParseTreeNode temp, Entorno ent)
        {
            switch (temp.Term.ToString())
            {
                case "DECLARACION":
                    LinkedList<Declaracion> ID = L_ID(temp.ChildNodes.ElementAt(1), ent);//Guardamos la lista de ID
                    
                    foreach (Declaracion dec in ID)
                    {

                        Simbolo sim = null;

                        if (dec.getExpresion() != null)
                        {
                            sim = new Simbolo(Tipo.Simbolo.variable, dec.getExpresion());
                        }
                        else if (dec.getArbol() != null)
                        {
                            sim = new Simbolo(Tipo.Simbolo.arreglo, dec.getArbol());
                        }
                        ent.add(dec.getIdentificador(), sim, 0, 0);//Guardamos en el entorno
                    }

                    return null;

                case "LOG":
                    Expresion expLog = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);//Resolver la expresion
                    return new Log(expLog);//Retornar instruccion Log

                case "ALERT":
                    Expresion expAlert = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);//Resolver la expresion
                    return new Alert(expAlert);

                case "ASIGNACION":

                    String name = temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0];
                    Expresion expAs = resolverExpresion(temp.ChildNodes.ElementAt(3), ent);

                    if (temp.ChildNodes.ElementAt(1).ChildNodes.Count != 0)
                    {
                        LinkedList<Expresion> l_dim = L_DIM(temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0), ent);
                        Simbolo aux = ent.search(name, 0, 0);

                        if (aux != null)
                        {
                            ArbolArreglo arbolAux = ((ArbolArreglo)aux.getContenido());
                            arbolAux.setValor(expAs, l_dim);
                            Simbolo simAs = new Simbolo(Tipo.Simbolo.arreglo, arbolAux);

                            ent.edit(name, simAs);
                        }
                    }
                    else
                    {
                        Simbolo simAs = new Simbolo(Tipo.Simbolo.variable, expAs);

                        ent.edit(name, simAs);
                    }

                    return null;

                case "GRAPH":
                    Expresion nameGraph = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);
                    Expresion grafo = resolverExpresion(temp.ChildNodes.ElementAt(4), ent);

                    if (nameGraph.getTipo().Equals(Tipo.Valor.cadena))
                    {
                        if (grafo.getTipo().Equals(Tipo.Valor.cadena))
                        {
                            return new Graph(nameGraph.getValor().ToString(), grafo.getValor().ToString());
                        }
                        listaErrores.Add(new Error(Tipo.Error.semantico, "Graph(Cadena,Cadena), Se esperaba una cadena" +
                        " en el segundo parametro", 0, 0));

                        MessageBox.Show("Graph(Cadena,Cadena), Se esperaba una cadena" +
                            " en el segundo parametro");
                    }

                    listaErrores.Add(new Error(Tipo.Error.semantico, "Graph(Cadena,Cadena), Se esperaba una cadena" +
                        " en el primer parametro", 0, 0));

                    MessageBox.Show("Graph(Cadena,Cadena), Se esperaba una cadena" +
                        " en el primer parametro");

                    return null;
                    break;
            }
            return null;
        }

        public LinkedList<Declaracion> L_ID(ParseTreeNode temp, Entorno ent)
        {
            LinkedList<Declaracion> l_id = null;

            if (temp.ChildNodes.Count == 4)
            {
                l_id = L_ID(temp.ChildNodes.ElementAt(0), ent);//Tomamos la lista
                String id = temp.ChildNodes.ElementAt(2).ToString().Split(' ')[0];//Tomamos el ID
                Declaracion dec = null;

                if (temp.ChildNodes.ElementAt(3).ChildNodes.Count != 0)//Si trae una asignacion
                {
                    if (!temp.ChildNodes.ElementAt(3).ChildNodes.ElementAt(0).Term.ToString().Equals("L_DIM"))//Si es variable normal
                    {
                        Expresion exp = resolverExpresion(temp.ChildNodes.ElementAt(3).ChildNodes.ElementAt(1), ent);

                        dec = new Declaracion(id, exp);

                        for (int x = l_id.Count - 1; x >= 0; x--)//Recorremos la lista para atras para cambiar lso nulos
                        {
                            if (l_id.ElementAt(x).getExpresion().getTipo().Equals(Tipo.Valor.rnull))
                            {
                                l_id.ElementAt(x).setExpresion(exp);
                            }
                        }
                    }
                    else
                    {//Si es un arreglo
                        LinkedList<Expresion> l_dim = L_DIM(temp.ChildNodes.ElementAt(3).ChildNodes.ElementAt(0), ent);
                        ArbolArreglo arreglo = new ArbolArreglo(id);

                        if (temp.ChildNodes.ElementAt(3).ChildNodes.ElementAt(1).ChildNodes.Count != 0)//si tra valores
                        {
                            LinkedList<NodoArreglo> valores = new LinkedList<NodoArreglo>();
                            valores = VAL_ARR(temp.ChildNodes.ElementAt(3).ChildNodes.ElementAt(1).ChildNodes.ElementAt(1), ent);
                            //Mandamos a traer los valores

                            
                            arreglo.setValores(valores.ElementAt(0).getListaHijos());
                            Console.WriteLine();
                        }
                        else
                        {
                            foreach (Expresion ex in l_dim)
                            {
                                arreglo.crearNodo(int.Parse(ex.getValor().ToString()));
                            }
                        }

                        dec = new Declaracion(id, arreglo);
                    }
                }
                else
                {
                    dec = new Declaracion(id, new Expresion(Tipo.Valor.rnull, null));
                }

                l_id.AddLast(dec);//Guardamos
            }
            else if (temp.ChildNodes.Count == 2)
            {
                l_id = new LinkedList<Declaracion>();//El primer ID, crreamos la lista
                String id = temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0];
                Declaracion dec = null;

                if (temp.ChildNodes.ElementAt(1).ChildNodes.Count != 0)
                {
                    if (!temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0).Term.ToString().Equals("L_DIM"))//Si es variable normal
                    {
                        Expresion exp = resolverExpresion(temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(1), ent);

                        dec = new Declaracion(id, exp);
                    }
                    else{//Si es un arreglo
                        LinkedList<Expresion> l_dim = L_DIM(temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0), ent);
                        ArbolArreglo arreglo = new ArbolArreglo(id);
                        if (temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(1).ChildNodes.Count != 0)
                        {

                            LinkedList<NodoArreglo> valores = new LinkedList<NodoArreglo>();
                            valores = VAL_ARR(temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(1).ChildNodes.ElementAt(1), ent);
                            //Mandamos a traer los valores

                            
                            arreglo.setValores(valores.ElementAt(0).getListaHijos());
                            Console.WriteLine();
                        }
                        else
                        {
                            foreach (Expresion ex in l_dim)
                            {
                                arreglo.crearNodo(int.Parse(ex.getValor().ToString()));
                            }
                        }

                        dec = new Declaracion(id, arreglo);
                    }
                }
                else
                {
                    dec = new Declaracion(id, new Expresion(Tipo.Valor.rnull, null));
                }

                l_id.AddLast(dec);//Guardamos
            }
            return l_id;
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
                    Expresion hijo1 = resolverExpresion(temp.ChildNodes.ElementAt(0), ent);
                    Expresion hijo2 = resolverExpresion(temp.ChildNodes.ElementAt(2), ent);

                    Object nuevoValor = null;
                    switch (operador)
                    {
                        case "+":
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
                            else
                            {
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede sumarse con la expresion: " + hijo2.getValor().ToString(), 0,0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede sumarse con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "-":
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
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede restarse con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede restarse con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "*":
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
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede multiplicarse con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede multiplicarse con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "/":
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
                                    listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                        + ". No puede dividirse en 0", 0, 0));

                                    MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                        + ". No puede dividirse en 0");
                                    return null;
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
                                if (!hijo2.getValor().ToString().Equals("0")) { 
                                    double car = Convert.ToChar(hijo1.getValor().ToString());

                                    nuevoValor = car / Double.Parse(hijo2.getValor().ToString());
                                    nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                    return new Expresion(Tipo.Valor.numero, nuevoValor);
                                }
                                else
                                {
                                    listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                        + ". No puede dividirse en 0", 0, 0));

                                    MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                        + ". No puede dividirse en 0");
                                    return null;
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
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede dividirse con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede dividirse con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "pow":
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

                                nuevoValor = Math.Pow(Double.Parse(hijo1.getValor().ToString()),car);
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.numero))
                            {
                                double car = Convert.ToChar(hijo1.getValor().ToString());

                                nuevoValor = Math.Pow(car,Double.Parse(hijo2.getValor().ToString()));
                                nuevoValor = Math.Truncate(((Double)nuevoValor) * 100000) / 100000;//Truncamos el valor en cinco decimales

                                return new Expresion(Tipo.Valor.numero, nuevoValor);
                            }
                            else if (hijo1.getTipo().Equals(Tipo.Valor.caracter) && hijo2.getTipo().Equals(Tipo.Valor.caracter))
                            {
                                double car1 = Convert.ToChar(hijo1.getValor().ToString());
                                double car2 = Convert.ToChar(hijo2.getValor().ToString());

                                nuevoValor = Math.Pow(car1,car2);

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede elevarse a la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede elevarse a la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case ">":
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
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (>) con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (>) con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "<":
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
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (<) con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (<) con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case ">=":
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
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (>=) con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (>=) con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "<=":
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
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (<=) con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (<=) con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "==":
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
                            else
                            {
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (==) con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (==) con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "<>":
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
                            else
                            {
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (<>) con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (<>) con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "&&":
                            if (hijo1.getTipo().Equals(Tipo.Valor.booleano) && hijo2.getTipo().Equals(Tipo.Valor.booleano))
                            {
                                nuevoValor = ((Boolean)hijo1.getValor()) && ((Boolean)hijo2.getValor());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (&&) con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (&&) con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "||":
                            if (hijo1.getTipo().Equals(Tipo.Valor.booleano) && hijo2.getTipo().Equals(Tipo.Valor.booleano))
                            {
                                nuevoValor = ((Boolean)hijo1.getValor()) || ((Boolean)hijo2.getValor());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (||) con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (||) con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;

                        case "^":
                            if (hijo1.getTipo().Equals(Tipo.Valor.booleano) && hijo2.getTipo().Equals(Tipo.Valor.booleano))
                            {
                                nuevoValor = ((Boolean)hijo1.getValor()) ^ ((Boolean)hijo2.getValor());

                                return new Expresion(Tipo.Valor.booleano, nuevoValor);
                            }
                            else
                            {
                                listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (^) con la expresion: " + hijo2.getValor().ToString(), 0, 0));

                                MessageBox.Show("La expresion: " + hijo1.getValor().ToString()
                                    + ". No puede compararse (^) con la expresion: " + hijo2.getValor().ToString());
                                return null;
                            }
                            break;
                    }
                }
            }
            else if (temp.ChildNodes.Count == 2)
            {
                if (!temp.ChildNodes.ElementAt(1).ToString().Equals("ZI2"))
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
                            listaErrores.Add(new Error(Tipo.Error.semantico, "La expresion: " + hijo2.getValor().ToString()
                                        + ". No puede negarse", 0, 0));

                            MessageBox.Show("La expresion: " + hijo2.getValor().ToString()
                                        + ". No puede negarse");
                            return null;
                        }
                    }
                }
                else
                {
                    Simbolo sim = ent.search(temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0], 0, 0);
                    LinkedList<Expresion> l_dim = L_DIM(temp.ChildNodes.ElementAt(1).ChildNodes.ElementAt(0), ent);
                    ArbolArreglo arbolAux = ((ArbolArreglo)sim.getContenido());

                    return arbolAux.getValor(l_dim);
                }
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
                        Simbolo sim = ent.search(temp.ChildNodes.ElementAt(0).ToString().Split(' ')[0], 0,0);

                        if (sim != null)
                        {
                            return new Expresion(((Expresion)sim.getContenido()).getTipo(), (sim.getContenido() as Expresion).getValor());
                        }
                        break;
                }
            }
            return null;
        }

    }

}
