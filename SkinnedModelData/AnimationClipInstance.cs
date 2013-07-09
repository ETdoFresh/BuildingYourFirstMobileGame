using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SkinnedModelData
{
    class AnimationClipInstance
    {
        public AnimationClip Clip { get; private set; }
        public event Action<string> AnimationComplete;

        public TimeSpan CurrentTime
        {
            get { return _currentAnimationTime; }
        }

        public Matrix[] BoneTransforms
        {
            get { return _boneTransforms; }
        }

        public bool IsLooped { get; private set; }

        private TimeSpan _currentAnimationTime;
        private int _currentKeyFrame;
        private Matrix[] _boneTransforms;

        public AnimationClipInstance(int boneCount)
        {
            _boneTransforms = new Matrix[boneCount];
        }


        public void SetAnimationClip(AnimationClip clip)
        {
            SetAnimationClip(clip,true);
        }

        public void SetAnimationClip(AnimationClip clip, bool isLooped)
        {
            Clip = clip;
            IsLooped = isLooped;
            Reset(false);
        }

        public void CopyFrom(AnimationClipInstance clipInstance)
        {
            Clip = clipInstance.Clip;
            IsLooped = clipInstance.IsLooped;
            _currentAnimationTime = clipInstance._currentAnimationTime;
            _currentKeyFrame = clipInstance._currentKeyFrame;
            clipInstance._boneTransforms.CopyTo(_boneTransforms, 0);
        }

        public void Reset(bool removeClip)
        {
            if (removeClip) Clip = null;

            _currentAnimationTime = TimeSpan.Zero;
            _currentKeyFrame = 0;
        }

        public void UpdateBoneTransforms(TimeSpan elapsedTime, bool relativeToCurrentTime)
        {
            if (Clip == null) return;

            if (relativeToCurrentTime) _currentAnimationTime += elapsedTime;
            else _currentAnimationTime = elapsedTime;

            if (_currentAnimationTime > Clip.Duration)
            {
                if (IsLooped)
                {
                    _currentAnimationTime -= Clip.Duration;
                    _currentKeyFrame = 0;
                }
                else
                {
                    _currentAnimationTime = Clip.Duration;
                    if (AnimationComplete != null) AnimationComplete(Clip.Name);
                }
            }

            while (_currentKeyFrame < Clip.Keyframes.Count)
            {
                var keyFrame = Clip.Keyframes[_currentKeyFrame];

                if (keyFrame.Time > _currentAnimationTime)
                    break;

                _boneTransforms[keyFrame.Bone] = keyFrame.Transform;

                ++_currentKeyFrame;
            }
        }
    }
}
