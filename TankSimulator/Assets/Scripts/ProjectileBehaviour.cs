using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ProjectileBehaviour : MonoBehaviour
{
    public float caliber;
    public float velocity;
    public float penn;
    private Rigidbody obj;
    public GameObject sparkParticles, smokeParticles;
    public PlayerBehaviour player;
    AudioSource impactSound;
    GameObject mySpark, mySmoke;
    private float dieTimer = 5.0f;
    private bool hitted = false;
    private float hitTime;
    void Start()
    {
        impactSound = GetComponent<AudioSource>();
        hitted=false;
    }

    private bool IsPenetrated(float thickness, Vector3 normal)
    {
        Debug.Log(Mathf.Abs(normal.x) * penn);
        if(Mathf.Abs(normal.x) * penn > thickness) return true;
        return false;
    }

    void Update()
    {
        if(hitted && (Time.time - hitTime) > dieTimer)
        {
            Destroy(mySpark);
            Destroy(gameObject);
            if(mySmoke != null) Destroy(mySmoke);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if(hitted) return;
        if(col.gameObject.CompareTag("armour"))
        {
            mySpark = Instantiate(sparkParticles, gameObject.transform.position, Quaternion.Euler(col.contacts[0].normal));
            if(col.gameObject.GetComponent<ArmourBehaviour>().owner.CompareTag("Player"))
            {
                if(IsPenetrated(col.collider.gameObject.GetComponent<ArmourBehaviour>().thickness, col.contacts[0].normal))
                {
                    col.collider.gameObject.GetComponent<ArmourBehaviour>().owner.GetComponent<PlayerBehaviour>().Kill();
                }
            }
            else if(col.gameObject.GetComponent<ArmourBehaviour>().owner.CompareTag("Enemy"))
            {
                if(IsPenetrated(col.collider.gameObject.GetComponent<ArmourBehaviour>().thickness, col.contacts[0].normal))
                {
                    Debug.Log("Destroyed");
                    mySmoke = Instantiate(smokeParticles, gameObject.transform.position, Quaternion.Euler(col.contacts[0].normal));
                    col.collider.gameObject.GetComponent<ArmourBehaviour>().owner.GetComponent<EnemyBahaviour>().Kill();
                }
                else
                {
                    col.collider.gameObject.GetComponent<ArmourBehaviour>().owner.GetComponent<EnemyBahaviour>().DealWithGettingShot();
                }
            }
            
            
        }
        hitted = true;
        hitTime = Time.time;
    }
}
