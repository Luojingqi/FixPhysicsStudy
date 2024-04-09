using Fix64Physics.Collision;
using Fix64Physics.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Fix64Physics.Mono
{
    public class FixPhysicsSystemMono : MonoBehaviour
    {
        private FixPhysicsSystem system;

        private void Awake()
        {
            system = new FixPhysicsSystem();
            system.IsRun = true;
        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.V))
                system.Update();
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector3 m = Input.mousePosition;
                m.z = 10;
                //   Ray ray = Camera.main.ScreenPointToRay(m);
                FixVector3 s = (FixVector3)Camera.main.ScreenToWorldPoint(m);
                FixVector3 c = (FixVector3)Camera.main.transform.position;
                FixRay ray = new FixRay(c, (s - c).Normalize());
                if (FixPhysicsSystem.Inst.Raycast(ray, Fix64.max, out FixRaycastHit hit))
                {
                    Debug.Log(hit.point);
                }
            }
        }

    }
}
