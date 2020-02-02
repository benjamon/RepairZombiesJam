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
    
    void Start() {
        torso = this.GetComponent<Torso>();
        animator = this.GetComponent<Animator>();
        action = ActionState.Idle;
        posture = PostureState.Stand;
    }

    void Update() {
        // posture = something from torso
        // switch (torso.movementState) {
        //     case 2: {
        //         Stand();
        //         break;
        //     }
        //     case 1: {
        //         Hobble();
        //         break;
        //     }
        //     case 0: {
        //         Crawl();
        //         break;
        //     }
        // }
        UpdatePostureAnim();

        if (action == ActionState.Attack) {
            animator.SetTrigger("Attack");
        }
        else if (action == ActionState.Move) {
            // var rb = GetComponent<Rigidbody2D>();
            posture = torso.posture;
            float delta = Time.deltaTime;
            float speed = torso.MovementVal * direction * delta * moveMult;
            // moveMult *= .95f;
            // rb.MovePosition(new Vector2(transform.position.x + speed, transform.position.y));
            // transform.position.Set(transform.position.x + speed, transform.position.y, transform.position.z);
            transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        }
    }

    float moveMult = 1;

    public void CommandMove() {
        moveMult = 1f;
    }

    public void Attack() {
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
        switch (posture) {
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
