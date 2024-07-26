using System;
using System.Collections;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Serializable]
    public class AnimationPath
    {
        public string animationName;
        public Animation.BaseAnimationPlayer player;
        
        public void Play() { player.Play(animationName); }
        public void Play(Action onEndCallback) { player.Play(animationName, onEndCallback); }
    }

    public AnimationPath Anim_idle;
    public AnimationPath Anim_accelerateForward;
    public AnimationPath Anim_turnLeft;
    public AnimationPath Anim_turnRight;

    public AnimationPath[] Anim_skills;

    public void PlayIdle()
    {
        Anim_idle.Play();
    }

    public void TurnLeft()
    {
        Anim_turnLeft.Play();
    }

    public void TurnRight()
    {
        Anim_turnRight.Play();
    }

    public void AccelerateForward()
    {
        Anim_accelerateForward.Play();
    }

    public void PlaySkill(string skillName, Action OnSkillEnd)
    {
        foreach (var skill in Anim_skills)
        {
            if(skill.animationName == skillName)
            {
                skill.Play(OnSkillEnd);
                return;
            }
        }
        Debug.LogError("No skill with name \"" + skillName + "\"");
        OnSkillEnd();
    }
}
