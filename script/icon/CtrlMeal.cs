using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UI2DSprite))]
public class CtrlMeal : MonoBehaviour {

	private UI2DSprite m_sprMeal;

	public void Initialize( string _strMeal,int _iDepth)
	{
		Debug.LogError(_strMeal);
		string[] strTypeArr = _strMeal.Split('|');
		int iNum = strTypeArr.Length;
		if(m_sprMeal == null)
		{
			m_sprMeal = gameObject.GetComponent<UI2DSprite>();
		}
		int iIndex = UtilRand.GetRand(iNum);
		if (iNum < iIndex)
		{
			iIndex = 0;
		}
		m_sprMeal.sprite2D = SpriteManager.Instance.Load(string.Format("food{0}.png", strTypeArr[iIndex]));

		m_sprMeal.depth = _iDepth;
		m_sprMeal.width = (int)m_sprMeal.sprite2D.rect.width;
		m_sprMeal.height = (int)m_sprMeal.sprite2D.rect.height;


	}


}
