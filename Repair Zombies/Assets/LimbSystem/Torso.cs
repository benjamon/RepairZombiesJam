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

    public float MovementVal { get; private set;}
    public float DamageVal { get; private set; }

    private bool isAwake = false;

    private int movementState = 2;



    // Start is called before the first frame update
    void Start()
    {
        GetComponent<InitialZombieParts>().AttachAll();
    }

    void LateStart()
    {
        isAwake = true;
        GetLimbVals();
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

        if (!armFront.IsAttachable) sockets.Add(armFront);
        if (!armBack.IsAttachable) sockets.Add(armBack);
        if (!legFront.IsAttachable) sockets.Add(legFront);
        if (!legBack.IsAttachable) sockets.Add(legBack);
        if (!head.IsAttachable) sockets.Add(head);

        sockets[(int)(UnityEngine.Random.value * sockets.Count)].TakeDamage(damage);
    }
}
