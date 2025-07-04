using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform _fovLightTransform;  
    [SerializeField] private SpriteRenderer _playerRenderer;

    [SerializeField] private float _lightOffsetDistance = 0.3f; 

    private void Update()
    {
        RotateToMouse();
        FlipToMouse();
    }

    private void RotateToMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 direction = (mouseWorldPos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _fovLightTransform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); 

        _fovLightTransform.position = transform.position + direction * _lightOffsetDistance;
    }

    private void FlipToMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; 

        Vector3 direction = mouseWorldPos - transform.position;

        _playerRenderer.flipX = direction.x < 0f;
    }
}
