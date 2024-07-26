namespace StateMachine
{
    public interface ITransition
    {
        public IState From { get; }
        public IState To { get; }
        public bool Evaluate();
    }
}