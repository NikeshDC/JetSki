using UnityEngine;
using DG.Tweening;

namespace Animation
{

    public class TweenAnimationPlayer : BaseAnimationPlayer
    {
        [SerializeField] private Transform target;
        [SerializeField][AnimationLabel] private TweenAnimation[] animations;

        private Tween usedTween;

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

        protected override void Play(IAnimation animationToPlay) => PlayAnimation(animationToPlay as TweenAnimation);
        private void PlayAnimation(TweenAnimation animationToPlay)
        {
            Vector3 endValue = animationToPlay.endValue;
            float duration = animationToPlay.duration;
            TweenAnimation.Space transformationSpace = animationToPlay.space;

            int numLoops = animationToPlay.numLoops;
            LoopType loopType = animationToPlay.loopType;
            bool isRelative = animationToPlay.isRelative;
            TweenParams tweenParams = new TweenParams().SetLoops(numLoops, loopType).SetRelative(isRelative).OnComplete(OnAnimationEndCallback);
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
                    usedTween = target.DOScale(endValue, duration).SetAs(tweenParams);
                    break;
                case TweenAnimation.Type.Rotate:
                    if (transformationSpace == TweenAnimation.Space.Local)
                    {
                        usedTween = target.DOLocalRotate(endValue, duration, animationToPlay.rotationMode).SetAs(tweenParams);
                    }
                    else
                    {
                        usedTween = target.DORotate(endValue, duration, animationToPlay.rotationMode).SetAs(tweenParams);
                    }
                    break;
                case TweenAnimation.Type.Move:
                    if (transformationSpace == TweenAnimation.Space.Local)
                    {
                        usedTween = target.DOLocalMove(endValue, duration).SetAs(tweenParams);
                    }
                    else
                    {
                        usedTween = target.DOMove(endValue, duration).SetAs(tweenParams);
                    }
                    break;
                case TweenAnimation.Type.Jump:
                    if (transformationSpace == TweenAnimation.Space.Local)
                    {
                        usedTween = target.DOLocalJump(endValue, animationToPlay.jumpPower, 1, duration).SetAs(tweenParams);
                    }
                    else
                    {
                        usedTween = target.DOJump(endValue, animationToPlay.jumpPower, 1, duration).SetAs(tweenParams);
                    }
                    break;
                default:
                    Debug.Log("Unsupported tween animation type");
                    break;
            }

        }

        public override void OnStop()
        {
            if (usedTween != null)
            {
                usedTween.Kill();
                usedTween = null;
            }
        }
    }

}