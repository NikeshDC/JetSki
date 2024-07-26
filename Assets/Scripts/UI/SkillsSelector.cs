using UnityEngine;
using UnityEngine.UI;

public class SkillsSelector : MonoBehaviour
{
    public Dropdown dropdown;
    
    public PlayerController playerController;

    void Start()
    {
        dropdown.options.Clear();
        foreach (string skill in playerController.playableSkills) 
        { dropdown.options.Add(new Dropdown.OptionData(skill)); }
        dropdown.value = 0;
        dropdown.RefreshShownValue();

        dropdown.onValueChanged.AddListener(OnDropDownValueChanged);
    }

    public void OnDropDownValueChanged(int value)
    {
        playerController.skillToPlay = value;
    }
}
