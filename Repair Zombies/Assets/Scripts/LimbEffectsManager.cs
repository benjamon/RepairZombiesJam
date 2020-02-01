using UnityEngine;

public class LimbEffectsManager : MonoBehaviour {
    [SerializeField]
    private Sprite[] spriteLevels;
    [SerializeField]
    private float[] healthThresholds;
    [SerializeField]
    private ParticleSystem[] particleSystems;

    private SpriteRenderer spriteRenderer;
    private int currentLevel;

    void Start() {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        currentLevel = 0;
    }

    void UpdateLimb(float health) {
        if (health < healthThresholds[currentLevel]) {
            currentLevel++;
            particleSystems[currentLevel].Play();
            spriteRenderer.sprite = spriteLevels[currentLevel];
        }
    }
}
