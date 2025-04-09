using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRope : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public Transform firePoint;
    //public GameObject jointPoint;

    public GameObject bulletPrefab;

    private GameObject Hammer;
    private Rigidbody rb;

    public float bulletSpeed;
    public float recallSpeed;

    public bool isFire;
    public bool isRecall;

    //public FixedJoint joint;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isFire) // 좌클릭 시
        {
            FireBullet();
        }

        if(Input.GetMouseButtonDown(1) && isFire)
        {
            RecallHammer();
        }

        if(isFire)
        {
            UpdateRope();
        }
    }

    void FireBullet()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 direction;

        if (Physics.Raycast(ray, out hit))
        {
            direction = (hit.point - firePoint.position).normalized;
        }
        else
        {
            direction = ray.direction; // 아무것도 안 맞았으면 그냥 쏘는 방향
        }

        Hammer = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        rb = Hammer.GetComponent<Rigidbody>();

        rb.AddForce(direction * bulletSpeed, ForceMode.Impulse);

        SetRope(Hammer);

        isFire = true;
    }

    void RecallHammer()
    {
        isRecall = true;

        rb.useGravity = false;

        Hammer.GetComponent<BoxCollider>().enabled = false;

        Vector3 direction = (firePoint.position - Hammer.transform.position).normalized;

        rb.AddForce(direction * recallSpeed, ForceMode.Impulse);
    }

    void SetRope(GameObject target)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.transform.position);
    }
    void UpdateRope()
    {
        lineRenderer.SetPosition(1, Hammer.transform.position);
    }

    public void SuccessReCall()
    {
        lineRenderer.positionCount = 0;
    }
}
