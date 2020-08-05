using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BlogCore.Models.ViewModels
{
    public class ArticuloVM
    {
        public Articulo Articulo { get; set; }

        public IEnumerable<SelectListItem> ListaCategorias { get; set; }

    }
}
