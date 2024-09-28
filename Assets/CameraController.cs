using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Vector3 offset;
    [SerializeField] LayerMask layerMask;
    Vector3 screenCenter;
    //[SerializeField] Material buildingMaterial;
    [SerializeField] MeshRenderer[] meshs;
    [SerializeField] Material[] materials;
    int size;

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

        //// layerMask�� ������ layer�� raycast�� ���� ��� �ش� ������Ʈ�� ���͸��� ������ 0.1f�� ����
        //if (Physics.Raycast(ray, out RaycastHit hit, 100, layerMask))
        //{
        //    buildingMaterial = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
        //    buildingMaterial.color = new Color(buildingMaterial.color.r, buildingMaterial.color.g, buildingMaterial.color.b, 0.1f);
        //}
        //// �ش� ������Ʈ�� �������ְ� �� ������Ʈ�� 0.1f�� ������ ������ ��� raycast�� ���� ������ ���� 1�� ����
        //// ���� �޺κ��� ������� ������ ������ �����ϴ� ������ ����
        //else if (buildingMaterial != null && buildingMaterial.color.a == 0.1f)
        //{
        //    buildingMaterial.color = new Color(buildingMaterial.color.r, buildingMaterial.color.g, buildingMaterial.color.b, 1f);
        //}
        if (Physics.Raycast(ray, out RaycastHit hit, 100, layerMask))
        {
            meshs = hit.collider.gameObject.GetComponentsInChildren<MeshRenderer>();
            size = meshs.Length;
            materials = new Material[size];

            for (int i = 0; i < size; i++)
            {
                materials[i] = meshs[i].material;
            }
            for (int j = 0; j < size; j++)
            {
                materials[j].color = new Color(materials[j].color.r, materials[j].color.g, materials[j].color.b, 0.1f);
            }

            //foreach (Material material in materials)
            //{
            //    material.color = new Color(material.color.r, material.color.g, material.color.b, 0.1f);
            //}
        }
        else if (materials != null)
        {
            // foreach (Material material in materials)
            // {
            //     material.color = new Color(material.color.r, material.color.g, material.color.b, 1f);
            // }
            
            for (int x = 0; x < size; x++)
            {
                materials[x].color = new Color(materials[x].color.r, materials[x].color.g, materials[x].color.b, 1f);
            }
        }
    }
}
