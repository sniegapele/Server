using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Request
    {
        private String type;
        public String Type { get { return type; } }
        private String url;
        public String Url { get { return url; } }
        private String host { get; }

        private Request(String type, String url, String host)
        {
            this.type = type;
            this.url = url;
            this.host = host;
        }

        public static Request GetRequest(String request)
        {
            if (String.IsNullOrEmpty(request))
                return null;

            String[] info = request.Split(' ');
            String type = info[0];
            String url = info[1];
            String host = info[4];
            return new Request(type, url, host);
        }
    }
}
