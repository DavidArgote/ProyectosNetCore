using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApiPeliculas.Controllers
{
    [Authorize]
    [Route("api/Usuarios")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiPeliculasUsuarios")]
    public class UsuariosController : Controller
    {

        private readonly IUsuarioRepository _userRepo;
        private readonly IMapper _maper;
        private readonly IConfiguration _config;

        public UsuariosController(IUsuarioRepository userRepo, IMapper maper, IConfiguration config)
        {
            _userRepo = userRepo;
            _maper = maper;
            _config = config;
        }
        
        [HttpGet]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _userRepo.GetUsuarios();
            var listaUsuariosDto = new List<UsuarioDto>();

            foreach (var lista in listaUsuarios)
            {
                listaUsuariosDto.Add(_maper.Map<UsuarioDto>(lista));
            }

            return Ok(listaUsuariosDto);
        }

        [HttpGet("{usuarioId:int}", Name = "GetUsuario")]
        public IActionResult GetUsuario(int usuarioId)
        {
            var itemUsuario = _userRepo.GetUsuario(usuarioId);

            if(itemUsuario == null)
            {
                return NotFound();
            }

            var itemUsuarioDto = _maper.Map<UsuarioDto>(itemUsuario);
            return Ok(itemUsuarioDto);

        }

        [AllowAnonymous]
        [HttpPost("Registro")]
        public IActionResult Registro(UsuarioAuthDto usuarioAuthDto)
        {
            usuarioAuthDto.Usuario = usuarioAuthDto.Usuario.ToLower();

            if (_userRepo.ExisteUsuario(usuarioAuthDto.Usuario))
            {
                return BadRequest("El usuario ya existe!");
            }

            var usuarioACrear = new Usuario
            {
                UsuarioA = usuarioAuthDto.Usuario
            };

            var usuarioCreado = _userRepo.RegistroUsuario(usuarioACrear, usuarioAuthDto.Password);
            return Ok(usuarioCreado);

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UsuarioAuthLoginDto usuarioAuthLogin)
        {
           
            var usuarioDesdeRepo = _userRepo.LoginUsuario(usuarioAuthLogin.Usuario, usuarioAuthLogin.Password);

            if (usuarioDesdeRepo == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioDesdeRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, usuarioDesdeRepo.UsuarioA.ToString())
            };

            //Generacion del token 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value)); // add AppSettings:Token a appsettings.json
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1), // Dias de valides del token
                SigningCredentials = credenciales
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });

        }

    }
}
