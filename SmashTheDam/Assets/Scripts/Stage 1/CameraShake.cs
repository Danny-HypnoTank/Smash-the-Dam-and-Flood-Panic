using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeTime;
    public float shakeSpeed;
    public float shakeIntensty;
    public float secondsPerShake;
    public Vector3 target = new Vector3(0, 0, 0);
    public bool isRunning = false;
    public void StartShake()
    {
        if(isRunning == false)
        {
            isRunning = true;
            StartCoroutine(ApplyShake());
        }
    }
    void Start()
    {
        //StartShake();
    }
    void Update()
    {
        //StartShake();
    }
    private IEnumerator ApplyShake()
    {
        float currentTime = 0;
        float previousTime = 0;
        Vector3 previousPos = new Vector3(0,0,0);
        float pathLength = 0;
        float timeStep = 0;
        SetShakeRandomTarget(out target);
        while(currentTime < shakeTime)
        {
            timeStep = Time.deltaTime + secondsPerShake;
            currentTime += timeStep;
            if(transform.localPosition == target)
            {
                previousPos = target;
                SetShakeRandomTarget(out target);
                pathLength = (target - previousPos).magnitude;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(previousPos, target, ((currentTime - previousTime) * shakeSpeed) / pathLength);
                //Debug.Log( ((currentTime - previousTime) * shakeSpeed) / pathLength);
            }
            yield return new WaitForSeconds(secondsPerShake);
        }
        previousPos = transform.localPosition;
        pathLength = (Vector3.zero - previousPos).magnitude;
        target = Vector3.zero;
        while(transform.localPosition != Vector3.zero)
        {
            timeStep = Time.deltaTime + secondsPerShake;
            currentTime += timeStep;
            transform.localPosition = Vector3.Lerp(previousPos, Vector3.zero, ((currentTime - previousTime) * shakeSpeed) / pathLength);
            yield return new WaitForSeconds(secondsPerShake);
        }
        currentTime = 0;
        isRunning = false;
        yield return 0;
    }

    private void SetShakeRandomTarget(out Vector3 v)
    {
        v = new Vector3(
            Random.Range(-shakeIntensty, shakeIntensty),
            Random.Range(-shakeIntensty, shakeIntensty),
            Random.Range(-shakeIntensty, shakeIntensty));
    }
}
