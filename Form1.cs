using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System.Reflection;

namespace MiniSpotify
{
    public partial class Form1 : Form
    {
        private static EmbedIOAuthServer _server;
        private static SpotifyClient _spotify;

        // BURAYA CLIENT ID YAZ
        private static string clientId = "35e3a33ec429452bb9bed1795addf566";
        private static string clientSecret = "8331fc3ebd7e45c48898c1b298e3a1c7";

        private string accessToken;
        private string refreshToken;
        private AuthorizationCodeTokenResponse token;

        public Form1()
        {
            InitializeComponent();
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - 390, 15);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var refreshToken = File.ReadAllText("D://C#/MiniSpotify/token.txt");

                var newToken = await new OAuthClient().RequestToken(
                    new AuthorizationCodeRefreshRequest(
                        clientId,
                        clientSecret,
                        refreshToken
                    )
                );
                _spotify = new SpotifyClient(newToken.AccessToken);
                var profile = await _spotify.UserProfile.Current();
                MessageBox.Show(profile.DisplayName);
            }
            catch (Exception ex)
            {

                await NewLogin();
            }
        }
        public static async Task NewLogin()
        {
            // Make sure "http://127.0.0.1:5000/callback" is in your spotify application as redirect uri!
            _server = new EmbedIOAuthServer(new Uri("http://127.0.0.1:5000/callback"), 5000);
            await _server.Start();

            _server.AuthorizationCodeReceived += OnAuthorizationCodeReceived;

            var request = new LoginRequest(_server.BaseUri, clientId, LoginRequest.ResponseType.Code)
            {
                Scope = new List<string> {
                    Scopes.UserModifyPlaybackState,
                    Scopes.UserReadPlaybackState,
                    Scopes.UserReadCurrentlyPlaying,
                    Scopes.AppRemoteControl,
                }
            };
            BrowserUtil.Open(request.ToUri());
        }
        private static async Task OnAuthorizationCodeReceived(object sender, AuthorizationCodeResponse response)
        {
            await _server.Stop();

            var config = SpotifyClientConfig.CreateDefault();
            var tokenResponse = await new OAuthClient(config).RequestToken(
              new AuthorizationCodeTokenRequest(
                clientId, clientSecret, response.Code, new Uri("http://127.0.0.1:5000/callback")
              )
            );

            _spotify = new SpotifyClient(tokenResponse.AccessToken);
            await File.WriteAllTextAsync("token.txt", tokenResponse.RefreshToken);
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Win32.SetRoundedCorner(Handle);
        }
    }
}
