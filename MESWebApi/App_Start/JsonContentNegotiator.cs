using System.Net.Http.Formatting;

namespace MESWebApi
{
    internal class JsonContentNegotiator
    {
        private JsonMediaTypeFormatter jsonFormatter;

        public JsonContentNegotiator(JsonMediaTypeFormatter jsonFormatter)
        {
            this.jsonFormatter = jsonFormatter;
        }
    }
}