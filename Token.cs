using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compilador
{
    public class Token
    {
        public enum Tipos
        {
            ST,
            SNT,
            Flecha,
            FinProduccion,
            Epsilon,
            Or,
            Derecho,
            Izquierdo,
            Tipo
        };
        private string contenido;
        private Tipos clasificacion;
        public Token()
        {
            contenido = "";
        }
        public Token(Tipos clasificacion,string contenido)
        {
            this.contenido = contenido;
            this.clasificacion = clasificacion;
        }
 
     public string Contenido
        {get ;set;}
        public Tipos Clasificacion
        {get; set;}
    }
}