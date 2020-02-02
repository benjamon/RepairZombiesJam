using UnityEngine;

public class ZombieController : MonoBehaviour {
    private Animator animator;
    private Torso torso;
    [SerializeField]
    private ActionState action;
    [SerializeField]
    private PostureState posture;
    [SerializeField]
    private int direction = 1;
    [SerializeField]
    private Transform sensor;
    [SerializeField]
    private LayerMask rayMask;
    [SerializeField]
    private float attackDistance = 0.5f;
    
    void Start() {
        torso = this.GetComponent<Torso>();
        animator = this.GetComponent<Animator>();
        action = ActionState.Idle;
        posture = PostureState.Stand;
    }

    void Update() {

        UpdatePostureAnim();

        if (action == ActionState.Attack) {
            animator.SetTrigger("Attack");
            moveMult = 0f;
        }
        else if (action == ActionState.Move) {
            float delta = Time.deltaTime;
            float speed = torso.MovementVal * direction * delta * moveMult;
            moveMult *= .925f;
            transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        }
    }

    float moveMult = 0;

    public void CommandMove() {
        moveMult = 1f;
    }

    public void Attack() {
        var rayHits = Physics2D.RaycastAll(sensor.position, Vector2.right, 1.5f, rayMask);
        Debug.DrawLine(sensor.position, new Vector3(sensor.position.x + 1.5f, sensor.position.y, sensor.position.z), Color.blue);
        foreach (var rayHit in rayHits) {
            if (rayHit.collider.gameObject != this.gameObject && rayHit.distance < attackDistance) {
                Debug.DrawLine(sensor.position, new Vector3(sensor.position.x + rayHit.distance, sensor.position.y, sensor.position.z), Color.red);
                var damageable = rayHit.collider.GetComponent<IDamageable>();
                if (damageable != null) {
                    damageable.DealDamage(torso.DamageVal);
                    // play obstacle damaged audio
                }
            }
        }

    }

    public void AttackGrunt() {
        // play zombie attack audio
    }

    public void StartAttack() {
        animator.SetTrigger("Attack");
        action = ActionState.Attack;
    }

    public void Move() {
        action = ActionState.Move;
    }

    public void Idle() {
        action = ActionState.Idle;
    }

    public void DirectionRight(int right) {
        if (direction != right) {
            direction *= -1;
            transform.localScale.Set(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void Stand() {
        posture = PostureState.Stand;
    }

    private void Hobble() {
        posture = PostureState.Hobble;
    }

    private void Crawl() {
        posture = PostureState.Crawl;
    }

    private void UpdatePostureAnim() {
        switch (torso.posture) {
            case PostureState.Stand: {
                animator.SetInteger("WalkStatus", 2);
                break;
            }
            case PostureState.Hobble: {
                animator.SetInteger("WalkStatus", 1);
                break;
            }
            case PostureState.Crawl: {
                animator.SetInteger("WalkStatus", 0);
                break;
            }
        }
        posture = torso.posture;
    }
}

public enum ActionState {
    Idle,
    Move,
    Attack
}

public enum PostureState {
    Crawl,
    Hobble,
    Stand
}
