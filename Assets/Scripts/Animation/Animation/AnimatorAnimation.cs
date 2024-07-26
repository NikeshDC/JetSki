using UnityEngine;

namespace Animation
{

    [CreateAssetMenu(fileName = "AnimatorAnimation_", menuName = "MyAnimation/Animator")]
    public class AnimatorAnimation : BaseAnimation
    {
        public bool forceDuration = true;

        [SerializeField] private string _layerName = "Base Layer";
        public string layerName => _layerName;

        [SerializeField] private string _stateName = "state";
        public string stateName => _stateName;
        public int nameHash => Animator.StringToHash(stateName);
        public int fullPathHash => Animator.StringToHash(layerName + "." + stateName);

        [SerializeField] private bool _crossFade = true;
        public bool crossFade => _crossFade;

        [SerializeField] private float _crossFadeDuration = 0.25f;
        public float crossFadeDuration => _crossFadeDuration;

        public void Awake()
        {
            _crossFadeDuration = Mathf.Min(_crossFadeDuration, duration);
        }
    }

}