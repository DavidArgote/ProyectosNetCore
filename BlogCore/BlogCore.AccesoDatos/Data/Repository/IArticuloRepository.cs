using BlogCore.Models;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public interface IArticuloRepository : IRepository<Articulo>
    {

        void Update(Articulo articulo);

    }
}
