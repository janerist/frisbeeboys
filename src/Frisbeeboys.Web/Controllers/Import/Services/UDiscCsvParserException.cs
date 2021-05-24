using System;
using System.Runtime.Serialization;

namespace Frisbeeboys.Web.Controllers.Import.Services
{
    [Serializable]
    public class UDiscCsvParserException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public UDiscCsvParserException()
        {
        }

        public UDiscCsvParserException(string message) : base(message)
        {
        }

        public UDiscCsvParserException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UDiscCsvParserException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}