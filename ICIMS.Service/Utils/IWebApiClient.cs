using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;

namespace ICIMS.Service
{
    /// <summary>
    ///   Web APIs.
    /// </summary>
    public interface IWebApiClient
    {
         string TenancyName { get; set; }

         string UserName { get; set; }

         string Password { get; set; }
        string BaseUrl { get; set; }

        //void CookieBasedAuth();
        AuthenticateResultDto TokenBasedAuth();

        /// <summary> 
        /// Default: 90 seconds.
        /// </summary>
        TimeSpan Timeout { get; set; }

        /// <summary>
        /// Used to set cookies for requests.
        /// </summary>
        Collection<Cookie> Cookies { get; }

        /// <summary>
        /// Request headers.
        /// </summary>
       // ICollection<NameValue> RequestHeaders { get; }

        /// <summary>
        /// Response headers.
        /// </summary>
        ICollection<NameValue> ResponseHeaders { get; }

        
        Task PostAsync(string url, int? timeout = null);

        /// <summary>
        /// Makes post request that gets input but does not return value.
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="input">Input</param>
        /// <param name="timeout">Timeout as milliseconds</param>
        Task PostAsync(string url, object input, int? timeout = null);

        /// <summary>
        /// Makes post request that does not get input but returns value.
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="timeout">Timeout as milliseconds</param>
        Task<TResult> PostAsync<TResult>(string url, int? timeout = null) where TResult : class;

        /// <summary>
        /// Makes post request that gets input and returns value.
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="input">Input</param>
        /// <param name="timeout">Timeout as milliseconds</param>
        Task<TResult> PostAsync<TResult>(string url, object input, int? timeout = null) where TResult : class;
        Task<TResult> GetAsync<TResult>(string url, int? timeout = null) where TResult : class;
        Task<TResult> GetAsync<TResult>(string url, object input, int? timeout = null) where TResult : class;
        Task<TResult> DeleteAsync<TResult>(string url, object input, int? timeout = null)
        where TResult : class;

        Task<TResult> PutAsync<TResult>(string url, object input, int? timeout = null) where TResult : class;
        Task<TResult> UploadFileAsync<TResult>(string url, List<KeyValuePair<string, string>> keyValuePairs, string filePath, string fileName, int? timeout = null) where TResult : class;
      

    }
}