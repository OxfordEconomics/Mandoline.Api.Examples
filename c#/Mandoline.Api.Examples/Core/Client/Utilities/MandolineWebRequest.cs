using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Client.Models;
using Core.Client.Utilities;
using Newtonsoft.Json;

namespace Core.Client.Utilities
{
    internal class MandolineWebRequestBuilder
    {
        private string _baseUrl;
        private WebProxy _proxy;
        private string _apiKey;
        private string _clientName;

        public MandolineWebRequestBuilder(string baseUrl, string apiKey, string clientName, WebProxy proxy)
        {
            this._baseUrl = baseUrl;
            this._proxy = proxy;
            this._apiKey = apiKey;
            this._clientName = clientName;
        }

        public string BaseUrl
        {
            get
            {
                return _baseUrl;
            }
        }

        public MandolineWebRequest CreateRequest(string url, params string[] urlParameters)
        {
            return new MandolineWebRequest(_baseUrl, _proxy, _apiKey, _clientName, url, urlParameters);
        }

        internal void SetApiKey(string apiKey)
        {
            this._apiKey = apiKey;
        }

        internal void SetProxy(WebProxy proxy)
        {
            this._proxy = proxy;
        }
    }

    internal class MandolineWebRequest
    {
        private Uri _uri;
        private WebProxy _proxy;
        private string _apiKey;
        private string _clientName;

        public MandolineWebRequest(string baseUrl, WebProxy proxy, string apiKey, string clientName, string url, params string [] urlParameters)
        {
            this._proxy = proxy;
            this._apiKey = apiKey;
            this._clientName = clientName;

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                this._uri = new Uri(url);
            }
            else if (urlParameters.Length > 0)
            {
                // fill in url parameters if supplied                
                this._uri = new Uri(baseUrl + String.Format(url, urlParameters));
            }
            else
            {
                this._uri = new Uri(baseUrl + url);
            }
        }

        public async Task<Assertion<T>> Execute<T>(object parameter, string method = "POST", CancellationToken cancel = default(CancellationToken)) where T:class
        {
            Trace.WriteLine($"{method} {this._uri}");

            try
            {
                var responseAssertion = await GetResponseStream(parameter, method, cancel);
                if (responseAssertion.Failed)
                {
                    Trace.WriteLine($"{method} {this._uri} ({responseAssertion.Description} {responseAssertion.Reason}");
                    return Assertion<T>.Fail(responseAssertion);
                }
                var responseStream = responseAssertion.Result;

                var reader = new StreamReader(responseStream);

                string responseText = await ReadToEndWithCancellationToken(reader, cancel);
                

                if (responseText == null)
                {
                    Trace.WriteLine($"{method} {this._uri} (cancelled)");
                    return Assertion<T>.Fail(null, HttpStatusCode.NoContent, "Request cancelled");
                }

                if (typeof(T) == typeof(string))
                {
                    return Assertion<T>.Pass(responseText as T);
                }

                var result = JsonConvert.DeserializeObject<T>(responseText);

                //Trace.WriteLine($"Passed {method} {this._uri}");
                //Trace.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

                return Assertion<T>.Pass(result);
            }
            catch(Exception e)
            {                
                Trace.WriteLine($"{method} {this._uri} ({e.Message})");
                throw e;
            }
        }

        public class StreamResult<T1,T2>
        {
            public T1 FirstObject { get; set; }
            public IEnumerable<T2> Remaining { get; set; }
        }

        public async Task<Assertion<StreamResult<T1, T2>>> ExecuteStream<T1, T2>(object parameter, string method, CancellationToken cancel, bool allowAutoRedirect) where T1 : class
        {            
            var responseAssertion = await GetResponseStream(parameter, method, cancel);

            if (responseAssertion.Failed)
            {
                return Assertion<StreamResult<T1, T2>>.Fail(responseAssertion);
            }
            var responseStream = responseAssertion.Result;
            var reader = new StreamReader(responseStream);
            var firstObject = ReadLineWithCancellationToken<T1>(reader, cancel);

            if (firstObject == null)
            {
                return Assertion<StreamResult<T1, T2>>.Fail(null, HttpStatusCode.NoContent, "Request cancelled");
            }

            var result = new StreamResult<T1, T2>();

            result.FirstObject = firstObject;
            result.Remaining = DownloadShapedStream<T2>(reader, cancel);                

            return Assertion<StreamResult<T1, T2>>.Pass(result);            
        }

