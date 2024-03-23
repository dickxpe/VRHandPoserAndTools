using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeController : MonoBehaviour
{

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.transform.Translate(Time.deltaTime * -1, 0, 0);
            transform.Translate(Time.deltaTime * -1, 0, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.transform.Translate(Time.deltaTime, 0, 0);
            transform.Translate(Time.deltaTime, 0, 0);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(0, Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(0, Time.deltaTime * -1, 0);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(0, Time.deltaTime, 0);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            rb.useGravity = !rb.useGravity;
            rb.isKinematic = !rb.isKinematic;
        }
    }
}
