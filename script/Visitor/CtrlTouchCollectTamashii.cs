﻿using UnityEngine;
//using UnityEngine.Events;
using System.Collections;


public class CtrlTouchCollectTamashii : MonoBehaviour {

	private int m_iTamashii;

	public UnityEventInt CollectTamashii = new UnityEventInt();

	public void OnPushed()
	{
		CollectTamashii.Invoke(m_iTamashii);
		gameObject.SetActive(false);
	}

	public void Initialize( int _iTamashii)
	{
		m_iTamashii = _iTamashii;
		gameObject.SetActive(true);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
