using Mafa2.Web.Models.LinqSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Mafa2.Web.Models;

namespace Mafa2.Web.Models
{
    public class UsersRoleProvider : RoleProvider
    {
        #region Fields
        private DataClasses1DataContext dc = new DataClasses1DataContext();
        #endregion

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            //Mafa2.Web.Models.LinqSql.Pristup user = dc.Pristups.Where(u => u.Username == username).SingleOrDefault();
            var rezultat = (from user in dc.Pristups
                            join role in dc.Uloges on user.IDUloge equals role.IDUloge
                            where user.Username == username
                            select role.ImeUloge).ToArray();
            //return user.Uloge.ImeUloge.ToArray();
            return rezultat;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}