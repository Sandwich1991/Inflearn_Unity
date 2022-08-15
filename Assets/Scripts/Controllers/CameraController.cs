using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterView;
    
    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f);
    
    [SerializeField]
    GameObject _player = null;

    public void SetPlayer(GameObject player) { _player = player;}
    void Start()
    {
        
    }

    
    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuarterView)
        {
            if (_player.IsValid() == false)
            {
                return;
            }   
            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Block")))
            {
                Vector3 charPos = new Vector3(_player.transform.position.x, _player.transform.position.y + 1,
                    _player.transform.position.z);
                
                float dist = (hit.point - _player.transform.position).magnitude * 0.3f;
                transform.position = charPos + _delta.normalized * dist;
            }
            else
            {
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);
            }
        }
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
