using System;
using UnityEngine;

namespace Animation
{

    public class GroupAnimationPlayer : BaseAnimationPlayer
    {
        [Serializable]
        public class AnimationGroup : IAnimation
        {
            [SerializeField] private string _name;
            public string name => _name;

            public float duration => -1f;  //cannot determine

            public enum AnimationType { Parallel, Serial };
            [SerializeField] private AnimationType _animationType = AnimationType.Parallel;
            public AnimationType animationType => _animationType;

            [Serializable]
            public class Animation
            {
                public string animationName;
                public BaseAnimationPlayer animationPlayer;
            }

            [SerializeField] private Animation[] _animations;
            public Animation[] animations => _animations;

            public void Play(int index)
            { animations[index].animationPlayer.Play(animations[index].animationName); }
            public void Play(int index, Action onAnimationEnd)
            { animations[index].animationPlayer.Play(animations[index].animationName, onAnimationEnd); }
        }

        public AnimationGroup[] animations;
        protected override IAnimation[] GetAnimations() => animations;

        private int currentSerialAnimationIndex;  //keep track of which serial animation is playing
        private int currentParallelAnimationCallbacksReceived;  //keep track of how many callbacks have been received for the parallel animations


        protected override void Play(IAnimation animationToPlay)
        {
            AnimationGroup animationGroupToPlay = (AnimationGroup)animationToPlay;

            if (animationGroupToPlay.animationType == AnimationGroup.AnimationType.Parallel)
            {
                currentParallelAnimationCallbacksReceived = 0;
                foreach (AnimationGroup.Animation animation in animationGroupToPlay.animations)
                { animation.animationPlayer.Play(animation.animationName, OnParallelAnimationEndCallback); }
            }
            else if (animationGroupToPlay.animationType == AnimationGroup.AnimationType.Serial)
            {
                currentSerialAnimationIndex = 0;
                animationGroupToPlay.Play(0, OnSerialAnimationEndCallback);
            }

        }
        public void OnSerialAnimationEndCallback()
        {
            AnimationGroup currentAnimation = (AnimationGroup)base.currentAnimation;

            currentSerialAnimationIndex++;
            if (currentSerialAnimationIndex < currentAnimation.animations.Length)
            {
                currentAnimation.Play(currentSerialAnimationIndex, OnSerialAnimationEndCallback);
            }
            else
            {//when all animations on series have been played call the callback function
                OnAnimationEndCallback();
            }
        }
        public void OnParallelAnimationEndCallback()
        {
            AnimationGroup currentAnimation = (AnimationGroup)base.currentAnimation;

            currentParallelAnimationCallbacksReceived++;
            if (currentParallelAnimationCallbacksReceived >= currentAnimation.animations.Length)
            {
                OnAnimationEndCallback();
            }
        }

        protected override bool CanPlay(IAnimation animationToPlay)
        {
            AnimationGroup animationGroupToPlay = (AnimationGroup)animationToPlay;
            if (animationGroupToPlay.animations == null || animationGroupToPlay.animations.Length == 0)
            {
                Debug.LogError("Animation group \"" + animationGroupToPlay.name + "\" has no animations");
                return false;
            }
            foreach (AnimationGroup.Animation animation in animationGroupToPlay.animations)
            {
                if (animation.animationPlayer == null)
                {
                    Debug.LogError("Animation group \"" + animationGroupToPlay.name + "\" has a null animationPlayer");
                    return false;
                }
                if (animation.animationName == "")
                {
                    Debug.LogError("Animation group \"" + animationGroupToPlay.name + "\" has a null animation name");
                    return false;
                }
            }

            return true;
        }

        public override void OnStop()
        {
            AnimationGroup currentAnimation = (AnimationGroup)base.currentAnimation;
            foreach (AnimationGroup.Animation animation in currentAnimation.animations)
            { animation.animationPlayer.Stop(); }
        }
    }
}