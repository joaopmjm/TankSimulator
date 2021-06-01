using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBahaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform turret, hull;
    public AudioSource CannonSound;
    public GameObject shell;
    private bool destroyed = false;
    private float turretTraverseSpeed = 5.0f, reloadTime=4.0f, lastShot=0, hullTraverseSpeed=4f;
    void Start()
    {
        turret = gameObject.transform.Find("Turret");
        hull = gameObject.transform.Find("Hull");
    }

    public void Kill()
    {
        destroyed = true;
        Rigidbody turretRigid = turret.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        float force = 1000;
        turretRigid.AddForce(transform.up * force);
    }

    private void Shoot()
    {
        if(Time.time - lastShot > reloadTime )
            {
                GameObject atirada = Instantiate(shell, transform.Find("Turret").Find("Barrel").Find("Cannon").position, transform.Find("Turret").Find("Barrel").Find("Cannon").rotation);
                atirada.GetComponent<ProjectileBehaviour>().player = gameObject.GetComponent<PlayerBehaviour>();
                atirada.GetComponent<Rigidbody>().AddForce(transform.Find("Turret").Find("Barrel").Find("Cannon").forward * atirada.GetComponent<ProjectileBehaviour>().velocity);
                CannonSound.Play();
                lastShot = Time.time;
            }
    }
    private Transform FindPlayer()
    {
        return GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    public void DealWithGettingShot()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(turret.Find("Barrel").Find("Cannon").position, turret.Find("Barrel").Find("Cannon").TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log($"Did Hit {hit.collider.name}");
            if(hit.collider.gameObject.CompareTag("armour") || hit.collider.gameObject.CompareTag("Player"))
            {
                Shoot();
            }
            else
            {
                turret.Rotate(new Vector3(0,-turretTraverseSpeed*Time.deltaTime ,0), Space.World);
            }
        }
        else
        {
            Debug.Log("Did not Hit");
            turret.Rotate(new Vector3(0,-turretTraverseSpeed*Time.deltaTime ,0), Space.World);
        }
    }

    void TurnTank(string side)
    {
        if(side == "right")
        {
            transform.Rotate(new Vector3(0,hullTraverseSpeed*Time.deltaTime,0), Space.World);
        }
        else if(side == "left")
        {
            transform.Rotate(new Vector3(0,-hullTraverseSpeed*Time.deltaTime,0), Space.World);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(destroyed) return;
        TurnTank("left");
    }
}
