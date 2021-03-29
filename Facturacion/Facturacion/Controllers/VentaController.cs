using Facturacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;


namespace Facturacion.Controllers
{
    public class VentaController : Controller
    {
        private TiendaEntities db = new TiendaEntities();
        private Respuesta respuesta = new Respuesta();

        // GET: Venta
        public ActionResult Index()
        {
            return View();
        }

        // GET: Venta
        public ActionResult Create()
        {
            int numero = db.VENTA.Select(v => v.id).DefaultIfEmpty(0).Max();
            int numeroFactura = (numero == 0) ? (1000000) : (1000000 + numero);

            ViewBag.fecha = DateTime.Now.Date.ToString("dd/MM/yyyy");
            ViewBag.numeroFactura = Guid.NewGuid().ToString();
            ViewBag.serie = "A";
            ViewBag.numeroDTE = numeroFactura.ToString();


            ViewBag.idProducto = new SelectList(db.PRODUCTO, "id", "nombre");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(VENTA venta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.VENTA.Add(venta);
                    await db.SaveChangesAsync();

                    respuesta.Estado = true;
                    respuesta.Mensaje = "! transaccion Exitosa";
                    respuesta.Resultado = venta.id;
                    return Json(respuesta);
                }
                catch (Exception ex)
                {
                    respuesta.Estado = false;
                    respuesta.Mensaje = "! Error de Servidor " + ex.Message;
                    return Json(respuesta);
                }
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));

                respuesta.Estado = false;
                respuesta.Mensaje = "! Validar " + messages;

                return Json(respuesta);
            }
        }

        [HttpPost]
        public ActionResult FindCliente(String nit)
        {
            try
            {
                var cliente = db.CLIENTE.Where(c => c.nit == nit).FirstOrDefault();

                if (cliente != null)
                {
                    var json = new
                    {
                        id = cliente.id,
                        nombre = cliente.nombre,
                        direccion = cliente.direccion
                    };

                    respuesta.Estado = true;
                    respuesta.Mensaje = "! transaccion Exitosa";
                    respuesta.Resultado = json;

                }
                else 
                {
                    respuesta.Estado = false;
                    respuesta.Mensaje = "! Cliente ingresado no existe";
                    respuesta.Resultado = 0;
                }

                return Json(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Estado = false;
                respuesta.Mensaje = "! Error de Servidor " + ex.Message;
                return Json(respuesta);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(VENTADETALLE detalle)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var producto = db.PRODUCTO.Where(p => p.id == detalle.idProducto).FirstOrDefault();

                    detalle.precio = producto.precioDeVenta;

                    db.VENTADETALLE.Add(detalle);
                    await db.SaveChangesAsync();

                    var json = new
                    {
                        idDetalle = detalle.id,
                        id = producto.id,
                        nombre = producto.nombre,
                        cantida = detalle.cantidad,
                        precio = detalle.precio,
                        subTotal = detalle.subTotal
                    };

                    respuesta.Estado = true;
                    respuesta.Mensaje = "! transaccion Exitosa";
                    respuesta.Resultado = json;
                    return Json(respuesta);
                }
                catch (Exception ex)
                {
                    respuesta.Estado = false;
                    respuesta.Mensaje = "! Error de Servidor " + ex.Message;
                    return Json(respuesta);
                }
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));

                respuesta.Estado = false;
                respuesta.Mensaje = "! Validar " + messages;

                return Json(respuesta);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            VENTADETALLE detalle = await db.VENTADETALLE.FindAsync(id);
            db.VENTADETALLE.Remove(detalle);
            await db.SaveChangesAsync();

            respuesta.Estado = true;
            respuesta.Mensaje = "! transaccion Exitosa";
            respuesta.Resultado = 1;
            return Json(respuesta);
        }
    }
}