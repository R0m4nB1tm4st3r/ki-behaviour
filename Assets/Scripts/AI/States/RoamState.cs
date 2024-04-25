using Components;
using UnityEngine;

namespace States
{
    public class RoamState : State
    {
        #region Constructors

        public RoamState(Entity entity) : base(entity) { }

        #endregion
    
        #region State Lifecycle Methods

        public override void OnStateEnter()
        {
            Debug.Log("Enter Roam.");

            // set moving direction randomly
            var randomDirection = Random.insideUnitCircle.normalized;
            entity.MovingDirection = new Vector3(randomDirection.x, 0, randomDirection.y);
        
            // set moving speed
            entity.Agent.speed = entity.WalkingSpeed;
        
            // reset timer
            entity.CurrentRoamingTime = 0.0f;
        
            // set time limit randomly
            entity.MaxRoamingTime = Random.Range(5.0f, 10.0f);
        
            // change color
            entity.MeshRenderer.material.color = Color.green;
        }

        public override void OnStateUpdate()
        {
            // count the timer
            entity.CurrentRoamingTime += Time.deltaTime;
        
            // move along moving direction
            entity.Agent.SetDestination(entity.transform.position + entity.MovingDirection);

            /*if (roamTimer >= enemy.RoamDuration)
        {
            enemy.TransitionToState(enemy.IdleState);
        }*/
        }

        public override void OnStateExit()
        {
            // change color
            entity.MeshRenderer.material.color = Color.white;
        
            // set moving speed back
            entity.Agent.speed = 0.0f;
        }

        #endregion
    }
}
