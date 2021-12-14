using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundParallax : MonoBehaviour
{
    [SerializeField] Ground _groundLeft;
    [SerializeField] Ground _groundRight;
    [SerializeField] PlayerController _player;
    [SerializeField] float _tolerance;

    private Vector3 delta;

    private void Start() 
    {
        delta = _groundRight.transform.position - _groundLeft.transform.position;
    }

    private void LateUpdate() 
    {
        if(_groundLeft.transform.position.x + _tolerance < _player.transform.position.x)
            _groundLeft.transform.position = _groundRight.transform.position + delta;

        if(_groundRight.transform.position.x + _tolerance < _player.transform.position.x)
            _groundRight.transform.position = _groundLeft.transform.position + delta;

    }
}