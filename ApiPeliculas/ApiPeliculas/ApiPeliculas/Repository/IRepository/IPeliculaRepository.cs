using ApiPeliculas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IPeliculaRepository
    {

        ICollection<Pelicula> GetPeliculas();

        ICollection<Pelicula> GetPeliculasInCategoria(int catId);

        Pelicula GetPelicula(int peliculaId);

        bool ExistePelicula(string nombre);

        IEnumerable<Pelicula> BuscarPelicula(string nombre);

        bool ExistePelicula(int id);

        bool CrearPelicula(Pelicula pelicula);

        bool ActualizarPelicula(Pelicula pelicula);

        bool BorrarPelicula(Pelicula pelicula);

        bool Guardar();

    }
}
