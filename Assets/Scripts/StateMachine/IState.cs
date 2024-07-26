namespace StateMachine
{
    public interface IState
    {
        public void OnEnter();
        public void OnUpdate();
        public void OnExit();

        public ITransition[] GetTransitions();
        public string name { get; }
    }
}