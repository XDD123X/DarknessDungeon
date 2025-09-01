using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float followRange;
	[SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private GameObject player;

    private enum State {
        Roaming, 
        Attacking,
		Following
	}

    private Vector2 roamPosition;
	private Vector2 playerPosition;
	private float timeRoaming = 0f;

    private State state;

    private EnemyPathfinding enemyPathfinding;

    private EntityInterface script;

	private void Awake() {
        script = GetComponent<EntityInterface>();   
        script.CanAttack = true;
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
		player = GameObject.FindGameObjectWithTag("Player");

	}

    private void Start() {
        //(enemyType as IEnemy).Spawn();
        roamPosition = GetRoamingPosition();
        playerPosition = GetPlayerPosition();
    }

    private void Update() {
        MovementStateControl();
        (enemyType as IEnemy).Move(roamPosition);
    }

    private void MovementStateControl() {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
            break;

            case State.Attacking:
                Attacking();
            break;

			case State.Following:
                Following();
            break;

		}
	}

	private void Following()
	{
        playerPosition = GetPlayerPosition();
		enemyPathfinding.MoveTo(playerPosition);
        if (Vector2.Distance(transform.position, player.transform.position) > followRange)
		{
			state = State.Roaming;
		}
		if (Vector2.Distance(transform.position, player.transform.position) < attackRange)
		{
			state = State.Attacking;
		}

	}

	private void Roaming() {
        timeRoaming += Time.deltaTime;

        enemyPathfinding.MoveTo(roamPosition);
        if(Vector2.Distance(transform.position, player.transform.position) < followRange)
        {
            state = State.Following;
        }

        if (Vector2.Distance(transform.position, player.transform.position) < attackRange) {
             state = State.Attacking;
        }


        if (timeRoaming > roamChangeDirFloat) {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking() {
        if (Vector2.Distance(transform.position, player.transform.position) > attackRange)
        {
            
            state = State.Roaming;
        }
		if (Vector2.Distance(transform.position, player.transform.position) < followRange)
		{
			state = State.Following;
		}
        if (attackRange != 0 && script.CanAttack) {

            script.CanAttack = false;
            (enemyType as IEnemy).Attack();
            
            if (stopMovingWhileAttacking) {
                enemyPathfinding.StopMoving();
            } else {
                enemyPathfinding.MoveTo(roamPosition);
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine() {
        yield return new WaitForSeconds(script.AttackSpeed);
        script.CanAttack = true;
    }

    private Vector2 GetRoamingPosition() {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private Vector2 GetPlayerPosition()
    {
		return (player.transform.position - transform.position).normalized;
	}
}
