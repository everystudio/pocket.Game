using UnityEngine;
//using UnityEngine.Events;
using System.Collections;


public class CtrlTouchCollectTamashii : MonoBehaviourEx {

	private int m_iTamashii;

	public UnityEventInt CollectTamashii = new UnityEventInt();

	void OnTriggerEnter(Collider other)
	{
		OnPushed();
	}

	private void OnPushed()
	{
		//Debug.LogError("pushed tamashii");
		CollectTamashii.Invoke(m_iTamashii);
		gameObject.SetActive(false);
	}

	public void Initialize( int _iTamashii)
	{
		m_iTamashii = _iTamashii;
		gameObject.SetActive(true);
	}

	public void SetSclaleFromFieldScale(float _fFieldScale)
	{
		if (_fFieldScale <= 0.0f)
		{
			_fFieldScale = 1.0f;
		}
		float fScale = 1.0f / _fFieldScale;
		//fScale = 10.0f;
		myTransform.localScale = new Vector3(fScale, fScale, fScale);

	}

	private void ChangeScale( float _fScale)
	{
		SetSclaleFromFieldScale(_fScale);
	}

	// Use this for initialization
	void Awake () {
		GameMain.Instance.ChangeParkScale.AddListener(ChangeScale);	
	}
	
}
