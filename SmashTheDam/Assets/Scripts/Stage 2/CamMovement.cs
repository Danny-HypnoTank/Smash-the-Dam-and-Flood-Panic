using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum WaitType
{
    eNone,
    eTime,
    eBool
}

[System.Serializable]
public class CamWaypoint
{
    public Vector3 pos = new Vector3();
    public Vector3 euler = new Vector3();
    public WaitType waitingType;
    [SerializeField]
    public UnityEvent waitAction;
    public float waitTime;
    public float length;
}

public class CamMovement : MonoBehaviour
{
    public List<CamWaypoint> waypoints = new List<CamWaypoint>();
    public float speed;
    [SerializeField]
    private float CamTransitionSpeed;
    private int waypointIndex = 0;
    private float t = 0;
    private bool waiting = false;
    public bool playerHasLost = false;
    void Start()
    {
        for(int i = 0; i < waypoints.Count; i++)
        {
            if(i == waypoints.Count - 1)
            {
                waypoints[i].length = 0;
                break;
            }
            else
            {
                waypoints[i].length = Vector3.Magnitude(waypoints[i+1].pos - waypoints[i].pos);
            }
        }
    }
    void Update()
    {
        if(waypointIndex == waypoints.Count - 1){return;}
        if(!waiting){t += Time.deltaTime * speed;}
        else{t = 0;}
        while(t > waypoints[waypointIndex].length)
        {
            if(waypointIndex == waypoints.Count - 1)
            {
                t = 0;
                transform.position = waypoints[waypointIndex].pos;
                return;
            }
            t -= waypoints[waypointIndex].length;
            waypointIndex++;
            switch(waypoints[waypointIndex].waitingType)
            {
                case WaitType.eNone:
                    break;
                case WaitType.eTime:
                    waiting = true;
                    StartCoroutine(WayPointWaitForLoss());
                    break;
                case WaitType.eBool:
                    waiting = true;
                    waypoints[waypointIndex].waitAction.Invoke();
                    break;
            }
        }


        if (waypointIndex == 1)
        {
            Debug.Log("running");
            t += Time.deltaTime * CamTransitionSpeed;
            transform.position = Vector3.Lerp(waypoints[waypointIndex].pos, waypoints[waypointIndex + 1].pos, t / waypoints[waypointIndex].length);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(waypoints[waypointIndex].euler,waypoints[waypointIndex + 1].euler,  t / waypoints[waypointIndex].length));
        }
        else
        {
            transform.position = Vector3.Lerp(waypoints[waypointIndex].pos, waypoints[waypointIndex + 1].pos, t / waypoints[waypointIndex].length);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(waypoints[waypointIndex].euler, waypoints[waypointIndex + 1].euler, t / waypoints[waypointIndex].length));
        }
    }
    public void StartWaitForPlayerLoss()
    {
        StartCoroutine(WayPointWaitForLoss());
    }
    private IEnumerator WayPointWaitForLoss()
    {
        while(!playerHasLost)
        {
            yield return new WaitForSeconds(0.1f);
        }
        waiting = false;
    }
    private IEnumerator WayPointWaitTime()
    {
        yield return new WaitForSeconds(waypoints[waypointIndex].waitTime);
        waiting = false;
    }
    void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        Gizmos.color = Color.green;
        for(int i = 0; i < waypoints.Count; i++)
        {
            Gizmos.DrawWireSphere(waypoints[i].pos, 5);
        }
        #endif
    }
}
