using System
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Client.Utilities
{
    public static class WebRequestExtensions
    {
        public static Task<WebResponse> GetResponseAsync(this WebRequest request, CancellationToken token)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            bool timeout = false;
            TaskCompletionSource<WebResponse> completionSource = new TaskCompletionSource<WebResponse>();

            AsyncCallback completedCallback =
                result =>
                {
                    try
                    {
                        completionSource.TrySetResult(request.EndGetResponse(result));
                    }
                    catch (WebException ex)
                    {
                        if (timeout)
                            completionSource.TrySetException(new WebException("No response was received during the time-out period for a request.", WebExceptionStatus.Timeout));
                        else if (token.IsCancellationRequested)
                            completionSource.TrySetCanceled();
                        else
                            completionSource.TrySetException(ex);
                    }
                    catch (Exception ex)
                    {
                        completionSource.TrySetException(ex);
                    }
                };

            IAsyncResult asyncResult = request.BeginGetResponse(completedCallback, null);
            if (!asyncResult.IsCompleted)
            {
                if (request.Timeout != Timeout.Infinite)
                {
                    WaitOrTimerCallback timedOutCallback =
                        (object state, bool timedOut) =>
                        {
                            if (timedOut)
                            {
                                timeout = true;
                                request.Abort();
                            }
                        };

                    ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, timedOutCallback, null, request.Timeout, true);
                }

                if (token != CancellationToken.None)
                {
                    WaitOrTimerCallback cancelledCallback =
                        (object state, bool timedOut) =>
                        {
                            if (token.IsCancellationRequested)
                                request.Abort();
                        };

                    ThreadPool.RegisterWaitForSingleObject(token.WaitHandle, cancelledCallback, null, Timeout.Infinite, true);
                }
            }

            return completionSource.Task;
        }
    }
}
