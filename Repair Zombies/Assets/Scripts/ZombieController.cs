using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {
    private Animator animator;
    private Torso torso;
    
    void Start() {
        torso = this.GetComponent<Torso>();
        animator = this.GetComponent<Animator>();
    }

    void Update() {
        
    }

    public void Attack() {

    }

    public void Move(bool right) {

    }

    private void Stand() {

    }

    private void Crawl() {

    }
}
