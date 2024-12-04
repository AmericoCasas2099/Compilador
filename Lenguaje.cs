using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


/*
    Requerimiento 1: Solo la primera produccion es publica, el resto es privada  -Listo
    Requerimiento 2: Implementar la cerradura Epsilon                            -Listo
    Requerimiento 3: Implementar  el operador OR                                 -Listo
    Requerimiento 4: Indentar el código                                         -Ya casi              
*/

namespace Compilador
{
    public class Lenguaje : Sintaxis
    {

        bool primeraVez = true;
        int ident = 0;
        public Lenguaje()
        {

        }

        public Lenguaje(string nombre) : base(nombre)
        {

        }
        private void esqueleto(string nspace)
        {
            lenguajecs.WriteLine("using System;");
            lenguajecs.WriteLine("using System.Collections.Generic;");
            lenguajecs.WriteLine("using System.Linq;");
            lenguajecs.WriteLine("using System.Net.Http.Headers;");
            lenguajecs.WriteLine("using System.Reflection.Metadata.Ecma335;");
            lenguajecs.WriteLine("using System.Runtime.InteropServices;");
            lenguajecs.WriteLine("using System.Threading.Tasks;");
            lenguajecs.WriteLine("\nnamespace " + nspace);
            lenguajecs.WriteLine("{");
            ident++;
            indentar();
            indentar();
            lenguajecs.WriteLine("public class Lenguaje : Sintaxis");
            ident++;
            indentar();
            lenguajecs.WriteLine("{");
            ident++;
            indentar();
            lenguajecs.WriteLine("public Lenguaje()");
            indentar();
            lenguajecs.WriteLine("{");
            indentar();
            lenguajecs.WriteLine("}");
            indentar();
            lenguajecs.WriteLine("public Lenguaje(string nombre) : base(nombre)");
            indentar();
            lenguajecs.WriteLine("{");
            indentar();
            lenguajecs.WriteLine("}");
        }


        public void genera()
        {
            match("namespace");
            match(":");
            esqueleto(Contenido);
            match(Tipos.SNT);
            match(";");
            producciones();
            ident--;
            indentar();
            lenguajecs.WriteLine("}");
            ident--;
            indentar();
            lenguajecs.WriteLine("}");
        }

        private void producciones()
        {
            if (Clasificacion == Tipos.SNT && primeraVez == true)
            {
                indentar();
                lenguajecs.WriteLine("public void " + Contenido + "()");
                indentar();
                lenguajecs.WriteLine("{");
                ident++;
                primeraVez = false;
            }
            else if (Clasificacion == Tipos.SNT)
            {
                indentar();
                lenguajecs.WriteLine("private void " + Contenido + "()");
                indentar();
                lenguajecs.WriteLine("{");
                ident++;
            }
            match(Tipos.SNT);
            match(Tipos.Flecha);
            conjuntoTokens(false);
            match(Tipos.FinProduccion);
            // indentar();
            ident--;
            indentar();
            lenguajecs.WriteLine("}");

            if (Clasificacion == Tipos.SNT)
            {
                producciones();
            }

        }

