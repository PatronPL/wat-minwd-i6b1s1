using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Zad3_BE
{
    public class repo
    {
        private readonly IHttpClientFactory clientFactory;

        public repo(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }


    }
}
