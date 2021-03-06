﻿using System;
using System.Threading.Tasks;

namespace EventCorp.Clients.Interfaces
{
    public interface IManagementSession<out TUserInterface, out TAdminInterface>:IManagementSessionBase
    {
        /// <summary>
        /// Encrypted Access Token
        /// </summary>
        string Token { get; }
        /// <summary>
        /// Access Token Expire Date
        /// </summary>
        DateTimeOffset ExpiresOn { get; }
        /// <summary>
        /// Performs a Refresh of the Access token
        /// </summary>
        /// <returns></returns>
        Task PerformRefreshAsync();
        /// <summary>
        /// Current User name
        /// </summary>
        string CurrentUserName { get; }
        /// <summary>
        /// Current User Id
        /// </summary>
        Guid CurrentUserId { get; }

        /// <summary>
        /// All Methods a users with role 'User' can perform
        /// </summary>
        TUserInterface Users { get; }
        /// <summary>
        /// All Methods a users with role 'Admin' can perform
        /// </summary>
        TAdminInterface Admins { get; }

    }
}