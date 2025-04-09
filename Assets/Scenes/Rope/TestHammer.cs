using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHammer : MonoBehaviour
{
    private TestRope ropeScript;

    private Rigidbody rb;
    public Transform firePosition;

    public float maxRopeDistance;

    private void OnEnable()
    {
        ropeScript = Camera.main.gameObject.GetComponent<TestRope>();

        rb = GetComponent<Rigidbody>();
        firePosition = Camera.main.gameObject.GetComponentInChildren<Transform>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.useGravity = true;

        
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, firePosition.position);
        if(Mathf.Abs(distance) >= maxRopeDistance && !ropeScript.isRecall)
        {
            rb.useGravity = true;
        }

        if (Mathf.Abs(distance) < 5f && ropeScript.isRecall)
        {
            Destroy(gameObject, 1f);

            ropeScript.isFire = false;
            ropeScript.isRecall = false;
            ropeScript.SuccessReCall();
        }

        if(distance < 5f)
        {
            Debug.Log(distance);
        }
    }
}
