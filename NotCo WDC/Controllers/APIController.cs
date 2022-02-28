using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Diagnostics;
//using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace NotCo_WDC.Controllers
{
    public static class APIController 
    {
        public static string Login(string usuario, string password)
        {
            try
            {
                string[] headers = new string[] { usuario, password };
                string parametros = "";
                string url = "https://thenot.bambub2b.com/api/sales.php?date_init=2021-01-01&date_end=2021-01-02";
                string xml = string.Empty;
                string metodo = "GET";
                string respuestaAPI = LlamarAPIGet(metodo, url, parametros, xml, headers);
                //JObject json = JObject.Parse(respuestaAPI);
                return respuestaAPI;
            }
            catch (Exception ex)
            {
                return ex.Message + "/ ERROR";
            }

        }




        public static string NotCo(string usuario, string password)
        {
            try
            {
                //Fecha límite: 2016 - hoy
                //Cómo 
                string[] headers = new string[] { usuario, password };
                string parametros = "";
                string url = "https://thenot.bambub2b.com/api/sales.php?date_init=2021-01-01&date_end=2021-01-02";
                string xml = string.Empty;
                string metodo = "GET";
                string respuestaAPI = LlamarAPIGet(metodo, url, parametros, xml, headers);
                //JObject json = JObject.Parse(respuestaAPI);
                return respuestaAPI;
            }
            catch (Exception ex)
            {
                return ex.Message + "/ ERROR";
            }

        }

        public static string NotCoDemo(string usuario, string password, string fechaanterior, string fechadespues)
        {
            try
            {
                //Fecha límite: 2016 - hoy
                DateTime date = DateTime.Now;
                DateTime oDate = Convert.ToDateTime(fechadespues);
                if (oDate > date) {
                    //string nuevaFecha = date.AddDays(-1).ToString("yyyy/MM/dd");
                    string nuevaFecha = date.ToString("yyyy/MM/dd");
                    fechadespues = nuevaFecha;
                }
                string[] headers = new string[] { usuario, password };
                string parametros = "";
                //string url = "https://thenot.bambub2b.com/api/sales.php?date_init=2021-01-01&date_end=2021-01-02";
                string url = "https://thenot.bambub2b.com/api/sales.php?date_init="+ fechaanterior + "& date_end=" + fechadespues;
                string xml = string.Empty;
                string metodo = "GET";
                string respuestaAPI = LlamarAPIGet(metodo, url, parametros, xml, headers);
                //JObject json = JObject.Parse(respuestaAPI);
                return respuestaAPI;
            }
            catch (Exception ex)
            {
                return ex.Message + "/ ERROR";
            }

        }
        public static string NotCoSalesPorAno(string usuario, string password, string fechaanterior, string fechadespues, string AnoSeleccionado)
        {
            try
            {

                DateTime date = Convert.ToDateTime(fechadespues);
                DateTime oDate = Convert.ToDateTime("31/12/" + AnoSeleccionado);
                if (date >= oDate)
                {
                    string nuevaFecha = oDate.ToString("yyyy/MM/dd");
                    fechadespues = nuevaFecha;
                }
                string[] headers = new string[] { usuario, password };
                string parametros = "";
                string url = "https://thenot.bambub2b.com/api/sales.php?date_init=" + fechaanterior + "& date_end=" + fechadespues;
                string xml = string.Empty;
                string metodo = "GET";
                string respuestaAPI = LlamarAPIGet(metodo, url, parametros, xml, headers);
                //JObject json = JObject.Parse(respuestaAPI);
                return respuestaAPI;
            }
            catch (Exception ex)
            {
                return ex.Message + "/ ERROR";
            }

        }
        public static string NotCoIventory(string usuario, string password, string fechaanterior, string fechadespues)
        {
            try
            {
                //Fecha límite: 2016 - hoy
                DateTime date = DateTime.Now;
                DateTime oDate = Convert.ToDateTime(fechadespues);
                if (oDate >= date)
                {
                    string nuevaFecha = date.AddDays(-1).ToString("yyyy/MM/dd");
                    fechadespues = nuevaFecha;
                }
                string[] headers = new string[] { usuario, password };
                string parametros = "";
                string url = "https://thenot.bambub2b.com/api/inventory.php?date_init=" + fechaanterior + "& date_end=" + fechadespues;
                string xml = string.Empty;
                string metodo = "GET";
                string respuestaAPI = LlamarAPIGet(metodo, url, parametros, xml, headers);
                //JObject json = JObject.Parse(respuestaAPI);
                return respuestaAPI;
            }
            catch (Exception ex)
            {
                return ex.Message + "/ ERROR";
            }

        }

        public static string NotCoIventoryPorAno(string usuario, string password, string fechaanterior, string fechadespues, string AnoSeleccionado)
        {
            try
            {

                DateTime date = Convert.ToDateTime(fechadespues);
                DateTime oDate = Convert.ToDateTime("31/12/"+ AnoSeleccionado);
                if (date >= oDate)
                {
                    string nuevaFecha = oDate.ToString("yyyy/MM/dd");
                    fechadespues = nuevaFecha;
                }
                string[] headers = new string[] { usuario, password };
                string parametros = "";
                string url = "https://thenot.bambub2b.com/api/inventory.php?date_init=" + fechaanterior + "& date_end=" + fechadespues;
                string xml = string.Empty;
                string metodo = "GET";
                string respuestaAPI = LlamarAPIGet(metodo, url, parametros, xml, headers);
                //JObject json = JObject.Parse(respuestaAPI);
                return respuestaAPI;
            }
            catch (Exception ex)
            {
                return ex.Message + "/ ERROR";
            }

        }

        public static string PruebaTeamwork()
        {
            try
            {
                string parametros = "";
                string url = "https://backspace.teamwork.com/tasks.json?page=1";
                string xml = string.Empty;
                string metodo = "GET";
                string respuestaAPI = LlamarAPIGetPrueba(metodo, url, parametros, xml);
                //JObject json = JObject.Parse(respuestaAPI);
                return respuestaAPI;
            }
            catch (Exception ex)
            {
                return ex.Message + "/ ERROR";
            }

        }

        public static string LlamarAPIGetPrueba(string metodo, string url, string parametros, string xml)
        {
            string respuesta = string.Empty;
            try
            {
                // Parámetros de prueba //
                string urlConsulta = url + parametros;

                // Establecer protocolo de seguridad
                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                // Link del servidor y la función
                Uri uri = new Uri(url);

                // Cuerpo del XML
                StringContent content = new StringContent(xml);

                // Crear cliente
                HttpClient client = new HttpClient();

                //Agregar headers
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Hacer llamada a la API
                HttpResponseMessage response = new HttpResponseMessage();

                if (metodo.ToUpper() == "POST")
                    response = client.PostAsync(uri, content).Result;
                else if (metodo.ToUpper() == "GET")
                    response = client.GetAsync(uri).Result;
                else
                    throw new Exception("Error: Método inválido. Solo se acepta GET y POST");

                // Si es correcta, lee todo como string
                if (response.IsSuccessStatusCode)
                {
                    respuesta = response.Content.ReadAsStringAsync().Result;
                }
                // Si no es correcta, muestra el mensaje de error
                else
                {
                    respuesta = "Error: " + (int)response.StatusCode + " - " + response.ReasonPhrase;
                }

                // Cerrar conexión
                client.Dispose();
            }
            catch (Exception ex)
            {
                respuesta = "Error: " + ex.Message;

            }
            return (respuesta);
        }


        public static string LlamarAPIGet(string metodo, string url, string parametros, string xml, string[] headers)
        {
            string respuesta = string.Empty;
            try
            {
                // Parámetros de prueba //
                string urlConsulta = url + parametros;

                // Establecer protocolo de seguridad
                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                // Link del servidor y la función
                Uri uri = new Uri(url);

                // Cuerpo del XML
                StringContent content = new StringContent(xml);

                // Crear cliente
                HttpClient client = new HttpClient();
                var usuario = headers[0];
                var password = headers[1];
                //Agregar headers
                client.DefaultRequestHeaders.Add("user", usuario);
                client.DefaultRequestHeaders.Add("password", password);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Hacer llamada a la API
                HttpResponseMessage response = new HttpResponseMessage();

                if (metodo.ToUpper() == "POST")
                    response = client.PostAsync(uri, content).Result;
                else if (metodo.ToUpper() == "GET")
                    response = client.GetAsync(uri).Result;
                else
                    throw new Exception("Error: Método inválido. Solo se acepta GET y POST");

                // Si es correcta, lee todo como string
                if (response.IsSuccessStatusCode)
                {
                    respuesta = response.Content.ReadAsStringAsync().Result;
                }
                // Si no es correcta, muestra el mensaje de error
                else
                {
                    respuesta = "Error: " + (int)response.StatusCode + " - " + response.ReasonPhrase;
                }

                // Cerrar conexión
                client.Dispose();
            }
            catch (Exception ex)
            {
                respuesta = "Error: " + ex.Message;
                
            }
            return (respuesta);
        }
    }
}