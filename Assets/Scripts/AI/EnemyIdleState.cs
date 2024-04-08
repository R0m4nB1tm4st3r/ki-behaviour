using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float IdleTimer { get; set; } = 0.0f;

    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Enter Idle.");

        // reset idle timer
        IdleTimer = 0.0f;
    }

    public override void Update(EnemyStateManager enemy)
    {
        // increment timer
        IdleTimer += Time.deltaTime;

        // look around, look left and right
        enemy.transform.Rotate(0.0f, 5.0f * Time.deltaTime, 0.0f);
        
        // if the timer has reached IdleDuration, transit to next state
        if (IdleTimer >= enemy.IdleDuration)
        {
            enemy.TransitionToState(enemy.RoamState);
        }
    }
}
