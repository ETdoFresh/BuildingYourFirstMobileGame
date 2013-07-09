#region File Description
//-----------------------------------------------------------------------------
// AnimationPlayer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

#endregion

namespace SkinnedModelData
{
    /// <summary>
    /// The animation player is in charge of decoding bone position
    /// matrices from an animation clip.
    /// </summary>
    public class AnimationPlayer
    {
        #region Fields


        // Information about the currently playing animation clip.
        private AnimationClipInstance _currentAnimationClip;
        private AnimationClipInstance _targetAnimationClip;

        private TimeSpan _totalBlendTime;
        private TimeSpan _currentBlendTime;

        float animationSpeed = 1.0f;

        public bool IsPauzed { get; private set; }
        public bool IsLooped { get; private set; }


        // Current animation transform matrices.
        Matrix[] boneTransforms;
        Matrix[] worldTransforms;
        Matrix[] skinTransforms;


        // Backlink to the bind pose and skeleton hierarchy data.
        SkinningData skinningDataValue;

        public event Action<string> AnimationComplete;


        #endregion


        /// <summary>
        /// Constructs a new animation player.
        /// </summary>
        public AnimationPlayer(SkinningData skinningData)
        {
            if (skinningData == null)
                throw new ArgumentNullException("skinningData");

            skinningDataValue = skinningData;

            boneTransforms = new Matrix[skinningData.BindPose.Count];
            worldTransforms = new Matrix[skinningData.BindPose.Count];
            skinTransforms = new Matrix[skinningData.BindPose.Count];

            _currentAnimationClip = new AnimationClipInstance(skinningData.BindPose.Count);
            _targetAnimationClip = new AnimationClipInstance(skinningData.BindPose.Count);

            _currentAnimationClip.AnimationComplete += (animName) => { if (AnimationComplete != null) AnimationComplete(animName); };
            _targetAnimationClip.AnimationComplete += (animName) => { if (AnimationComplete != null) AnimationComplete(animName); };
        }

        public void StartClip(AnimationClip clip)
        {
            StartClip(clip,true,0f);
        }

        public void StartClip(AnimationClip clip, bool loop)
        {
            StartClip(clip, loop, 0f);
        }

        /// <summary>
        /// Starts decoding the specified animation clip.
        /// </summary>
        public void StartClip(AnimationClip clip, bool loop, float blendTime)
        {
            if(IsPauzed)
            {
                IsPauzed = false;
                return;
            }

            if(clip==null)
                throw new ArgumentNullException("clip");

            if (_currentAnimationClip.Clip == clip || _targetAnimationClip.Clip == clip) return;

            if (blendTime>0 && _currentAnimationClip.Clip != null)
            {
                _currentBlendTime = TimeSpan.Zero;
                _totalBlendTime = TimeSpan.FromSeconds(blendTime);
                _targetAnimationClip.SetAnimationClip(clip, loop);
                skinningDataValue.BindPose.CopyTo(_targetAnimationClip.BoneTransforms, 0);
            }
            else
            {
                _targetAnimationClip.Reset(true);
                _currentAnimationClip.SetAnimationClip(clip, loop);
                skinningDataValue.BindPose.CopyTo(_currentAnimationClip.BoneTransforms, 0);
            }
        }

        public void StartClip()
        {
            if(IsPauzed)
            {
                IsPauzed = false;
            }
        }

        public void PauzeClip()
        {
            IsPauzed = true;
        }


        /// <summary>
        /// Advances the current animation position.
        /// </summary>
        public void Update(TimeSpan time, bool relativeToCurrentTime,
                           Matrix rootTransform)
        {
            UpdateBoneTransforms(time, relativeToCurrentTime);      
            UpdateWorldTransforms(rootTransform);
            UpdateSkinTransforms();
        }


        public void SetAnimationSpeed(float speedScale)
        {
            if (speedScale < 0) speedScale = 0;

            animationSpeed = speedScale;
        }

