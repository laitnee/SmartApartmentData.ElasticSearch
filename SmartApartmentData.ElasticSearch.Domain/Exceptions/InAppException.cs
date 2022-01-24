using System.Globalization;

namespace SmartApartmentData.ElasticSearch.Domain.Exceptions;

public class InAppException : Exception
{
    public InAppException() : base() {}
        
    public InAppException(string message) : base(message)
    {

    }
        
    public InAppException(string message, params object[] args) 
        : base(String.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}