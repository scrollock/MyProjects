using ModelZipLocation;
using System;
using System.Text;

namespace WebZipLocation.Models
{
    public class ErrorHelper
    {
        public static string ProcessingErrors(AggregateException ae, Location location)
        {
            var exceptions = ae.Flatten();
            var strBuilder = new StringBuilder();
            foreach (var exception in exceptions.InnerExceptions)
            {
                location.Exceptions.Add(exception);
                strBuilder.AppendLine(exception.Message + StaticConstants.ColoneSpace);
            }
            return strBuilder.ToString();
        }
    }
}