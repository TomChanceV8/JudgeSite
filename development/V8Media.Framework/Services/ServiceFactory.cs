using System;
using System.Web;

namespace V8Media.Framework.Services
{
    public static class ServiceFactory
    {

        public static TService GetService<TService>()
        {
            var key = string.Concat("factory-", typeof(TService).Name);
            if (!HttpContext.Current.Items.Contains(key))
                HttpContext.Current.Items.Add(key, (TService)Activator.CreateInstance(typeof(TService), new object[] { }));
            return (TService)HttpContext.Current.Items[key];
        }
    }
}