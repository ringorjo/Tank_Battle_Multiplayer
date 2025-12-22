using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Indicator2D : MonoBehaviour, IPlayerInfoReciever
{
    [SerializeField]
    private GameObject _indicator;
    [SerializeField]
    private float _offsetAmount = 15f;
    [SerializeField,ReadOnly]
    private Vector2 _ofsset;
    private Player _player;
    private Camera _mainCamera;
    private Vector2 _center;
    private Vector2 _worldToScreenPos;
    private RectTransform _rectTransform;

    public Player Player => _player;

    private void Start()
    {
        _ofsset = new Vector2();
    }
    public void ReceivePlayerInfo(Player player)
    {
        _player = player;
        _mainCamera = Camera.main;
        _center = new Vector2(Screen.width, Screen.height) / 2;
        _rectTransform = GetComponent<RectTransform>();
    }

    public void UpdateIndicator()
    {
        if (_player == null)
        {
            Debug.LogError("Player Reference is Null");
            return;
        }
        _worldToScreenPos = _mainCamera.WorldToScreenPoint(_player.transform.position);
        Vector2 _direction = _worldToScreenPos - _center;
        _direction.x = Mathf.Clamp(_direction.x, -_center.x, _center.x);
        _direction.y = Mathf.Clamp(_direction.y, -_center.y, _center.y);
        PerformOffset();
        _rectTransform.anchoredPosition = _direction + _ofsset;
        _indicator.SetActive(!IsOnScreen());
        if (!IsOnScreen())
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            _rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void PerformOffset()
    {
        _ofsset.x = GetScreenPos().x > 1 ? -_offsetAmount : _offsetAmount;
        _ofsset.y = GetScreenPos().y > 1 ? -_offsetAmount : _offsetAmount;
    }


    private Vector2 GetScreenPos()
    {
        return new Vector2(_worldToScreenPos.x / Screen.width, _worldToScreenPos.y / Screen.height);
    }

    private bool IsOnScreen()
    {
        if (GetScreenPos().x > 1 || GetScreenPos().x < 0)
            return false;
        if (GetScreenPos().y > 1 || GetScreenPos().y < 0)
            return false;
        return true;
    }

}
