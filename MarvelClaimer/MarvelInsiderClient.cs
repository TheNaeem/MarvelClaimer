using RestSharp;
using MarvelClaimer.Models;
using System.Text.Json;

namespace MarvelClaimer;

public class MarvelInsiderClient : RestClient
{
    public MarvelInsiderClient(string cookies) : base("https://loyalty.marvel.com")
    {
        Authenticator = new MarvelAuthenticator(cookies);
    }

    public void DoActivities(int id, string body)
    {
        var req = new RestRequest($"request?widgetId={id}", Method.Post);

        req.AddJsonBody(body);

        var response = Execute(req);

        if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
            return;

        using var doc = JsonDocument.Parse(response.Content);

        if (!doc.RootElement.TryGetProperty("model_data", out var modelData) ||
            !modelData.TryGetProperty("activity", out var activityProp) ||
            !activityProp.TryGetProperty("activities", out var activities))
        {
            return;
        }

        var list = activities.Deserialize<List<Activity>>();

        if (list is null)
            return;

        foreach (var activity in list)
        {
            if (string.IsNullOrEmpty(activity.link_href))
                continue;

            if (activity.link_href.StartsWith('/'))
                continue;

            Log.Information("Opening link {Link}", activity.link_href);

            Chrome.OpenUrl(activity.link_href);
        }
    }

    public void RedeemCode(int id, string answer)
    {
        var req = new RestRequest($"code-redemption-campaign/redeem?cid={id}", Method.Post);

        req.AddJsonBody(new
        {
            code = answer
        });

        Execute(req);

        Log.Information("Redeemed {id} with answer {answer}", id, answer);
    }

    public void FillQuestionare(string body)
    {
        var req = new RestRequest("questionnaire/rpc", Method.Post);
        req.AddJsonBody(body);

        Execute(req);
    }

    public void VisitTwitter()
    {
        Execute(new("ca/c2a92ef1d1757d9005adc5f0861fa621"));
    }

    public void VisitFacebook()
    {
        Execute(new("ca/c2a92ef1d1757d90c3720bb6136f15ea"));
    }

    public void DoReferrals()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var random = new Random();

        for (int i = 0; i < 10; i++)
        {

            var code = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray()); 
            var link = "https://marvel.com/insider?_cts_=" + code;

            var refReq = new RestRequest("user/rpc", Method.Post);

            refReq.AddJsonBody(new
            {
                dest_id = 2,
                reason_id = 6,
                code = code,
                url = link,
                ct_rpc_action = "process_social_post"
            });

            Execute(refReq);
        }
    }
}
