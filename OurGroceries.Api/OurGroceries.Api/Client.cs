using System;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using OurGroceries.Api.Entities;
using System.Collections.Generic;

namespace OurGroceries.Api
{
    public class Client : IDisposable
    {
        const string _baseUrl = "https://www.ourgroceries.com";        

        CookieWebClient client;

        public Client()
        {            
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            WebClient = new CookieWebClient();
        }

        ~Client()
        {            
            client = null;
        }

        public CookieWebClient WebClient
        {
            get
            {                
                return client;
            }
            
            private set
            {
                client = value;
            }            
        }

        public string SignIn(string emailAddress, string password)
        {
            WebClient.Headers.Add("Host", "www.ourgroceries.com");            
            WebClient.Headers.Add("Upgrade-Insecure-Requests", "1");
            WebClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");
            WebClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            var postData = new NameValueCollection()
            {
                {"emailAddress", emailAddress },
                {"action", "sign-me-in" },
                {"password", password }
            };

            var url = $"{_baseUrl}/sign-in";

            var byteResult = client.UploadValues(url, postData);

            var resultContent = System.Text.Encoding.UTF8.GetString(byteResult);

            var shoppingTeam = CreateShoppingTeam(resultContent);

            var shoppingTeamJson = JsonConvert.SerializeObject(shoppingTeam);

            return shoppingTeamJson;
        }

        public string GetList(string listId, string teamId)
        {            
            var message = new OurGroceriesMessage();

            message.command = "getList";
            message.listId = listId;
            message.teamId = teamId;
            message.versionId = string.Empty;

            var serializedMessage = JsonConvert.SerializeObject(message);

            WebClient.Headers["Content-Type"] = "application/json; charset=UTF-8";
            WebClient.Headers.Add("Accept", "application/json, text/javascript, */*");

            var response = string.Empty;
            var url = $"{_baseUrl}/your-lists/list/{listId}";

            var result = WebClient.UploadString(url, serializedMessage);            
            
            return result;
        }

        private ShoppingTeam CreateShoppingTeam(string content2parse)
        {
            var shoppingTeam = new ShoppingTeam();

            if (!string.IsNullOrEmpty(content2parse))
            {
                //parse out teamId
                var matchedTeamId = Regex.Match(content2parse, @"var\sg_teamId\s=\s""(?<teamId>[a-zA-Z0-9]+)""");

                if (matchedTeamId.Success)
                {
                    shoppingTeam.teamId = matchedTeamId.Groups["teamId"].Value;
                }

                //parse out ShoppingList
                var matchedShoppingLists = Regex.Matches(content2parse, @"id:\s""(?<id>\w+)"",\sname:\s""(?<name>[\s\\'a-zA-Z0-9]+)""");
                var shoppingList = new List<ShoppingList>();

                if(matchedShoppingLists.Count > 0)
                {
                    foreach(Match matchedShoppingList in matchedShoppingLists)
                    {
                        if(matchedShoppingList.Success)
                        {
                            var id = matchedShoppingList.Groups["id"].Value;
                            var name = matchedShoppingList.Groups["name"].Value;

                            shoppingList.Add(new ShoppingList() { id = id, name = name });
                        }
                    }
                }

                shoppingTeam.shoppingLists = shoppingList;
            }

            return shoppingTeam;
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
