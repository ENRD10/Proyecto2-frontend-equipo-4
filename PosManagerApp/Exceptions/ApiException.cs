using System;
using System.Collections.Generic;
using System.Text;

namespace PosManagerApp.Exceptions
{
    public class ApiException(string message) : Exception(message)
    {
    }
}
