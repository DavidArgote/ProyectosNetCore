using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.Models;
using System.Linq;

namespace BlogCore.AccesoDatos.Data
{
    public class ArticuloRepository : Repository<Articulo>, IArticuloRepository
    {

        private readonly ApplicationDbContext _db;

        public ArticuloRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Articulo articulo)
        {
            var objetoDesdeDb = _db.Articulos.FirstOrDefault(s => s.Id == articulo.Id);
            objetoDesdeDb.Nombre = articulo.Nombre;
            objetoDesdeDb.Descripcion = articulo.Descripcion;
            objetoDesdeDb.UrlImagen = articulo.UrlImagen;
            objetoDesdeDb.CategoriaId = articulo.CategoriaId;

            //_db.SaveChanges();

        }
    }
}
