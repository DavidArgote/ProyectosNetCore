using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _db;

        public UsuarioRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ExisteUsuario(string usuario)
        {
            if (_db.Usuario.Any(x => x.UsuarioA == usuario))
            {
                return true;
            }
            return false;
        }

        public Usuario GetUsuario(int usuarioId)
        {
            return _db.Usuario.FirstOrDefault(u => u.Id == usuarioId);
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _db.Usuario.OrderBy(u => u.UsuarioA).ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public Usuario LoginUsuario(string usuario, string password)
        {
            var user = _db.Usuario.FirstOrDefault(x => x.UsuarioA == usuario);

            if (user == null)
            {
                return null;
            }

            if (!VerificaPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;

        }

        public Usuario RegistroUsuario(Usuario usuario, string password)
        {
            byte[] passwordHash, passwordSalt;

            CrearUsuarioHash(password, out passwordHash, out passwordSalt);

            usuario.PasswordHash = passwordHash;
            usuario.PasswordSalt = passwordSalt;

            _db.Usuario.Add(usuario);
            Guardar();
            return usuario;
        }


        private bool VerificaPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var hashComputado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i =0; i < hashComputado.Length; i++)
                {
                    if (hashComputado[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        private void CrearUsuarioHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
