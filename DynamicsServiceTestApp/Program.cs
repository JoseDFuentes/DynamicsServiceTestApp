using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AuthenticationUtility;
using Newtonsoft.Json;

namespace DynamicsServiceTestApp
{
    public class APFractalParametroCompaniesContract
    {
        public string codCompania { get; set; }
        public string nomCompania { get; set; }
        public string nomCortoCompania { get; set; }

    }
    public class requestService
    {
        public List<APFractalParametroCompaniesContract> APFractalParametrosCompaniesContract { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string serviceendpoint = "APFractalServiceGroup/APFractalService/insertarParametrosCompanies";

                       
            string endpoint = $"{ClientConfiguration.Default.ActiveDirectoryResource}/api/services/{serviceendpoint}";

            requestService requestService = new requestService(){ APFractalParametrosCompaniesContract = new List<APFractalParametroCompaniesContract>()
            };

            requestService.APFractalParametrosCompaniesContract.Add( new APFractalParametroCompaniesContract()
            {
                codCompania = "GTHQ",
                nomCompania = "High Q Guatemala",
                nomCortoCompania = "High Q"
            });

            string jsonRequest = JsonConvert.SerializeObject(requestService);

            string Toke = OAuthHelper.GetAuthenticationHeader(true);


            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var request = HttpWebRequest.Create(endpoint);
            //Generación del token
            //Revisar la clase ClientConfiguration para establecer los valores de autenticación
            request.Headers[OAuthHelper.OAuthHeader] = Toke;
            
            request.Method = "POST";
            using (var stream = request.GetRequestStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(jsonRequest);
                }
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream))
                    {
                        string responseString = streamReader.ReadToEnd();
                        Console.WriteLine(responseString);
                    }
                }
            }


            Console.ReadLine();

        }
    }
}
