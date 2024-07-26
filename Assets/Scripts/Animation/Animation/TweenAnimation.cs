using UnityEngine;
using DG.Tweening;

namespace Animation
{

    [CreateAssetMenu(fileName = "TweenAnimation_", menuName = "MyAnimation/Tween")]
    public class TweenAnimation : BaseAnimation
    {
        public enum Type { Rotate, Scale, Move, Jump }
        [SerializeField] private Type _type = Type.Rotate;
        public Type type => _type;

        [SerializeField] private RotateMode _rotationMode = RotateMode.Fast;
        public RotateMode rotationMode => _rotationMode;

        public enum Space { Local, Global }
        [SerializeField] private Space _space = Space.Local;
        public Space space => _space;

        [SerializeField] private Vector3 _endValue;
        public Vector3 endValue => _endValue;

        [SerializeField] private float _jumpPower = 1f;
        public float jumpPower => _jumpPower;

        [Tooltip("Is the end value relative to current value")]
        [SerializeField] private bool _isRelative = true;
        public bool isRelative => _isRelative;

        [SerializeField] private Ease _ease = Ease.Linear;
        public Ease ease => _ease;

        [Tooltip("For custom ease option")]
        [SerializeField] private bool _useCustomEaseCurve;
        public bool useCustomEaseCurve => _useCustomEaseCurve;

        [SerializeField] private AnimationCurve _easeCurve;
        public AnimationCurve easeCurve => _easeCurve;

        [SerializeField] private int _loops = 1;
        public int numLoops => _loops;

        [SerializeField] private LoopType _loopType = LoopType.Restart;
        public LoopType loopType => _loopType;
    }
}