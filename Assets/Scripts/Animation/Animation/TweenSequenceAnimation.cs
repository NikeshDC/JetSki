using UnityEngine;


namespace Animation
{
    [CreateAssetMenu(fileName = "TweenSequence_", menuName = "MyAnimation/TweenSequence")]
    public class TweenSequenceAnimation : BaseAnimation
    {
        public enum AnimationType { Parallel, Serial };
        [SerializeField] private AnimationType _animationType = AnimationType.Parallel;
        public AnimationType animationType => _animationType;

        [SerializeField] private TweenAnimation[] _animations;
        public TweenAnimation[] animations => _animations;

        public override float duration { get { SetDuration(); return _duration; } }

        public void SetDuration()
        {
            _duration = 0f;
            foreach (var animation in animations)
            {
                _duration += animation.duration;
            }
        }
    }

}