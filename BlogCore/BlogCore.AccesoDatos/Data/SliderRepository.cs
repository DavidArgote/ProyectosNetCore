using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.Models;
using System.Linq;

namespace BlogCore.AccesoDatos.Data
{
    public class SliderRepository : Repository<Slider>, ISliderRepository
    {

        private readonly ApplicationDbContext _db;

        public SliderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Slider slider)
        {
            var objetoDesdeDb = _db.Slider.FirstOrDefault(s => s.Id == slider.Id);
            objetoDesdeDb.Nombre = slider.Nombre;
            objetoDesdeDb.Estado = slider.Estado;
            objetoDesdeDb.UrlImagen = slider.UrlImagen;

            _db.SaveChanges();

        }
    }
}
