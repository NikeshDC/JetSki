using UnityEngine;

public class BoatEffectsPlayer : MonoBehaviour
{
    public PlayerController controller;

    public GameObject sprayEffect;

    void Update()
    {
        if(controller.movingForward)
        {
            sprayEffect.SetActive(true);
        }
        else
        {
            sprayEffect.SetActive(false);
        }
    }
}
