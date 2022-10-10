using RestSharp;
using RestSharp.Authenticators;

namespace MarvelClaimer;

public class MarvelAuthenticator : IAuthenticator
{
    string _cookies;

    public MarvelAuthenticator(string cookies)
    {
        _cookies = cookies;
    }

    public ValueTask Authenticate(RestClient client, RestRequest request)
    {
        request.AddOrUpdateHeader("Cookie", _cookies);

        return new();
    }


}
