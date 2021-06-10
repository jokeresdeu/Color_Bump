using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private float _speedModificator = 0.02f;
    private Vector2 _prevMousePosition;
    private float _delta;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            _prevMousePosition = Input.mousePosition;
            return;
        }

        if (Input.GetButton("Fire2"))
        {
            Vector2 currentMousePosition = Input.mousePosition;

            _delta = currentMousePosition.y - _prevMousePosition.y;
            _prevMousePosition = currentMousePosition;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            _delta = 0;
        }

        if(_delta!=0)
            transform.Translate(new Vector3(0, _delta, 0) * _speedModificator, Space.World);
    }
}
