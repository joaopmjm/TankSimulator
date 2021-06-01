using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourBehaviour : MonoBehaviour
{
    public float thickness;
    public float angle;
    public bool penned = false;
    public GameObject owner;
    // Start is called before the first frame update
    public bool GotPenetrated()
    {
        return false;
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Got hit!");
        }
    }

}
