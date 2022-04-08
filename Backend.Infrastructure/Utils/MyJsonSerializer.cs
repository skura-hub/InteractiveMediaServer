using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Utils
{
    class MyJsonSerializer
    {
        public static JsonSerializerOptions Options;
        static MyJsonSerializer()
        {
            Options = new JsonSerializerOptions()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder
            .Create(System.Text.Unicode.UnicodeRanges.All)
            };
        }

        public static string Serialize(object? value)
        {
            return JsonSerializer.Serialize(value, MyJsonSerializer.Options);
        }
    }
}