        private IEnumerable<T> DownloadShapedStream<T>(StreamReader reader, CancellationToken cancel)
        {
            try
            {
                T line;

                while (
                    !reader.EndOfStream &&
                    (line = ReadLineWithCancellationToken<T>(reader, cancel)) != null &&
                     !cancel.IsCancellationRequested)
                {
                    yield return line;
                }
            }            
            finally
            {
                reader.Dispose();
            }
        }

        private T ReadLineWithCancellationToken<T>(StreamReader reader, CancellationToken token)
        {
            var json = ReadLineWithCancellationToken(reader, token);
            return JsonConvert.DeserializeObject<T>(json);
        }

        private string ReadLineWithCancellationToken(StreamReader reader, CancellationToken token)
        {
            try
            {
                var x = reader.ReadLineAsync();
                x.Wait(token);

                if (x.IsCompleted)
                {
                    return x.Result;
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.RequestCanceled)
                {
                    return null;
                }

                throw;
            }
            catch (OperationCanceledException)
            {
                return String.Empty;
            }
            catch (AggregateException)
            {
                return String.Empty;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        private Assertion<T> ExceptionToAssertion<T>(Exception e) where T:class
        {
            if (e is AggregateException)
            {
                var a = (AggregateException)e;

                return ExceptionToAssertion<T>(a.InnerExceptions.First());
            }
            if (e is WebException)
            {
                return HandleWebException<T>((WebException)e);
            }
            if (e is OperationCanceledException)
            {
                return Assertion<T>.Fail(null, HttpStatusCode.NoContent, "Request cancelled");
            }
            else
            {
                return Assertion<T>.Fail(null, HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private static Assertion<T> HandleWebException<T>(WebException we) where T:class
        {
            if (we.Response == null)
            {
                return Assertion<T>.Fail(default(T), HttpStatusCode.InternalServerError, we.Status + " " + we.Message);
            }

            var response = we.Response as HttpWebResponse;
            var content = "";
            using (var reader = new StreamReader(we.Response.GetResponseStream()))
            {
                content = reader.ReadToEnd();
            }

            var msg = content;

            try
            {
                if (!String.IsNullOrWhiteSpace(content))
                {
                    var errorObject = JsonConvert.DeserializeObject<ErrorDto>(content);

                    msg = errorObject.MessageDetails;
                    if (string.IsNullOrWhiteSpace(msg))
                    {
                        msg = errorObject.Message;
                    }
                }
            }
            catch (JsonException)
            {
            }

            if (String.IsNullOrWhiteSpace(msg))
            {
                msg = we.Message;
            }

            return Assertion<T>.Fail(default(T), response.StatusCode, msg);
        }

        private async Task<string> ReadToEndWithCancellationToken(StreamReader reader, CancellationToken token)
        {
            try
            {
                var x = reader.ReadToEndAsync();
                
                await Task.WhenAny(x, Task.Delay(Timeout.Infinite, token));

                if (x.IsCompleted)
                {
                    return x.Result;
                }
                else
                {
                    return null;
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.RequestCanceled)
                {
                    return null;
                }

                throw;
            }
        }


        private async Task<Assertion<Stream>> GetResponseStream(object parameter, string method, CancellationToken cancel)
        {
            try
            {
                return await GetResponseStreamWithoutExceptionHandling(this._uri, parameter, method, cancel, "application/json");
            }
            catch (WebException e)
            {
                switch (e.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        var response = e.Response as HttpWebResponse;
                        if (response == null)
                        {
                            return Assertion<Stream>.Fail(null, HttpStatusCode.NotFound, "Not found");
                        }
                        else
                        {
                            return Assertion<Stream>.Fail(null, response.StatusCode, response.StatusDescription);
                        }
                }
                return ExceptionToAssertion<Stream>(e);
            }
            catch (OperationCanceledException)
            {
                // TODO: allow non http assertions, GatewayTimeout is way wrong here
                return Assertion<Stream>.Fail(null, HttpStatusCode.GatewayTimeout, "Operation cancelled");
            }
            catch (Exception e)
            {
                return ExceptionToAssertion<Stream>(e);
            }
        }

        private async Task<Assertion<Stream>> GetResponseStreamWithoutExceptionHandling(Uri uri, object parameter, string method, CancellationToken cancel, string accept)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);

            if (this._proxy != null)
            {
                request.Proxy = this._proxy;
            }

            // expect-100-continue does not play well with squid
            // http://haacked.com/archive/2004/05/15/http-web-request-expect-100-continue.aspx
            // http://gionn.net/2010/09/07/how-to-circumvent-the-417-expectation-failed-behind-a-squid-proxy/
            // System.Net.ServicePointManager.Expect100Continue = false;
            request.ServicePoint.Expect100Continue = false;

            request.Timeout = 180 * 60 * 1000;
            request.Method = method;
            request.AllowAutoRedirect = false /*allowAutoRedirect*/;
            request.KeepAlive = true;
            
            // headers
            request.Accept = accept;            
            request.Headers.Add(Constants.API_KEY_KEY, this._apiKey);
            if (!String.IsNullOrWhiteSpace(_clientName))
            {
                request.Headers.Add(Constants.CLIENT_KEY, _clientName);
            }                        

            if (parameter != null)
            {
                request.ContentType = "application/json; charset=utf-8";
                // request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
                       
                var postJson = JsonConvert.SerializeObject(parameter);
                var postData = Encoding.UTF8.GetBytes(postJson);

                request.ContentLength = postData.Length;

                // Get the request stream.
                //Stream dataStream = request.GetRequestStreamAsync().ExecuteSynchronously(cancel);
                Stream dataStream = await request.GetRequestStreamAsync();

                // Write the data to the request stream.
                dataStream.Write(postData, 0, postData.Length);

                await dataStream.FlushAsync();

                // Close the Stream object.
                dataStream.Close();
            }                        

            var response = await request.GetResponseAsync(cancel) as HttpWebResponse;

            var responseStream = response.GetResponseStream();

            //var rateLimited = response.GetResponseHeader("Rate-Limited");
            //if (rateLimited != null)
            //{
            //    Trace.WriteLine("Request was rate limited");
            //}

            if (response.StatusCode == System.Net.HttpStatusCode.OK ||
                    response.StatusCode == System.Net.HttpStatusCode.Created ||
                    response.StatusCode == System.Net.HttpStatusCode.NoContent ||
                    response.StatusCode == System.Net.HttpStatusCode.Accepted ||
                    response.StatusCode == System.Net.HttpStatusCode.NotModified)
            {                            

                return Assertion<Stream>.Pass(responseStream, response.StatusCode);
            }
            else
            {
                var msg = response.StatusDescription;

                if (response.StatusCode == HttpStatusCode.Found)
                {
                    msg = "Access Denied";
                }
                else if (response.StatusCode == HttpStatusCode.Moved)
                {
                    msg = "Moved";
                    return await GetResponseStreamWithoutExceptionHandling(new Uri(response.Headers["Location"]), null, "GET", cancel, null);
                }
                else
                {
                    try
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            var content = reader.ReadToEndAsync().ExecuteSynchronously(cancel);

                            if (!String.IsNullOrWhiteSpace(content))
                            {
                                var errorObject = JsonConvert.DeserializeObject<ErrorDto>(content);

                                msg = errorObject.MessageDetails;
                                if (string.IsNullOrWhiteSpace(msg))
                                {
                                    msg = errorObject.Message;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                if (String.IsNullOrWhiteSpace(msg))
                {
                    msg = response.StatusCode.ToString();
                }                

                return Assertion<Stream>.Fail(null, response.StatusCode, msg);
            }            
        }
    }
}
