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

    // �ǹ�����
    void SeeThroughBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        // layerMask�� ������ layer�� raycast�� ���� ��� �ش� ������Ʈ�� ���͸��� ������ 0.1f�� ����
        if(Physics.Raycast(ray, out RaycastHit hit, 100, layerMask))
        {
            buildingMaterial = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
            buildingMaterial.color = new Color(buildingMaterial.color.r, buildingMaterial.color.g, buildingMaterial.color.b, 0.1f);
        }
        // �ش� ������Ʈ�� �������ְ� �� ������Ʈ�� 0.1f�� ������ ������ ��� raycast�� ���� ������ ���� 1�� ����
        // ���� �޺κ��� ������� ������ ������ �����ϴ� ������ ����
        else if(buildingMaterial != null && buildingMaterial.color.a == 0.1f)
        {
            buildingMaterial.color = new Color(buildingMaterial.color.r, buildingMaterial.color.g, buildingMaterial.color.b, 1f);
        }
    }
}
