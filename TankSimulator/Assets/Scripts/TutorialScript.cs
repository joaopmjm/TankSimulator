using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Waypoints;
    public GameObject[] Targets;
    private GameObject currentWaypoint, currentTarget;
    private int currentWaypointId = 0, currentTargetId; 
    private bool StartEngine = false, ControlTurret=false, ChangeViews=false, MovedAroundBase=false, HittedAllTargets=false;
    bool up=false, down=false, left=false, right=false, gunner=true, commander=false, driver=false, moved=false, fired=false;
    void Start()
    {
        currentWaypoint = Waypoints[currentWaypointId];
        currentTarget = Targets[currentTargetId];
        foreach(GameObject waypoint in Waypoints){
            waypoint.SetActive(false);
        }
        PutText("Press V to start your engine");
    }
    void PutText(string texto)
    {
        Camera.allCameras[0].transform.Find("Canvas").Find("Text (TMP)").gameObject.GetComponent<TMP_Text>().SetText(texto);
    }

    void StartEngineCheck()
    {
        if(Input.GetButtonDown("KeyV"))
        {
            StartEngine = true;
        }
    }

    

    void ControlTurretCheck()
    {
        var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        if(mouse.y < Screen.height/2) up = true;
        if(mouse.y > Screen.height/2) down = true;
        if(mouse.x > Screen.width/2) right = true;
        if(mouse.x < Screen.width/2) left = true;
        if(left && right && up && down) ControlTurret = true;
    }

    void ChangeViewsCheck()
    {
        if(Input.GetButtonDown("Key2")) commander = true;
        if(Input.GetButtonDown("Key3")) driver = true;
        if(commander && gunner && driver) ChangeViews = true; 
    }

    void FollowWaypoints()
    {
        if(Input.GetAxis("Horizontal") != 0) moved=true;
        currentWaypoint.SetActive(true);
        if(currentWaypoint.GetComponent<Waypoint>().arrived)
        {
            Destroy(currentWaypoint);
            currentWaypointId++;
            if(currentWaypoint == Waypoints[Waypoints.Length-1])
            {
                MovedAroundBase = true;
                return;
            }
            currentWaypoint = Waypoints[currentWaypointId]; 
        }
    }

    void HittedAllTargetsCheck()
    {
        if(currentTarget.GetComponent<Target>().hitted)
        {
            currentTargetId++;
            if(currentTarget == Targets[Targets.Length-1])
            {
                HittedAllTargets = true;
                return;
            }
            currentTarget = Targets[currentTargetId];
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(!StartEngine)
        {
            PutText("Press V to start your engine");
            StartEngineCheck();
            return;
        }
        PutText("");
        if(!ControlTurret)
        {
            PutText("Use your mouse to move the turret");
            ControlTurretCheck();
            return;
        }
        PutText("");
        if(!ChangeViews)
        {
            PutText("Use nuber 1, 2 and 3 to change between the tank crew");
            ChangeViewsCheck();
            return;
        }
        PutText("");
        if(!MovedAroundBase)
        {
            PutText("");
            if(!moved) PutText("Follow the waypoint using WASD on driver view, hold them to move");
            FollowWaypoints();
            return;
        }
        PutText("");
        if(!HittedAllTargets)
        {
            PutText("Aim at the closest target, press mouse 1 to shoot, then aim for the next");
            if(fired) PutText("");
            if(Input.GetButtonDown("Fire1")) fired = true;
            HittedAllTargetsCheck();
            return;
        }
        PutText("You Finished the Tutorial");
    }
}
