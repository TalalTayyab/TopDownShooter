using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;

public class DamagePopUpScript : MonoBehaviour
{
    [SerializeField] private Vector3 _moveVector;
    [SerializeField] private float _decreaseSpeed;
    [SerializeField] private float _dissapearTimerMax;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _criticalHitColor;
    [SerializeField] private int _normalFontSize;
    [SerializeField] private int _criticalHitFontSize;
    [SerializeField] private float _scaleAmount;

    private TextMeshPro _textMesh;
    private Color _textColor;
    private bool _isCriticalHit;
    private float _dissapearTimer;
    private static int _sortingOrder = 0;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isCriticalHit)
    {
        _textMesh.sortingOrder = ++_sortingOrder;

        _isCriticalHit = isCriticalHit;
        _textMesh.SetText(damageAmount.ToString());

        if (_isCriticalHit)
        {
            _textMesh.fontSize = _criticalHitFontSize;
            _textColor = _criticalHitColor;
            _textMesh.fontStyle = FontStyles.Bold;
        }
        else
        {
            _textMesh.fontSize = _normalFontSize;
            _textColor = _normalColor;
        }

        _textMesh.color = _textColor;
        _dissapearTimer = _dissapearTimerMax;

        _moveVector = new Vector3(Random.Range(1, _moveVector.x), Random.Range(1, _moveVector.y));

    }

    void Update()
    {
        transform.position += _moveVector * Time.deltaTime;
        _moveVector -= _moveVector * _decreaseSpeed * Time.deltaTime;

        _dissapearTimer -= Time.deltaTime;

        if (_dissapearTimer > _dissapearTimerMax * 0.5f)
        {
            transform.localScale += Vector3.one * _scaleAmount * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * _scaleAmount * Time.deltaTime;
        }

        if (_dissapearTimer < 0)
        {
            _textColor.a -= 3f * Time.deltaTime;
            _textMesh.color = _textColor;

            if (_textMesh.color.a <0)
            {
                Destroy(gameObject);
            }
        }
    }
}
