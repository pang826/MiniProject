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

    // 건물투시
    void SeeThroughBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 50, layerMask))
        {
            // raycast에 닿은 오브젝트의 모든 메쉬렌더 컴포넌트를 배열에 담기(자식오브젝트 포함)
            meshs = hit.collider.gameObject.GetComponentsInChildren<MeshRenderer>();
            // 메쉬렌더 배열의 사이즈 체크
            size = meshs.Length;
            // 매터리얼 배열의 사이즈도 동일하게 초기화
            materials = new Material[size];
            // 메터리얼 배열에 각 오브젝트의 메쉬렌더의 메터리얼 넣기
            for (int i = 0; i < size; i++)
            {
                materials[i] = meshs[i].material;
            }
            // 각 오브젝트 메터리얼의 투명도 조절
            for (int j = 0; j < size; j++)
            {
                materials[j].color = new Color(materials[j].color.r, materials[j].color.g, materials[j].color.b, 0.1f);
            }
        }
        // 플레이어가 raycast를 쏜 오브젝트와 떨어지고 메터리얼 배열에 값이 있을 경우에만 진행
        else if (materials != null)
        {
            // 각 오브젝트 메터리얼의 투명도 복구
            for (int x = 0; x < size; x++)
            {
                materials[x].color = new Color(materials[x].color.r, materials[x].color.g, materials[x].color.b, 1f);
            }
        }
    }
}
