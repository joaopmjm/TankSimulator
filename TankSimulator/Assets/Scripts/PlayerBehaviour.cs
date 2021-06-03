using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    private Transform turret, manlet;
    private Image shellLoadedImage;
    private float gunElevationAngles = 0.0f;
    public enum Side {RIGHT, LEFT, MIDDLE};
    public enum Height {UP, DOWN, MIDDLE};
    enum seat {GUNNER, COMMANDER, DRIVER};
    private Rigidbody rb;
    private GameObject gunnerView, commanderView, driverView;
    public GameObject APCBshell;
    public AudioSource tankEngineSound, CannonSound;
    public AudioClip tankStartClip,tankStopClip, tankIdleClip, tankHighRPMClip, tankLowRPMClip;
    private float engineStartTimer = 3.2f;
    private bool engineStarting = false;
    private float engineStartInitTime = 0.0f;
    private bool engine = false;
    private seat currentSeat = seat.GUNNER;
    private float turretTraverseSpeed = 6f, gunElevationSpeed = 1f, gunDepressionLimit = -3f, gunElevationLimit = 18f, hullTraverseSpeed = 2f;
    private float tankAccelarating = 0.5f, tankBreaking=1f, reloadTime=3.0f, lastShot=0f, maxSpeed=7f, tankVelocity=0f, minSpeed=-1f;
    private float aimDeviationMin = 50f, aimHeightDeviationMin = 50f;
    private float lookDeviationMin = 10;
    Vector3 cameraInitialPosition;
    Camera currentCamera;
    public bool hitEnemy = false;
    public bool gotHit = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        turret = gameObject.transform.Find("Turret");
        manlet = turret.Find("Manlet");
        
        gunnerView = gameObject.transform.Find("Turret").Find("Manlet").Find("GunnerCamera").gameObject;
        if(gunnerView == null) Debug.Log("404 Gunner Not Found");
        shellLoadedImage = gunnerView.transform.Find("Canvas").Find("Image").gameObject.GetComponent<Image>();
        commanderView= gameObject.transform.Find("Turret").Find("Cupolla").Find("CommanderCamera").gameObject;
        if(commanderView == null) Debug.Log("404 Commander Not Found");
        driverView = gameObject.transform.Find("Hull").Find("DriverCamera").gameObject;
        if(driverView == null) Debug.Log("404 Driver Not Found");
        currentCamera = gunnerView.GetComponent<Camera>();
        gunnerView.SetActive(true);
        commanderView.SetActive(false);
        driverView.SetActive(false);
        lastShot = Time.time;
    }

    public void Kill()
    {
        Debug.Log("Player Killed");
    }
    private void Shoot(GameObject shell)
    {
        if(Time.time - lastShot > reloadTime )
        {
            GameObject atirada = Instantiate(shell, manlet.Find("GunBarrel").position, manlet.Find("GunBarrel").rotation);
            atirada.GetComponent<ProjectileBehaviour>().player = gameObject.GetComponent<PlayerBehaviour>();
            atirada.GetComponent<Rigidbody>().AddForce(manlet.forward * atirada.GetComponent<ProjectileBehaviour>().velocity);
            CannonSound.Play();
            lastShot = Time.time;
        }
    }

    private Side GetScreenSideMouseIs(float deviation)
    {
        var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        if( mouse.x < Screen.width/2 - deviation) {
            return Side.LEFT;
        } else if(mouse.x > Screen.width/2 + deviation) {
            return Side.RIGHT;
        } else {
            return Side.MIDDLE;
        }
    }

    private Height GetScreenHeightMouseIs(float deviation)
    {
        var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        if( mouse.y > Screen.height/2 + deviation) {
            return Height.DOWN;
        } else if(mouse.y < Screen.height/2 - deviation) {
            return Height.UP;
        } else {
            return Height.MIDDLE;
        }
    }

    private void CameraChangeCheck()
    {
        if(Input.GetButtonDown("Key1") && currentSeat != seat.GUNNER){
            gunnerView.SetActive(true);
            commanderView.SetActive(false);
            driverView.SetActive(false);
            currentSeat = seat.GUNNER;
            currentCamera = gunnerView.GetComponent<Camera>();
        }
        if(Input.GetButtonDown("Key2") && currentSeat != seat.COMMANDER){
            gunnerView.SetActive(false);
            commanderView.SetActive(true);
            driverView.SetActive(false);
            currentSeat = seat.COMMANDER;
            currentCamera = commanderView.GetComponent<Camera>();
        }
        if(Input.GetButtonDown("Key3") && currentSeat != seat.DRIVER){
            gunnerView.SetActive(false);
            commanderView.SetActive(false);
            driverView.SetActive(true);
            currentSeat = seat.DRIVER;
            currentCamera = driverView.GetComponent<Camera>();
        }
    }

    private void GunnerBehaviour()
    {
        float traverseVel = turretTraverseSpeed;
        if(!engine) traverseVel = turretTraverseSpeed/2;
        shellLoadedImage.fillAmount = Mathf.Clamp(Time.time - lastShot, 0, reloadTime)/reloadTime;
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot(APCBshell);
        }
        Side side = GetScreenSideMouseIs(aimDeviationMin);
        Height height = GetScreenHeightMouseIs(aimHeightDeviationMin);
        if(side == Side.RIGHT){
            turret.Rotate(new Vector3(0,traverseVel * Time.deltaTime,0), Space.Self);
        }
        else if(side == Side.LEFT)
        {
            turret.Rotate(new Vector3(0,-traverseVel * Time.deltaTime,0), Space.Self);
        }
        if(height == Height.DOWN)
        {
            gunElevationAngles -= (gunElevationSpeed)*Time.deltaTime;
        }
        else if(height == Height.UP)
        {
            gunElevationAngles += (gunElevationSpeed)*Time.deltaTime;
        }
        gunElevationAngles = Mathf.Clamp(gunElevationAngles, gunDepressionLimit, gunElevationLimit);
        manlet.transform.eulerAngles = new Vector3(
            -gunElevationAngles,
            turret.transform.eulerAngles.y,
            turret.transform.eulerAngles.z
        );
    }

    private void CommanderBehaviour()
    {
        Side side = GetScreenSideMouseIs(lookDeviationMin);
            if(side == Side.RIGHT){
                commanderView.transform.Rotate(new Vector3(0,turretTraverseSpeed * Time.deltaTime,0), Space.Self);
            }
            else if(side == Side.LEFT)
            {
                commanderView.transform.Rotate(new Vector3(0,-turretTraverseSpeed * Time.deltaTime,0), Space.Self);
            }
    }

    private void DriverBehaviour()
    {
        float axisX = Input.GetAxis("Horizontal");
        float axisY = Input.GetAxis("Vertical");
        Vector3 m_EulerAngleVelocity = new Vector3(0, axisX*hullTraverseSpeed, 0);
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
        if(axisY > 0)tankVelocity += axisY * tankAccelarating * Time.deltaTime;
        if(axisY < 0)tankVelocity += axisY * tankBreaking * Time.deltaTime;
        tankVelocity = Mathf.Clamp(tankVelocity, minSpeed, maxSpeed);
        Vector3 dir = transform.forward * tankVelocity;
        dir.y = 0;
        rb.velocity = dir;
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
    void Update()
    {
        Vector3 dir = transform.forward * tankVelocity;
        CameraChangeCheck();
        if(Input.GetButtonDown("KeyV")) // IgniteEngine is "V"
        {
            engine = !engine;
            if(engine)
            {
                tankEngineSound.Stop();
                engineStarting = true;
                engineStartInitTime = Time.time;
                tankEngineSound.clip = tankStartClip;
                tankEngineSound.loop = false;
                tankEngineSound.Play();
            }
            else
            {
                tankEngineSound.Stop();
                tankEngineSound.clip = tankStopClip;
                tankEngineSound.loop = false;
                tankEngineSound.Play();
            }
        }
        if(currentSeat == seat.GUNNER)
        {
            GunnerBehaviour();
        }
        if(!engine) return;
        if(engineStarting && (Time.time - engineStartInitTime) > engineStartTimer)
        {
            tankEngineSound.Stop();
            engineStarting = false;
            tankEngineSound.clip = tankIdleClip;
            tankEngineSound.loop = true;
            tankEngineSound.Play();
        }
        
        if(currentSeat == seat.COMMANDER)
        {
            CommanderBehaviour();
        }
        if(currentSeat == seat.DRIVER)
        {
            DriverBehaviour();
        }
    }
}
