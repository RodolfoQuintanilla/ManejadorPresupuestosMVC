
using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IServicioUsuarios servicioUsuarios;

        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IServicioUsuarios servicioUsuarios)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.servicioUsuarios = servicioUsuarios;
        }


        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.OptenerUsuarioId();
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
            return View(tiposCuentas);
        }


        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tiposCuenta)
        {
            //Evita que que exista el tipo cuenta con el mismo nombre
            if (!ModelState.IsValid)
            {
                return View(tiposCuenta);
            }

            //Se modificara
            tiposCuenta.UsuarioId =  servicioUsuarios.OptenerUsuarioId();

            var yaExisteTipoCuenta =
            await repositorioTiposCuentas.Existe(tiposCuenta.Nombre, tiposCuenta.UsuarioId);

            if (yaExisteTipoCuenta)
            {
                ModelState.AddModelError(nameof(tiposCuenta.Nombre),
                $"El nombre {tiposCuenta.Nombre} ya existe.");

                return View(tiposCuenta);
            }

            await repositorioTiposCuentas.Crear(tiposCuenta);

            return RedirectToAction("Index");
        }



        // Verificar si existe el tipo de cuenta 
        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            var usuarioId = 1;
            var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(nombre, usuarioId);

            if (yaExisteTipoCuenta)
            {
                return Json($"El nombre {nombre} ya existe.");
            }
            return Json(true);
        }
    }
}
