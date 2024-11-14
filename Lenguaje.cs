using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

/*
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
            lenguajecs.WriteLine("\nnamespace "+nspace);
            lenguajecs.WriteLine("{");
            lenguajecs.WriteLine("    public class Lenguaje : Sintaxis");
            lenguajecs.WriteLine("    {");
            lenguajecs.WriteLine("        public Lenguaje()");
            lenguajecs.WriteLine("        {");
            lenguajecs.WriteLine("        }");
            lenguajecs.WriteLine("        public Lenguaje(string nombre) : base(nombre)");
            lenguajecs.WriteLine("        {");
            lenguajecs.WriteLine("        }");
            lenguajecs.WriteLine("        public void ");
        }
        public void genera()
        {
            match("namespace");
            match(":");
            esqueleto(getContenido());
            match(Tipos.SNT);
            match(";");
            lenguajecs.WriteLine("    }");
            lenguajecs.WriteLine("}");
        }
        private void producciones(){
            if(getClasificacion() == Tipos.SNT){
                lenguajecs.WriteLine("        public void "+getContenido()+"()");

            }
            match(Tipos.SNT);
            match(Tipos.Flecha);
            match(Tipos.FinProduccion);
            lenguajecs.WriteLine(" }");
            if(getClasificacion() == Tipos.SNT){
                producciones();
            }
        }
    }
}