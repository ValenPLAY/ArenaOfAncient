using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : Unit
{
    Unit currentTarget;
    Vector3 currentTargetPosition;
    NavMeshAgent agent;
    [Header("Path Finding Options")]
    [SerializeField] float retargetMovingTargetDistance = 1.0f;
    [SerializeField] float retargetDuration = 2.0f;
    protected float retargetDurationCurrent;
    protected float distanceTillTarget;
    protected float targetMovedDistance;

    private float missRangeMultiplier = 1.4f;

    // Start is called before the first frame update
    override protected void Awake()
    {
        base.Awake();
        CurrentHealth = Health;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = MovementSpeed;
        onMovementSpeedChange += UpdateMovementSpeed;

        unitState = state.paused;
    }

    protected void UpdateMovementSpeed(float newMovementSpeed)
    {
        agent.speed = newMovementSpeed;
    }

    // Update is called once per frame
    override protected void Update()
    {
        BasicEnemyBehaviour();
        base.Update();
    }

    protected virtual void BasicEnemyBehaviour()
    {
        if (unitState == state.paused) return;
        if (unitState == state.dying) return;

        if (currentTarget == null)
        {
            if (retargetDurationCurrent <= 0)
            {
                Retarget();
                retargetDurationCurrent = retargetDuration;
            }

            retargetDurationCurrent -= Time.deltaTime;
        }
        if (currentTarget != null)
        {
            targetMovedDistance = Vector3.Distance(currentTarget.transform.position, agent.destination);

            if (targetMovedDistance >= retargetMovingTargetDistance)
            {
                agent.SetDestination(currentTarget.transform.position);
            }
            if (AbleToHitCheck())
            {
                agent.ResetPath();
                OrderAttack();
            }
        }
        unitAnimator.SetBool("isWalking", agent.velocity != Vector3.zero);
    }

    protected virtual void Retarget()
    {
        if (GameController.Instance == null) return;
        if (GameController.Instance.selectedHero == null) return;

        if (GameController.Instance.selectedHero != null)
        {
            currentTarget = GameController.Instance.selectedHero;
            //currentTargetPosition = currentTarget.transform.position;
        }


    }

    protected override void Attack()
    {
        base.Attack();

        if (AbleToHitCheck()) DealDamage(currentTarget);
    }

    protected virtual bool AbleToHitCheck()
    {
        if (currentTarget != null)
        {
            distanceTillTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
            if (distanceTillTarget <= (attackRange * missRangeMultiplier) + currentTarget.UnitWidth + UnitWidth)
            {
                return true;
            }
        }
        return false;
    }

    protected override void Death()
    {
        base.Death();
    }
}
