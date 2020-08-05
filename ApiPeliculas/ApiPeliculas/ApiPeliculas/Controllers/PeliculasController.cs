using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Authorize]
    [Route("api/Peliculas")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiPeliculas")]
    public class PeliculasController : Controller
    {

        private readonly IPeliculaRepository _peRepo;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _maper;

        public PeliculasController(IPeliculaRepository peRepo, IMapper maper, IWebHostEnvironment hostEnvironment)
        {
            _peRepo = peRepo;
            _maper = maper;
            _hostEnvironment = hostEnvironment;
        }
        
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _peRepo.GetPeliculas();
            var listaPeliculasDto = new List<PeliculaDto>();

            foreach (var lista in listaPeliculas)
            {
                listaPeliculasDto.Add(_maper.Map<PeliculaDto>(lista));
            }

            return Ok(listaPeliculasDto);
        }

        [AllowAnonymous]
        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        public IActionResult GetPelicula(int peliculaId)
        {
            var itemPelicula = _peRepo.GetPelicula(peliculaId);

            if(itemPelicula == null)
            {
                return NotFound();
            }

            var itemCatergotiaDto = _maper.Map<PeliculaDto>(itemPelicula);

            return Ok(itemCatergotiaDto);

        }

        [AllowAnonymous]
        [HttpGet("GetPeliculasEnCategora/{categoriaId:int}")]
        public IActionResult GetPeliculasEnCategora(int categoriaId)
        {
            var listaPeliculas = _peRepo.GetPeliculasInCategoria(categoriaId);

            if (listaPeliculas == null)
            {
                return NotFound();
            }

            var itemPelicula = new List<PeliculaDto>();

            foreach (var item in listaPeliculas)
            {
                itemPelicula.Add(_maper.Map<PeliculaDto>(item));
            }

            return Ok(itemPelicula);

        }

        [AllowAnonymous]
        [HttpGet("buscar")]
        public  IActionResult Buscar(string nombre)
        {
            try
            {
                var resultado = _peRepo.BuscarPelicula(nombre);

                if (resultado.Any())
                {
                    return Ok(resultado);
                }

                return NotFound();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando los datos de la aplicacion");
            }
        }


        [HttpPost]
        public IActionResult CrearPelicula([FromForm] PeliculaCreateDto peliculaDto)
        {
            if (peliculaDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_peRepo.ExistePelicula(peliculaDto.Nombre))
            {
                ModelState.AddModelError("message", "La pelicula ya existe");
                return StatusCode(404, ModelState);
            }

            //Subida de archivos
            var archivo = peliculaDto.Foto;
            string rutaPrincipal = _hostEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            if (archivo.Length > 0)
            {
                var nombreFoto = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPrincipal, @"fotos");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(subidas, nombreFoto + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStream);
                }

                peliculaDto.RutaImagen = @"\fotos\" + nombreFoto + extension;

            }

            var pelicula = _maper.Map<Pelicula>(peliculaDto);

            if(!_peRepo.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("message", $"Algo salio mal guardado el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id}, pelicula);

        }

        [HttpPatch("{peliculaId:int}", Name = "ActualizarPelicula")]
        public IActionResult ActualizarPelicula(int peliculaId, [FromBody]PeliculaDto peliculaDto)
        {

            if(peliculaDto == null ||  peliculaId !=  peliculaDto.Id)
            {
                return BadRequest(ModelState);
            }

            var pelicula = _maper.Map<Pelicula>(peliculaDto);

            if (!_peRepo.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("message", $"Algo salio mal actualizando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        public IActionResult BorrarPelicula(int peliculaId)
        {

            if (!_peRepo.ExistePelicula(peliculaId))
            {
                return NotFound();
            }

            var pelicula = _peRepo.GetPelicula(peliculaId);

            if (!_peRepo.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("message", $"Algo salio mal borrando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


    }
}
