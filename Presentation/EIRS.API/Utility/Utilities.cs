//using EIRS.Models;
using EIRS.API.Controllers;
using EIRS.API.Models;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace EIRS.API.Utility
{
    public static class Utilities
    {
        //private static string baseAddress = "https://api.eirs.gov.ng/";
        //private static string baseAddress = "http://localhost:56892/";
        //private static string baseAddress = "http://localhost:6500/";
        private static string baseAddress = ConfigurationManager.AppSettings["TokenApiBaseUrl"];

        private static string conString = ConfigurationManager.ConnectionStrings["ERASEntitiesLife"].ToString();


        public static TokenResponse GetAccessToken(string username, string password)
        {
            TokenResponse token = new TokenResponse();
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);

            var requestBody = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"username", username},
                {"password", password},
                //{"status", status.ToString()},
            };

            var tokenResponse = client.PostAsync(baseAddress + "token", new FormUrlEncodedContent(requestBody)).Result;

            if (tokenResponse.IsSuccessStatusCode)
            {
                var JsonContent = tokenResponse.Content.ReadAsStringAsync().Result;
                token = JsonConvert.DeserializeObject<TokenResponse>(JsonContent);
                token.Error = null;
            }
            else
            {
                token.Error = "Not able to generate Access Token Invalid usrename or password";
            }
            return token;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static int GetUserId(string token)
        {
            string query = $"Select UserID from MST_UserToken where Token ='{token}'";
            SqlConnection sqlConnection = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
           var result  = cmd.ExecuteScalar();
            sqlConnection.Close();
            int userId = Convert.ToInt32(result);
            
            return userId;
        }

    }

}

