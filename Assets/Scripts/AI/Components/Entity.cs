using System;
using System.Collections.Generic;
using Other;
using States;
using UnityEngine;
using UnityEngine.AI;

namespace Components
{
    [RequireComponent(typeof(NavMeshAgent), typeof(MeshRenderer))]
    public class Entity : MonoBehaviour
    {
        #region Constant Values

        #endregion
    
        #region Component References

        private NavMeshAgent agent = null;
        private MeshRenderer meshRenderer = null;

        #endregion

        #region Private Attributes

        [SerializeField] private float walkingSpeed;
        [SerializeField] private float fleeingSpeed;
        [SerializeField] private float attentionRadius;
        [SerializeField] private float idleLookYAngle = 10.0f;
        [SerializeField] private LayerMask mask;
        [SerializeField] private EStateMachineType stateMachineType;    // will be ignored for now
        private Vector3 movingDirection = Vector3.zero;
        private float currentRoamingTime;
        private float currentIdleTime;
        private float maxRoamingTime;
        private float maxIdleTime;
        private StateMachine stateMachine = null;

        #endregion

        #region Public Properties

        public NavMeshAgent Agent => agent;
        public MeshRenderer MeshRenderer => meshRenderer;
        public float WalkingSpeed => walkingSpeed;
        public float FleeingSpeed => fleeingSpeed;
        public float AttentionRadius => attentionRadius;
        public float IdleLookYAngle => idleLookYAngle;
        public LayerMask Mask => mask;
        public Vector3 MovingDirection
        {
            get => movingDirection;
            set => movingDirection = value;
        }

        public float CurrentRoamingTime
        {
            get => currentRoamingTime;
            set => currentRoamingTime = value;
        }
        public float CurrentIdleTime
        {
            get => currentIdleTime;
            set => currentIdleTime = value;
        }
        public float MaxRoamingTime
        {
            get => maxRoamingTime;
            set => maxRoamingTime = value;
        }
        public float MaxIdleTime
        {
            get => maxIdleTime;
            set => maxIdleTime = value;
        }

        #endregion

        private void SetStateMachine()
        {
            var idleState = new IdleState(this);
            var roamState = new RoamState(this);

            var transitions = new Dictionary<State, List<Transition>>()
            {
                {
                    idleState, new List<Transition>()
                    {
                        new Transition(() => currentIdleTime >= maxIdleTime, roamState)
                    }
                },
                {
                    roamState, new List<Transition>()
                    {
                        new Transition(() => currentRoamingTime >= maxRoamingTime, idleState)
                    }
                }
            };

            stateMachine = new StateMachine(idleState, transitions);

            // TODO: define more states for different state machine types
            // switch (stateMachineType)
            // {
            //     case EStateMachineType.Prey:
            //         break;
            //     case EStateMachineType.Predator:
            //         break;
            //     case EStateMachineType.Enemy:
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }
        }
        
        #region Unity Lifecycle Methods

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            meshRenderer = GetComponent<MeshRenderer>();
            
            SetStateMachine();
        }

        private void Update()
        {
            stateMachine.Tick();
        }

        #endregion
    }
}
