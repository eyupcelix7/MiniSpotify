using MiniSpotify.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Media.Control;

namespace MiniSpotify
{
    public class MediaManager: IDisposable
    {
        private GlobalSystemMediaTransportControlsSessionManager? _manager;
        private GlobalSystemMediaTransportControlsSession? _currentSession;
        private bool _started, _disposed;
        private byte[]? _lastThumbnailBytes;
        private string? _lastTitle;     
        private string? _lastArtist;
        public event Action<MediaInfo>? MediaChanged;

        public async Task Start()
        {
            if (_started)
                return;
            _started = true;
            try
            {
                _manager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
                return;
            }
            var spotifySession = _manager.GetSessions()
                .FirstOrDefault(x => x.SourceAppUserModelId.Contains("Spotify.exe")); // Spotify oturumunu seçer
            if (spotifySession == null)
            {
                MessageBox.Show("Lütfen Spotifyı Açın.");
                Application.Exit();
                return;
            }
            await AttachToSession(spotifySession); // Doğru oturuma bağlanır
        }
        public async Task AttachToSession(GlobalSystemMediaTransportControlsSession? session)
        {
            if (_currentSession != null)
            {
                _currentSession.MediaPropertiesChanged -= OnMediaPropertiesChanged; // Eski aboneliği temizler
                _currentSession.PlaybackInfoChanged -= OnPlaybackInfoChanged; // Eski aboneliği temizler
            }
            _currentSession = session;
            if (_currentSession != null)
            {
                _currentSession.MediaPropertiesChanged += OnMediaPropertiesChanged;
                _currentSession.PlaybackInfoChanged += OnPlaybackInfoChanged;

            }
            //var x = _currentSession.GetPlaybackInfo();
            //var dd = await _currentSession.TryGetMediaPropertiesAsync();

            await NotifyMediaChanged();
        }
        public void OnMediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
        {
            _ = NotifyMediaChanged();
        }
        private void OnPlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender,PlaybackInfoChangedEventArgs args)
        {
            _ = NotifyMediaChanged();
        }
        public async Task NotifyMediaChanged()
        {
            var session = _currentSession;
            if (session == null) return;
            var playbackInfo = session.GetPlaybackInfo();
            var props = await session.TryGetMediaPropertiesAsync();
            if (props == null) return;
            var title = props.Title ?? "Bulunamadı";
            var artist = props.Artist ?? "Bulunamadı";
            var thumbnailBytes = await AlbumArtHelper.GetThumbnailBytesAsync(props.Thumbnail);
            if (thumbnailBytes == null || thumbnailBytes.Length == 0)
            {
                if (_lastThumbnailBytes != null && _lastTitle == title && _lastArtist == artist) 
                {
                    thumbnailBytes = _lastThumbnailBytes;
                }
            }
            else
            {
                _lastThumbnailBytes = thumbnailBytes; // Başarılı resmi önbelleğe alır
                _lastTitle = title; // Son başlığı saklar
                _lastArtist = artist; // Son sanatçıyı saklar
            }
            var mediaInfo = new MediaInfo
            (
                Title: title,
                Artist: artist,
                ThumbnailBytes: thumbnailBytes,
                SourceAppUserModelId: session.SourceAppUserModelId,
                PlaybackStatus: playbackInfo.PlaybackStatus,
                Session: session
            );
            MediaChanged?.Invoke(mediaInfo);
        }
        public async Task TogglePlayPause()
        {
            var session = _currentSession;
            if (session == null) return;
            //await NotifyMediaChanged();
            await session.TryTogglePlayPauseAsync();
        }
        public Task<bool> IsPlaying()
        {
            var status = _currentSession?.GetPlaybackInfo().PlaybackStatus;
            var isPlaying = status == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing;
            return Task.FromResult(isPlaying);
        }
        public async Task Next()
        {
            var session = _currentSession;
            if (session == null)
                return;
            try
            {
                await session.TrySkipNextAsync();
            }
            catch (Exception)
            {
            }
        }
        public async Task Previous()
        {
            var session = _currentSession;
            if (session == null)
                return;
            try
            {
                await session.TrySkipPreviousAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            if (_currentSession != null)
            {
                _currentSession.MediaPropertiesChanged -= OnMediaPropertiesChanged;
                _currentSession.PlaybackInfoChanged -= OnPlaybackInfoChanged;
            }
        }
    }
}
