using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace PokedexAPI.Extensions
{
    public static class PokeapiRedirectionExtensions
    {
        static HttpClient client = new HttpClient();
        static Regex urlMatch = new Regex(@$"(?<Host>{LOCAL_POKEAPI})/(?<Replacing>{REPLACING_URL})/(?<Route>[\w/?=&-]+)");
        const string LOCAL_POKEAPI = "http://127.0.0.1:8000";
        const string REPLACING_URL = "api/v2";
        const string OWN_URL = "pokeapi";

        public static IApplicationBuilder UsePokeapiRedirection(this IApplicationBuilder app)
        {
            app.Use(next => async context =>
            {
                string[] path = context.Request.Path.ToString().Split('/');
                if (path[1] == OWN_URL)
                {
                    path[1] = REPLACING_URL;
                    string url = LOCAL_POKEAPI + String.Join('/', path);
                    NameValueCollection query = HttpUtility.ParseQueryString(context.Request.QueryString.ToString());
                    if (query.Get("limit") == "-1")
                    {
                        var innerresp = await client.GetAsync(url);
                        string innerbody = await innerresp.Content.ReadAsStringAsync();
                        PokeapiGetAll innerbodyobj = JsonConvert.DeserializeObject<PokeapiGetAll>(innerbody);
                        int count = innerbodyobj.Count;

                        var newquerystring = HttpUtility.ParseQueryString(context.Request.QueryString.ToString());
                        newquerystring.Remove("limit");
                        newquerystring.Add("limit", count.ToString());
                        query = newquerystring;
                    }
                    if (query.HasKeys())
                        url = url.TrimEnd('/') + "/?" + query;

                    var response = await client.GetAsync(url);
                    string body = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()));
                    string host = context.Request.Host.ToString();
                    //context.Request.Protocol.Split('/')[0].ToLower() + "://" +
                    body = urlMatch.Replace(body, (m) =>
                    {
                        string replacing = m.Groups["Replacing"].Value;
                        string pokeapihost = m.Groups["Host"].Value;
                        string route = m.Groups["Route"].Value;
                        return "https://" + host + "/" + OWN_URL + "/" + route;
                    });
                    //string returningBody = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(body), Formatting.Indented);

                    context.Response.StatusCode = (int)response.StatusCode;
                    context.Response.Headers.Clear();
                    context.Response.Headers.Add("Content-Type", "application/json");
                    await context.Response.WriteAsync(body, Encoding.UTF8);
                    return;
                }

                await next(context);
            });

            return app;
        }
    }
    class PokeapiGetAll
    {
        public int Count { get; set; }
    }

}

