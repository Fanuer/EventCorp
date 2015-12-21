using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCorps.Helper.HttpAccess
{
  public static class Const
  {
    private static readonly Dictionary<string, string> _audiences = new Dictionary<string, string>()
        {
            {"0dd23c1d3ea848a2943fa8a250e0b2ad",  "Uy6qvZV0iA2/drm4zACDLCCm7BE9aCKZVQ16bg80XiU="}, //SPA
            {"4d874d92589f4357ba19a2b91e0be5ff",  "PUP3EI8G137YIW1TnoCVrwhu0KnpYYQHNWJcM9UU+mI="}, //Rec
            {"b9f6fba178aa4081a010f3b7a2c91ded",  "5U06jgp+GLxa32YnPjZSvfxL8QQjPmKtyELnygljJN4="}, //Auth
            {"0d108cd5b299400385c74763e55a83cb",  "zoS/pKDuKZ3ZE8Hyg795/itak6In0Mc2jIR+4R2t5io="}, //EventServer
            {"e8e6ec747f754ef2a2e86bff5f239aa7",  "Q65+1pjma7olrvJ5gWcloSR1bRHTt8CmpI6JRGGu91E="}, //Email
            {"2aa91a05973a4edc84c5d06a3fadd011",  "m+GyJVpH0ohLla95on5mJdupNJ0yeVYQNif33NJPtBM="}, //Statistics
        };
    
    public static Dictionary<string, string> Audiences
    {
      get { return _audiences; }
    }

    public static string Issuer
    {
      get { return "EventCorp.AuthServer"; }
    }
  }
}
