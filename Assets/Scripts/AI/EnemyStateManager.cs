using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStateManager : MonoBehaviour
{
    // Components
    public NavMeshAgent Agent { get; private set; } = null;


    // States
    public EnemyBaseState CurrentState { get; private set; } = null;
    public EnemyRoamState RoamState { get; private set; } = new EnemyRoamState();
    public EnemyIdleState IdleState { get; private set; } = new EnemyIdleState();



    // Properties
    [field: SerializeField] public float IdleDuration { get; private set; } = 10.0f;
    [field: SerializeField] public float RoamDuration { get; private set; } = 15.0f;

    public void TransitionToState(EnemyBaseState state)
    {
        CurrentState = state;
        CurrentState.EnterState(this);
    }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        // set the first state to begin with (Idle)
        CurrentState = IdleState;
        CurrentState.EnterState(this);
    }

    private void Update()
    {
        CurrentState.Update(this);

    }
}
