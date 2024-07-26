using System;
using UnityEngine;

public class AnimationPlayerTest : MonoBehaviour
{
    public Animation.BaseAnimationPlayer player;
    public string animationName;

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse down");
            player.Play(animationName);
        }
        /*if(Input.GetMouseButtonUp(0)) 
        {
            Debug.Log("Mouse up");
            player.Stop();
        }*/
    }
}
