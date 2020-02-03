using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IDamageable
{
    public float Health;
    public SpriteRenderer Alive;
    public SpriteRenderer Detroyed;
    Collider2D collider;
    Animator WobbleAnimator;
    public float wobbleDecay = .9f;
    float wobbleAmount = 0f;

    
    void Start()
    {
        WobbleAnimator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Health < 0f)
        {
            collider.enabled = false;
            Detroyed.enabled = true;
            Alive.enabled = false;
        }
        WobbleAnimator.SetFloat("Blend", Mathf.Clamp01(wobbleAmount));
        wobbleAmount *= wobbleDecay;
        WobbleAnimator.speed = Mathf.Max(wobbleAmount * .5f, .7f);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DealDamage(5f);
        }
    }

    public void DealDamage(float damage)
    {
        Health -= damage;
        wobbleAmount += damage * .25f;
    }
}
