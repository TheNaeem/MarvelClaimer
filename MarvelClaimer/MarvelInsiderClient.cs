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

    public IEnumerable<Activity> GetActivites()
    {
        var req = new RestRequest("request", Method.Post);

        req.AddJsonBody(new
        {
            page_data = new
            {
                text = new List<string>()
            },
            model_data = new
            {
                activity = new
                {
                    activities = new
                    {
                        properties = new List<string> {
                                "id",
                                 "link_href",
                                 "name",
                                 "num_points",
                                 "title"
                            },
                        query = new
                        {
                            type = "activities_by_name",
                            args = new
                            {
                                names = new List<string>
                                    {
                                        "women of marvel panel 2022",
                                        "nycc 2022 livestream day 4",
                                        "twitter trivia 2022 day 4",
                                        "latest:  article - connycc22day3recap",
                                        "latest: video - connycclivestream2022",
                                        "marvel comics next big thing panel 2022",
                                        "nycc 2022 livestream day 3",
                                        "marvels voices the world outside of your window",
                                        "twitter trivia 2022 day 3",
                                        "latest: article - merchshehulkmmhep8",
                                        "latest:  article - connycc22day2recap",
                                        "latest:  article - tvwerewolfbynightdisney+",
                                        "nycc 2022 livestream day 2",
                                        "latest:  article - generalmilb",
                                        "twitter trivia 2022 day 2",
                                        "latest:  article - connycc22day1recap",
                                        "nycc 2022 livestream day 1",
                                        "instagram picture scramble 2022",
                                        "twitter trivia 2022 day 1",
                                        "latest: article - tvshehulkep8recap",
                                        "latest:  video - generalbeyondamazingexhibition",
                                        "latest:  article - merchchroniclemarvelmazes",
                                        "latest:  article - merchoracledeck",
                                        "latest:  article - connycc22boothschedule",
                                        "latest:  article - generalnyplspidermancard",
                                        "latest:  article - merchoctober22",
                                        "latest:  video - generalmarvelminutenycc22",
                                        "latest:  article - movieblackpanthertrailer",
                                        "survey answered d73684fd-5b79-4a92-bfe6-3c154301e1b6",
                                        "latest:  article - tvwerewolfbynightfeaturette",
                                        "latest: article - merchshehulkmmhep7",
                                        "latest:  article - moviearmorwarstheatricalrelease",
                                        "latest:  article - merchfallmmh",
                                        "latest: article - tvshehulkep7recap",
                                        "latest:  article - merchtargetblackpanther",
                                        "latest:  article - moviedeadpool3",
                                        "latest:  article - mercharizonasuperlxr",
                                        "latest:  article - merchpenguincaptainamerica",
                                        "latest: article - merchshehulkmmhep6",
                                        "latest:  article - parksachulk",
                                        "latest: article - tvshehulkep6recap",
                                        "latest:  article - merchtoyawards",
                                        "latest:  article - merchpenguinblackpanther",
                                        "survey answered ea6dd091-4d42-4981-9d32-6c4eeb48e164",
                                        "latest: article - merchshehulkmmhep5",
                                        "latest: article - tvshehulkep5recap",
                                        "latest:  article - merchinfinityrelics",
                                        "latest:  article - connycc23panels",
                                        "latest:  video - cond23marvelminute",
                                        "latest:  article - podcastwastelandersdoom",
                                        "latest:  video - cond23stars",
                                        "latest:  video - parksacd23",
                                        "latest:  video - cond23studiosannouncements",
                                        "latest:  article - tvmoongirlfebruary",
                                        "latest: podcast - wastelandersdoom",
                                        "latest: podcast - marvelsvoices",
                                        "latest: podcast - marvelspulllist",
                                        "latest: podcast - thisweekinmarvel",
                                        "visit the marvel shop"
                                    }
                            }
                        }
                    }
                }
            },
        });

        var response = Execute(req);

        if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
            return Enumerable.Empty<Activity>();

        using var doc = JsonDocument.Parse(response.Content);

        if (!doc.RootElement.TryGetProperty("model_data", out var modelData) ||
            !modelData.TryGetProperty("activity", out var activity) ||
            !activity.TryGetProperty("activities", out var activities))
        {
            return Enumerable.Empty<Activity>();
        }

        return activities.Deserialize<List<Activity>>() ?? Enumerable.Empty<Activity>();
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
