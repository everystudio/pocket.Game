using UnityEngine;
using System.Collections;

public class CtrlScareEffect : MonoBehaviourEx {

	[SerializeField]
	private UI2DSprite [] m_sprEffect;

	void Start()
	{
		Initialize();
	}
	public void Initialize()
	{
		// とりあえず
		myTransform.localScale = Vector3.zero;
		foreach ( UI2DSprite sprite in m_sprEffect)
		{
			sprite.enabled = false;
		}
	}

	public void Scare( int _iDepth , int _iRange , int _iRate )
	{
		float fTime = 1.5f;
		float fRange = 1.5f * ((float)_iRange / 100.0f);
		iTween.ScaleTo(gameObject, iTween.Hash(
			"x", fRange,
			"y", fRange,
			"time", fTime,
			"oncomplete", "OnCompleteHandler"));
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", 1.0f,
			"to", 0.0f,
			"time", fTime * 0.95f,
			"delay", 0.5f,
			"onupdate", "OnUpdateFadeout"
			));

		int iFlashCount = 15;
		iTween.ValueTo(gameObject, iTween.Hash(
			"from", 0,
			"to", iFlashCount,
			"time", fTime*0.95f,
			"onupdate", "OnUpdateFlash"
			));

		Color color = Color.white;
		if(_iRate <= 100)
		{
			color = Color.white;
		}
		else if( _iRate <= 120)
		{
			color = Color.blue;
		}
		else if( _iRate <= 140)
		{
			color = Color.yellow;
		}
		else
		{
			color = Color.red;
		}

		foreach (UI2DSprite sprite in m_sprEffect)
		{
			sprite.enabled = false;
			sprite.color = color;
			sprite.depth = _iDepth + 10;
		}
	}

	private void OnCompleteHandler()
	{
		foreach (UI2DSprite sprite in m_sprEffect)
		{
			sprite.enabled = false;
		}
		myTransform.localScale = Vector3.zero;

	}

	private void OnUpdateFlash( int _iValue )
	{
		for( int i = 0; i < m_sprEffect.Length; i++) {
			bool bFlag = _iValue % m_sprEffect.Length == i;
			m_sprEffect[i].enabled = bFlag;
		}
	}
	private void OnUpdateFadeout( float _fValue)
	{

		for (int i = 0; i < m_sprEffect.Length; i++)
		{
			Color color = m_sprEffect[i].color;
			color.a = _fValue;
			m_sprEffect[i].color = color;
		}

	}


	// Update is called once per frame
	void Update () {
	
	}
}
