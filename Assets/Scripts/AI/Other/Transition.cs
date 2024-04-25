using System;
using States;

namespace Other
{
    public class Transition
    {
        #region Private Attributes

        private Func<bool> isConditionMet;
        private State targetState;

        #endregion

        #region Public Properties

        public Func<bool> IsConditionMet => isConditionMet;
        public State TargetState => targetState;

        #endregion

        #region Constructors

        public Transition(Func<bool> isConditionMet, State targetState)
        {
            this.isConditionMet = isConditionMet;
            this.targetState = targetState;
        }

        #endregion
    }
}