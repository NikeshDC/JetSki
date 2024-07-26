using System;

namespace Animation
{

    public interface IAnimationPlayer
    {
        public void Play(string animationName, bool restart = false);
        public void Play(string animationName, Action onAnimationEnd, bool restart = false);
        public void Stop();

        public AnimationStatus currentAnimationStatus { get; }


        public readonly struct AnimationStatus
        {
            public readonly IAnimation animation;
            public readonly float elapsedTime;

            public AnimationStatus(IAnimation animation, float elapsedTime)
            {
                this.animation = animation;
                this.elapsedTime = elapsedTime;
            }
        }
    }
}