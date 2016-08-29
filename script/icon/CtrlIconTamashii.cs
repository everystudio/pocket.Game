using UnityEngine;
using System.Collections;

public class CtrlIconTamashii : MonoBehaviour {

	[SerializeField]
	private UI2DSprite m_sprIcon;

	public void SetDepth( int _iDepth)
	{
		// そのまま
		m_sprIcon.depth = _iDepth;
	}



}
