using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Vector3 offset;
    [SerializeField] LayerMask layerMask;
    Vector3 screenCenter;
    [SerializeField] MeshRenderer[] meshs;
    [SerializeField] Material[] materials;
    int size;

    private void Awake()
    {
        screenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2 - 1);
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
        
        if (Physics.Raycast(ray, out RaycastHit hit, 50, layerMask))
        {
            // raycast�� ���� ������Ʈ�� ��� �޽����� ������Ʈ�� �迭�� ���(�ڽĿ�����Ʈ ����)
            meshs = hit.collider.gameObject.GetComponentsInChildren<MeshRenderer>();
            // �޽����� �迭�� ������ üũ
            size = meshs.Length;
            // ���͸��� �迭�� ����� �����ϰ� �ʱ�ȭ
            materials = new Material[size];
            // ���͸��� �迭�� �� ������Ʈ�� �޽������� ���͸��� �ֱ�
            for (int i = 0; i < size; i++)
            {
                materials[i] = meshs[i].material;
            }
            // �� ������Ʈ ���͸����� ���� ����
            for (int j = 0; j < size; j++)
            {
                materials[j].color = new Color(materials[j].color.r, materials[j].color.g, materials[j].color.b, 0.1f);
            }
        }
        // �÷��̾ raycast�� �� ������Ʈ�� �������� ���͸��� �迭�� ���� ���� ��쿡�� ����
        else if (materials != null)
        {
            // �� ������Ʈ ���͸����� ���� ����
            for (int x = 0; x < size; x++)
            {
                materials[x].color = new Color(materials[x].color.r, materials[x].color.g, materials[x].color.b, 1f);
            }
        }
    }
}
