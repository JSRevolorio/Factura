using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Facturacion.Models;

namespace Facturacion.Controllers
{
    public class ProductoController : Controller
    {
        private TiendaEntities db = new TiendaEntities();

        // GET: Producto
        public async Task<ActionResult> Index()
        {
            var pRODUCTO = db.PRODUCTO.Include(p => p.PROVEEDOR);
            return View(await pRODUCTO.ToListAsync());
        }

        // GET: Producto/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCTO pRODUCTO = await db.PRODUCTO.FindAsync(id);
            if (pRODUCTO == null)
            {
                return HttpNotFound();
            }
            return View(pRODUCTO);
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            ViewBag.idProveedor = new SelectList(db.PROVEEDOR, "id", "nombre");
            return View();
        }

        // POST: Producto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,nombre,descripcion,idProveedor,precioDeCompra,precioDeVenta,cantidad,estado")] PRODUCTO pRODUCTO)
        {
            if (ModelState.IsValid)
            {
                db.PRODUCTO.Add(pRODUCTO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.idProveedor = new SelectList(db.PROVEEDOR, "id", "nombre", pRODUCTO.idProveedor);
            return View(pRODUCTO);
        }

        // GET: Producto/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCTO pRODUCTO = await db.PRODUCTO.FindAsync(id);
            if (pRODUCTO == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProveedor = new SelectList(db.PROVEEDOR, "id", "nombre", pRODUCTO.idProveedor);
            return View(pRODUCTO);
        }

        // POST: Producto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,nombre,descripcion,idProveedor,precioDeCompra,precioDeVenta,cantidad,estado")] PRODUCTO pRODUCTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pRODUCTO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.idProveedor = new SelectList(db.PROVEEDOR, "id", "nombre", pRODUCTO.idProveedor);
            return View(pRODUCTO);
        }

        // GET: Producto/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCTO pRODUCTO = await db.PRODUCTO.FindAsync(id);
            if (pRODUCTO == null)
            {
                return HttpNotFound();
            }
            return View(pRODUCTO);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PRODUCTO pRODUCTO = await db.PRODUCTO.FindAsync(id);
            db.PRODUCTO.Remove(pRODUCTO);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
