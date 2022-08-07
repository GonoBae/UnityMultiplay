using UnityEngine;
using Photon.Pun;

public class NeckControl : MonoBehaviour, IPunObservable
{
	[SerializeField] GameObject _camHolder;
	private PhotonView _pv;
	// 카메라 위아래로 움직일 때 고개 움직이기
	Vector3 _neckDir;
	Vector3 _neckOffset = new Vector3(0, -50, -70);
	Vector3 _neckRot;
	
	private float _aniTime;
	private float _verticalLookRotation;
	private float _verticalCamLookRotation;
	private float mx;
	private float my;

	private void Awake()
	{
		_pv = GetComponent<PhotonView>();
		_pv.Synchronization = ViewSynchronization.UnreliableOnChange;
	}
	[SerializeField] float _mouseSensitivity;
	[SerializeField] Vector3 _testRot;
	
	// Mouse 로 카메라 X 축 회전
	private void Update()
	{
		if(_pv.IsMine)
		{
			my = Input.GetAxisRaw("Mouse Y");
		
			if(my != 0)
			{		
				_verticalLookRotation += my * _mouseSensitivity;
				_verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -50, 50);
				_verticalCamLookRotation = Mathf.Clamp(_verticalLookRotation, -30, 30);
				_camHolder.transform.localEulerAngles = Vector3.left * _verticalCamLookRotation;
				_aniTime = 0;
			}
			else
			{
				_aniTime += Time.deltaTime;
				if(_aniTime > 3f)
				{
					_verticalLookRotation = 0;
					_verticalCamLookRotation = 0;
					_camHolder.transform.localEulerAngles = Vector3.zero;
					_aniTime = 0;
				}
			}
		}
	}
	
	private void LateUpdate()
	{
		if(_pv.IsMine)
		{
			_neckDir = _camHolder.transform.position + _camHolder.transform.forward * 50;
			transform.LookAt(-_neckDir);
			transform.localRotation = transform.localRotation * Quaternion.Euler(_neckOffset);
		
			_testRot = transform.localEulerAngles;
		}
		else
		{
			transform.localEulerAngles = _neckRot;
		}
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.IsWriting)
		{
			stream.SendNext(_testRot);
		}
		else
		{
			_neckRot = (Vector3)stream.ReceiveNext();
		}
	}
}
