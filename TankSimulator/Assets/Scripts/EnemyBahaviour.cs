using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBahaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform turret, hull;
    public AudioSource CannonSound;
    public GameObject shell;
    public bool destroyed = false;
    public bool gotShot = false;
    private float turretTraverseSpeed = 5.0f, reloadTime=4.0f, lastShot=0, hullTraverseSpeed=4f, minDistanceFromPlayer=150, minAngle = 30;
    void Start()
    {
        turret = gameObject.transform.Find("Turret");
        hull = gameObject.transform.Find("Hull");
    }

    public void Kill()
    {
        if(destroyed) return;
        destroyed = true;
        Rigidbody turretRigid = turret.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        float force = 5000;
        turretRigid.mass = 3000;
        turretRigid.AddForce(transform.up * force);
        turretRigid.AddTorque(new Vector3(Random.value*10,Random.value*10,Random.value*10));
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

    private bool IsLeft(float a, float b)
    {
        float right;
        float left;
        if(a > b)
        {
            right = 360-a+b;
            left = a-b;
        }
        else
        {
            right = b-a;
            left = 360-b+a;
        }
        return left < right;
    }

    public void DealWithGettingShot()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Raycast(turret.Find("Barrel").Find("Cannon").position, turret.Find("Barrel").Find("Cannon").TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            try
            {
                if((bool)hit.collider?.GetComponent<ArmourBehaviour>().owner.CompareTag("Player"))
                {
                    Shoot();
                    return;
                }
            }catch
            {
                Debug.Log("");
            }  
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                Shoot();
                return;
            }
        }
        Transform cannon = turret.Find("Barrel").Find("Cannon");
        if(IsLeft(cannon.rotation.eulerAngles.y,cannon.Find("Observer").rotation.eulerAngles.y))
        {
            turret.Rotate(new Vector3(0,-turretTraverseSpeed*Time.deltaTime ,0), Space.World);
        }
        else
        {
            turret.Rotate(new Vector3(0,turretTraverseSpeed*Time.deltaTime ,0), Space.World);
        }
    }

    private bool InDistanceFromPlayer()
    {
        Transform player = FindPlayer();
        float dist = Vector3.Distance(player.position, transform.position);
        return dist < minDistanceFromPlayer;
    }

    private bool PlayerInSight(float a, float b)
    {
        float right;
        float left;
        if(a > b)
        {
            right = 360-a+b;
            left = a-b;
        }
        else
        {
            right = b-a;
            left = 360-b+a;
        }
        if(InDistanceFromPlayer())
        {
            if(Mathf.Min(Mathf.Abs(left), Mathf.Abs(right)) < minAngle) return true;
        }
        return false;
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

    void Update()
    {
        if(destroyed) return;
        turret.Find("Barrel").Find("Cannon").Find("Observer").LookAt(FindPlayer());
        if(gotShot) DealWithGettingShot();
        if(PlayerInSight(turret.Find("Barrel").Find("Cannon").rotation.eulerAngles.y,turret.Find("Barrel").Find("Cannon").Find("Observer").rotation.eulerAngles.y)) DealWithGettingShot();
    }
}
