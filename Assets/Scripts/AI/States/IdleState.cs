using Components;
using UnityEngine;
using Random = UnityEngine.Random;

namespace States
{
    public class IdleState : State
    {
        #region Constant Values

        private const float IdleMinTime = 4.0f;
        private const float IdleMaxTime = 8.0f;
        
        #endregion

        #region Private Attributes

        #endregion
    
        #region Public Properties

        #endregion

        #region Constructors

        public IdleState(Entity entity) : base(entity)
        {
        }

        #endregion
    
        #region State Lifecycle Methods

        public override void OnStateEnter()
        {
            Debug.Log("Enter Idle.");

            // reset idle timer
            entity.CurrentIdleTime = 0.0f;
        
            // set time limit
            entity.MaxIdleTime = Random.Range(IdleMinTime, IdleMaxTime);
        
            // change color
            foreach (var material in entity.MeshRenderer.materials)
            {
                material.color = Color.blue;
            }
        }

        public override void OnStateUpdate()
        {
            // increment timer
            entity.CurrentIdleTime += Time.deltaTime;

            // look around, look left and right
            entity.transform.Rotate(0.0f, entity.IdleLookYAngle * Time.deltaTime, 0.0f);
        }

        public override void OnStateExit()
        {
            // change color
            foreach (var material in entity.MeshRenderer.materials)
            {
                material.color = Color.white;
            }
        }

        #endregion
    }
}
