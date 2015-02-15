using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectManagement.Common;
using ProjectManagement.Common.Contracts;
using WebConsole.Interfaces;

namespace WebConsole.Helpers
{
    public class ProjectGateway : GatewayBase, IProjectGateway
    {
        private static ProjectGateway instance = null;
        private static string lockObject = "lock";

        //Singleton pattern implementation
        public static ProjectGateway Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new ProjectGateway();
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