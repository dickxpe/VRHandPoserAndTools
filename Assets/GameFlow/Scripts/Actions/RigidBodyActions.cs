using UnityEngine;


public class RigidBodyActions : MonoBehaviour
{



    public void AddForce(GameObject go, Vector3 forceMin, Vector3 forceMax, ForceMode forceMode)
    {
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(Random.Range(forceMin.x, forceMax.x), Random.Range(forceMin.y, forceMax.y), Random.Range(forceMin.z, forceMax.z)), forceMode);
    }

    public void AddTorque(GameObject go, Vector3 forceMin, Vector3 forceMax, ForceMode forceMode)
    {
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.AddRelativeTorque(Random.Range(forceMin.x, forceMax.x), Random.Range(forceMin.y, forceMax.y), Random.Range(forceMin.z, forceMax.z), forceMode);
    }


}
