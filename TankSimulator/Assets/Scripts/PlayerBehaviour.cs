using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Transform turret;
    private Transform manlet;
    private float gunElevationAngles = 0.0f;
    private enum Side {RIGHT, LEFT, MIDDLE};
    private enum Height {UP, DOWN, MIDDLE};
    private enum seat {GUNNER, COMMANDER, DRIVER};
    private GameObject gunnerView;
    private GameObject commanderView;
    private GameObject driverView;
    public AudioSource tankEngineSound;
    public AudioClip tankStartClip;
    public AudioClip tankStopClip;
    public AudioClip tankIdleClip;
    public AudioClip tankHighRPMClip;
    public AudioClip tankLowRPMClip;
    private float engineStartTimer = 3.2f;
    private bool engineStarting = false;
    private float engineStartInitTime = 0.0f;
    private bool engine = false;
    private seat currentSeat = seat.GUNNER;
    private float turretTraverseSpeed = 6f, gunElevationSpeed = 1f, gunDepressionLimit = -6f, gunElevationLimit = 10f;
    private float hullTraverseSpeed = 10f;
    private float tankSpeed = 5f;
    private float aimDeviationMin = 50f, aimHeightDeviationMin = 50f;
    private float lookDeviationMin = 10;
    Vector3 cameraInitialPosition;
	// public float shakeMagnetude = 0.0f, shakeUp = 0.05f, upLim = 0.1f, downLim = -0.1f;
    private Camera currentCamera;
    // Start is called before the first frame update
    void Start()
    {
        // tankEngineSound = GetComponent<AudioSource>();
        turret = gameObject.transform.Find("Turret");
        manlet = turret.Find("Manlet");
        gunnerView = gameObject.transform.Find("Turret").Find("Manlet").Find("GunnerCamera").gameObject;
        if(gunnerView == null) Debug.Log("404 Gunner Not Found");
        commanderView= gameObject.transform.Find("Turret").Find("Cupolla").Find("CommanderCamera").gameObject;
        if(commanderView == null) Debug.Log("404 Commander Not Found");
        driverView = gameObject.transform.Find("Hull").Find("DriverCamera").gameObject;
        if(driverView == null) Debug.Log("404 Driver Not Found");
        currentCamera = gunnerView.GetComponent<Camera>();
        gunnerView.SetActive(true);
        commanderView.SetActive(false);
        driverView.SetActive(false);
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
        Side side = GetScreenSideMouseIs(aimDeviationMin);
        Height height = GetScreenHeightMouseIs(aimHeightDeviationMin);
        if(side == Side.RIGHT){
            turret.Rotate(new Vector3(0,turretTraverseSpeed * Time.deltaTime,0), Space.Self);
        }
        else if(side == Side.LEFT)
        {
            turret.Rotate(new Vector3(0,-turretTraverseSpeed * Time.deltaTime,0), Space.Self);
        }
        if(height == Height.UP)
        {
            gunElevationAngles -= (gunElevationSpeed)*Time.deltaTime;
        }
        else if(height == Height.DOWN)
        {
            gunElevationAngles += (gunElevationSpeed)*Time.deltaTime;
        }
        gunElevationAngles = Mathf.Clamp(gunElevationAngles, gunDepressionLimit, gunElevationLimit);
        Debug.Log(gunElevationAngles);
        manlet.transform.eulerAngles = new Vector3(
            gunElevationAngles,
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
        return;
    }
    void Update()
    {
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
        if(!engine) return;
        if(engineStarting && (Time.time - engineStartInitTime) > engineStartTimer)
        {
            tankEngineSound.Stop();
            engineStarting = false;
            tankEngineSound.clip = tankIdleClip;
            tankEngineSound.loop = true;
            tankEngineSound.Play();
        }
        if(currentSeat == seat.GUNNER)
        {
            GunnerBehaviour();
        }
        if(currentSeat == seat.COMMANDER)
        {
            CommanderBehaviour();
        }
        if(currentSeat == seat.DRIVER)
        {
            DriverBehaviour();
        }
        float axisX = Input.GetAxis("Horizontal");
        float axisY = Input.GetAxis("Vertical");
        transform.Rotate(new Vector3(0,axisX*hullTraverseSpeed*Time.deltaTime,0));
        Vector3 direction = transform.forward * axisY;
        transform.position += direction * Time.deltaTime * tankSpeed;
    }
}
