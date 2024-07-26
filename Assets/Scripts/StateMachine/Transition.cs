using System;

namespace StateMachine
{
    public class Transition : ITransition
    {
        private static bool FalseFunc() => false;

        private IState _from;
        public IState From => _from;
        private IState _to;
        public IState To => _to;

        private Func<bool> evaluate;

        public Transition(IState from, IState to,Func<bool> evaluate)
        {
            this._from = from; 
            this._to = to;
            this.evaluate = evaluate ?? FalseFunc;
        }

        public bool Evaluate()
        { return evaluate(); }
    }
}