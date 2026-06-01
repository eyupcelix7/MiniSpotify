using MiniSpotify.Helper;
using MiniSpotify.Properties;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System.Net.Http.Headers;
using System.Reflection;
using Timer = System.Windows.Forms.Timer;
namespace MiniSpotify
{
    public partial class Form1 : Form
    {
        private static EmbedIOAuthServer? _server;
        private static SpotifyClient? _spotify;
        private static string clientId = "CLIENT_ID";
        private static string clientSecret = "CLIENT_SECRET";
        private string? accessToken;
        private AuthorizationCodeTokenResponse? token;
        private MediaManager _mediaManager;
        private HttpClient _client;
        private bool _isPlaying;
        // --- Animasyon için ---
        private double _opacity = 0;
        private int _fadeStep;
        private bool _fadingIn;
        private bool _fadingOut;
        private readonly Timer _fadeTimer;
        private readonly Timer _displayTimer;
        NotifyIcon _trayIcon;
        public Form1()
        {
            var syncContext = new WindowsFormsSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(syncContext);
            Opacity = 0;
            _fadeTimer = new System.Windows.Forms.Timer { Interval = 16 }; // 60fps
            _fadeTimer.Tick += OnFadeTick;

            _displayTimer = new System.Windows.Forms.Timer { Interval = 5000 };
            _displayTimer.Tick += OnDisplayTimerTick;

            // NotifyICon
            _trayIcon = new NotifyIcon
            {
                Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Text = "MiniSpotify",
                Visible = true,
                ContextMenuStrip = new ContextMenuStrip()
            };
            _trayIcon.ContextMenuStrip.Items.Add("Çıkış", null, (_, _) =>
            {
                _trayIcon.Visible = false;
                ClearDisposes();
                Application.Exit();
            });

            this.CreateControl();
            InitializeComponent();
            Location = new Point(Screen.PrimaryScreen!.WorkingArea.Right - 390, 15);
            _mediaManager = new MediaManager();
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            _mediaManager.MediaChanged += OnMediaChanged;
            await _mediaManager.Start();
            _isPlaying = await _mediaManager.IsPlaying();
            DoubleBuffered = true; // Titreşimi önlemek için
            TopMost = true;
            ShowInTaskbar = false;
            likeBtn.BackgroundImage = Resources.heartWhite;
            _client = new HttpClient();
            CheckTogglePlayBtn();
            SetTooltips();
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
                    var oldImage = pctBoxImage.BackgroundImage;
                    pctBoxImage.BackgroundImage = System.Drawing.Image.FromStream(ms);
                    oldImage?.Dispose();
                }
                catch { pctBoxImage.Image = AlbumArtHelper.DefaultArt; }
            }
            else
            {
                pctBoxImage.Image = AlbumArtHelper.DefaultArt;
            }
            if (info.Session.GetPlaybackInfo().PlaybackStatus == Windows.Media.Control.GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                _isPlaying = true;
            else
                _isPlaying = false;
            likeBtn.BackgroundImage = Resources.heartWhite;
            CheckTogglePlayBtn();
            ShowPopup();
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
                likeBtn.BackgroundImage = Resources.heart;
            }
            catch (APIException ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.Response?.StatusCode);
            }
        }
        private async void togglePlayBtn_Click(object sender, EventArgs e)
        {
            _isPlaying = !_isPlaying;
            CheckTogglePlayBtn();
            await _mediaManager.TogglePlayPause();
        }
        public void CheckTogglePlayBtn()
        {
            if (_isPlaying)
                togglePlayBtn.BackgroundImage = Resources.stop;
            else
                togglePlayBtn.BackgroundImage = Resources.start;
        }
        private async void prevBtn_Click(object sender, EventArgs e)
        {
            await _mediaManager.Previous();
        }
        private async void nextBtn_Click(object sender, EventArgs e)
        {
            await _mediaManager.Next();
        }
        private void SetTooltips()
        {
            ToolTip toolTipLike = new ToolTip();
            ToolTip toolTipPrev = new ToolTip();
            ToolTip toolTipNext = new ToolTip();
            ToolTip toolTipPlayToggle = new ToolTip();

            toolTipLike.SetToolTip(likeBtn, "Beğen");
            toolTipPrev.SetToolTip(prevBtn, "Önceki");
            toolTipNext.SetToolTip(nextBtn, "Sonraki");
            toolTipNext.SetToolTip(togglePlayBtn, "Durdur / Çal");
        }
        private void ShowPopup()
        {
            if (IsDisposed) return;

            _displayTimer.Stop();
            _fadeTimer.Stop();
            _fadingIn = false;
            _fadingOut = false;

            if (!Visible)
            {
                Opacity = 0;
                Location = new Point(Screen.PrimaryScreen!.WorkingArea.Right - 390, 15);
                Show();
                _opacity = 0;
                _fadeStep = 0;
                _fadingIn = true;
                _fadeTimer.Start();
            }
            else
            {
                Opacity = 1;

                _opacity = 1;
                _fadeStep = 0;

                _displayTimer.Start();
            }
        }
        private void OnFadeTick(object? sender, EventArgs e)
        {
            if (_fadingIn)
            {
                _fadeStep += 16;
                _opacity = Math.Min(1.0, (double)_fadeStep / 100);
                Opacity = _opacity;
                Location = new Point(
                    (Screen.PrimaryScreen!.WorkingArea.Right - 390) - (int)(10 * _opacity),
                    Location.Y
                );
                if (_fadeStep >= 100)
                {
                    _fadingIn = false;
                    _fadeTimer.Stop();
                    _displayTimer.Start(); // Görünme taömam, display süresini başlat
                }
            }
            else if (_fadingOut)
            {
                _fadeStep += 16;
                _opacity = Math.Max(0.0, 1.0 - (double)_fadeStep / 300);
                Opacity = _opacity;

                if (_fadeStep >= 300)
                {
                    _fadingOut = false;
                    _fadeTimer.Stop();
                    Hide();
                }
            }
        }
        private void OnDisplayTimerTick(object? sender, EventArgs e)
        {
            _displayTimer.Stop();
            StartFadeOut();
        }
        private void StartFadeOut()
        {
            if (_fadingOut || !Visible) return;
            _fadingOut = true;
            _fadeStep = 0;
            _fadeTimer.Start();
        }
        private void ClearDisposes()
        {
            _trayIcon?.Dispose();
            _mediaManager.Dispose();
            this.Close();
            this.Dispose();
        }
    }
}
