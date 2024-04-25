using Components;

namespace States
{
    public abstract class State
    {
        protected Entity entity;

        protected State(Entity entity)
        {
            this.entity = entity;
        }
    
        public abstract void OnStateEnter();
        public abstract void OnStateUpdate();
        public abstract void OnStateExit();
    }
}
