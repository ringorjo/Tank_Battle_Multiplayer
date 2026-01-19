using System;
using Unity.Netcode;
using UnityEngine;

public class PivotTankRotation : NetworkBehaviour
{
    [Header("References")]
    [SerializeField]
    private InputReader _inputActionAsset;

    private Vector3 _mousePos;


    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
            return;

        _inputActionAsset.OnLookEvent += OnRotatePivot;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
            return;
        _inputActionAsset.OnLookEvent -= OnRotatePivot;
    }

    private void LateUpdate()
    {
        if (!IsOwner)
            return;

        Vector3 aimWorldPos = Camera.main.ScreenToWorldPoint(_mousePos);
        Vector3 direction = aimWorldPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

    }

    private void OnRotatePivot(Vector2 vector)=> _mousePos = vector;
    


}
