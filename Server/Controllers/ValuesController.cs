using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        

       
        public class data
        {

            public int Cur_ID { get; set; }
            public DateTime Date { get; set; }
            public double Cur_OfficialRate { get; set; }
        }
        public class data2
        {
            public double priceUsd { get; set; }
            public long time { get; set; }
            public DateTime date { get; set; }
            
        }

        [HttpGet("{value}&{value2}&{value3}")]
        public async Task<string> Get1(string value, string value2, string value3)
        {
            //получение данных по api
            List<data> itemsServer = new List<data>();
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback= (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient(clientHandler))
            {
                string quer = "https://www.nbrb.by/API/ExRates/Rates/Dynamics/" + value + "?startDate=" + value2 + "&endDate=" + value3;
                if (value == "1")
                {
                    if (value2[6] == '-')
                    {
                        value2 = value2[..5] + "0" + value2[5..];
                    }
                    if (value2.Length == 9)
                    {
                        value2 = value2[..8] + "0" + value2[8..];
                    }
                    if (value3[6] == '-')
                    {
                        value3 = value3[..5] + "0" + value3[5..];
                    }
                    if (value3.Length == 9)
                    {
                        value3 = value3[..8] + "0" + value3[8..];
                    }
                   
                    DateTime myDate = DateTime.ParseExact(value2, "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    DateTime myDate2 = DateTime.ParseExact(value3, "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    double utcmillis = (myDate - epoch).TotalMilliseconds;
                    double utcmillis2 = (myDate2 - epoch).TotalMilliseconds;
                    quer = "https://api.coincap.io/v2/assets/bitcoin/history?interval=d1&start="+ utcmillis + "&end="+ utcmillis2;
                    
                   
                }
                var responce = await client.GetAsync(quer);
                responce.EnsureSuccessStatusCode();
                if (responce.IsSuccessStatusCode)
                {
                    string message = await responce.Content.ReadAsStringAsync();
                    string output = JsonConvert.SerializeObject(message);
                    List<data> m = new List<data>();
                    if (value == "1")
                    {
                        
                        int end = message.LastIndexOf("\"timestamp\"") - 1;
                        int start = message.LastIndexOf("\"data\"") + 7;
                        message = message[start..end];
                        
                        List<data2> m2 = JsonConvert.DeserializeObject<List<data2>>(message);
                        for (int i = 0; i < m2.Count; i++)
                        {
                            m.Add(new data { Cur_ID = 1, Cur_OfficialRate = m2[i].priceUsd, Date = m2[i].date });
                        }
                    }
                    else
                    {
                        m = JsonConvert.DeserializeObject<List<data>>(message);
                    }
                   
                    itemsServer = m;
                   
                
                }
                else
                {
                    return "error";
                }
            }
            //проверка файла
            if (!System.IO.File.Exists("qwe.json"))
            {
                System.IO.File.Create("qwe.json").Close();
                System.IO.File.WriteAllText("qwe.json", "[]");
            }
            //получение данных из JSON
            string json;
            List<data> items = new List<data>();
            using (StreamReader r = new StreamReader("qwe.json"))
            {
                string json2 = r.ReadToEnd();
                json = json2;
                items = JsonConvert.DeserializeObject<List<data>>(json2);

            }
            //добавление данных, которых нет в базе, в базу соотвественно и сортировка значений по дате
            foreach (data cl in itemsServer)
            {
                if (!items.Any<data>(x => x.Cur_OfficialRate == cl.Cur_OfficialRate && x.Cur_ID == cl.Cur_ID && x.Date == cl.Date))
                {
                    items.Add(cl);
                }

            }
            items.Sort((a, b) => a.Date.CompareTo(b.Date));
            string jsonData = JsonConvert.SerializeObject(items.ToArray());
            
            
            //запись всех изменений в json базу
            System.IO.File.WriteAllText(@"qwe.json", jsonData);
            //отделение дат, которые не нужны от тех которые нужны и отправка ответа
            DateTime date1 = DateTime.Parse(value2);
            DateTime date2 = DateTime.Parse(value3);
            items.RemoveAll(s => s.Date < date1 || s.Date > date2 || s.Cur_ID != int.Parse(value));
            return JsonConvert.SerializeObject(items.ToArray());
        }

        

    }
}
