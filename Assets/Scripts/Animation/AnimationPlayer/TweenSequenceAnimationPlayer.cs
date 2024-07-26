using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Animation
{

    public class TweenSequenceAnimationPlayer : BaseAnimationPlayer
    {
        [SerializeField] private Transform target;
        [SerializeField][AnimationLabel] private TweenSequenceAnimation[] animations;

        private List<Tween> usedTweens = new List<Tween>();
        private int currentSerialAnimationIndex;  //keep track of which serial animation is playing
        private int currentParallelAnimationCallbacksReceived;  //keep track of how many callbacks have been received for the parallel animations

        protected override IAnimation[] GetAnimations() => animations;

        protected override bool CanPlay(IAnimation animationToPlay)
        {
            if (target == null)
            {
                Debug.LogError("Transform target for tweening can't be null");
                return false;
            }
            return true;
        }

        protected override void Play(IAnimation animationToPlay) => PlayAnimation(animationToPlay as TweenSequenceAnimation);
        private void PlayAnimation(TweenSequenceAnimation animationToPlay)
        {
            if (animationToPlay.animationType == TweenSequenceAnimation.AnimationType.Parallel)
            {
                currentParallelAnimationCallbacksReceived = 0;
                foreach (TweenAnimation animation in animationToPlay.animations)
                { PlayTweenAnimation(animation, OnParallelAnimationEndCallback); }
            }
            else if (animationToPlay.animationType == TweenSequenceAnimation.AnimationType.Serial)
            {
                currentSerialAnimationIndex = 0;
                PlayTweenAnimation(animationToPlay.animations[0], OnSerialAnimationEndCallback);
            }
        }
        public void OnSerialAnimationEndCallback()
        {
            TweenSequenceAnimation currentAnimation = (TweenSequenceAnimation)base.currentAnimation;

            currentSerialAnimationIndex++;
            if (currentSerialAnimationIndex < currentAnimation.animations.Length)
            {
                PlayTweenAnimation(currentAnimation.animations[currentSerialAnimationIndex], OnSerialAnimationEndCallback);
            }
            else
            {//when all animations on series have been played call the callback function
                OnAnimationEndCallback();
            }
        }
        public void OnParallelAnimationEndCallback()
        {
            TweenSequenceAnimation currentAnimation = (TweenSequenceAnimation)base.currentAnimation;

            currentParallelAnimationCallbacksReceived++;
            if (currentParallelAnimationCallbacksReceived >= currentAnimation.animations.Length)
            {
                OnAnimationEndCallback();
            }
        }

        private void PlayTweenAnimation(TweenAnimation animationToPlay, TweenCallback onAnimationEnd)
        {
            Vector3 endValue = animationToPlay.endValue;
            float duration = animationToPlay.duration;
            TweenAnimation.Space transformationSpace = animationToPlay.space;

            int numLoops = animationToPlay.numLoops;
            LoopType loopType = animationToPlay.loopType;
            bool isRelative = animationToPlay.isRelative;
            TweenParams tweenParams = new TweenParams().SetLoops(numLoops, loopType).SetRelative(isRelative).OnComplete(onAnimationEnd);

            if (animationToPlay.useCustomEaseCurve)
            {
                tweenParams.SetEase(animationToPlay.easeCurve);
            }
            else
            {
                tweenParams.SetEase(animationToPlay.ease);
            }

            switch (animationToPlay.type)
            {
                case TweenAnimation.Type.Scale:
                    usedTweens.Add(target.DOScale(endValue, duration).SetAs(tweenParams));
                    break;
                case TweenAnimation.Type.Rotate:
                    if (transformationSpace == TweenAnimation.Space.Local)
                    {
                        usedTweens.Add(target.DOLocalRotate(endValue, duration, animationToPlay.rotationMode).SetAs(tweenParams));
                    }
                    else
                    {
                        usedTweens.Add(target.DORotate(endValue, duration, animationToPlay.rotationMode).SetAs(tweenParams));
                    }
                    break;
                case TweenAnimation.Type.Move:
                    if (transformationSpace == TweenAnimation.Space.Local)
                    {
                        usedTweens.Add(target.DOLocalMove(endValue, duration).SetAs(tweenParams));
                    }
                    else
                    {
                        usedTweens.Add(target.DOMove(endValue, duration).SetAs(tweenParams));
                    }
                    break;
                case TweenAnimation.Type.Jump:
                    if (transformationSpace == TweenAnimation.Space.Local)
                    {
                        usedTweens.Add(target.DOLocalJump(endValue, animationToPlay.jumpPower, 1, duration).SetAs(tweenParams));
                    }
                    else
                    {
                        usedTweens.Add(target.DOJump(endValue, animationToPlay.jumpPower, 1, duration).SetAs(tweenParams));
                    }
                    break;
                default:
                    Debug.Log("Unsupported tween animation type");
                    break;
            }

        }

        public override void OnStop()
        {
            if (usedTweens.Count > 0)
            {
                foreach (Tween usedTween in usedTweens)
                { usedTween.Kill(); }
                usedTweens.Clear();
            }
        }
    }
}