using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly DbappContext _context;

        public ServiciosController(DbappContext context)
        {
            _context = context;
        }

        // GET: Servicios
        public async Task<IActionResult> Index()
        {
            var dbappContext = _context.Servicios.Include(s => s.Cliente);
            return View(await dbappContext.ToListAsync());
        }

        // GET: Servicios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Servicios == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios
                .Include(s => s.Cliente)
                .FirstOrDefaultAsync(m => m.ServicioId == id);
            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }

        // GET: Servicios/Create
        public IActionResult Create()
        {
            ViewData["TiposServicio"] = new SelectList(new List<string> { "Cable", "Internet" });
            ViewData["Velocidades"] = new SelectList(new List<int> { 15, 25, 50 });
            ViewData["TiposCable"] = new SelectList(new List<string> { "Básico", "Premium" });
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nombre");
            return View();
        }

        // POST: Servicios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(servicio);
                //await _context.SaveChangesAsync();
                // Llama al procedimiento almacenado para crear el servicio
                var clienteId = new SqlParameter("@ClienteID", servicio.ClienteId);
                var tipoServicio = new SqlParameter("@TipoServicio", servicio.TipoServicio);
                var velocidad = new SqlParameter("@Velocidad", servicio.Velocidad);
                var tipoCable = new SqlParameter("@TipoCable", servicio.TipoCable);
                var ubicacion = new SqlParameter("@Ubicacion", servicio.Ubicacion);

                await _context.Database.ExecuteSqlRawAsync("EXEC AsignarServiciosACliente @ClienteID, @TipoServicio, @Velocidad, @TipoCable, @Ubicacion",
                    clienteId, tipoServicio, velocidad, tipoCable, ubicacion);

                return RedirectToAction(nameof(Index));
            }
            ViewData["TiposServicio"] = new SelectList(new List<string> { "Cable", "Internet" });
            ViewData["Velocidades"] = new SelectList(new List<int> { 15, 25, 50 });
            ViewData["TiposCable"] = new SelectList(new List<string> { "Básico", "Premium" });
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nombre", servicio.ClienteId);
            return View(servicio);
        }

        // GET: Servicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Servicios == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }
            ViewData["TiposServicio"] = new SelectList(new List<string> { "Cable", "Internet" });
            ViewData["Velocidades"] = new SelectList(new List<int> { 15, 25, 50 });
            ViewData["TiposCable"] = new SelectList(new List<string> { "Básico", "Premium" });
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nombre", servicio.ClienteId);
            return View(servicio);
        }

        // POST: Servicios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Servicio servicio)
        {
            if (id != servicio.ServicioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(servicio);
                    //await _context.SaveChangesAsync();
                    var servicioId = new SqlParameter("@ServicioID", servicio.ServicioId);
                    var clienteId = new SqlParameter("@ClienteID", servicio.ClienteId);
                    var tipoServicio = new SqlParameter("@TipoServicio", servicio.TipoServicio);
                    var velocidad = new SqlParameter("@Velocidad", servicio.Velocidad);
                    var tipoCable = new SqlParameter("@TipoCable", servicio.TipoCable);
                    var ubicacion = new SqlParameter("@Ubicacion", servicio.Ubicacion);
                    await _context.Database.ExecuteSqlRawAsync("EXEC EditarServicio @ServicioID, @ClienteID, @TipoServicio, @Velocidad, @TipoCable, @Ubicacion",
                     servicioId, clienteId, tipoServicio, velocidad, tipoCable, ubicacion);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicioExists(servicio.ServicioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TiposServicio"] = new SelectList(new List<string> { "Cable", "Internet" });
            ViewData["Velocidades"] = new SelectList(new List<int> { 15, 25, 50 });
            ViewData["TiposCable"] = new SelectList(new List<string> { "Básico", "Premium" });
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nombre", servicio.ClienteId);
            return View(servicio);
        }

        // GET: Servicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Servicios == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios
                .Include(s => s.Cliente)
                .FirstOrDefaultAsync(m => m.ServicioId == id);
            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }

        // POST: Servicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Servicios == null)
            {
                return Problem("No existe registro.");
            }
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio != null)
            {
                _context.Servicios.Remove(servicio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServicioExists(int id)
        {
            return (_context.Servicios?.Any(e => e.ServicioId == id)).GetValueOrDefault();
        }
    }
}
