using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateForwardTest : MonoBehaviour
{
    [SerializeField] private Camera cam;
    // Start is called before the first frame update
    void Start() => cam = GameObject.Find("PlayerCamera").GetComponent<Camera>();

    private void Update()
    {
        transform.rotation = cam.transform.rotation;
    }
}
