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
    private int crawlThreshold, hopThreshold;

    public float MovementVal;
    public float DamageVal;
    public int movementState = 2;

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
            MovementVal = 0;
            DamageVal = 0;


            Limb.LimbValsMultiplier _headMult = head.GetHeadMult();
            Limb.LimbVals _armVals = armFront.GetVals().Add(armBack.GetVals()).Multiply(_headMult);
            Limb.LimbVals _legVals = legFront.GetVals().Add(legBack.GetVals()).Multiply(_headMult);

            if (_legVals.legScore <= crawlThreshold)
            {
                movementState = 0;
            }
            else if (_legVals.legScore <= hopThreshold)
            {
                movementState = 1;
            }
            else
            {
                movementState = 2;
            }

            switch (movementState)
            {
                case 2:
                    {
                        MovementVal += _legVals.movementVal;
                        DamageVal += _armVals.damageVal;
                        break;
                    }
                case 1:
                    {
                        MovementVal += _legVals.movementVal * 0.75f;
                        DamageVal += _armVals.damageVal * 0.5f;
                        break;
                    }
                case 0:
                    {
                        MovementVal += _armVals.movementVal * 0.5f;
                        DamageVal += _legVals.damageVal * 0.5f;
                        break;
                    }
            }
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
