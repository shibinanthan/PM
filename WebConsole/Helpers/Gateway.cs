using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectManagement.Common;
using ProjectManagement.Common.Contracts;
using WebConsole.Interfaces;

namespace WebConsole.Helpers
{
    public class ProductGateway : GatewayBase, IProjectGateway
    {
        private static ProductGateway instance = null;
        private static string lockObject = "lock";

        //Singleton pattern implementation
        public static ProductGateway Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new ProductGateway();
                    }
                    return instance;
                }
            }
        }

        protected override string EndpointName
        {
            get { return "ProjectManagement.ConsoleService.Http"; }
        }

        public bool ValidateUser(PMUser user)
        {
            return WcfWrapper((IProjectService projectService) =>
                {
                    return projectService.ValidateUser(user);
                });
        }
    }
}