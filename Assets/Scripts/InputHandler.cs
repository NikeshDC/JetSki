using System;
using System.Collections;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public SimpleTouchArea leftSideTouchArea;
    public SimpleTouchArea rightSideTouchArea;

    private bool _leftTouch = false;
    public bool turnLeft => _leftTouch;
    private bool _rightTouch = false;
    public bool turnRight => _rightTouch;

    [SerializeField] private bool _moveForward;
    public bool moveForward => _moveForward;

    private bool _playSkill;
    public bool playSkill => _playSkill;

    public void OnPlaySkillButtonPress()
    {
        _playSkill = true;
        if(_playSkillCoroutine != null)
        { StopCoroutine(_playSkillCoroutine); }
        _playSkillCoroutine = StartCoroutine(C_AfterDelayPerform(() => { _playSkill = false; }, 0.1f));
    }
    private Coroutine _playSkillCoroutine;
    IEnumerator C_AfterDelayPerform(Action func, float delay)
    {
        yield return new WaitForSeconds(delay);
        func();
    }

    public void OnForwardMovementButtonPress()
    { _moveForward = true; }
    public void OnStopButtonPress()
    { _moveForward = false; }

    private void Start()
    {
        leftSideTouchArea.OnTouchBegin.AddListener(SetLeftTouch);
        leftSideTouchArea.OnTouchEnd.AddListener(ResetLeftTouch);

        rightSideTouchArea.OnTouchBegin.AddListener(SetRightTouch);
        rightSideTouchArea.OnTouchEnd.AddListener(ResetRightTouch);
    }

    public void SetLeftTouch() { _leftTouch = true; }
    public void ResetLeftTouch() { _leftTouch = false; }
    public void SetRightTouch() { _rightTouch = true; }
    public void ResetRightTouch() { _rightTouch = false; }

}
