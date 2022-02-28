using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Xml;
using System.IO;
using System.IO.Compression;


namespace NotCo_WDC.Controllers
{
    public class HomeController : Controller
    {

        // LOGIN //
        // global int
        public static string anoSeleccionado;

        [HttpPost]
        public string SetAnio(string num)
        {
            anoSeleccionado = num;
            return num;
        }
        //Get Año

        [HttpPost]
        public string GetAnio(string AnoSeleccionado)
        {
            return anoSeleccionado;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public string ConsultaLogin(string usuario, string password)
        {
            string respuesta = APIController.Login(usuario, password);
            if (respuesta == "Error: 401 - Unauthorized")
            {
                respuesta = "ERROR";
            }
            return usuario+";"+password;
        }


        // INDEX //

        public ActionResult Index(string id)
        {
            var asd = APIController.PruebaTeamwork();
            string[] datosUsuario = Base64Decode(id).Split(';');
            string usuario = datosUsuario[0];
            string password = datosUsuario[1];
            ViewData["usuario"] = usuario;
            ViewData["password"] = password;
            return View();
        }


        [HttpPost]
        public string ConsultaApi(string usuario, string password)
        {
            string respuesta = APIController.NotCo(usuario, password);
            return respuesta;
        }


        // WDC //

        public ActionResult SalesActual()
        {
            ViewData["usuario"] = "notapi";
            ViewData["password"] = "notc.5405";
            return View();
        }
        public ActionResult InventoryActual()
        {
            ViewData["usuario"] = "notapi";
            ViewData["password"] = "notc.5405";
            return View();
        }

        public ActionResult SalesPorAnio()
        {
            ViewData["usuario"] = "notapi";
            ViewData["password"] = "notc.5405";
            return View();
        }
        public ActionResult InventoryPorAnio()
        {
            ViewData["usuario"] = "notapi";
            ViewData["password"] = "notc.5405";
            return View();
        }


        //CONSULTAS//

        [HttpPost]
        public string ConsultaApiDemo(string usuario, string password, string fechaanterior, string fechacomodin)
        {

            string respuesta = APIController.NotCoDemo(usuario, password, fechaanterior, fechacomodin);
            return respuesta;
        }
        public string ConsultaApiSalesPorAnio(string usuario, string password, string fechaanterior, string fechacomodin, string AnoSeleccionado)
        {

            string respuesta = APIController.NotCoSalesPorAno(usuario, password, fechaanterior, fechacomodin, AnoSeleccionado);
            return respuesta;
        }
        public string ConsultaApiIventory(string usuario, string password, string fechaanterior, string fechacomodin)
        {

            string respuesta = APIController.NotCoIventory(usuario, password, fechaanterior, fechacomodin);
            return respuesta;
        }

        public string ConsultaApiIventoryPorAnio(string usuario, string password, string fechaanterior, string fechacomodin, string AnoSeleccionado)
        {

            string respuesta = APIController.NotCoIventoryPorAno(usuario, password, fechaanterior, fechacomodin, AnoSeleccionado);
            return respuesta;
        }



        //Funciones//



        //Decodificar Base 64
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


        [HttpPost]
        //Controlador fechas
        public string ControlFechasHoyYTresAnos(string fechaHoy, string fechaTresAnos)
        {
            fechaHoy = DateTime.Now.ToString("dd/MM/yyyy");
            var result = DateTime.Parse(fechaHoy).Year;
            fechaTresAnos = "01/01/" + result;
            //fechaTresAnos = (DateTime.Now.AddMonths(-12)).ToString("dd/MM/yyyy");
            return fechaHoy+";"+fechaTresAnos;
        }

        [HttpPost]
        //Controlador fechas
        public string ControlFechasPorAno(string AnoSeleccionado)
        {
            string primerDia = "01/01/"+ AnoSeleccionado;
            string ultimoDia = "31/12/" + AnoSeleccionado;
            return primerDia + ";" + ultimoDia;
        }

        public string Sumar7Dias(string tresAnios)
        {
            tresAnios = Convert.ToDateTime(tresAnios).AddDays(7).ToString("dd/MM/yyyy");
            return tresAnios;
        }
        public string Sumar7Dias2(string primerDia)
        {
            primerDia = Convert.ToDateTime(primerDia).AddDays(7).ToString("dd/MM/yyyy");
            return primerDia;
        }

        public string CambiarFormato(string fechaanterior, string fechacomodin)
        {
            fechaanterior = Convert.ToDateTime(fechaanterior).ToString("yyyy/MM/dd");
            fechacomodin = Convert.ToDateTime(fechacomodin).ToString("yyyy/MM/dd");
            return fechaanterior+";"+fechacomodin;
        }

        public string SumarUnDia(string fechacomodin)
        {
            fechacomodin = Convert.ToDateTime(fechacomodin).AddDays(1).ToString("dd/MM/yyyy");
            return fechacomodin;
        }

    }
}