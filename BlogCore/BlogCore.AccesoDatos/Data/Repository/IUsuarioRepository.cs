using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public interface IUsuarioRepository : IRepository<ApplicationUser>
    {

        void BloquearUsuario(string IdUsuario);

        void DesbloquearUsuario(string IdUsuario);
    }
}
