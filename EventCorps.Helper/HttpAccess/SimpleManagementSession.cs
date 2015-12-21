using System;
using System.Data.Entity;
using System.Net.Http;
using System.Net.Http.Headers;
using EventCorps.Helper.HttpAccess;

namespace EventCorp.Clients
{
  public class SimpleManagementSession : IDisposable
  {
    #region Field
    private HttpClient _userClient;
    private HttpClient _eventClient;
    private HttpClientHandler _handler;
    private bool disposed;
    #endregion

    #region Ctor
    public SimpleManagementSession(string userbaseUri, string eventbaseUri)
    {
      _handler = new HttpClientHandler();
      //InitialiseClient(_userClient, userbaseUri);
      _userClient = new HttpClient(_handler)
      {
        BaseAddress = new Uri(userbaseUri)
      };
      _userClient.DefaultRequestHeaders.Accept.Clear();
      _userClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
      _userClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      this.Users = new UserManagement(_userClient);

      //InitialiseClient(_eventClient, eventbaseUri);
      _eventClient = new HttpClient(_handler)
      {
        BaseAddress = new Uri(eventbaseUri)
      };
      _eventClient.DefaultRequestHeaders.Accept.Clear();
      _eventClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
      _eventClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

      this.Events = new EventManagement(_eventClient);
    }

    private void InitialiseClient(HttpClient client, string userbaseUri)
    {
      client = new HttpClient(_handler)
      {
        BaseAddress = new Uri(userbaseUri)
      };
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    ~SimpleManagementSession()
    {
      Dispose(false);
    }
    #endregion

    #region Methods

    public static SimpleManagementSession Create()
    {
      var userBaseURL = "";
      var eventBaseURL = "";
#if DEBUG
      userBaseURL = "http://localhost:54867/";
      eventBaseURL = "http://localhost:55811/";

#else
      userBaseURL = "http://ec-auth.azurewebsites.net/";
      eventBaseURL = "http://ecevent.azurewebsites.net/";
#endif


      return new SimpleManagementSession(userBaseURL, eventBaseURL);
    }
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Schliesst die Verbindung zum Server
    /// </summary>
    /// <param name="disposing">Flag, welches anzeigt, ob das Disposing durchgeführt werden soll</param>
    protected void Dispose(bool disposing)
    {
      if (disposed) return;

      if (!disposing)
      {
        try
        {
          _userClient.CancelPendingRequests();
          _eventClient.CancelPendingRequests();
        }
        catch { }

        _userClient.Dispose();
        _eventClient.Dispose();
        _handler.Dispose();
      }

      disposed = true;
    }

    #endregion

    public UserManagement Users { get; set; }
    public EventManagement Events { get; set; }
  }
}