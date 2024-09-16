using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using RestSharp;

namespace EIRS.Common
{
    public static class APICall
    {
        public static IDictionary<string, object> GetData(string pStrUrl, IDictionary<string, object> pdcParameters = null)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            IRestClient mObjRestClient = new RestClient(GlobalDefaultValues.SEDE_APIURL + pStrUrl);
            //IRestRequest mObjRestRequest = new RestRequest(Method.GET);
           // mObjRestRequest.AddParameter("Content-Type", "application/json", ParameterType.HttpHeader);
           // mObjRestRequest.AddParameter("X-ApiKey", GlobalDefaultValues.SEDE_APIToken, ParameterType.HttpHeader);

            if (pdcParameters != null)
            {
                foreach (var item in pdcParameters)
                {
                    //mObjRestRequest.AddParameter(item.Key, item.Value, ParameterType.QueryString);
                }
            }

            //IRestResponse mObjRestResponse = mObjRestClient.Execute(mObjRestRequest);

           // if (mObjRestResponse.IsSuccessful)
           // {
               // IDictionary<string, object> dcAPIResponse = JsonConvert.DeserializeObject<IDictionary<string, object>>(mObjRestResponse.Content);
                //IDictionary<string, object> dcAPIResponse = new IDictionary<string, object>();
                //if (!dcAPIResponse.ContainsKey("error"))
                //{
                //    dcResponse = dcAPIResponse;
                //    dcResponse["success"] = true;
                //}
                //else
                //{
                //    dcResponse["success"] = false;
                //    dcResponse["Message"] = dcAPIResponse["error"];
                //}
           // }
            //else
           // {
                //dcResponse["success"] = false;
                //dcResponse["Message"] = "Error Occurred in Calling API";
            ///}

            return dcResponse;
        }

        public static IDictionary<string, object> PostData(string pStrUrl, object pObjData, IDictionary<string, string> pdcFileData = null)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            using (var mObjHttpClient = new HttpClient())
            {
                mObjHttpClient.DefaultRequestHeaders.Add("X-ApiKey", GlobalDefaultValues.SEDE_APIToken);
                using (var vPostContent = new MultipartFormDataContent())
                {
                    if (pObjData != null)
                    {
                        StringContent mStrModelContent = new StringContent(JsonConvert.SerializeObject(pObjData));
                        mStrModelContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                        mStrModelContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        vPostContent.Add(mStrModelContent);
                    }

                    if (pdcFileData != null)
                    {
                        foreach (var item in pdcFileData.Keys)
                        {
                            byte[] mArrFileData = File.ReadAllBytes(pdcFileData[item]);
                            var vFileContent = new ByteArrayContent(mArrFileData);
                            vFileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { Name = item, FileName = Path.GetFileNameWithoutExtension(pdcFileData[item]).ToSeoUrl() + Path.GetExtension(pdcFileData[item]) };
                            vPostContent.Add(vFileContent);
                        }
                    }

                    var mObjHttpResponse = mObjHttpClient.PostAsync(GlobalDefaultValues.SEDE_APIURL + pStrUrl, vPostContent).Result;

                    if (mObjHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string strResponseData = mObjHttpResponse.Content.ReadAsStringAsync().Result;
                        IDictionary<string, object> dcAPIResponse = JsonConvert.DeserializeObject<IDictionary<string, object>>(strResponseData);
                        if (!dcAPIResponse.ContainsKey("error"))
                        {
                            dcResponse = dcAPIResponse;
                            dcResponse["success"] = true;
                        }
                        else
                        {
                            dcResponse["success"] = false;
                            dcResponse["Message"] = dcAPIResponse["error"];
                        }
                    }
                    else
                    {
                        dcResponse["success"] = false;
                        dcResponse["Message"] = mObjHttpResponse.StatusCode; //"Error Occurred in Calling API";
                    }
                }
            }

            return dcResponse;
        }
    }
}
