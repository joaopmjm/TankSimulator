using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool arrived = false;
    // Update is called once per frame
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == "Terrain") return;
        Destroy(GetComponent<BoxCollider>());
        arrived = true;
    }
}
