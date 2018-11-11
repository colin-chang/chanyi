using Owin.AspEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Chanyi.Web.Official
{
    public class TinyAdapter
    {
        public Task OwinMain(IDictionary<string, object> env)
        {
            return AspNet.Process(env);
        }
    }
}