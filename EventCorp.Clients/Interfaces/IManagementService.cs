using System.Threading.Tasks;
using EventCorps.Helper.Models;

namespace EventCorp.Clients.Interfaces
{
    public interface IManagementService<TIUserManagement, TIAdminManagement>
    {
        /// <summary>
        /// Baut eine Verbindung zum Webserver auf
        /// </summary>
        /// <param name="username">Name des Nutzers</param>
        /// <param name="password">Passwort des Nutzers</param>
        /// <returns></returns>
        Task<IGenericManagementSession<TIUserManagement, TIAdminManagement>> LoginAsync(string username, string password);
        /// <summary>
        /// Gibt einen Zeitstempel der Anfrage zurueck. Diese MEthode sollte BEnutzt werden, um zu pruefen, ob eine Verbindung zum Server besteht
        /// </summary>
        /// <returns></returns>
        Task<bool> PingAsync();
        /// <summary>
        /// Registriert einen neuen Benutzer an der Web-Application
        /// </summary>
        /// <param name="model">Daten des neuen Nutzers</param>
        /// <returns></returns>
        Task RegisterAsync(CreateUserModel model);

    }
}