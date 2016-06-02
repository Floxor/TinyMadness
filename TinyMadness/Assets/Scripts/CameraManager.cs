using UnityEngine;
using System.Collections;

public enum ShakeForce
{
	minimum = 1,
	weak = 7,
	medium = 15,
	strong = 25
}

public enum ShakeLength
{
	minimumTime = 1,
	shortTime = 100,
	mediumTime = 300,
	longTime = 500
}

public class CameraManager : MonoBehaviour
{
	

	private Vector3 _screenShakeOffset = Vector3.zero;

	private Vector3 _targetPosition;
	private Vector3 _UItargetPosition;
	private RectTransform _gameUI;

	private bool _isShaking = true;
	private float _shakeTimeLeft = 0;
	private float _activeShakeForce = 0;

	void Start()
	{
		_targetPosition = transform.position;
		_gameUI = GameObject.FindGameObjectWithTag("TransformUI").GetComponent<RectTransform>();
		_UItargetPosition = _gameUI.position;
	}



	void LateUpdate()
	{
		this.ProcessScreenShake();
		transform.position = _targetPosition + _screenShakeOffset;
		_gameUI.position = _UItargetPosition + _screenShakeOffset * 20;
		if (Input.GetKeyDown(KeyCode.F12))
			ScreenShake(ShakeForce.weak, ShakeLength.shortTime);
	}



	private void ProcessScreenShake()
	{
		if (this._shakeTimeLeft > 0)
		{
			this._shakeTimeLeft -= Time.deltaTime * 1000;
			this._screenShakeOffset.x = UnityEngine.Random.Range(-this._activeShakeForce, this._activeShakeForce) * 0.1f;
			this._screenShakeOffset.y = UnityEngine.Random.Range(-this._activeShakeForce, this._activeShakeForce) * 0.1f;
			this._screenShakeOffset = this.transform.rotation * this._screenShakeOffset;
		}
		else if (this._isShaking)
			this.StopShake();

	}


	public void ScreenShake(ShakeForce force = ShakeForce.minimum, ShakeLength shakeDuration = ShakeLength.minimumTime)
	{
		this._activeShakeForce = (float)force;
		this._shakeTimeLeft = (float)shakeDuration;
		this._isShaking = true;
	}

	public void StopShake()
	{
		this._activeShakeForce = 0;
		this._shakeTimeLeft = 0;
		this._screenShakeOffset = Vector3.zero;
		this._isShaking = false;
	}
}
