using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCalculation : MonoBehaviour
{
    public static WaterCalculation instance;

    private Material waveComp;

    private float gravity, depth, phase, neighborDist, amp1, amp2, amp3, amp4;

    private Vector3 dir1, dir2, dir3, dir4;

    private Vector4 timeScaled;

    void Awake() 
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    void Start()
    {
        waveComp = GetComponent<MeshRenderer>().material;

        gravity = waveComp.GetFloat("_gravity");
        depth = waveComp.GetFloat("_depth");
        phase = waveComp.GetFloat("_phase");
        neighborDist = waveComp.GetFloat("_neighborDistance");
        amp1 = waveComp.GetFloat("_amplitude1");
        amp2 = waveComp.GetFloat("_amplitude2");
        amp3 = waveComp.GetFloat("_amplitude3");
        amp4 = waveComp.GetFloat("_amplitude4");

        dir1 = waveComp.GetVector("_direction1");
        dir2 = waveComp.GetVector("_direction2");
        dir3 = waveComp.GetVector("_direction3");
        dir4 = waveComp.GetVector("_direction4");

        timeScaled = waveComp.GetVector("_timeScaled");

    }


    //Left as a note on the fact the wave is an offset
    //objectHere.transform.position = new Vector3(objectHere.transform.position.x, GetWavePosition(movepoint.position).y, objectHere.transform.position.z);

    public Vector3 GetWavePosition(Vector3 _currentPos)
    {
        return MultiDisplacement(_currentPos, depth, phase, gravity, dir1, dir2, dir3, dir4, amp1, amp2, amp3, amp4, (timeScaled * Time.time));
    }


    private Vector3 MultiDisplacement(Vector3 _position, float _depth, float _phase, float _gravity, Vector3 _direction1, Vector3 _direction2, Vector3 _direction3, Vector3 _direction4, float _amplitude1, float _amplitude2, float _amplitude3, float _amplitude4, Vector4 _timeScales)
    {
        Vector3 displacement = ((Displacement(_position, _direction1, _depth, _amplitude1, _phase, _timeScales.x, _gravity) + 
                                Displacement(_position, _direction2, _depth, _amplitude2, _phase, _timeScales.y, _gravity)) +
                                (Displacement(_position, _direction3, _depth, _amplitude3, _phase, _timeScales.z, _gravity) +
                                Displacement(_position, _direction4, _depth, _amplitude4, _phase, _timeScales.w, _gravity)))
                                 + _position;
        
        return displacement;        
    }

    private Vector3 Displacement(Vector3 _position, Vector3 _direction, float _depth, float _amplitude, float _phase, float _time, float _gravity)
    {
        float theta = Theta(_position, _direction, _phase, _time, _gravity, _depth);
        Vector3 displacement =  new Vector3();

        displacement.x = (_direction.x / _direction.magnitude) * (_amplitude / (float)System.Math.Tanh(_direction.magnitude * _depth));
        displacement.x = 1 - (Mathf.Sin(theta) * displacement.x);

        displacement.y = Mathf.Cos(theta) * _amplitude;

        displacement.z = (_direction.z / _direction.magnitude) * (_amplitude / (float)System.Math.Tanh(_direction.magnitude * _depth));
        displacement.z = 1 - (Mathf.Sin(theta) * displacement.z);

        return displacement;

    }

    private float Theta(Vector3 _position, Vector3 _direction, float _phase, float _time, float _gravity, float _depth)
    {
        float theta = (_direction.x * _position.x) + (_direction.z * _position.z);
        theta = theta - (Frequency(_gravity, _direction, _depth) * _time);
        theta -= _phase;
        return theta;
    }


    private float Frequency(float _gravity, Vector3 _direction, float _depth)
    {
        float freqNum = _direction.magnitude;
        freqNum = (float)System.Math.Tanh((freqNum * _depth)) * (_gravity * freqNum);
        freqNum = Mathf.Sqrt(freqNum);
        return freqNum;
    }
}
