using System.Linq;
using Components;
using UnityEngine;

namespace States
{
    public class FleeState : State
    {
        #region Constant Values

        #endregion

        #region Private Attributes

        private Vector3 fleeingDirection = Vector3.zero;

        #endregion

        #region Constructors

        public FleeState(Entity entity) : base(entity)
        {
        }

        #endregion

        #region Methods

        private Vector3 CalculateFleeingDirection()
        {
            var difference = Vector3.zero;
            var newDirection = Vector3.zero;

            foreach (var obj in entity.DangerObjects)
            {
                // calculate vector between Entity and DangerObject
                difference = obj.position - entity.transform.position;
                
                // add weighted difference to new direction (the smaller the distance the greater the impact) 
                newDirection += difference / difference.sqrMagnitude;
            }

            // return opposite normalized new direction
            return (newDirection * -1).normalized;
        }

        #endregion

        #region State Lifecycle Methods

        public override void OnStateEnter()
        {
            Debug.Log("Enter Flee.");

            // set fleeing direction
            entity.FleeingDirection = CalculateFleeingDirection();

            // set moving speed
            entity.Agent.speed = entity.FleeingSpeed;

            // change color
            foreach (var material in entity.MeshRenderer.materials)
            {
                material.color = Color.yellow;
            }
        }

        public override void OnStateUpdate()
        {
            // move along moving direction
            entity.Agent.SetDestination(entity.transform.position + entity.FleeingDirection);

            // update fleeing direction in case number of dangers changes
            entity.FleeingDirection = CalculateFleeingDirection();
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