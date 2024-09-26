using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Vector3 offset;
    [SerializeField] LayerMask layerMask;
    Vector3 screenCenter;
    //Renderer obj;

    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material buildingMaterial;

    private void Awake()
    {
        screenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
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
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        // layerMask로 설정한 layer에 raycast가 닿을 경우 해당 오브젝트의 매터리얼 투명도를 0.1f로 설정
        if(Physics.Raycast(ray, out RaycastHit hit, 100, layerMask))
        {
            buildingMaterial = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
            buildingMaterial.color = new Color(buildingMaterial.color.r, buildingMaterial.color.g, buildingMaterial.color.b, 0.1f);
        }
        // 해당 오브젝트가 정해져있고 그 오브젝트가 0.1f의 투명도를 가졌을 경우 raycast에 닿지 않으면 투명도 1로 복구
        // 조건 뒷부분을 사용하지 않으면 무한히 실행하는 문제가 있음
        else if(buildingMaterial != null && buildingMaterial.color.a == 0.1f)
        {
            buildingMaterial.color = new Color(buildingMaterial.color.r, buildingMaterial.color.g, buildingMaterial.color.b, 1f);
        }
    }
}
