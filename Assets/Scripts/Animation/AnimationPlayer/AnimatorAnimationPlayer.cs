using System.Collections;
using UnityEngine;

namespace Animation
{

    public class AnimatorAnimationPlayer : BaseAnimationPlayer
    {
        [SerializeField] private Animator _animator;
        public Animator animator => _animator;

        public enum AnimationDurationType { ForceDurationForAll, NotEnforced, AnimationParameter }
        [SerializeField] private AnimationDurationType animationDurationType;

        [SerializeField][AnimationLabel] private AnimatorAnimation[] animations;

        private float currentAnimationLength;
        private bool animationEndCallbackCalled;

        public static string GetStateSpeedParameter(string layerName, string stateName)
        {
            return layerName + "." + stateName + "." + "speed";
        }

        protected override IAnimation[] GetAnimations() => animations;

        protected override bool CanPlay(IAnimation animationToPlay)
        {
            AnimatorAnimation animation = animationToPlay as AnimatorAnimation;

            if (animator == null)
            {
                Debug.LogError("Target animator has not been set");
                return false;
            }

            int layerIndex = animator.GetLayerIndex(animation.layerName);
            if (layerIndex == -1)
            {
                Debug.LogError("Layer name \"" + animation.layerName + "\" is wrong");
                return false;
            }
            int stateHash = animation.fullPathHash;
            if (animator.HasState(layerIndex, stateHash))
            {
                return true;
            }
            else
            {
                Debug.LogError("State name \"" + animation.stateName + "\" is wrong");
                return false;
            }
        }

        protected override void Play(IAnimation animationToPlay) => PlayAnimation(animationToPlay as AnimatorAnimation);
        private void PlayAnimation(AnimatorAnimation animationToPlay)
        {
            animator.speed = 1.0f;
            currentAnimationLength = animationToPlay.duration;
            animationEndCallbackCalled = false;

            int layerIndex = animator.GetLayerIndex(animationToPlay.layerName);
            int stateHash = animationToPlay.fullPathHash;
            //play animation through animator
            if (animationToPlay.crossFade)
            { animator.CrossFadeInFixedTime(stateHash, animationToPlay.crossFadeDuration); }
            else
            { animator.Play(stateHash, layerIndex); }

            if (animationDurationType == AnimationDurationType.ForceDurationForAll ||
                    (animationDurationType == AnimationDurationType.AnimationParameter && animationToPlay.forceDuration))
            {
                SetStateSpeedToMatchAnimation(animationToPlay, true);
            }
            else
            {
                ResetSpeedParameter(animationToPlay.layerName, animationToPlay.stateName);
            }
        }

        private void SetStateSpeedToMatchAnimation(AnimatorAnimation animation, bool crossFade)
        {
            if (SetStateSpeedToMatchAnimation_routine != null)
            { StopCoroutine(SetStateSpeedToMatchAnimation_routine); }
            SetStateSpeedToMatchAnimation_routine = StartCoroutine(C_SetStateSpeedToMatchAnimation(animation, crossFade));
        }
        private Coroutine SetStateSpeedToMatchAnimation_routine = null;
        private IEnumerator C_SetStateSpeedToMatchAnimation(AnimatorAnimation animation, bool crossFade)
        {//set speed so that animation at the given state will finish in animationDuration
            yield return null;  //wait for a frame to get information 

            int layerIndex = animator.GetLayerIndex(animation.layerName);
            AnimatorStateInfo stateInfo;
            if (!crossFade)
            {
                stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
            }
            else
            {
                stateInfo = animator.GetNextAnimatorStateInfo(layerIndex);
                if (stateInfo.fullPathHash == 0) //no next state; crossfade has already finished
                {
                    stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
                }
            }

            string speedParemeter = GetStateSpeedParameter(animation.layerName, animation.stateName);
            float desiredSpeedMultiplier = stateInfo.length * stateInfo.speedMultiplier / animation.duration;
            animator.SetFloat(speedParemeter, desiredSpeedMultiplier);
            currentAnimationLength = stateInfo.length;
        }
        private void ResetSpeedParameter(string layerName, string stateName)
        {
            string speedParemeter = GetStateSpeedParameter(layerName, stateName);
            animator.SetFloat(speedParemeter, 1.0f);
        }

        public override void OnStop()
        {
            animator.speed = 0f;
        }

        public new void Update()
        {
            if (currentAnimation != null)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime > currentAnimationLength && !animationEndCallbackCalled)
                {
                    animationEndCallbackCalled = true;
                    OnAnimationEndCallback();
                }
            }
        }
    }
}