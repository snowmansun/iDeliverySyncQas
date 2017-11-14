using System;
using System.Data;
using System.Collections;
using System.Configuration;

using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Caching;

using eBest.Mobile.SyncConfig;


namespace eBest.SyncServer
{
    public class Global : System.Web.HttpApplication
    {
        static CacheItemRemovedCallback onRemove = null;
        static HttpContext context = null;

        protected void RemovedCallback(String k, Object v, CacheItemRemovedReason r)
        {
            object obj = eBest.SyncConfiguration.SyncConfiguration.Section;
            context.Cache.Insert("SyncConfiguration",
                                  obj,
                                  new CacheDependency(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile),
                                                      DateTime.Now.AddHours(6),
                                                      Cache.NoSlidingExpiration,
                                                      System.Web.Caching.CacheItemPriority.High,
                                                      onRemove);
        }


        protected void Application_Start(object sender, EventArgs e)
        {
            eBest.SyncConfiguration.SyncConfiguration.Init(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            onRemove = new CacheItemRemovedCallback(RemovedCallback);
            context = this.Context;
            RemovedCallback(null, null, new CacheItemRemovedReason());


            //日志开启控制
            var mySync = (SyncConfigManager)ConfigurationManager.GetSection("sync");
            if ("open".Equals(mySync.Common["log"].Value, StringComparison.OrdinalIgnoreCase))
                log4net.Config.XmlConfigurator.Configure();

        }



        protected void Session_Start(object sender, EventArgs e)
        {


        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            AppDomain.Unload(AppDomain.CurrentDomain);
        }
    }
}