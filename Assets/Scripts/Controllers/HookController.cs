using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : BaseController
{
    public event Action<GameObject> CatchedSmthEvent;
    public event Action PulledOutEvent;

    private GameObject _hook;
    private GameObject _fishRod;
    private Transform _rodEnd;
    private bool _isFishing;
    private Vector3 _direction = Vector3.zero;
    private LineRenderer _lineRenderer;
    private bool _isCatchedSmth;
    private GameObject _catchedItem;
    private InteractableBehaviour _interactableBehaviour;
    public override void Initialise()
    {
        _hook = GameObject.Find("hook");
        _fishRod = GameObject.Find("fish-rod");
        _rodEnd = _fishRod.transform.GetChild(0);
        _lineRenderer = _hook.GetComponent<LineRenderer>();
        _interactableBehaviour = _hook.GetComponent<InteractableBehaviour>();
        _interactableBehaviour.CatchedEvent += OnCatched;
    }

    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_isFishing)
                _isFishing = true;
            else
                _isFishing = false;
        }
        HookMove();
        SetRodLine();
    }
    public override void Dispose()
    {
        _interactableBehaviour.CatchedEvent -= OnCatched;
    }

    private void HookMove()
    {
        if (_isFishing && !_isCatchedSmth && _hook.transform.position.y > -4f)
        {
            _direction = Vector3.down;
        }
        else if (!_isFishing && _hook.transform.position.y < 0)
        {
            _direction = Vector3.up;
        }
        else
        {
            if (!_isFishing && _isCatchedSmth)
            {
                _catchedItem.SetActive(false);
                _catchedItem.transform.SetParent(null);
                _isCatchedSmth = false;
                _catchedItem = null;
                _interactableBehaviour.CatchedEvent += OnCatched;
                PulledOutEvent?.Invoke();
            }
            _direction = Vector3.zero;
        }
        _hook.transform.Translate(_direction * 1.8f * Time.deltaTime);
    }

    private void SetRodLine()
    {
        _lineRenderer.SetPosition(0, _hook.transform.position);
        _lineRenderer.SetPosition(1, _rodEnd.position);
    }

    private void OnCatched(GameObject catchedItem, GameObject hook)
    {
        if (_isCatchedSmth)
            return;
        CatchedSmthEvent?.Invoke(catchedItem);

        _catchedItem = catchedItem;
        _catchedItem.transform.SetParent(hook.transform);
        _isCatchedSmth = true;
        _interactableBehaviour.CatchedEvent -= OnCatched;
    }


}
