using UnityEngine;

public class Offset : MonoBehaviour
{
    [SerializeField] [Range(-1.0f, 1.0f)] private float _MoveSpeed = 0.1f;
    private Material _Material;

    private void Awake()
    {
        _Material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        _Material.SetTextureOffset("_MainTex", Vector2.right * _MoveSpeed * Time.time);
    }
}

