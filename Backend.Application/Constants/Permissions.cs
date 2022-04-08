using System.Collections.Generic;

namespace Backend.Application.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }

        public static class Dashboard
        {
            public const string View = "Permissions.Dashboard.View";
            public const string Create = "Permissions.Dashboard.Create";
            public const string Edit = "Permissions.Dashboard.Edit";
            public const string Delete = "Permissions.Dashboard.Delete";
        }

        public static class User
        {
            public const string View = "Permissions.User.View";
            public const string Create = "Permissions.User.Create";
            public const string Edit = "Permissions.User.Edit";
            public const string Delete = "Permissions.User.Delete";
        }

        public static class X
        {
        
        }

         
    }
}