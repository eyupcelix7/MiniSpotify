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
            if(!_manager.GetSessions().Any(x=> x.SourceAppUserModelId.Contains("Spotify.exe")))
            {
                MessageBox.Show("Lütfen Spotifyı Açın.");
                Application.Exit();
                return;
            }
            await AttachToSession(_manager.GetCurrentSession());
        }
        public async Task AttachToSession(GlobalSystemMediaTransportControlsSession? session)
        {
            _currentSession = session;
            if(_currentSession != null)
            {
                _currentSession.MediaPropertiesChanged += OnMediaPropertiesChanged;
            }
            //var x = _currentSession.GetPlaybackInfo();
            //var dd = await _currentSession.TryGetMediaPropertiesAsync();

            await NotifyMediaChanged();
        }
        public async void OnMediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
        {
            await NotifyMediaChanged();
        }
        public async Task NotifyMediaChanged()
        {
            var session = _currentSession;
            var playbackInfo = session.GetPlaybackInfo();
            var props = await session.TryGetMediaPropertiesAsync();
            if (props == null) return;
            var thumbnailBytes = await AlbumArtHelper.GetThumbnailBytesAsync(props.Title, props.Artist, props.Thumbnail);
            var mediaInfo = new MediaInfo
            (
                Title: props.Title ?? "Bulunamadı",
                Artist: props.Artist ?? "Bulunamadı",
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
            await session.TryTogglePlayPauseAsync();
        }
        public async Task<bool> IsPlaying()
        {
            var session = _currentSession;
            var status = _currentSession!.GetPlaybackInfo().PlaybackStatus;
            if (status == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                return true;
            else
                return false;
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
            throw new NotImplementedException();
        }
    }
}
