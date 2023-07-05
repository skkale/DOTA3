using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class first_person_movement : MonoBehaviour
{
    [SerializeField] public float mvSpeed = 5f;

    public float jopa = 0f;

    private Vector2 velocity;

    private void Update()
    {
        velocity.x = Input.GetAxis("Horizontal") * mvSpeed * Time.deltaTime;
        velocity.y = Input.GetAxis("Vertical") * mvSpeed * Time.deltaTime; 

        transform.Translate(velocity.x, 0f,velocity.y);
    }
}
