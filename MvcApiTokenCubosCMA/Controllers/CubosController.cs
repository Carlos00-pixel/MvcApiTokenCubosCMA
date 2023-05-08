using Microsoft.AspNetCore.Mvc;
using MvcApiTokenCubosCMA.Filters;
using MvcApiTokenCubosCMA.Models;
using MvcApiTokenCubosCMA.Services;

namespace MvcApiTokenCubosCMA.Controllers
{
    public class CubosController : Controller
    {
        private ServiceApiCubos service;

        public CubosController(ServiceApiCubos service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<Cubo> cubos =
                await this.service.GetCubosAsync();
            return View(cubos);
        }

        public async Task<IActionResult> CubosPorMarca(string marca)
        {
            if(marca == null)
            {
                ViewData["MENSAJE"] = "Inserte Marca";
                return View();
            }
            else
            {
                List<Cubo> cubo =
                    await this.service.FindCuboAsync(marca);
                return View(cubo);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cubo cubo)
        {
            await this.service.InsertCuboAsync
                (cubo.Nombre, cubo.Marca, cubo.Imagen, cubo.Precio);
            return RedirectToAction("Index");
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(Usuario user)
        {
            await this.service.CrearUsuarioAsync
                (user.Nombre, user.Email, user.Pass, user.Imagen);
            return RedirectToAction("Index", "Home");
        }

        [AuthorizeCubos]
        public async Task<IActionResult> PerfilUsuario()
        {
            string token =
            HttpContext.Session.GetString("TOKEN");
            if (token == null)
            {
                ViewData["MENSAJE"] = "Debe realizar Login para visualizar datos";
                return View();
            }
            else
            {
                Usuario user =
                    await this.service.FindUsuarioAsync(token);
                return View(user);
            }
        }

        [AuthorizeCubos]
        public async Task<IActionResult> PedidosUsuario()
        {
            string token =
            HttpContext.Session.GetString("TOKEN");
            if (token == null)
            {
                ViewData["MENSAJE"] = "Debe realizar Login para visualizar datos";
                return View();
            }
            else
            {
                List<Pedido> pedidos =
                    await this.service.FindPedidosAsync(token);
                return View(pedidos);
            }
        }
    }
}
