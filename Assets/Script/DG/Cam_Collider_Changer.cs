using UnityEngine;
using Cinemachine;

public class CamCollideChanger : MonoBehaviour
{
    private PolygonCollider2D newCollider;
    private CinemachineConfiner2D a;
    
    private void Start()
    {
        newCollider = GetComponent<PolygonCollider2D>();
        
        a = GameObject.Find("Virtual Camera").GetComponent<CinemachineConfiner2D>();
        a.m_BoundingShape2D = newCollider;
        
        Debug.Log("실행");
    }
}
