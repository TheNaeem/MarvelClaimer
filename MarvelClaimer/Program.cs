global using Serilog;
using MarvelClaimer;

Log.Logger =
    new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .CreateLogger();

Dictionary<int, string> IdToCode = new()
{
    { 10913, "LM18" },
    { 10912, "MW37" },
    { 10917, "Dora Milaje" },
    { 10918, "new orleans" },
    { 10916, "blood transfusion" },
    { 10914, "JJ15" },
    { 10910, "EJ98Q" },
    { 10920, "Miracleman" },
    { 10919, "Kelly Thompson" },
    { 10930, "TOTEM" },
    { 10915, "EL61" }
};

string cookie = " country=us; device_75123983=8b48d010-faea-4b67-bccf-8b5b5a7287b1; device_3809a71c=0bbb927c-f1b2-42ca-994f-7cf0d0c82b7f; _gcl_au=1.1.1729499553.1665433723; mboxEdgeCluster=34; _clck=1g3v0ve|1|f5l|0; s_vnc365=1696969942047%26vn%3D1; s_ivc=true; _fbp=fb.1.1665433950424.1884917753; __gads=ID=9f6e710bcd1effe1:T=1665433953:S=ALNI_MY9Edoa1mD-3y41Ou_VOc9PW84pSA; __gpi=UID=0000087ed55164f8:T=1665433953:RT=1665433953:S=ALNI_Majfvaa1JglGJx2l_JjxnlHFVRjzA; MARVEL-MARVEL.COM.WEB-PROD.api=9pVwZu19tZ91uXnNq4vlZr+EAl2L7yCzTUl5HRAqpinSbLR6ZK7W6nlIwX4zF00b0+bS/B1MA8gQh1hFfYnkoPA9NuKrWd99+20=; device_18f582bd=7221c24e-c291-4960-be46-0664c49bb3c2; SWID={3FBC8D2C-FAE3-4AEB-A7C4-46A95D8BDD9C}; MARVEL-MARVEL.COM.WEB-PROD-ac=XUS; device_26179cd5=316d7fe8-318e-45e4-8234-f9963e54bcec; friend_code=dZF5jmfj; check=true; s_plt=3.45; s_pltp=undefined; AMCVS_D83AE33C56937B7B7F000101%40AdobeOrg=1; AMCV_D83AE33C56937B7B7F000101%40AdobeOrg=-2121179033%7CMCIDTS%7C19276%7CMCMID%7C75336372569745636701668968959145976654%7CMCOPTOUT-1665445108s%7CNONE%7CvVersion%7C5.3.0%7CMCAAMLH-1666042708%7C7%7CMCAAMB-1666042708%7Cj8Odv6LonN4r3an7LhD3WZrU1bUpAkFkkiY1ncBR96t2PTI%7CMCSYNCSOP%7C411-19283; s_cc=true; marvel_autologin=%7B%22username%22%3A+%22dresdenmichaels%40gmail.com%22%2C+%22loginDate%22%3A+%222022-10-10+21%3A38%3A29%22%2C+%22signature%22%3A+%2205dffdd473c95d3ef5db766b372bb2bcae3b73f383591df7ec8423c4f692382f4cc569e60498d678823a0581e3acc23511ace21e3d4e9c4e1e617bbaf17e670a%22%7D; PHPSESSID=1gs5va4fm7vbpia8gp8hs8unup; prod_prod_ss_ctuc_2_66=9827405324def8535f1b4382ec5a1ee8dcf15b2fdaf76cea24afbd8bb2fa7a7c13b9fb9847a03bbd04e539bb4fca9a4ef59fd6b245bcd07dbc96f58420a6667cacb82c0709551a2d974cca24effd69c023f25316074cb010a496f131975e99657eb678946773c80b489a636d9712801eaf4adb90c8e6e2c703db7d4033e1478016beed2f941f71eaf6c8e13008bc3c253bacfecb054d7877c74f6db8cbb29d55c4984dd607f46b89390926c87c38184f-9b616aff3cdd4bf27d1a147100b8fa44a6c4e415; prod_prod_ctuc_2_66=9827405324def8535f1b4382ec5a1ee8dcf15b2fdaf76cea24afbd8bb2fa7a7c13b9fb9847a03bbd04e539bb4fca9a4ef59fd6b245bcd07dbc96f58420a6667cacb82c0709551a2d974cca24effd69c023f25316074cb010a496f131975e99657eb678946773c80b489a636d9712801eaf4adb90c8e6e2c703db7d4033e1478016beed2f941f71eaf6c8e13008bc3c253bacfecb054d7877c74f6db8cbb29d55c4984dd607f46b89390926c87c38184f-9b616aff3cdd4bf27d1a147100b8fa44a6c4e415; prod_prod_ss_ctut_2_66=395413c0f21f59dc6ebaefaf20f02037a599752eb7360eeea22e000b1fdc923df874f4bdeaadbc48-1f8f99c68e5a9df945e47bb5695868a632207154; prod_prod_ctut_2_66=395413c0f21f59dc6ebaefaf20f02037a599752eb7360eeea22e000b1fdc923df874f4bdeaadbc48-1f8f99c68e5a9df945e47bb5695868a632207154; MARVEL-MARVEL.COM.WEB-PROD.ts=2022-10-10T22:08:30.256Z; MARVEL-MARVEL.COM.WEB-PROD.token=5=eyJhY2Nlc3NfdG9rZW4iOiI2NjlmZDQxMDVlYzk0MmJlYjM0OGNlMTczOTFmYTRmNSIsInJlZnJlc2hfdG9rZW4iOiJiMTFkODFhMWY5Yjg0NTY4ODhjNzY5MGJhNTMwNGRmMCIsInN3aWQiOiJ7M0ZCQzhEMkMtRkFFMy00QUVCLUE3QzQtNDZBOTVEOEJERDlDfSIsInR0bCI6ODY0MDAsInJlZnJlc2hfdHRsIjoxNTU1MjAwMCwiaGlnaF90cnVzdF9leHBpcmVzX2luIjoxNzk5LCJpbml0aWFsX2dyYW50X2luX2NoYWluX3RpbWUiOjE2NjU0Mzc5MTA5MTksImlhdCI6MTY2NTQzNzkxMDAwMCwiZXhwIjoxNjY1NTI0MzEwMDAwLCJyZWZyZXNoX2V4cCI6MTY4MDk4OTkxMDAwMCwiaGlnaF90cnVzdF9leHAiOjE2NjU0Mzk3MDkwMDAsInNzbyI6bnVsbCwiYXV0aGVudGljYXRvciI6ImRpc25leWlkIiwibG9naW5WYWx1ZSI6bnVsbCwiY2xpY2tiYWNrVHlwZSI6bnVsbCwic2Vzc2lvblRyYW5zZmVyS2V5IjoiazZKRGlmV3hySXJJeFRKbE03VGdOYUZmWWZ5ZjFKWjhsS2xORlgvQ3ZIVnJibW4vWUxUNnE2Tld1bGpscVJuR250a04zWjVzK3VtQzVOUnVEUU1acU54U0NCNitwRzVoWEZoMmN3U3EvTW96UHh2c0pQcz0iLCJjcmVhdGVkIjoiMjAyMi0xMC0xMFQyMTozODozMS4yNTZaIiwibGFzdENoZWNrZWQiOiIyMDIyLTEwLTEwVDIxOjM4OjMxLjI1NloiLCJleHBpcmVzIjoiMjAyMi0xMC0xMVQyMTozODozMC4wMDBaIiwicmVmcmVzaF9leHBpcmVzIjoiMjAyMy0wNC0wOFQyMTozODozMC4wMDBaIiwiaXNFeHBpcmVkIjpmYWxzZX0=|eyJraWQiOiJxUEhmditOL0tONE1zYnVwSE1PWWxBc0pLcWVaS1U2Mi9DZjNpSm1uOEJ6dzlwSW5xbTVzUnc9PSIsImFsZyI6IlJTMjU2In0.eyJpc3MiOiJodHRwczovL2F1dGhvcml6YXRpb24uZ28uY29tIiwic3ViIjoiezNGQkM4RDJDLUZBRTMtNEFFQi1BN0M0LTQ2QTk1RDhCREQ5Q30iLCJhdWQiOiJNQVJWRUwtTUFSVkVMLkNPTS5XRUItUFJPRCIsImV4cCI6MTY2NTUyNDMxMCwiaWF0IjoxNjY1NDM3OTEwLCJqdGkiOiI1M0xZeU1uaGpyN0RrS3NRZDV2OE1BIiwibmJmIjoxNjY1NDM3ODUwLCJhX3R5cCI6Ik9ORUlEX1RSVVNURUQiLCJhX2NhdCI6IkdVRVNUIiwiYXRyIjoiZGlzbmV5aWQiLCJzY29wZXMiOlsiQVVUSFpfR1VFU1RfU0VDVVJFRF9TRVNTSU9OIl0sImNfdGlkIjoiMTgxOSIsImlnaWMiOjE2NjU0Mzc5MTA5MTksImh0YXYiOjIsImh0ZCI6MTgwMCwicnR0bCI6MTU1NTIwMDB9.NNTZAEpikDzjpSOuwsmKeplSR2pG-8tHCLf-S1s1IlfK8mFRBZPktm5D4bVdvz7u_BYfTiUgothxdASYO5XjoPyzVYL54LphgPTfbdUSkTiRpHx93EOX7m2ouQD050AuYfPCc9VBvg_rfj2l6n2hZ5s-s1Jo0Z793ROOUSvPMRUaEclSnurqOPq8JuPLyTlgg3seJ5RU3mQKHUhKXIiMwu0sxtONjzkLH6qged5YRm8f-DC3U5dJKy8jzEYYG35pJsG-Gcpc0uWt5oyQ5WoD1W6OhPgOTuq4I47HsTV-XBVRGXfxZGQdUFWsC5NI9Jn0w7fyGY8BqxMZfN6cD0tINg; MARVEL-MARVEL.COM.WEB-PROD.idn=00a34e275f; s_sq=%5B%5BB%5D%5D; _uetsid=22a63e2048da11eda4b225a42c286003; _uetvid=22a65cd048da11edaa91dbcf70a4ce50; mbox=session#fa13b9efe93d4bc784f800ba89ddb807#1665440245|PC#fa13b9efe93d4bc784f800ba89ddb807.34_0#1728678524; s_nr30=1665438389147-New; OptanonConsent=isIABGlobal=false&datestamp=Mon+Oct+10+2022+17%3A46%3A29+GMT-0400+(Eastern+Daylight+Time)&version=6.20.0&hosts=&consentId=9d06389f-a7d4-4b35-ba78-f510ff5c3b4e&interactionCount=1&landingPath=NotLandingPage&groups=C0001%3A1%2CC0003%3A1%2CC0002%3A1%2CSPD_BG%3A1%2CC0004%3A1&AwaitingReconsent=false; s_ips=1327; s_tp=2033; _clsk=14f8o06|1665438392107|35|1|b.clarity.ms/collect; s_ppv=marvel%2520insider%2520%257C%2520marvel%2C92%2C65%2C1869%2C1%2C2";

var client = new MarvelInsiderClient(cookie);

client.DoActivities(5638, MarvelClaimer.Properties.Resources.GamesActivitiesBody);
client.DoActivities(1374, MarvelClaimer.Properties.Resources.ComicsActivitiesBody);
client.DoActivities(1438, MarvelClaimer.Properties.Resources.LatestActivitiesBody);

client.FillQuestionare(MarvelClaimer.Properties.Resources.ProfileQnaBody);
client.FillQuestionare(MarvelClaimer.Properties.Resources.NftQnaBody);
client.FillQuestionare(MarvelClaimer.Properties.Resources.SuperheroQnaBody);
client.FillQuestionare(MarvelClaimer.Properties.Resources.VillainQnaBody);
client.FillQuestionare(MarvelClaimer.Properties.Resources.MonsterQnaBody);

Parallel.ForEach(IdToCode, (kvp, _) =>
{
    client.RedeemCode(kvp.Key, kvp.Value);
});

client.VisitTwitter();
client.VisitFacebook();

client.DoReferrals();

Chrome.Stop();