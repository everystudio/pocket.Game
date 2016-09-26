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

	private void ChangeScale( float _fScale)
	{
		if(_fScale < 0.0f )
		{
			_fScale = 1.0f;
		}
		float fScale = 1.0f / _fScale;
		//fScale = 10.0f;
		myTransform.localScale = new Vector3(fScale, fScale, fScale);
	}

	// Use this for initialization
	void Awake () {
		GameMain.Instance.ChangeParkScale.AddListener(ChangeScale);	
	}
	
}
