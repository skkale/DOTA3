using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firts_person_look : MonoBehaviour
{
    [Space]
   [SerializeField] public float mouseSensitivity = 1f;
   [SerializeField] public float smooth = 2f;
    [Space]
   [SerializeField] private float minClamp = -90f;
   [SerializeField] private float maxClamp = 90f;

    private Transform character;
    private Vector2 currentMouseLook;
    private Vector2 appliedMouseDelta;

    void Start() => character = GameObject.FindGameObjectWithTag("Player").transform;

    void Update()
    {
    var smoothMouseDelta = Vector2.Scale(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")), Vector2.one * mouseSensitivity * smooth);
    appliedMouseDelta = Vector2.Lerp(appliedMouseDelta, smoothMouseDelta, 1 / smooth);
    currentMouseLook += appliedMouseDelta;
    currentMouseLook.y = Mathf.Clamp(currentMouseLook.y, minClamp, maxClamp);

        character.localRotation = Quaternion.AngleAxis(currentMouseLook.x, Vector3.up);
        transform.localRotation = Quaternion.AngleAxis(-currentMouseLook.y, Vector3.right);


    }
}
