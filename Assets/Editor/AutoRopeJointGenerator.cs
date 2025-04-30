using UnityEngine;
using UnityEditor;

public class SoftRopeJointGenerator : MonoBehaviour
{
    [MenuItem("Tools/Rope/Add SoftJoints To Bone Chain")]
    private static void AddSoftJointsToBones()
    {
        if (Selection.activeTransform == null)
        {
            Debug.LogWarning("Bone 체인의 Root Transform을 선택하세요.");
            return;
        }

        Transform[] allBones = Selection.activeTransform.GetComponentsInChildren<Transform>();
        Rigidbody previousRb = null;

        foreach (var bone in allBones)
        {
            if (bone == Selection.activeTransform) continue; // 루트 제외

            // Rigidbody 추가
            Rigidbody rb = bone.GetComponent<Rigidbody>();
            if (rb == null) rb = bone.gameObject.AddComponent<Rigidbody>();

            rb.mass = 0.5f;
            rb.drag = 0.2f;
            rb.angularDrag = 0.2f;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // CapsuleCollider 추가
            if (bone.GetComponent<Collider>() == null)
            {
                CapsuleCollider capsule = bone.gameObject.AddComponent<CapsuleCollider>();
                capsule.direction = 2; // Z 방향
                capsule.radius = 0.05f;
                capsule.height = 0.2f;
            }

            // ConfigurableJoint 추가
            if (previousRb != null)
            {
                ConfigurableJoint joint = bone.GetComponent<ConfigurableJoint>();
                if (joint == null) joint = bone.gameObject.AddComponent<ConfigurableJoint>();

                joint.connectedBody = previousRb;

                joint.xMotion = ConfigurableJointMotion.Limited;
                joint.yMotion = ConfigurableJointMotion.Limited;
                joint.zMotion = ConfigurableJointMotion.Limited;
                joint.angularXMotion = ConfigurableJointMotion.Limited;
                joint.angularYMotion = ConfigurableJointMotion.Limited;
                joint.angularZMotion = ConfigurableJointMotion.Limited;

                SoftJointLimitSpring spring = new SoftJointLimitSpring
                {
                    spring = 100f,
                    damper = 150f
                };
                joint.linearLimitSpring = spring;
                joint.angularXLimitSpring = spring;
                joint.angularYZLimitSpring = spring;

                SoftJointLimit limit = new SoftJointLimit
                {
                    limit = 0.1f
                };
                joint.linearLimit = limit;

                joint.massScale = 1f;
                joint.connectedMassScale = 1f;
            }

            previousRb = rb;
        }

        Debug.Log("SoftJoint 기반 ConfigurableJoint 설치 완료!");
    }
}
