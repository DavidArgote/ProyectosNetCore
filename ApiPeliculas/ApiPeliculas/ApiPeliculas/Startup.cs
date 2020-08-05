using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ApiPeliculas.Data;
using ApiPeliculas.Helpers;
using ApiPeliculas.PeliculasMapper;
using ApiPeliculas.Repository;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ApiPeliculas
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(Options => Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IPeliculaRepository, PeliculaRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            // Agregar dependencia del token
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                });

            services.AddAutoMapper(typeof(PeliculasMappers));

            // Confiuracion de documentacion de nuestra api
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiPeliculasCategorias", new OpenApiInfo()
                {
                    Title = "Api Categorias Peliculas",
                    Version = "1",
                    Description = "Backend peliculas",
                    Contact = new OpenApiContact()
                    {
                        Email = "argote.2075@gmail.com",
                        Name = "David Argote",
                        Url = new Uri("https://pictoo-db.web.app/")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://pictoo-db.web.app/")
                    }
                });

                options.SwaggerDoc("ApiPeliculas", new OpenApiInfo()
                {
                    Title = "Api Peliculas",
                    Version = "1",
                    Description = "Backend peliculas",
                    Contact = new OpenApiContact()
                    {
                        Email = "argote.2075@gmail.com",
                        Name = "David Argote",
                        Url = new Uri("https://pictoo-db.web.app/")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://pictoo-db.web.app/")
                    }
                });

                options.SwaggerDoc("ApiPeliculasUsuarios", new OpenApiInfo()
                {
                    Title = "Api Usuario",
                    Version = "1",
                    Description = "Backend peliculas",
                    Contact = new OpenApiContact()
                    {
                        Email = "argote.2075@gmail.com",
                        Name = "David Argote",
                        Url = new Uri("https://pictoo-db.web.app/")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://pictoo-db.web.app/")
                    }
                });

                var archivoXmlComentarios = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaApiComentarios = Path.Combine(AppContext.BaseDirectory, archivoXmlComentarios);
                options.IncludeXmlComments(rutaApiComentarios);

                // definir esquema de seguridad
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Autenticacion JWT (Bearer)",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, new List<string>()
                }
                });
      
            });

            services.AddControllers();

            //Soporte para CORS
            services.AddCors();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                app.UseExceptionHandler(builder =>
                {
                builder.Run(
                       async context => {
                           context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                           var error = context.Features.Get<IExceptionHandlerFeature>();
                           if (error != null)
                           {
                               context.Response.AddApplicationError(error.Error.Message);
                               await context.Response.WriteAsync(error.Error.Message);
                           }
                       });
                });
            }

            app.UseHttpsRedirection();

            // Linea para documentacion api
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/ApiPeliculasCategorias/swagger.json", "Api Categorias Peliculas");
                options.SwaggerEndpoint("/swagger/ApiPeliculas/swagger.json", "Api Peliculas");
                options.SwaggerEndpoint("/swagger/ApiPeliculasUsuarios/swagger.json", "Api Usuarios Peliculas");
                options.RoutePrefix = "";
            });

            app.UseRouting();

            // Autenticacion y autorizacion
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Soporte para CORS
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        }
    }
}
