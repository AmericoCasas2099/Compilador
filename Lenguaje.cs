using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

/*
    Requerimiento 1: Solo la primera produccion es publica, el resto es privada  -Listo
    Requerimiento 2: Implementar la cerradura Epsilon
    Requerimiento 3: Implementar  el operador OR
    Requerimiento 4: Indentar el código
*/

namespace Compilador
{
    public class Lenguaje : Sintaxis
    {
        bool primera = true;
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
            lenguajecs.WriteLine("public class Lenguaje : Sintaxis");
            indentar();
            lenguajecs.WriteLine("{");
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
            if (Clasificacion == Tipos.SNT && primera == true)
            {
                indentar();
                lenguajecs.WriteLine("public void " + Contenido + "()");
                indentar();
                lenguajecs.WriteLine("{");
                primera = false;
            }
            else
            {
                indentar();
                lenguajecs.WriteLine("private void " + Contenido + "()");
                indentar();
                lenguajecs.WriteLine("{");
            }
            match(Tipos.SNT);
            match(Tipos.Flecha);
            conjuntoTokens();
            match(Tipos.FinProduccion);
            lenguajecs.WriteLine("        }");
            if (Clasificacion == Tipos.SNT)
            {
                producciones();
            }
        }
        private void conjuntoTokens()
        {
            if (Clasificacion == Tipos.SNT)
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
            else if (Clasificacion == Tipos.Izquierdo)
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

                match(Tipos.Derecho);
                if (Clasificacion == Tipos.Epsilon)
            {
                string tokenOpcional = Contenido.Replace("?", "");
                lenguajecs.WriteLine("if (getContenido() == \"" + tokenOpcional + "\")");
                lenguajecs.WriteLine("{");
                lenguajecs.WriteLine("match(\"" + tokenOpcional + "\");");
                lenguajecs.WriteLine("}");
                match(Tipos.Epsilon);
            }
                lenguajecs.WriteLine("}");
            }
            
            if (Clasificacion != Tipos.FinProduccion)
            {
                conjuntoTokens();
            }
        }

        private void indentar()
        {
            for (int i = 0; i < ident; i++)
            {
                lenguajecs.Write("\t");
            }
                
            }
        }
    }