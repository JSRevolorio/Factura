using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturacion.Models
{
    public class Respuesta
    {
        public bool Estado { get; set; }
        public string Mensaje { get; set; }
        public dynamic Resultado { get; set; }
    }
}