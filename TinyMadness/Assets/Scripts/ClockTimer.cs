using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClockTimer : MonoBehaviour
{
	private float _timeMax;
	private float _timeLeft;
	[SerializeField]
	private Image _fillImage;
	
	void Start()
	{
		_fillImage.type = Image.Type.Filled;
		_fillImage.fillOrigin = (int) Image.Origin360.Top;
		_fillImage.fillMethod = Image.FillMethod.Radial360;
		_fillImage.fillClockwise = true;
	}

	void Update()
	{
		_timeLeft -= Time.deltaTime;
		
		_fillImage.fillAmount = _timeLeft / _timeMax;
	}

	public void Stop()
	{
		_timeLeft = 0;
	}

	public void Launch(float maxTime) 
	{
		_timeMax = maxTime;
		_timeLeft = maxTime;
	}
}
