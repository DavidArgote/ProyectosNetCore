using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace BlogCore.AccesoDatos.Data
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {

        private readonly ApplicationDbContext _db;

        public CategoriaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetListCategoria()
        {
            return _db.Categoria.Select(i => new SelectListItem()
            {
                Text = i.Nombre,
                Value = i.Id.ToString()
            });
        }

        public void Update(Categoria categoria)
        {
            var objetoDesdeDb = _db.Categoria.FirstOrDefault(s => s.Id == categoria.Id);
            objetoDesdeDb.Nombre = categoria.Nombre;
            objetoDesdeDb.Id = categoria.Id;

            _db.SaveChanges();

        }
    }
}
