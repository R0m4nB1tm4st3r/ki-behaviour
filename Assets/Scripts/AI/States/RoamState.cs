using Components;
using UnityEngine;

namespace States
{
    public class RoamState : State
    {
        #region Constant Values

        private const float RoamMinTime = 5.0f;
        private const float RoamMaxTime = 10.0f;
        
        #endregion
        
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
            entity.MaxRoamingTime = Random.Range(RoamMinTime, RoamMaxTime);
        
            // change color
            foreach (var material in entity.MeshRenderer.materials)
            {
                material.color = Color.green;
            };
        }

        public override void OnStateUpdate()
        {
            // count the timer
            entity.CurrentRoamingTime += Time.deltaTime;
        
            // move along moving direction
            entity.Agent.SetDestination(entity.transform.position + entity.MovingDirection);
        }

        public override void OnStateExit()
        {
            // change color
            foreach (var material in entity.MeshRenderer.materials)
            {
                material.color = Color.white;
            }
        
            // set moving speed back
            entity.Agent.speed = 0.0f;
        }

        #endregion
    }
}
