using UnityEngine;

public class Brain : MonoBehaviour
{
    [SerializeField]
    private bool confused = false;
    [SerializeField]
    private bool right = true;
    [SerializeField]
    private Decision decision = Decision.idle;
    [SerializeField]
    private float attackDistance = 0.5f;
    [SerializeField]
    private float idleDistance = 0.5f;
    [SerializeField]
    private float idleReleaseDistance = 1.0f;
    private string target = null;
    private float confusionTimer;
    private float maxConfusionTimer = 5.0f;
    private ZombieController zombieController;

    void Start() {
        // get reference to torso component
        zombieController = this.GetComponent<ZombieController>();
    }

    void Update() {
        // check if there is an obstacle, enemy, or zombie
        if (confused) {
            ConfusedUpdate();
        }
        else {
            NormalUpdate();
        }
    }

    private void NormalUpdate() {
        Decision newDecision = decision;
        string newTarget = target;
        var doDefault = true;

        var rayHit = getNearestRayHit();
        if (rayHit.HasValue) {
            var hitLayerName = LayerMask.LayerToName(rayHit.Value.collider.gameObject.layer);
            switch (hitLayerName) {
                case "Obstacle":
                case "Enemy": {
                    if (rayHit.Value.distance < attackDistance) {
                        newDecision = Decision.attack;
                        newTarget = hitLayerName;
                        doDefault = false;
                    }
                    break;
                }
                case "Zombie": {
                    if (rayHit.Value.distance < idleDistance) {
                        newDecision = Decision.idle;
                        newTarget = hitLayerName;
                        doDefault = false;
                    }
                    else if (rayHit.Value.distance < idleReleaseDistance) {
                        newDecision = Decision.move;
                        newTarget = null;
                        doDefault = false;
                    }
                    break;
                }
            }
        }
        
        if (doDefault) {
            newDecision = Decision.move;
            newTarget = null;
            right = true;
        }

        if (decision != newDecision || target != newTarget) {
            // start doing something new
            UpdateController(newDecision, right);
            decision = newDecision;
            target = newTarget;
        }
    }

    private void ConfusedUpdate() {
        // when headless, move left or right randomly for 5 seconds each, attack if anything is in front of it
        confusionTimer -= Time.deltaTime;
        if (confusionTimer <= 0) {
            switch (decision) {
                case Decision.idle:
                case Decision.move: {
                    if (Random.value < 0.5f) {
                        decision = Decision.move;
                        right = !right;
                    }
                    else {
                        decision = Decision.attack;
                    }
                    break;
                }
                case Decision.attack: {
                    decision = Decision.move;
                    right = !right;
                    break;
                }
            }
            confusionTimer = maxConfusionTimer;
            // start doing something new
            UpdateController(decision, right);
        }
        else if (decision == Decision.move) {
            var rayHit = getNearestRayHit();
            if (rayHit.HasValue) {
                var hitLayerName = LayerMask.LayerToName(rayHit.Value.collider.gameObject.layer);
                switch (hitLayerName) {
                    case "Obstacle":
                    case "Zombie":
                    case "Enemy": {
                        if (rayHit.Value.distance < attackDistance) {
                            if (Random.value < 0.5f) {
                                decision = Decision.move;
                                right = !right;
                            }
                            else {
                                decision = Decision.attack;
                            }
                        }
                        break;
                    }
                }
            }
            // start doing something new
            UpdateController(decision, right);
        }
    }


    private void UpdateController(Decision decision, bool right) {
        switch (decision) {
            case Decision.attack: {
                zombieController.Attack();
                break;
            }
            case Decision.move: {
                zombieController.DirectionRight(right ? 1 : -1);
                zombieController.Move();
                break;
            }
            case Decision.idle: {
                zombieController.Idle();
                break;
            }
        }
    }

    private RaycastHit2D? getNearestRayHit() {
        var rayHits = Physics2D.RaycastAll(transform.position, Vector2.right, 5);
        foreach (var rayHit in rayHits) {
            if (rayHit.collider.gameObject != this.gameObject) {
                return rayHit;
            }
        }
        return null;
    }

    void LoseHead() {
        confusionTimer = 2.0f;
        confused = true;
        ConfusedUpdate();
    }

    void GainHead() {
        confused = false;
        NormalUpdate();
    }
}

public enum Decision {
    move, attack, idle
}
