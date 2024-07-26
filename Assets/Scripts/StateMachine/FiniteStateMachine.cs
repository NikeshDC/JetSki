namespace StateMachine
{
    public class FiniteStateMachine
    {
        private IState currentState;
        private ITransition[] currentStateTransitions;

        public void SetCurrentState(IState state)
        { 
            currentState = state;
            currentStateTransitions = currentState.GetTransitions();
            currentState.OnEnter();
        }
        public IState GetCurrentState() => currentState;
        public string GetCurrentStateName() => currentState.name;

        public void Update()
        {
            if(currentStateTransitions != null)
            {
                foreach(var transition in currentStateTransitions)
                {
                    if(transition.Evaluate())
                    {
                        currentState.OnExit();
                        SetCurrentState(transition.To);
                    }
                }
            }

            currentState.OnUpdate();
        }

    }
}