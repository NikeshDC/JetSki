using UnityEngine;
using StateMachine;

public class PlayerController : MonoBehaviour
{
    public InputHandler inputHandler;
    public PlayerAnimationController animController;

    public float speed = 5f;
    public float turnSpeed = 20f;

    public int skillToPlay = 0;
    private bool skillHasEnded;

    [SerializeField] private bool _movingForward;
    public bool movingForward => _movingForward;

    public string[] playableSkills;

    private FiniteStateMachine playerStatemachine;

    private void Awake()
    {
        playerStatemachine = GetStateMachine();
        playableSkills = GetPlayableSkills();
    }

    void Update()
    {
        playerStatemachine.Update();

        _movingForward = false;
        if (playerStatemachine.GetCurrentStateName() != "idle")
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            _movingForward = true;
        }
    }

    public void SkillEndCallback()
    {
        skillHasEnded = true;
    }

    private FiniteStateMachine GetStateMachine()
    {
        FiniteStateMachine statemachine = new FiniteStateMachine();

        //states
        State idleState = new State("idle",
            OnEnter: () => { animController.PlayIdle(); }
            );
        State ridingForwardState = new State("ridingForward",
            OnEnter: () => { animController.AccelerateForward(); ; }
            //OnUpdate: () => { transform.Translate(Vector3.forward * Time.deltaTime * speed); }
            );
        State turnLeftState = new State("turnLeft",
            OnEnter: () => { animController.TurnLeft(); },
            OnUpdate: () => { transform.Rotate(transform.up, -Time.deltaTime * turnSpeed); }
            //                   transform.Translate(Vector3.forward * Time.deltaTime * speed); }
            );
        State turnRightState = new State("turnRight",
            OnEnter: () => { animController.TurnRight(); },
            OnUpdate: () => { transform.Rotate(transform.up, Time.deltaTime * turnSpeed); }
            //                   transform.Translate(Vector3.forward * Time.deltaTime * speed); }
            );
        State playSkillState = new State("playSkill",
            OnEnter: () =>
            {
                skillHasEnded = false;
                animController.PlaySkill(playableSkills[skillToPlay], SkillEndCallback);
            }
            //OnUpdate: () => { transform.Translate(Vector3.forward * Time.deltaTime * speed); }
            );

        //transitions
        idleState.AddTransitionTo(ridingForwardState,
            evaluationFunc: () => { return inputHandler.moveForward; }
            );
        ridingForwardState.AddTransitionTo(idleState,
            evaluationFunc: () => { return !inputHandler.moveForward; }
            );
        ridingForwardState.AddTransitionTo(turnLeftState,
            evaluationFunc: () => { return inputHandler.turnLeft; }
            );
        ridingForwardState.AddTransitionTo(turnRightState,
            evaluationFunc: () => { return inputHandler.turnRight; }
            );
        ridingForwardState.AddTransitionTo(playSkillState,
            evaluationFunc: () => { return inputHandler.playSkill; }
            );
        turnLeftState.AddTransitionTo(ridingForwardState,
            evaluationFunc: () => { return !inputHandler.turnLeft; }
            );
        turnRightState.AddTransitionTo(ridingForwardState,
            evaluationFunc: () => { return !inputHandler.turnRight; }
            );
        playSkillState.AddTransitionTo(ridingForwardState,
            evaluationFunc: () => { return skillHasEnded; }
            );

        statemachine.SetCurrentState(idleState);

        return statemachine;
    }

    private string[] GetPlayableSkills()
    {
        int noOfSkills = animController.Anim_skills.Length;
        string[] skills = new string[noOfSkills];
        for(int i=0; i < noOfSkills; i++)
        {
            skills[i] = animController.Anim_skills[i].animationName;
        }

        return skills;
    }
}