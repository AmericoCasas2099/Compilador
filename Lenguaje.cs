using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

/*
    Requerimiento 1: Solo la primera produccion es publica, el resto es privada
    Requerimiento 2: Implementar la cerradura Epsilo
    Requerimiento 3: Implementar la el operador OR
    Requerimiento 4: Indentar el código
*/

namespace Compilador
{
    public class Lenguaje : Sintaxis
    {
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
            lenguajecs.WriteLine("    public class Lenguaje : Sintaxis");
            lenguajecs.WriteLine("    {");
            lenguajecs.WriteLine("        public Lenguaje()");
            lenguajecs.WriteLine("        {");
            lenguajecs.WriteLine("        }");
            lenguajecs.WriteLine("        public Lenguaje(string nombre) : base(nombre)");
            lenguajecs.WriteLine("        {");
            lenguajecs.WriteLine("        }");
        }
        public void genera()
        {
            match("namespace");
            match(":");
            esqueleto(getContenido());
            match(Tipos.SNT);
            match(";");
            producciones();
            lenguajecs.WriteLine("    }");
            lenguajecs.WriteLine("}");
        }
        private void producciones()
        {
            if (getClasificacion() == Tipos.SNT)
            {
                lenguajecs.WriteLine("        public void " + getContenido() + "()");
                lenguajecs.WriteLine("        {");
            }
            match(Tipos.SNT);
            match(Tipos.Flecha);
            conjuntoTokens();
            match(Tipos.FinProduccion);
            lenguajecs.WriteLine("        }");
            if (getClasificacion() == Tipos.SNT)
            {
                producciones();
            }
        }
        private void conjuntoTokens()
        {
            if (getClasificacion() == Tipos.SNT)
            {
                lenguajecs.WriteLine("            " + getContenido() + "();");
                match(Tipos.SNT);
            }
            else if (getClasificacion() == Tipos.ST)
            {
                lenguajecs.WriteLine("            match(\"" + getContenido() + "\");");
                match(Tipos.ST);
            }
            else if (getClasificacion() == Tipos.Tipo)
            {
                lenguajecs.WriteLine("            match(Tipos." + getContenido() + ");");
                match(Tipos.Tipo);
            }
            else if (getClasificacion() == Tipos.Izquierdo)
            {
                match(Tipos.Izquierdo);
                lenguajecs.Write("            if (");
                if (getClasificacion() == Tipos.ST)
                {
                    lenguajecs.WriteLine("getContenido() == \"" + getContenido() + "\")");
                    lenguajecs.WriteLine("            {");
                    lenguajecs.WriteLine("                match(\"" + getContenido() + "\");");
                    match(Tipos.ST);
                }
                else if (getClasificacion() == Tipos.Tipo)
                {
                    lenguajecs.WriteLine("getClasigficacion() == Tipos." + getContenido() + ")");
                    lenguajecs.WriteLine("            {");
                    lenguajecs.WriteLine("                match(Tipos." + getContenido() + ");");
                    match(Tipos.Tipo);
                }
                match(Tipos.Derecho);
                lenguajecs.WriteLine("            }");
            }
            if (getClasificacion() != Tipos.FinProduccion)
            {
                conjuntoTokens();
            }
        }
    }
}