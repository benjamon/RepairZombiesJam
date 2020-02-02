using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullLimbBuilder : MonoBehaviour
{
    public Limb[] limbs = new Limb[3];

    public Limb BuildLimb()
    {
        Limb limb0 = Instantiate(limbs[0]);

        if(limbs[1] != null)
        {
            Limb limb1 = Instantiate(limbs[1]);
            limb0.AttachChild(limb1);
            if(limbs[2] != null)
            {
                limb1.AttachChild(Instantiate(limbs[2]));
            }
        }

        return limb0;
    }

}
