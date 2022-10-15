using RestSharp;

namespace MarvelClaimer.Marvel;

public class MarvelInsiderClient : RestClient
{
    public MarvelInsiderClient(string cookies) : base("https://loyalty.marvel.com")
    {
        Authenticator = new MarvelAuthenticator(cookies);
    }
}
