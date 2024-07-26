using UnityEngine;

namespace Animation
{

    public abstract class BaseAnimation : ScriptableObject, IAnimation
    {
        [SerializeField] protected string _name = "NewAnimation";
        public new string name => _name;

        [SerializeField] protected float _duration = 1f;
        public virtual float duration => _duration;
    }
}