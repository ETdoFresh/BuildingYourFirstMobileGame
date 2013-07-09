using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildingYourFirstMobileGame.Engine.Objects
{
    class GameAnimatedSprite : GameSprite
    {
        private readonly int _rowCount;
        private readonly int _columnCount;
        private int _totalFrameTime;
        private Rectangle _frameRect;

        public int NumFrames { get; private set; }
        public Point FrameSize { get; private set; }
        public int CurrentFrame { get; private set; }
        public bool IsPlaying { get; private set; }
        public bool IsPaused { get; private set; }

        public int FrameInterval { get; set; }
        public bool IsLooping { get; set; }

        public GameAnimatedSprite(string assetFile, int numFrames, int frameInterval, Point frameSize) :
            this(assetFile, numFrames, frameInterval, frameSize, numFrames) { }

        public GameAnimatedSprite(string assetFile, int numFrames, int frameInterval, Point frameSize, int framesPerRow) :
            base(assetFile)
        {
            NumFrames = numFrames;
            FrameInterval = frameInterval;
            FrameSize = frameSize;

            _frameRect = new Rectangle(0, 0, frameSize.X, frameSize.Y);
            _rowCount = 1;
            _columnCount = numFrames;

            if (framesPerRow < numFrames)
            {
                _rowCount = numFrames / framesPerRow;
                _columnCount = framesPerRow;
            }

            DrawRect = _frameRect;
        }

        public void PlayAnimation()
        {
            PlayAnimation(false);
        }

        public void PlayAnimation(bool loop)
        {
            if (IsPaused)
            {
                IsPaused = false;
                return;
            }

            IsPlaying = true;
            IsLooping = loop;
        }

        public void StopAnimation()
        {
            IsPlaying = false;
            CurrentFrame = 0;
            _totalFrameTime = 0;
        }

        public void PauseAnimation()
        {
            IsPaused = true;
        }

        public override void Update(RenderContext renderContext)
        {
            if (IsPlaying && !IsPaused)
            {
                _totalFrameTime += renderContext.GameTime.ElapsedGameTime.Milliseconds;
                if (_totalFrameTime >= FrameInterval)
                {
                    _totalFrameTime = 0;
                    if (_rowCount > 1)
                    {
                        _frameRect.Location = new Point(FrameSize.X * (CurrentFrame % _columnCount), FrameSize.Y * (int)Math.Floor((decimal)CurrentFrame / _columnCount));
                    }
                    else _frameRect.Location = new Point(
                        FrameSize.X * CurrentFrame, 0);

                    DrawRect = _frameRect;
                    ++CurrentFrame;
                    if (CurrentFrame >= NumFrames)
                    {
                        CurrentFrame = 0;
                        IsPlaying = IsLooping;
                    }
                }
            }
            base.Update(renderContext);
        }
    }
}
