using System;
using System.Collections.Generic;
using System.Linq;
using Other;
using States;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Components
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Entity : MonoBehaviour
    {
        #region Constant Values

        private const int MaxHitCount = 3;
        private const float SphereCastMaxDistance = 50.0f;

        #endregion

        #region Component References

        private NavMeshAgent agent = null;
        private SkinnedMeshRenderer meshRenderer = null;

        #endregion

        #region Private Attributes

        [SerializeField] private float sphereCastRadius = 0.15f;
        [SerializeField] private float walkingSpeed;
        [SerializeField] private float fleeingSpeed;
        [SerializeField] private float attentionRadius;
        [SerializeField] private float idleLookYAngle = 10.0f;
        [SerializeField] private LayerMask dangerObjectMask;
        [SerializeField] private EStateMachineType stateMachineType; // will be ignored for now
        private Vector3 movingDirection = Vector3.zero;
        private Vector3 fleeingDirection = Vector3.zero;
        private float currentRoamingTime;
        private float currentIdleTime;
        private float maxRoamingTime;
        private float maxIdleTime;
        private StateMachine stateMachine = null;
        [SerializeField] private Transform[] dangerObjects = null;

        #endregion

        #region Properties

        public NavMeshAgent Agent => agent;
        public SkinnedMeshRenderer MeshRenderer => meshRenderer;
        public float WalkingSpeed => walkingSpeed;
        public float FleeingSpeed => fleeingSpeed;
        public float AttentionRadius => attentionRadius;
        public float IdleLookYAngle => idleLookYAngle;
        public LayerMask DangerObjectMask => dangerObjectMask;

        public Vector3 MovingDirection
        {
            get => movingDirection;
            set => movingDirection = value;
        }

        public Vector3 FleeingDirection
        {
            get => fleeingDirection;
            set => fleeingDirection = value;
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

        public Transform[] DangerObjects
        {
            get => dangerObjects;
            set => dangerObjects = value;
        }

        #endregion

        #region Methods

        private void SetStateMachine()
        {
            // create state instances
            var idleState = new IdleState(this);
            var roamState = new RoamState(this);
            var fleeState = new FleeState(this);

            // define state transitions
            var transitions = new Dictionary<State, List<Transition>>()
            {
                {
                    idleState, new List<Transition>()
                    {
                        new Transition(() => currentIdleTime >= maxIdleTime, roamState),
                        new Transition(() => IsInDanger(), fleeState)
                    }
                },
                {
                    roamState, new List<Transition>()
                    {
                        new Transition(() => currentRoamingTime >= maxRoamingTime, idleState),
                        new Transition(() => IsInDanger(), fleeState)
                    }
                },
                {
                    fleeState, new List<Transition>()
                    {
                        new Transition(() => !IsInDanger(), idleState)
                    }
                }
            };

            // create state machine
            stateMachine = new StateMachine(idleState, transitions);
        }

        private bool IsInDanger()
        {
            // Check if any danger objects are in attention range
            var colliders = Physics.OverlapSphere(transform.position,
                attentionRadius, dangerObjectMask);

            // filter out only objects that are visible via SphereCast
            var visibleObjects = colliders
                .Where(col => Physics.SphereCast(transform.position, sphereCastRadius,
                    (col.transform.position - transform.position).normalized, out _,
                    attentionRadius, dangerObjectMask))
                .Select(col => col.transform)
                .ToArray();

            // assign visible objects to danger array
            dangerObjects = visibleObjects;

            // are there any visible objects?
            return visibleObjects.Length > 0;
        }

        #endregion

        #region Unity Lifecycle Methods

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

            SetStateMachine();
        }

        private void Update()
        {
            stateMachine.Tick();
        }

        #endregion
    }
}