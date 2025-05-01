using UnityEngine;


public class UserSpots : MonoBehaviour
{
    public static UserSpots Instance { get; private set; }

    [SerializeField] private Transform _centerEyeAnchor;
    [SerializeField] private Transform _leftIndexAnchor;
    [SerializeField] private Transform _rightIndexAnchor;

    [SerializeField] private GameObject _hoverSpotObject;
    [SerializeField] private GameObject _perchSpotObject;

    public Transform GetHoverTarget() 
    {
        return _hoverSpotObject.transform;
    }

    public Transform GetPerchTarget() 
    {
        return _hoverSpotObject.transform;
    }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _hoverSpotObject = new GameObject("Hover Head Spot");
        _hoverSpotObject.transform.SetParent(this.transform);
        
        _perchSpotObject = new GameObject("Perch Index Spot");
        _perchSpotObject.transform.SetParent(this.transform);
    }


    void Update()
    {
        _hoverSpotObject.transform.rotation = _centerEyeAnchor.rotation;
        _hoverSpotObject.transform.position = 
            _centerEyeAnchor.position + _centerEyeAnchor.up;

        _hoverSpotObject.transform.rotation = Quaternion.LookRotation(
            -_rightIndexAnchor.up, -_rightIndexAnchor.right
        );
        _hoverSpotObject.transform.position = _rightIndexAnchor.position;
    }
}
