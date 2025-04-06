using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FIMSpace.RagdollAnimatorDemo
{
    public class Demo_Ragd_Shot : MonoBehaviour
    {
        public GameObject Bullet;
        public float Power = 10f;

        void Update()
        {
#if UNITY_2019_4_OR_NEWER
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray.origin, ray.direction, out hit))
                {
                    Vector3 targetPos = hit.point;

                    GameObject b = Instantiate(Bullet);
                    Rigidbody r = b.GetComponent<Rigidbody>();

                    Vector3 dir = targetPos - transform.position; dir.Normalize();
                    r.position = transform.position + dir;

                    r.AddForce(dir * Power, ForceMode.VelocityChange);
                }

            }
#endif
#else
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray.origin, ray.direction, out hit))
                {
                    Vector3 targetPos = hit.point;

                    GameObject b = Instantiate(Bullet);
                    Rigidbody r = b.GetComponent<Rigidbody>();

                    Vector3 dir = targetPos - transform.position; dir.Normalize();
                    r.position = transform.position + dir;

                    r.AddForce(dir * Power, ForceMode.VelocityChange);
                }

            }
#endif

        }
    }
}