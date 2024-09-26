using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Vector3 offset;
    Vector3 screenCenter;
    [SerializeField] LayerMask layerMask;
    Ray ray;
    Renderer obj;
    private void Awake()
    {
        screenCenter = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        FollowPlayer();
        
        
    }
    private void FixedUpdate()
    {
        SeeThroughBuilding();
    }
    void FollowPlayer()
    {
        transform.position = target.transform.position + offset;
    }

    // 건물투시
    void SeeThroughBuilding()
    {
        ray = Camera.main.ScreenPointToRay(screenCenter);

        if(Physics.Raycast(ray, out RaycastHit hit, 100, layerMask))
        {
            obj = hit.collider.gameObject.GetComponent<MeshRenderer>();
            obj.enabled = false;
        }
        if(Physics.Raycast(ray, 100, layerMask) == false)
        {
            obj.enabled = true;
        }
    }
}
