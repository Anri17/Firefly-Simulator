using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Fly : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 4.0f;

    private Camera _camera;
    private SpriteRenderer _spriteRenderer;
    
    // play field limits
    private float _topLimit;
    private float _bottomLimit;
    private float _leftLimit;
    private float _rightLimit;

    private void Awake()
    {
        _camera = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Vector2 spriteSize = _spriteRenderer.sprite.bounds.size;
        Vector2 spriteHalfSize = spriteSize / 2;

        // get limits
        _topLimit = _camera.orthographicSize - spriteHalfSize.y;
        _bottomLimit = -_topLimit;
        _rightLimit = (_camera.orthographicSize * _camera.aspect) - spriteHalfSize.x;
        _leftLimit = -_rightLimit;
        
        // set random angle
        float angle = UnityEngine.Random.Range(0, 360);
        
        transform.Rotate(0.0f, 0.0f, angle);
        
        // set random position
        float xPos = UnityEngine.Random.Range(_leftLimit+0.1f, _rightLimit-0.1f);
        float yPos = UnityEngine.Random.Range(_bottomLimit+0.1f, _topLimit-0.1f);

        transform.position = new Vector3(xPos, yPos, 0.0f);
    }

    private void Update()
    {
        // move forward
        transform.Translate(Vector3.up * (Time.deltaTime * movementSpeed));

        transform.Rotate(0.0f, 0.0f, 0.05f);
        
        // bounce firefly from limit
        if (transform.position.x > _rightLimit || transform.position.y > _topLimit ||
            transform.position.x < _leftLimit || transform.position.y < _bottomLimit)
        {
            // rotate
            transform.Rotate(0.0f, 0.0f, 180f);
            
            // move away from limit
            Vector2 newPos = transform.position;
            if (transform.position.x > _rightLimit)
            {
                newPos.x = _rightLimit;
            }
            else if (transform.position.x < _leftLimit)
            {
                newPos.x = _leftLimit;
            }

            if (transform.position.y > _topLimit)
            {
                newPos.y = _topLimit;
            }
            else if (transform.position.y < _bottomLimit)
            {
                newPos.y = _bottomLimit;
            }
            // no need to add a value to the limits because we are not evaluating if the positions are equals, just greater or smaller, so no buggy movement
            
            transform.position = newPos;
        }
    }
}
