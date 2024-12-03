using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

/*
    Requerimiento 1: Solo la primeraVez produccion es publica, el resto es privada  -Listo
    Requerimiento 2: Implementar la cerradura Epsilon
    Requerimiento 3: Implementar  el operador OR                                    -?
    Requerimiento 4: Indentar el código                                              -Ya casi
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
            //ident--;
            // indentar();
            producciones();
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
                primeraVez = false;
            }
            else if (Clasificacion == Tipos.SNT)
            {
                indentar();
                lenguajecs.WriteLine("private void " + Contenido + "()");
                indentar();
                lenguajecs.WriteLine("{");
            }
            match(Tipos.SNT);
            match(Tipos.Flecha);
            conjuntoTokens(false);
            match(Tipos.FinProduccion);
            ident--;
            indentar();
            lenguajecs.WriteLine("}");
            if (Clasificacion == Tipos.SNT)
            {
                producciones();
            }
        }
        private void conjuntoTokens(bool b_Or)
        {
            bool end = false;
            if (Clasificacion == Tipos.Izquierdo || b_Or == true)
            {
                bool generaCond = false;
                if (b_Or == true)
                {
                    match(Tipos.Izquierdo);
                    indentar();
                    lenguajecs.Write("if (");
                    if (Clasificacion == Tipos.ST)
                    {
                        lenguajecs.WriteLine("getContenido() == \"" + Contenido + "\")");
                        lenguajecs.WriteLine("{");
                        lenguajecs.WriteLine("match(\"" + Contenido + "\");");
                        match(Tipos.ST);
                    }
                    else if (Clasificacion == Tipos.Tipo)
                    {
                        lenguajecs.WriteLine("getClasificacion() == Tipos." + Contenido + ")");
                        lenguajecs.WriteLine("{");
                        lenguajecs.WriteLine("match(Tipos." + Contenido + ");");
                        match(Tipos.Tipo);
                    }
                }
                else
                {
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
                }
                if (Clasificacion != Tipos.Derecho)
                {
                    indentar();
                    lenguajecs.WriteLine("if (");
                    generaCond = true;
                }
                else
                {
                    match(Tipos.Derecho);
                    if (Clasificacion == Tipos.Epsilon)
                    {
                        end = true;
                        generaCond = true;
                        match(Tipos.Epsilon);
                        indentar();
                        lenguajecs.WriteLine("if ()");
                        /*string tokenOpcional = Contenido.Replace("?", "");
                        lenguajecs.WriteLine("if (getContenido() == \"" + tokenOpcional + "\")");
                        lenguajecs.WriteLine("{");
                        lenguajecs.WriteLine("match(\"" + tokenOpcional + "\");");
                        lenguajecs.WriteLine("}");*/
                        match(Tipos.Epsilon);
                    }
                }


                lenguajecs.WriteLine("}");
            }
            else if (Clasificacion == Tipos.SNT)
            {
                indentar();
                lenguajecs.WriteLine("            " + Contenido + "();");
                match(Tipos.SNT);
            }
            else if (Clasificacion == Tipos.ST)
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

            else if (Clasificacion == Tipos.Or)
            {
                //Or_Ep();
                if (Clasificacion == Tipos.SNT || Clasificacion == Tipos.ST || Clasificacion == Tipos.Tipo)
                {
                    ident--;
                    lenguajecs.WriteLine("}");
                    lenguajecs.WriteLine("else");
                    conjuntoTokens(true);
                }
                else
                {
                    throw new Error("Semántico" + linea + "Se esperaba un SNT, ST o Tipo", log);
                }
                match(Tipos.Or);
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
            while (Clasificacion != Tipos.Derecho)
            {
                if (Clasificacion == Tipos.ST)
                {
                    listaTokensOp.Add(new Token(Clasificacion, Contenido));
                    match(Tipos.ST);
                }
                else if (Clasificacion == Tipos.SNT)
                {
                    listaTokensOp.Add(new Token(Clasificacion, Contenido));
                    match(Tipos.SNT);
                }
                else if (Clasificacion == Tipos.Tipo)
                {
                    listaTokensOp.Add(new Token(Clasificacion, Contenido));
                    match(Tipos.Tipo);
                }
                else if (Clasificacion == Tipos.Or)
                {
                    listaTokensOp.Add(new Token(Clasificacion, Contenido));
                    match(Tipos.Or);
                }
                else
                {
                    Or_Ep();
                }
            }

        }
    }
}