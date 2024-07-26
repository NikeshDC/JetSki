using System;
using System.Collections.Generic;

namespace StateMachine
{
    public class State : IState
    {
        private static void Noop() { }

        private Action onEnter;
        private Action onUpdate;
        private Action onExit;

        private List<Transition> transitions = new List<Transition>();
        private ITransition[] itransitionsArray;

        public string name { get; set; }

        public ITransition[] GetTransitions() => itransitionsArray;

        public State(string name, Action OnEnter = null, Action OnUpdate = null, Action OnExit = null) 
        {
            this.name = name;
            this.onEnter = OnEnter ?? Noop;
            this.onUpdate = OnUpdate ?? Noop;
            this.onExit = OnExit ?? Noop;
        }

        public void AddTransitionTo(IState toState, Func<bool> evaluationFunc)
        { 
            transitions.Add(new Transition(this, toState, evaluationFunc));
            itransitionsArray = this.transitions.ToArray();
        }
        public bool RemoveTransitionFrom(Transition transition) 
        { 
            bool elementFound = transitions.Remove(transition); 
            if(elementFound)
            { itransitionsArray = this.transitions.ToArray(); }
            return elementFound;
        }

        public void OnEnter()
        { onEnter(); }

        public void OnExit()
        { onExit(); }

        public void OnUpdate()
        { onUpdate(); }
    }
}