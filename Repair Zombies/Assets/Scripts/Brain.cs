using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    [SerializeField]
    private bool confused = false;
    [SerializeField]
    private bool right = true;
    [SerializeField]
    private Decision decision = Decision.idle;
    private string target = null;

    void Start() {
        // get reference to torso component
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
        var rayHit = getNearestRayHit();
        // sense
        // determine state
        // default to moving to the right

        // if a zombie is ahead of it, idle until the zombie is far enough away

        // if an obstacle or enemy is ahead of it, attack

        // if state has changed, update torso
    }

    private void ConfusedUpdate() {
        // when headless, move left or right randomly for 5 seconds each, attack if anything is in front of it
        Decision newDecision;
        string newTarget;

        var rayHit = getNearestRayHit();
        if (rayHit.HasValue) {
            var hitLayerName = LayerMask.LayerToName(rayHit.Value.collider.gameObject.layer);
            switch (hitLayerName) {
                case "Obstacle":
                case "Zombie":
                case "Enemy": {
                    if (rayHit.Value.distance < 0.5f) {
                        newDecision = Decision.attack;
                        newTarget = hitLayerName;
                    }
                    break;
                }
            }
        }
        // sense
        // determine state
        // if state has changed, update torso
        // if state has not changed but 5 seconds have passed, randomly change state, update torso
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
