using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketEnabler : MonoBehaviour
{
    UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor xRSocketInteractor;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        xRSocketInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        rb = GetComponent<Rigidbody>();
    }

    public void EnableSocket(SelectEnterEventArgs args)
    {
        if (args == null)
        {
            xRSocketInteractor.enabled = true;
        }
        else
        {
            SphereCollider[] sphereColliders = args.interactableObject.transform.gameObject.GetComponents<SphereCollider>();
            foreach (SphereCollider sphereCollider in sphereColliders)
            {
                sphereCollider.enabled = false;
            }
        }
    }

    public void ResetVelocity(SelectExitEventArgs args)
    {
        if (args == null)
        {
            Vector3 pos = transform.position;
            rb.linearVelocity = new Vector3(0f, 0f, 0f);
            rb.angularVelocity = new Vector3(0f, 0f, 0f);
            transform.rotation = Quaternion.identity;
            transform.position = pos;
        }
    }
}
