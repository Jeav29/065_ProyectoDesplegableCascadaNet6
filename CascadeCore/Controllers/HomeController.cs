using CascadeCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace CascadeCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _cadenasql;

        public HomeController(IConfiguration config)
        {
            _cadenasql = config.GetConnectionString("cadenaSQL");
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult obtenerUbigeo(string obtener, string b_departamento, string b_provincia) {

            List<Ubigeo> lista = new List<Ubigeo>();

            using (var cn = new SqlConnection(_cadenasql)) { 
                cn.Open();
                var cmd = new SqlCommand("sp_obtener_ubigeo", cn);
                cmd.Parameters.AddWithValue("obtener",obtener);
                cmd.Parameters.AddWithValue("b_departamento",b_departamento is null ? "" : b_departamento);
                cmd.Parameters.AddWithValue("b_provincia",b_provincia is null ? "" : b_provincia);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader()) {
                    while (dr.Read()) {
                        lista.Add(new Ubigeo { 
                            IdUbigeo = dr["IdUbigeo"].ToString(),
                            Departamento = dr["Departamento"].ToString(),
                            Provincia = dr["Provincia"].ToString(),
                            Distrito = dr["Distrito"].ToString(),
                        });
                    }
                }
            }

            return Json(lista);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}