using MiniSpotify.Helper;
using MiniSpotify.Properties;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System.Net.Http.Headers;
using System.Reflection;

namespace MiniSpotify
{
    public partial class Form1 : Form
    {
        private static EmbedIOAuthServer? _server;
        private static SpotifyClient? _spotify;

        private static string clientId = "35e3a33ec429452bb9bed1795addf566";
        private static string clientSecret = "8331fc3ebd7e45c48898c1b298e3a1c7";

        private string? accessToken;
        private string? refreshToken;
        private AuthorizationCodeTokenResponse? token;
        private MediaManager _mediaManager;
        private HttpClient _client;
        public Form1()
        {
            var syncContext = new WindowsFormsSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(syncContext);
            this.CreateControl();
            InitializeComponent();
            Location = new Point(Screen.PrimaryScreen!.WorkingArea.Right - 390, 15);
        }
        private void OnMediaChanged(MediaInfo info)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnMediaChanged(info));
                return;
            }
            label1.Text = info.Title;
            label2.Text = info.Artist;
            if (info.ThumbnailBytes != null)
            {
                try
                {
                    using var ms = new MemoryStream(info.ThumbnailBytes);
                    var oldImage = pictureBox1.Image;
                    pictureBox1.BackgroundImage = System.Drawing.Image.FromStream(ms);
                    oldImage?.Dispose();
                }
                catch { pictureBox1.Image = AlbumArtHelper.DefaultArt; }
            }
            else
            {
                pictureBox1.Image = AlbumArtHelper.DefaultArt;
            }
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            TopMost = true;
            ShowInTaskbar = false;
            likeBtn.BackgroundImage = Resources.music;
            _client = new HttpClient();
            _mediaManager = new MediaManager();
            await _mediaManager.Start();
            _mediaManager.MediaChanged += OnMediaChanged;
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
                accessToken = newToken.AccessToken;
                _spotify = new SpotifyClient(newToken.AccessToken);
                var profile = await _spotify.UserProfile.Current();
                //MessageBox.Show(profile.DisplayName);
            }
            catch (Exception)
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
                    Scopes.UserLibraryModify,
                    Scopes.UserLibraryRead,
                    Scopes.AppRemoteControl,
                    Scopes.PlaylistModifyPublic,
                    Scopes.PlaylistReadPrivate,
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
            await File.WriteAllTextAsync("D://C#/MiniSpotify/token.txt", tokenResponse.RefreshToken);
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            Win32.SetRoundedCorner(Handle);
        }
        private async void likeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var current = await _spotify!.Player.GetCurrentPlayback();
                if (current?.Item is not FullTrack track)
                    return;
                _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                var uri = $"https://api.spotify.com/v1/me/library?uris=spotify:track:{track.Id}";
                var response = await _client.PutAsync(uri, null);
                var body = await response.Content.ReadAsStringAsync();
                likeBtn.BackgroundImage = Resources.musicLoveFix;
            }
            catch (APIException ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.Response?.StatusCode);
            }
        }
    }
}
