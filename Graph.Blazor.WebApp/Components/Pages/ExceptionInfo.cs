using System;

namespace Graph.Blazor.WebApp.Components.Pages
{
    public class ExceptionInfo
    {
        public ExceptionInfo(Exception exception)
        {
            if (exception is null)
            {
                return;
            }

            Type = exception.GetType().FullName;
            Message = exception.Message;
            Source = exception.Source;
            StackTrace = exception.StackTrace;
            if (exception.InnerException != null)
            {
                InnerException = new ExceptionInfo(exception.InnerException);
            }
        }

        public ExceptionInfo InnerException { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string StackTrace { get; set; }

        public string Type { get; set; }
    }
}