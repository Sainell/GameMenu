using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HookController : BaseController
{
    public event Action<GameObject> CatchedSmthEvent;
    public event Action PulledOutEvent;


    private const float HOOK_STOP_OFFSET = 0.5f;
    private const float HOOK_SPEED = 3.8f;

    private GameObject _hook;
    private GameObject _fishRod;
    private Transform _rodEnd;
    private Transform _boat;
    private Vector3 _rodEndPositionWithOffset;
    private bool _isThrowingHook;
    private bool _canThrowHook;
    private LineRenderer _lineRenderer;
    private bool _isCatchedSmth;
    private GameObject _catchedItem;
    private InteractableBehaviour _interactableBehaviour;
    private Vector3 _hookTargetPosition;
    public override void Initialise(LevelData levelData)
    {
        var playerController = GameController.Instance.PlayerController;
        _hook = playerController.Hook;
        _fishRod = playerController.Rod;
        _rodEnd = playerController.RodEnd;
        _boat = playerController.Boat.transform;
        _rodEndPositionWithOffset = new Vector3(_rodEnd.position.x, _rodEnd.position.y - HOOK_STOP_OFFSET);
        _lineRenderer = _hook.GetComponent<LineRenderer>();
        _interactableBehaviour = _hook.GetComponent<InteractableBehaviour>();
        _interactableBehaviour.CatchedEvent += OnCatched;

        ResetHookPositionToDefault();
        base.Initialise(levelData);
    }

    public override void Execute()
    {
        if (!IsInitialised)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!_isThrowingHook && _canThrowHook)
            {
                SetHookTarget();
            }
        }
        //for TEST
        if (Input.GetMouseButtonDown(1))
        {
            _hookTargetPosition = _rodEndPositionWithOffset;
        }

        if (_hook != null)
        {
            var target = _isThrowingHook ? _hookTargetPosition : _rodEndPositionWithOffset;
            HookMoveToPosition(target);
            CheckHookTargetPossition();
            SetRodLine();
        }
    }
    public override void Dispose()
    {
        Clear();
    }

    public override void Clear()
    {
        if (_interactableBehaviour != null)
        {
            _interactableBehaviour.CatchedEvent -= OnCatched;
            _isThrowingHook = false;
            _hook = null;
            _fishRod = null;
            _rodEnd = null;
            _lineRenderer = null;
            _interactableBehaviour = null;
        }
    }

    private void ResetHookPositionToDefault()
    {
        _hook.transform.position = _rodEndPositionWithOffset;
    }
    private void CheckHookTargetPossition()
    {
        if (_hook.transform.position == _hookTargetPosition)
        {
            _isThrowingHook = false;
        }
        if(_hook.transform.position == _rodEndPositionWithOffset)
        {
            _canThrowHook = true;
            if (_isCatchedSmth)
            {
                _catchedItem.SetActive(false);
                _catchedItem.transform.SetParent(null);
                _isCatchedSmth = false;
                _catchedItem = null;
                _interactableBehaviour.CatchedEvent += OnCatched;
                PulledOutEvent?.Invoke();
            }
        }
    }

    private void SetHookTarget()
    {
        var targetVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (targetVector.y >= _boat.position.y)
            return;
        _hookTargetPosition = new Vector3(targetVector.x, targetVector.y);
        _isThrowingHook = true;
        _canThrowHook = false;
    }

    private void HookMoveToPosition(Vector3 position)
    {
        _hook.transform.position = Vector3.MoveTowards(_hook.transform.position, position, HOOK_SPEED * Time.deltaTime);
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
        _isThrowingHook = false;
        _catchedItem = catchedItem;
        _catchedItem.transform.SetParent(hook.transform);
        _isCatchedSmth = true;
        _interactableBehaviour.CatchedEvent -= OnCatched;
    }
}
