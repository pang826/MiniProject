using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Vector3 offset;
    Vector3 screenCenter;
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

        Physics.Raycast(ray, out RaycastHit hit, 100);

        if (hit.collider != target.GetComponent<Collider>())
        {
            obj = hit.collider.gameObject.GetComponent<MeshRenderer>();
            obj.enabled = false;

            //hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
            
            Debug.Log(hit.collider.gameObject.name);
        }
        if(hit.collider == target.GetComponent<Collider>())
        {
            obj.enabled = true;
            //hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        
            //meshRenderer.enabled = true;
        
    }
}
