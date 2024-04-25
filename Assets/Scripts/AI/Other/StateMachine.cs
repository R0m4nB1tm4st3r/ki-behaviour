using System.Collections.Generic;
using System.Linq;
using States;

namespace Other
{
    public class StateMachine
    {
        #region Private Attributes

        private State currentState;
        private Dictionary<State, List<Transition>> transitions;

        #endregion

        #region Constructors

        public StateMachine(State initialState, Dictionary<State, List<Transition>> transitions)
        {
            currentState = initialState;
            this.transitions = transitions;
            
            currentState.OnStateEnter();
        }

        #endregion

        #region Methods

        private State GetNextState()
        {
            var currentTransitions = transitions[currentState];

            return (from transition in currentTransitions where transition.IsConditionMet() select transition.TargetState)
                .FirstOrDefault();
        }

        private void SwitchState(State targetState)
        {
            if (currentState == targetState) return;
        
            currentState.OnStateExit();
            targetState.OnStateEnter();

            currentState = targetState;
        }

        public void Tick()
        {
            var nextState = GetNextState();
        
            if (nextState != null) SwitchState(nextState);
        
            currentState.OnStateUpdate();
        }

        #endregion
    }
}