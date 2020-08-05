using ApiPeliculas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IUsuarioRepository
    {

        ICollection<Usuario> GetUsuarios();

        Usuario GetUsuario(int usuarioId);

        bool ExisteUsuario(string usuario);

        Usuario RegistroUsuario(Usuario usuario, string password);

        Usuario LoginUsuario(string usuario, string password);

        bool Guardar();

    }
}