        /// <summary>
        /// Helper used by the Update method to update the bonetransforms based on the current time
        /// </summary>
        /// <param name="time"></param>
        /// <param name="relativeToCurrentTime"></param>
        private void UpdateBoneTransforms(TimeSpan time, bool relativeToCurrentTime)
        {
            if(_currentAnimationClip.Clip==null)
            {
                boneTransforms = skinningDataValue.BindPose.ToArray();
                return;
            }

            time = TimeSpan.FromMilliseconds(time.TotalMilliseconds*animationSpeed);

            _currentAnimationClip.UpdateBoneTransforms(time, relativeToCurrentTime);

            if (_targetAnimationClip.Clip == null)
            {
                _currentAnimationClip.BoneTransforms.CopyTo(boneTransforms, 0);
            }
            else
            {
                _targetAnimationClip.UpdateBoneTransforms(time, relativeToCurrentTime);

                _currentBlendTime += time;

                var blendFactor = (float) (_currentBlendTime.TotalSeconds/_totalBlendTime.TotalSeconds);

                if(blendFactor>=1.0f)
                {
                    _currentAnimationClip.CopyFrom(_targetAnimationClip);
                    _targetAnimationClip.Reset(true);
                    blendFactor = 1.0f;
                }

                Quaternion currentRotation, targetRotation, finalRotation;
                Vector3 currentTranslation, targetTranslation, finalTranslation;

                for(var i=0; i<boneTransforms.Length;++i)
                {
                    //ROTATION LERP
                    currentRotation =
                        Quaternion.CreateFromRotationMatrix(_currentAnimationClip.BoneTransforms[i]);
                    targetRotation =
                        Quaternion.CreateFromRotationMatrix(_targetAnimationClip.BoneTransforms[i]);

                    Quaternion.Slerp(ref currentRotation, ref targetRotation, blendFactor, out finalRotation);

                    //TRANSLATION LERP
                    currentTranslation = _currentAnimationClip.BoneTransforms[i].Translation;
                    targetTranslation = _targetAnimationClip.BoneTransforms[i].Translation;

                    Vector3.Lerp(ref currentTranslation, ref targetTranslation, blendFactor, out finalTranslation);

                    //FINAL MATRIX
                    boneTransforms[i] = Matrix.CreateFromQuaternion(finalRotation)*
                                        Matrix.CreateTranslation(finalTranslation);
                }
            }

            
        }


        /// <summary>
        /// Helper used by the Update method to refresh the WorldTransforms data.
        /// </summary>
        private void UpdateWorldTransforms(Matrix rootTransform)
        {
            // Root bone.
            worldTransforms[0] = boneTransforms[0] * rootTransform;

            // Child bones.
            for (int bone = 1; bone < worldTransforms.Length; bone++)
            {
                int parentBone = skinningDataValue.SkeletonHierarchy[bone];

                worldTransforms[bone] = boneTransforms[bone] *
                                             worldTransforms[parentBone];
            }
        }


        /// <summary>
        /// Helper used by the Update method to refresh the SkinTransforms data.
        /// </summary>
        private void UpdateSkinTransforms()
        {
            for (int bone = 0; bone < skinTransforms.Length; bone++)
            {
                skinTransforms[bone] = skinningDataValue.InverseBindPose[bone] *
                                            worldTransforms[bone];
            }
        }


        /// <summary>
        /// Gets the current bone transform matrices, relative to their parent bones.
        /// </summary>
        public Matrix[] GetBoneTransforms()
        {
            return boneTransforms;
        }


        /// <summary>
        /// Gets the current bone transform matrices, in absolute format.
        /// </summary>
        public Matrix[] GetWorldTransforms()
        {
            return worldTransforms;
        }


        /// <summary>
        /// Gets the current bone transform matrices,
        /// relative to the skinning bind pose.
        /// </summary>
        public Matrix[] GetSkinTransforms()
        {
            return skinTransforms;
        }

        public Matrix GetBoneTransform(string boneName)
        {
            var index = skinningDataValue.BoneNames.IndexOf(boneName);
            if (index >= 0) return worldTransforms[index];

            return Matrix.Identity;
        }


        /// <summary>
        /// Gets the clip currently being decoded.
        /// </summary>
        public AnimationClip CurrentClip
        {
            get { return _currentAnimationClip.Clip; }
        }


        /// <summary>
        /// Gets the current play position.
        /// </summary>
        public TimeSpan CurrentTime
        {
            get { return _currentAnimationClip.CurrentTime; }
        }
    }
}
