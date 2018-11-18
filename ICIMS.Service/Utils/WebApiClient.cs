using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks; 
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ICIMS.Service
{
    public class WebApiClient :  IWebApiClient
    {
        public static TimeSpan DefaultTimeout { get; set; }

        public string BaseUrl { get; set; }

        public TimeSpan Timeout { get; set; }

        public Collection<Cookie> Cookies { get; private set; }

        public ICollection<NameValue> RequestHeaders { get; set; }

        public ICollection<NameValue> ResponseHeaders { get; set; }
        public string TenancyName { get; set; }
        public string UserName { get ; set ; }
        public string Password { get; set; }

        static WebApiClient()
        {
            DefaultTimeout = TimeSpan.FromSeconds(90);
        }

        public WebApiClient()
        {
            Timeout = DefaultTimeout;
            Cookies = new Collection<Cookie>();
            RequestHeaders = new List<NameValue>();
            ResponseHeaders = new List<NameValue>();
            BaseUrl = "http://localhost:21025/";
        }

        public virtual async Task PostAsync(string url, int? timeout = null)
        {
            await PostAsync<object>(url, timeout);
        }

        public virtual async Task PostAsync(string url, object input, int? timeout = null)
        {
            await PostAsync<object>(url, input, timeout);
        }

        public virtual async Task<TResult> PostAsync<TResult>(string url, int? timeout = null)
            where TResult : class
        {
            return await PostAsync<TResult>(url, null, timeout);
        }

        public virtual async Task<TResult> PostAsync<TResult>(string url, object input, int? timeout = null)
            where TResult : class
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler {CookieContainer = cookieContainer})
            {
                using (var client = new HttpClient(handler))
                {
                    client.Timeout = timeout.HasValue ? TimeSpan.FromMilliseconds(timeout.Value) : Timeout;

                    if (!BaseUrl.IsNullOrEmpty())
                    {
                        client.BaseAddress = new Uri(BaseUrl);
                    }

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    foreach (var header in RequestHeaders)
                    {
                        client.DefaultRequestHeaders.Add(header.Name, header.Value);
                    }
                    
                    using (var requestContent = new StringContent(Object2JsonString(input), Encoding.UTF8, "application/json"))
                    {
                        foreach (var cookie in Cookies)
                        {
                            if (!BaseUrl.IsNullOrEmpty())
                            {
                                cookieContainer.Add(new Uri(BaseUrl), cookie);
                            }
                            else
                            {
                                cookieContainer.Add(cookie);
                            }
                        }

                        using (var response = await client.PostAsync(url, requestContent))
                        {
                            SetResponseHeaders(response);

                            if (!response.IsSuccessStatusCode)
                            {
                                throw new ICIMSException("Could not made request to " + url + "! StatusCode: " + response.StatusCode + ", ReasonPhrase: " + response.ReasonPhrase);
                            }

                            var ajaxResponse = JsonStringToObject<AjaxResponse<TResult>>(await response.Content.ReadAsStringAsync());
                            if (!ajaxResponse.Success)
                            {
                                throw new RemoteCallException(ajaxResponse.Error);
                            }

                            return ajaxResponse.Result;
                        }
                    }
                }
            }
        }

        private void SetResponseHeaders(HttpResponseMessage response)
        {
            ResponseHeaders.Clear();
            foreach (var header in response.Headers)
            {
                foreach (var headerValue in header.Value)
                {
                    ResponseHeaders.Add(new NameValue(header.Key, headerValue));
                }
            }
        }

        private static string Object2JsonString(object obj)
        {
            if (obj == null)
            {
                return "";
            }

            return JsonConvert.SerializeObject(obj,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }

        private static TObj JsonStringToObject<TObj>(string str)
        {
            return JsonConvert.DeserializeObject<TObj>(str,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }

        
        private void TokenBasedAuth(string url)
        {
            var result = AsyncHelper.RunSync(() =>
                PostAsync<AuthenticateResultDto>(
                    url,
                    new
                    {
                        TenancyName = TenancyName,
                        UsernameOrEmailAddress = UserName,
                        Password = Password
                    }));

            RequestHeaders.Add(new NameValue("Authorization", "Bearer " + result.AccessToken));

             
        }
        public void TokenBasedAuth()
        {
            TokenBasedAuth(BaseUrl + "api/TokenAuth/Authenticate");
        }
        public async Task<TResult> GetAsync<TResult>(string url, object input, int? timeout = null)
            where TResult : class
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
            {
                using (var client = new HttpClient(handler))
                {
                    client.Timeout = timeout.HasValue ? TimeSpan.FromMilliseconds(timeout.Value) : TimeSpan.FromSeconds(90);

                    if (!BaseUrl.IsNullOrEmpty())
                    {
                        client.BaseAddress = new Uri(BaseUrl);
                    }

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    foreach (var header in RequestHeaders)
                    {
                        client.DefaultRequestHeaders.Add(header.Name, header.Value);
                    }

                    using (var requestContent = new StringContent(Object2JsonString(input), Encoding.UTF8, "application/json"))
                    {
                        foreach (var cookie in Cookies)
                        {
                            if (!BaseUrl.IsNullOrEmpty())
                            {
                                cookieContainer.Add(new Uri(BaseUrl), cookie);
                            }
                            else
                            {
                                cookieContainer.Add(cookie);
                            }
                        }

                        using (var response = await client.GetAsync(url + "?" + ObjectToQueryString(input)))
                        {
                            if (!response.IsSuccessStatusCode)
                            {
                                throw new ICIMSException("Could not made request to " + url + "! StatusCode: " + response.StatusCode + ", ReasonPhrase: " + response.ReasonPhrase);
                            }

                            var rsStr = await response.Content.ReadAsStringAsync();
                            var ajaxResponse = JsonStringToObject<AjaxResponse<TResult>>(rsStr);
                            if (!ajaxResponse.Success)
                            {
                                throw new RemoteCallException(ajaxResponse.Error);
                            }

                            return ajaxResponse.Result;
                        }
                    }
                }
            }
        }
        public virtual async Task<TResult> DeleteAsync<TResult>(string url, object input, int? timeout = null)
          where TResult : class
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler { CookieContainer = cookieContainer })
            {
                using (var client = new HttpClient(handler))
                {
                    client.Timeout = timeout.HasValue ? TimeSpan.FromMilliseconds(timeout.Value) : Timeout;

                    if (!BaseUrl.IsNullOrEmpty())
                    {
                        client.BaseAddress = new Uri(BaseUrl);
                    }

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    foreach (var header in RequestHeaders)
                    {
                        client.DefaultRequestHeaders.Add(header.Name, header.Value);
                    }

                    using (var requestContent = new StringContent(Object2JsonString(input), Encoding.UTF8, "application/json"))
                    {
                        foreach (var cookie in Cookies)
                        {
                            if (!BaseUrl.IsNullOrEmpty())
                            {
                                cookieContainer.Add(new Uri(BaseUrl), cookie);
                            }
                            else
                            {
                                cookieContainer.Add(cookie);
                            }
                        }

                        using (var response = await client.DeleteAsync(url+"?"+ ObjectToQueryString(requestContent)))
                        {
                            SetResponseHeaders(response);

                            if (!response.IsSuccessStatusCode)
                            {
                                throw new ICIMSException("Could not made request to " + url + "! StatusCode: " + response.StatusCode + ", ReasonPhrase: " + response.ReasonPhrase);
                            }

                            var rsStr = await response.Content.ReadAsStringAsync();
                            var ajaxResponse = JsonStringToObject<AjaxResponse<TResult>>(rsStr);
                            if (!ajaxResponse.Success)
                            {
                                throw new RemoteCallException(ajaxResponse.Error);
                            }

                            return ajaxResponse.Result; 
                            
                        }
                    }
                }
            }
        }


        private string ObjectToQueryString(object requestContent)
        {
            var properties = from p in requestContent.GetType().GetProperties()
                             where p.GetValue(requestContent, null) != null
                             select p.Name + "=" +UrlEncode(p.GetValue(requestContent, null).ToString());

            return String.Join("&", properties.ToArray());
        }
        private string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }
    }
}