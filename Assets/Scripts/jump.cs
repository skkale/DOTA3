using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class jump : MonoBehaviour
{

    [SerializeField] public float jumpStrength = 2f;
    [SerializeField] private float maxGroundDistance = 0.3f;

    private Rigidbody _rigidbody;
    private Transform _groundCheckObject;

    private bool isground;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundCheckObject = GameObject.FindGameObjectWithTag("GroundCheck").transform;
    }

    private void Update()
    {
        isground = Physics.Raycast(_groundCheckObject.transform.position, Vector3.down, maxGroundDistance);

        if(Input.GetButtonDown("Jump") && isground) 
            _rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
    }
}
