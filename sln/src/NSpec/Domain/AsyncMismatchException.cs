using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.Domain
{
    public class AsyncMismatchException : Exception
    {
        public AsyncMismatchException(string message)
            : base(message) { }
    }
}
