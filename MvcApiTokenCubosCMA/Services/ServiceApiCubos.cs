using MvcApiTokenCubosCMA.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace MvcApiTokenCubosCMA.Services
{
    public class ServiceApiCubos
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApiCubos;

        public ServiceApiCubos(IConfiguration configuration)
        {
            this.UrlApiCubos =
                configuration.GetValue<string>("ApiUrls:ApiOAuthCubosCMA");
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<string> GetTokenAsync
            (string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/auth/login";
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel
                {
                    Email = email,
                    Password = password
                };
                string jsonModel = JsonConvert.SerializeObject(model);
                StringContent content =
                    new StringContent(jsonModel, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data =
                        await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(data);
                    string token =
                        jsonObject.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        private async Task<T> CallApiAsync<T>
            (string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        //METODOS PROTEGIDOS
        public async Task<Usuario> FindUsuarioAsync(string token)
        {
            string request = "/api/usuarios/";
            Usuario user =
                await this.CallApiAsync<Usuario>(request, token);
            return user;
        }

        public async Task<List<Pedido>> FindPedidosAsync(string token)
        {
            string request = "/api/pedidos/";
            List<Pedido> user =
                await this.CallApiAsync<List<Pedido>>(request, token);
            return user;
        }

        public async Task<Pedido> RealizarPedidoAsync()
        {
            string request = "/api/pedidos/RealizarPedido";
            Pedido ped =
              await this.CallApiAsync<Pedido>(request);
            return ped;
        }

        //METODOS LIBRES
        public async Task<List<Cubo>> GetCubosAsync()
        {
            string request = "/api/cubos/";
            List<Cubo> cubos =
                await this.CallApiAsync<List<Cubo>>(request);
            return cubos;
        }

        public async Task<List<Cubo>> FindCuboAsync(string marca)
        {
            string request = "/api/cubos/" + marca;
            List<Cubo> cubos =
                await this.CallApiAsync<List<Cubo>>(request);
            return cubos;
        }

        public async Task InsertCuboAsync
            (string nombre, string marca, string imagen, int precio)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/cubos/InsertarCubo";
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Cubo cubo = new Cubo();
                cubo.Nombre = nombre;
                cubo.Marca = marca;
                cubo.Imagen = imagen;
                cubo.Precio = precio;

                string json = JsonConvert.SerializeObject(cubo);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }

        public async Task CrearUsuarioAsync
            (string nombre, string email, string pass, string imagen)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/usuarios/CrearUsuario";
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Usuario user = new Usuario();
                user.Nombre = nombre;
                user.Email = email;
                user.Pass = pass;
                user.Imagen = imagen;

                string json = JsonConvert.SerializeObject(user);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }
    }
}