        private void condInic(bool b_Or)
        {
            Tipos tipoT;
            String contenidoT;
            bool end = false;
            if (Clasificacion == Tipos.Izquierdo || b_Or == true)
            {
                bool generaCond;
                if (!b_Or)
                {
                    indentar();
                    match(Tipos.Izquierdo);
                    lenguajecs.Write("if (");
                    generaCond = true;
                    tipoT = Clasificacion;
                    contenidoT = Contenido;

                    if (Clasificacion == Tipos.SNT)
                    {
                        //modificar
                        throw new Error(" Semantico, Linea " + linea + ": No puede haber un SNT", log);
                    }
                    else if (Clasificacion == Tipos.ST)
                    {
                        match(Tipos.ST);
                    }
                    else if (Clasificacion == Tipos.Tipo)
                    {
                        match(Tipos.Tipo);
                    }
                    else
                    {
                        throw new Error(" Semantico, Linea " + linea + ": Se espera una sentencia", log);
                    }

                }
                else
                {
                    tipoT = Clasificacion;
                    contenidoT = Contenido;
                    if (Clasificacion == Tipos.SNT)
                    {
                        match(Tipos.SNT);
                    }
                    else if (Clasificacion == Tipos.ST)
                    {
                        match(Tipos.ST);
                    }
                    else if (Clasificacion == Tipos.Tipo)
                    {
                        match(Tipos.Tipo);
                    }
                    else
                    {
                        throw new Error(" Semantico, Linea " + linea + ": Se espera un sentencia", log);
                    }

                    if (Clasificacion != Tipos.Derecho)
                    {
                        //indentar();
                        lenguajecs.Write("if (");
                        generaCond = true;
                    }
                    else
                    {
                        match(Tipos.Derecho);
                        if (Clasificacion == Tipos.Epsilon)
                        {
                            match(Tipos.Epsilon);
                            lenguajecs.Write("if (");
                            generaCond = true;
                            end = true;
                        }
                        else
                        {

                            if (tipoT == Tipos.SNT)
                            {
                                lenguajecs.WriteLine("");
                                indentar();
                                lenguajecs.WriteLine("{");
                                ident++;
                                indentar();
                                lenguajecs.WriteLine(contenidoT + "()");
                            }
                            else if (tipoT == Tipos.ST)
                            {
                                indentar();
                                lenguajecs.WriteLine("{");
                                ident++;
                                indentar();
                                lenguajecs.WriteLine("match(\"" + contenidoT + "\");");
                            }
                            else
                            {
                                indentar();
                                lenguajecs.WriteLine("{");
                                ident++;
                                indentar();
                                lenguajecs.WriteLine("match(Tipos." + contenidoT + ");");
                            }
                            generaCond = false;
                            ident--;
                            indentar();
                            lenguajecs.WriteLine("}");

                        }
                    }

                }

                if ((tipoT == Tipos.SNT || tipoT == Tipos.ST || tipoT == Tipos.Tipo || tipoT == Tipos.Or) && generaCond)
                {
                    if (Clasificacion == Tipos.Or)
                    {
                        if (tipoT == Tipos.SNT)
                        {
                            throw new Error(" Semantico, Linea " + linea + ": No puede haber un SNT", log);
                        }
                        else if (tipoT == Tipos.ST)
                        {
                            lenguajecs.WriteLine("Contenido == \"" + contenidoT + "\")");
                            indentar();
                            lenguajecs.WriteLine("{");
                            ident++;
                            indentar();
                            lenguajecs.WriteLine("match(\"" + contenidoT + "\");");
                        }
                        else if (tipoT == Tipos.Tipo)
                        {
                            lenguajecs.WriteLine("Clasificacion == Tipos." + contenidoT + ")");
                            indentar();
                            lenguajecs.WriteLine("{");
                            ident++;
                            indentar();
                            lenguajecs.WriteLine("match(Tipos." + contenidoT + ");");
                        }

                        match(Tipos.Or);

                        if (Clasificacion == Tipos.SNT || Clasificacion == Tipos.ST || Clasificacion == Tipos.Tipo)
                        {
                            ident--;
                            indentar();
                            lenguajecs.WriteLine("}");
                            indentar();
                            lenguajecs.Write("else ");

                            conjuntoTokens(true);
                        }
                        else
                        {
                            throw new Error(" Semantico, Linea " + linea + ": Se espera un sentencia", log);
                        }
                    }
                    else
                    {
                        if (tipoT == Tipos.SNT && Clasificacion != Tipos.Derecho)
                        {

                            throw new Error(" Semantico, Linea " + linea + ": No puede haber un SNT", log);
                        }
                        else if (tipoT == Tipos.ST)
                        {
                            lenguajecs.WriteLine("Contenido == \"" + contenidoT + "\")");
                            indentar();
                            lenguajecs.WriteLine("{");
                            ident++;
                            indentar();
                            lenguajecs.WriteLine("match(\"" + contenidoT + "\");");
                        }
                        else if (tipoT == Tipos.Tipo)
                        {
                            lenguajecs.WriteLine("Clasificacion == Tipos." + contenidoT + ")");
                            indentar();
                            lenguajecs.WriteLine("{");
                            ident++;
                            indentar();
                            lenguajecs.WriteLine("match(Tipos." + contenidoT + ");");
                        }

                        if (end)
                        {
                            ident--;
                            indentar();
                            lenguajecs.WriteLine("}");
                        }

                    }
                }
            }
        }
        private void conjuntoTokens(bool b_Or)
        {
            condInic(b_Or);
            if (Clasificacion == Tipos.ST)
            {
                indentar();
                lenguajecs.WriteLine("match(\"" + Contenido + "\");");
                match(Tipos.ST);
            }
            else if (Clasificacion == Tipos.Tipo)
            {
                indentar();
                lenguajecs.WriteLine("match(Tipos." + Contenido + ");");
                match(Tipos.Tipo);
            }
            else if (Clasificacion == Tipos.SNT)
            {
                indentar();
                lenguajecs.WriteLine(Contenido + "();");
                match(Tipos.SNT);
            }
            else if (Clasificacion == Tipos.Derecho)
            {
                ident--;
                indentar();
                lenguajecs.WriteLine("}");
                match(Tipos.Derecho);
            }
            else if (Clasificacion == Tipos.Or)
            {
                match(Tipos.Or);

                if (Clasificacion == Tipos.SNT || Clasificacion == Tipos.ST || Clasificacion == Tipos.Tipo)
                {
                    ident--;
                    indentar();
                    lenguajecs.WriteLine("}");
                    indentar();
                    lenguajecs.WriteLine("else ");

                    conjuntoTokens(true);
                }
                else
                {
                    throw new Error(" Semantico, Linea " + linea + ": Se espera un sentencia", log);
                }
            }
            if (Clasificacion != Tipos.FinProduccion)
            {
                conjuntoTokens(false);
            }
        }

        private void indentar()
        {
            for (int i = 0; i < ident; i++)
            {
                lenguajecs.Write("\t");
            }

        }
        private void Or_Ep()
        {
            List<Token> listaTokensOp = new List<Token>();
            match(Tipos.Izquierdo);
        }
    }
}