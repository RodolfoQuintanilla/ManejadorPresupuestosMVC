
using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(TipoCuenta tiposCuenta)
        {
            //Validar Datos que ingresa el usuario
            if (!ModelState.IsValid)
            {
                return View(tiposCuenta);
            }

            return View();
        }
    }
}