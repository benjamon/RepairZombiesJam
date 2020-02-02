using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torso : MonoBehaviour, IDamageable
{
    public Animator anim;

    [SerializeField]
    private SocketHandler head, armFront, armBack, legBack, legFront;

    [SerializeField]
    private float crawlThreshold, hopThreshold;

    public float MovementVal;
    public float DamageVal;

    public PostureState posture;
    private bool isAwake = false;


    //USE MOVEMENTVAL TO GET MOVEMENT SPEED
    //USE DAMAGEVAL TO GET DAMAGE STAT
    //USE MOVEMENTSTATE TO TELL WHETHER TO CRAWL HOP OR WALK
    //USE DEAL DAMAGE TO DEAL DAMAGE TO THE ZOMBIE
    //USE HASHEAD TO GET WHETHER OR NOT THE ZOMBIE HAS A USABLE HEAD

    
    void Awake()
    {
        GetComponent<InitialZombieParts>().AttachAll();
    }

    void Start()
    {
        isAwake = true;
        GetLimbVals();
    }

    public bool HasHead()
    {
        return head.attachedLimb != null && head.attachedLimb.IsHead;
    }

    public void GetLimbVals() 
    {
        if (isAwake)
        {
            Limb.LimbValsMultiplier _headMult = head.GetHeadMult();
            Limb.LimbVals _armVals = armFront.GetVals().Add(armBack.GetVals()).Multiply(_headMult);
            Debug.Log("armVals" + _armVals);
            Limb.LimbVals _legVals = legFront.GetVals().Add(legBack.GetVals()).Multiply(_headMult);
            Debug.Log("legVals" + _legVals);

            GetMovementState(_legVals.legScore);

            switch (posture)
            {
                case PostureState.Stand:
                    {
                        MovementVal = _legVals.movementVal;
                        DamageVal = _armVals.damageVal;
                        break;
                    }
                case PostureState.Hobble:
                    {
                        MovementVal = _legVals.movementVal * 0.75f;
                        DamageVal = _armVals.damageVal * 0.5f;
                        break;
                    }
                case PostureState.Crawl:
                    {
                        MovementVal = _armVals.movementVal * 0.5f;
                        DamageVal = _legVals.damageVal * 0.5f;
                        break;
                    }
            }
            MovementVal = Math.Max(MovementVal, 0.5f);
            DamageVal = Math.Max(DamageVal, 0f);
        }
    }

    private void GetMovementState(int legScore)
    {
        if (legBack.attachedLimb == null && legFront.attachedLimb == null) posture = PostureState.Crawl;
        else if (legBack.attachedLimb == null)
        {
            if (legFront.attachedLimb.NumChildren <= 1) posture = PostureState.Crawl;
            else posture = PostureState.Hobble;
        }
        else if (legFront.attachedLimb == null)
        {
            if (legBack.attachedLimb.NumChildren <= 1) posture = PostureState.Crawl;
            else posture = PostureState.Hobble;
        }
        else if (legBack.attachedLimb.NumChildren <= 1 && legFront.attachedLimb.NumChildren <= 1) posture = PostureState.Crawl;
        else if (legBack.attachedLimb.NumChildren <= 1 || legFront.attachedLimb.NumChildren <= 1) posture = PostureState.Hobble;
        else posture = PostureState.Stand;


        if (legScore <= crawlThreshold && (posture == PostureState.Hobble || posture == PostureState.Stand))
        {
            posture = PostureState.Crawl;
        }
        else if (legScore <= hopThreshold && posture == PostureState.Stand)
        {
            posture = PostureState.Hobble;
        }
    }

    public void DealDamage(float damage)
    {
        List<SocketHandler> sockets = new List<SocketHandler>();

        if (armFront.gameObject.layer == LayerMask.NameToLayer("Detachable")) sockets.Add(armFront);
        if (armBack.gameObject.layer == LayerMask.NameToLayer("Detachable")) sockets.Add(armBack);
        if (legFront.gameObject.layer == LayerMask.NameToLayer("Detachable")) sockets.Add(legFront);
        if (legBack.gameObject.layer == LayerMask.NameToLayer("Detachable")) sockets.Add(legBack);
        if (head.gameObject.layer == LayerMask.NameToLayer("Detachable")) sockets.Add(head);

        sockets[(int)(UnityEngine.Random.value * sockets.Count)].TakeDamage(damage);
    }
}
