using System;
using System.Text;

namespace ProcessorIndeed.CommonData
{
    public class ErrorHelper
    {
        public static string ProcessingErrors(AggregateException ae)
        {
            var exceptions = ae.Flatten();
            var strBuilder = new StringBuilder();
            foreach (var exception in exceptions.InnerExceptions)
            {
                if (exception is OperationCanceledException)
                {
                    System.Diagnostics.Debug.WriteLine("Cancelled processing");
                }
                else
                    strBuilder.AppendLine(exception.Message);
            }
            return strBuilder.ToString();
        }
    }
}