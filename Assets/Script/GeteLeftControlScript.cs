using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeteLeftControlScript : MonoBehaviour
{
    [SerializeField] private GameObject _border1;
    [SerializeField] private GameObject _border2;
    [SerializeField] private float GateOpenHeight;
    [SerializeField] private float TimeUntilGateOpens;
    [SerializeField] private float TimeUntilGateCloses;
    [SerializeField] private bool Horizontal;

    private float _currentTime;
    private Vector3 _targetGoalTopPos;
    private Vector3 _targetGoalBottomPos;
    private enum GateState { GateClosed = 0, GateOpening = 1, GateOpened = 2, GateClosing = 3 }
    private GateState _gateState;
    private Vector3 _gateVelocityTop = Vector3.zero;
    private Vector3 _gateVelocityBottom = Vector3.zero;

    private bool HaveGatesOpenedOrClosed()
    {
        var top = _targetGoalTopPos - _border1.transform.position;
        var bottom = _targetGoalBottomPos - _border2.transform.position;
        return top.magnitude <= 0.001f && bottom.magnitude <= 0.001f;
    }

    // Start is called before the first frame update
    void Start()
    {
        _gateState = GateState.GateClosed;
        _currentTime = TimeUntilGateOpens;
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime -= Time.deltaTime;

        GateControl();
    }

    void GateControl()
    {
        Debug.Log(_gateState.ToString());
        switch (_gateState)
        {
            case GateState.GateClosed:
                GateClosed();
                break;

            case GateState.GateOpening:
                GateOpening();
                break;

            case GateState.GateOpened:
                GateOpened();
                break;

            case GateState.GateClosing:
                GateClosing();
                break;
        }
    }

    void GateClosed()
    {
        if (_currentTime <= 0)
        {
            if (Horizontal)
            {
                _targetGoalTopPos = new Vector3(_border1.transform.position.x, _border1.transform.position.y + GateOpenHeight, 1);
                _targetGoalBottomPos = new Vector3(_border2.transform.position.x, _border2.transform.position.y - GateOpenHeight, 1);
            }
            else
            {
                _targetGoalTopPos = new Vector3(_border1.transform.position.x + GateOpenHeight, _border1.transform.position.y , 1);
                _targetGoalBottomPos = new Vector3(_border2.transform.position.x - GateOpenHeight, _border2.transform.position.y , 1);
            }
            _gateVelocityTop = Vector3.zero;
            _gateVelocityBottom = Vector3.zero;

            _gateState = GateState.GateOpening;
        }
        
    }

    void GateOpening()
    {
        if (!HaveGatesOpenedOrClosed())
        {
            _border1.transform.position = Vector3.SmoothDamp(_border1.transform.position, _targetGoalTopPos, ref _gateVelocityTop, 0.6f);
            _border2.transform.position = Vector3.SmoothDamp(_border2.transform.position, _targetGoalBottomPos, ref _gateVelocityBottom, 0.6f);
        }
        else
        {
            _gateState = GateState.GateOpened;
            _currentTime = TimeUntilGateCloses;
        }
    }


    void GateOpened()
    {
        if (_currentTime <= 0)
        {
            if (Horizontal)
            {
                _targetGoalTopPos = new Vector3(_border1.transform.position.x, _border1.transform.position.y - GateOpenHeight, 1);
                _targetGoalBottomPos = new Vector3(_border2.transform.position.x, _border2.transform.position.y + GateOpenHeight, 1);
            }
            else
            {
                _targetGoalTopPos = new Vector3(_border1.transform.position.x - GateOpenHeight, _border1.transform.position.y, 1);
                _targetGoalBottomPos = new Vector3(_border2.transform.position.x + GateOpenHeight, _border2.transform.position.y, 1);
            }

            _gateVelocityTop = Vector3.zero;
            _gateVelocityBottom = Vector3.zero;
            _gateState = GateState.GateClosing;
        }
    }

    void GateClosing()
    {
        if (!HaveGatesOpenedOrClosed())
        {
            _border1.transform.position = Vector3.SmoothDamp(_border1.transform.position, _targetGoalTopPos, ref _gateVelocityTop, 0.6f);
            _border2.transform.position = Vector3.SmoothDamp(_border2.transform.position, _targetGoalBottomPos, ref _gateVelocityBottom, 0.6f);
        }
        else
        {
            _gateState = GateState.GateClosed;
            _currentTime = TimeUntilGateOpens;
        }
    }
}
