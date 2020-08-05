using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogCore.AccesoDatos.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class UsuariosController : Controller
    {

        private readonly IContenedorTrabajo _contendorTrabajo;

        public UsuariosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contendorTrabajo = contenedorTrabajo;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_contendorTrabajo.Usuario.GetAll(u => u.Id != usuarioActual.Value));
        }

        public IActionResult Bloquear(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            _contendorTrabajo.Usuario.BloquearUsuario(id);
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _contendorTrabajo.Usuario.DesbloquearUsuario(id);
            return RedirectToAction(nameof(Index));

        }

    }
}
