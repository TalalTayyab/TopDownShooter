using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{

    private Transform _cameraTransform;
    private float _textureUnitSizeX;
    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = Camera.main.transform;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        var texture = sprite.texture;
        _textureUnitSizeX = texture.width / sprite.pixelsPerUnit;


    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Vector3 deltaMovement = _cameraTransform.position - _lastCameraPosition;
        //transform.position += new Vector3(deltaMovement.x * _parallaxEffectMultipier.x, deltaMovement.y * _parallaxEffectMultipier.y);
        //_lastCameraPosition = _cameraTransform.position;
        if (_cameraTransform.position.x - transform.position.x >= _textureUnitSizeX)
        {
            var offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureUnitSizeX;
            transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y);
        }


    }
}
