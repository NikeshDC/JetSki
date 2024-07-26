using System;
using UnityEngine;

namespace Animation
{

    public abstract class BaseAnimationPlayer : MonoBehaviour, IAnimationPlayer
    {
        protected abstract IAnimation[] GetAnimations();
        protected IAnimation currentAnimation;
        protected float elapsedTime = 0f;

        protected Action OnAnimationEnd;

        public IAnimationPlayer.AnimationStatus currentAnimationStatus => new IAnimationPlayer.AnimationStatus(currentAnimation, elapsedTime);

        private IAnimation GetAnimationFromName(string animationName)
        {
            IAnimation[] animations = GetAnimations();
            foreach (IAnimation animation in animations)
            {
                if (animation.name == animationName)
                {
                    return animation;
                }
            }
            Debug.LogError("Animation with name \"" + animationName + "\" not found!");
            return null;
        }

        public bool HasAnimationWithName(string animationName)
        {
            IAnimation[] animations = GetAnimations();
            foreach (IAnimation animation in animations)
            {
                if (animation.name == animationName)
                {
                    return true;
                }
            }
            return false;
        }

        public void Play(string animationName, bool restart = false)
        {
            Play(animationName, null, restart);
        }
        public void Play(string animationName, Action onAnimationEnd, bool restart = false)
        {
            if (!restart)
            {
                if (currentAnimation != null && currentAnimation.name == animationName)
                { return; } //if already playing animation of given name just return instead of restarting
            }

            IAnimation animationToPlay = GetAnimationFromName(animationName);
            if (animationToPlay != null && CanPlay(animationToPlay))
            {
                OnNewAnimationPlay(animationToPlay, onAnimationEnd);
                currentAnimation = animationToPlay;
                Play(animationToPlay);
            }
        }
        protected virtual void OnNewAnimationPlay(IAnimation animationToPlay, Action onAnimationEnd)
        {
            Stop();
            elapsedTime = 0f;
            this.OnAnimationEnd = onAnimationEnd;
        }

        protected abstract void Play(IAnimation animationToPlay);
        protected abstract bool CanPlay(IAnimation animationToPlay);

        public void Stop()
        {
            if (currentAnimation == null)
            { return; }

            OnStop();
            currentAnimation = null;
            elapsedTime = 0f;
            OnAnimationEnd = null;
        }
        public virtual void OnStop() { }

        public virtual void OnAnimationEndCallback()
        {
            OnAnimationEnd?.Invoke();
            currentAnimation = null;
            elapsedTime = 0f;
        }

        public void Update()
        {
            if (currentAnimation != null)
            {
                elapsedTime += Time.deltaTime;
            }
        }
    }
}