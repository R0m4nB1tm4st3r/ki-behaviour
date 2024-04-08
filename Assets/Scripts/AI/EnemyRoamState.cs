using UnityEngine;

public class EnemyRoamState : EnemyBaseState
{
    private float roamTimer = 0.0f;
    private Vector2 randomDirection = Vector2.zero;

    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Enter Roam.");

        randomDirection = Random.insideUnitCircle.normalized;
        roamTimer = 0.0f;
    }

    public override void Update(EnemyStateManager enemy)
    {
        roamTimer += Time.deltaTime;
        enemy.Agent.SetDestination(enemy.transform.position + new Vector3(randomDirection.x, 0, randomDirection.y));

        if (roamTimer >= enemy.RoamDuration)
        {
            enemy.TransitionToState(enemy.IdleState);
        }
    }
}
