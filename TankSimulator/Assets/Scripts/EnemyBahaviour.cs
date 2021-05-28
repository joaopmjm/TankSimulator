using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBahaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform turret;
    private Transform hull;
    private float turretTraverseSpeed = 5.0f;
    void Start()
    {
        turret = gameObject.transform.Find("Turret");
        hull = gameObject.transform.Find("Hull");
    }

    // Update is called once per frame
    void Update()
    {
        turret.Rotate(new Vector3(0,turretTraverseSpeed*Time.deltaTime,0), Space.World);
    }
}
