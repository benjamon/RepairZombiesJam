using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torso : MonoBehaviour
{
    public Animator anim;

    [SerializeField]
    private SocketHandler head, armFront, armBack, legBack, legFront;

    [SerializeField]
    private int crawlThreshold, hopThreshold;

    [SerializeField]
    public float MovementVal { get; private set;}
    public float DamageVal { get; private set; }

    private int movementState = 2;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetLimbVals() 
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
                    MovementVal += _legVals.movementVal*0.75f;
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
