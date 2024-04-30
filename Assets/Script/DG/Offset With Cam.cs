using UnityEngine;

public class OffsetWithCam : MonoBehaviour
{
    [SerializeField] private Transform _CameraTransfrom;

    private Vector3 _CameraStartPosition;
    private float _Distance;

    private Material[] _Materials;
    private float[] _LayerMoveSpeed;

    [SerializeField] [Range(0.01f, 1.0f)] private float _ParallaxSpeed;

    private void Awake()
    {
        _CameraStartPosition = _CameraTransfrom.position;

        int BackGroundCount = transform.childCount;
        GameObject[] BackGrounds = new GameObject[BackGroundCount];

        _Materials = new Material[BackGroundCount];
        _LayerMoveSpeed = new float[BackGroundCount];

        for (int i = 0; i < BackGroundCount; ++i)
        {
            BackGrounds[i] = transform.GetChild(i).gameObject;
            _Materials[i] = BackGrounds[i].GetComponent<Renderer>().material;
        }
        
        CalculateMoveSpeedByLayer(BackGrounds,BackGroundCount);
    }

    private void CalculateMoveSpeedByLayer(GameObject[] BackGrounds, int Count)
    {
        float FarthestBackDistance = 0;
        for (int i = 0; i < Count; i++)
        {
            if ((BackGrounds[i].transform.position.z - _CameraTransfrom.position.z) > FarthestBackDistance)
            {
                FarthestBackDistance =
                    BackGrounds[i].transform.position.z - _CameraTransfrom.position.z;
            }
        }

        for (int i = 0; i < Count; i++)
        {
            _LayerMoveSpeed[i] =
                1 - (BackGrounds[i].transform.position.z - _CameraTransfrom.position.z) /
                FarthestBackDistance;
        }
    }

    private void LateUpdate()
    {
        _Distance = _CameraTransfrom.position.x - _CameraStartPosition.x;
        transform.position = new Vector3(_CameraTransfrom.position.x, transform.position.y, 0);

        for (int i = 0; i < _Materials.Length; i++)
        {
            float Speed = _LayerMoveSpeed[i] * _ParallaxSpeed;
            _Materials[i].SetTextureOffset("_MainTex",new Vector2(_Distance,0)*Speed);
        }
    }
}