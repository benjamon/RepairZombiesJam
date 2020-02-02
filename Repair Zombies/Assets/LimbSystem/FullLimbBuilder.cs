using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullLimbBuilder : MonoBehaviour
{
    public Limb[] limbs = new Limb[3];

    public Limb BuildLimb(Vector3 _pos)
    {
        Limb limb0 = Instantiate(limbs[0], _pos, Quaternion.identity);

        if(limbs[1] != null)
        {
            Limb limb1 = Instantiate(limbs[1], _pos, Quaternion.identity);
            limb0.AttachChild(limb1);
            if(limbs[2] != null)
            {
                limb1.AttachChild(Instantiate(limbs[2], _pos, Quaternion.identity));
            }
        }

        return limb0;
    }

}
