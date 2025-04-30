using UnityEngine;

public class RopeVisualController : MonoBehaviour
{
    public Transform hand; // 플레이어 손 위치
    public RopeTubeMeshGenerator ropeGen; // 로프 메쉬 생성기
    private RopeTubeMeshGenerator rtm;

    [Header("Dynamic Hammer")]
    public HammerThrowController hammerThrow; // 해머 인스턴스를 추적할 컨트롤러

    public float sag = 0.2f; // 중간 쳐짐 정도

    private void Start()
    {
        rtm = GetComponent<RopeTubeMeshGenerator>();
    }

    void LateUpdate()
    {
        // 현재 던져진 해머가 있는지 확인
        if (hammerThrow == null || hammerThrow.ActiveHammer == null)
        {
            ropeGen.SetPoints(new Vector3[0]); // 해머 없으면 로프 비우기
            rtm.ropeRenderer.enabled = false;
            return;
        }

        Transform hammer = hammerThrow.ActiveHammer.transform;

        Vector3[] points = new Vector3[]
        {
            hand.position,
            Vector3.Lerp(hand.position, hammer.position, 0.5f) + Vector3.down * sag,
            hammer.position
        };
        rtm.ropeRenderer.enabled = true;
        ropeGen.SetPoints(points);
    }
}
