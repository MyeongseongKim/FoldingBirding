using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public Animator Animator 
    {
        get { return _animator; }
    }

    [SerializeField] private IBirdState _currentState;
    private Coroutine _behaviourCoroutine;
    public Coroutine BehaviourCoroutine {
        get { return _behaviourCoroutine; }
        set { _behaviourCoroutine = value; }
    }


    [Header("Moving")]
    [SerializeField] private Vector3 _direction;
    public Vector3 Direction {
        get { return _direction; }
        set { _direction = value; }
    }
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _detection;
    [SerializeField] private Transform _target;
    public Transform Target {
        get { return _target; }
        set { _target = value; }
    }

    [Header("Avoidance")]
    [SerializeField] private float _minAvoidStrength;
    [SerializeField] private float _maxAvoidStrength;

    [Header("Wander")]
    [SerializeField] private float _wanderStrength;
    private float _nextWanderTime;

    [Header("Spot")]
    [SerializeField] private Spot _curSpot;
    public Spot CurSpot {
        get { return _curSpot; }
        set { _curSpot = value; }
    }
    [SerializeField] private Spot _preSpot;
    public Spot PreSpot {
        get { return _preSpot; }
        set { _preSpot = value; }
    }
    [SerializeField] private GameObject _spotContainer;
    [SerializeField] private List<Spot> _perchingSpots;


    void Start()
    {
        if (_spotContainer != null) 
        {
            for (int i = 0; i < _spotContainer.transform.childCount; i++) 
            {
                _perchingSpots.Add(_spotContainer.transform.GetChild(i).GetComponent<Spot>());
            }
        }

        transform.rotation = UnityEngine.Random.rotation;
        _direction = transform.forward;

        TransitState(new NullState());
    }


    void Update()
    {
        _currentState?.Update(this);

    }

    public void OnPalmUpSelected() 
    {
        _currentState?.HandlePalmUpSelected(this);
    }

    public void OnPalmUpUnselected() 
    {
        _currentState?.HandlePalmUpUnselected(this);
    }

    public void OnPerchSelected() 
    {
        _currentState?.HandlePerchSelected(this);
    }

    public void OnPerchUnselected() 
    {
        _currentState?.HandlePerchUnselected(this);
    }


    public void TransitState(IBirdState state) 
    {
        _currentState?.Exit(this);
        _currentState = state;
        _currentState?.Enter(this);
    }

    
    public IEnumerator HangAround(Action<BirdBehaviour> onDone) 
    {
        float waitTime = UnityEngine.Random.Range(5f, 15f);
        yield return new WaitForSeconds(waitTime);
        onDone?.Invoke(this);
    } 


    public IEnumerator HoverAround(Action<BirdBehaviour> onDone)  
    {
        while (true) 
        {            
            Vector3 toTarget = _target.position - transform.position;
            float distanceToTarget = toTarget.magnitude;
            float factor = Mathf.Clamp01(distanceToTarget / _detection);

            _direction = Vector3.SlerpUnclamped(_direction, toTarget.normalized, Time.deltaTime).normalized;

            Quaternion rot = Quaternion.LookRotation(_direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, _rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * _speed * Time.deltaTime;
 
            yield return null;
        }
    }


    public IEnumerator ApproachTo(Action<BirdBehaviour> onDone)
    {
        Vector3 p0 = transform.position;
        Vector3 m0 = transform.forward;
        Vector3 p1 = _target.position;
        Vector3 m1 = _target.forward;

        float totalLength = Utils.EstimateHermiteLength(p0, m0, p1, m1);
        float duration = totalLength / _speed;

        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / duration;
            Vector3 pos = Utils.Hermite(p0, m0, p1, m1, t);
            Vector3 tangent = Utils.Hermite(p0, m0, p1, m1, t + 0.001f) - pos;
            
            transform.position = pos;
            transform.rotation = Quaternion.LookRotation(tangent.normalized, Vector3.up);

            yield return null;
        }

        transform.position = p1;
        transform.forward = m1;
        onDone?.Invoke(this);
    }


    public IEnumerator FlyAround(Action<BirdBehaviour> onDone) 
    {
        while (true) 
        {
            // Avoiding with sampling
            Vector3 heading = transform.forward;
            Vector3[] samples = {
                Quaternion.AngleAxis(15, transform.up) * heading,
                Quaternion.AngleAxis(-15, transform.up) * heading,
                Quaternion.AngleAxis(15, transform.right) * heading,
                Quaternion.AngleAxis(-15, transform.right) * heading
            };

            RaycastHit hit;
            // if (Physics.Raycast(transform.position, heading, out hit, _detection))
            if (Physics.SphereCast(transform.position, 0.1f, heading, out hit, _detection))
            {
                var count = 0;
                var noramlSum = Vector3.zero;
                
                RaycastHit sampleHit;
                foreach (var rayDir in samples) {
                    if (Physics.Raycast(transform.position, rayDir, out sampleHit, _detection)) {
                        count++;
                        noramlSum += sampleHit.normal;
                    }
                }
                var avgNormal = (noramlSum / count).normalized;

                Vector3 avoidDir;
                if (noramlSum == Vector3.zero) {
                    avoidDir = -_direction;
                }
                else {
                    avoidDir = Vector3.Reflect(_direction, avgNormal).normalized;
                }

                float t = Mathf.Clamp01(1f - hit.distance / _detection);
                float avoidStrength = Mathf.Lerp(_minAvoidStrength, _maxAvoidStrength, t);

                _direction = Vector3.SlerpUnclamped(_direction, avoidDir, count * avoidStrength * Time.deltaTime).normalized;
            }

            // Wandering
            else 
            {
                if (Time.time >= _nextWanderTime) 
                {
                    Vector3 randomJitter = _wanderStrength * UnityEngine.Random.onUnitSphere;
                    _direction = (_direction + randomJitter).normalized;
                    _nextWanderTime = Time.time + UnityEngine.Random.value;
                }
            }

            Quaternion rot = Quaternion.LookRotation(_direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, _rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * _speed * Time.deltaTime;

            yield return null;
        }
    }


    public Spot FindPerchingSpot() 
    {
        foreach (var spot in _perchingSpots) 
        {
            float distance = Vector3.Distance(transform.position, spot.transform.position);
            if (distance < _detection * 0.5f) 
            {
                return spot;
            }
        }
        return null;
    }

}
