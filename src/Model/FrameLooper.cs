using System;

namespace QMEditor.Model;

public class FrameLooper {

    public int CurrentFrame { get => _currentFrame; }
    public Action<int> FrameChanged;

    private readonly int _frames;
    private readonly float _frameDuration;
    
    private int _currentFrame;
    private float _secondsElapsedSincePreviousFrame;
    private bool _paused = false;

    public FrameLooper(int frames, float frameDuration) {
        _frames = frames;
        _frameDuration = frameDuration;
        _currentFrame = 0;
    }

    public static FrameLooper FromAppSettings() => new FrameLooper(AppSettings.RenderFrameCount.Get(), AppSettings.RenderFrameDuration.Get()/1000f);

    public void Update(float secondsElapsed) {
        if (_paused) return;

        _secondsElapsedSincePreviousFrame += secondsElapsed;
        if (_secondsElapsedSincePreviousFrame < _frameDuration) return;

        _secondsElapsedSincePreviousFrame -= _frameDuration;
        NextFrame();
    }

    public void NextFrame() {
        _currentFrame++;
        if (_currentFrame >= _frames)
            _currentFrame = _currentFrame % _frames;
        FrameChanged?.Invoke(_currentFrame);
    }

    public void PrevFrame() {
        _currentFrame--;
        if (_currentFrame < 0)
            _currentFrame = _frames + _currentFrame % _frames;
        FrameChanged?.Invoke(_currentFrame);
    }

    public void TogglePause() {
        _paused = !_paused;
    }

}