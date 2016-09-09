using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlVisitor : MonoBehaviourEx {

	public enum STEP
	{
		NONE		= 0,
		WAIT		,
		MOVE		,
		SCARE		,
		END			,
		MAX			,
	}
	public STEP m_eStep;
	private STEP m_eStepPre;
	private STEP m_eStepSave;
	
	public bool m_bUp;

	class RoadPosition{
		public int x;
		public int y;
	}
	public int m_iVisitorSerial;

	[SerializeField]
	private float m_fTimer;

	[SerializeField]
	private int m_iType;
	[SerializeField]
	private float m_fAnimationTimer;
	[SerializeField]
	private int m_iAnimationFrame;

	[SerializeField]
	private int m_iPosX;
	[SerializeField]
	private int m_iPosY;

	[SerializeField]
	private int m_iTargetX;
	[SerializeField]
	private int m_iTargetY;
	[SerializeField]
	private int m_iTargetItemSerial;

	[SerializeField]
	private Vector3 m_v3StartPosition;
	[SerializeField]
	private Vector3 m_v3TargetPosition;

	[SerializeField]
	private GameObject m_goHitCenter;
	public Vector3 GetCenterPos()
	{
		return m_goHitCenter.transform.position;
	}

	[SerializeField]
	private CtrlTouchCollectTamashii m_ctrlTouchCollectTamashii;

	private float INTERVAL = 1.0f;
	private float INTERVAL_ANIMATION = 0.4f;

	[SerializeField]
	private UI2DSprite m_sprChara;

	private void setSprite (int _iType, int _iFrame){
		string people = string.Format ("texture/ui/people{0:D3}_{1:D2}.png", _iType, _iFrame);
		//Debug.Log (people);
		m_sprChara.sprite2D = SpriteManager.Instance.Load (people);
		//Debug.Log (m_sprChara.sprite2D);
		m_sprChara.width = (int)m_sprChara.sprite2D.textureRect.width;
		m_sprChara.height = (int)m_sprChara.sprite2D.textureRect.height;
	}

	private Vector3 getPosition( int _iX , int _iY ){
		return (DefineOld.CELL_X_DIR.normalized * DefineOld.CELL_X_LENGTH * ((float)_iX+0.5f)) + (DefineOld.CELL_Y_DIR.normalized * DefineOld.CELL_Y_LENGTH * ((float)_iY+0.5f));
	}
	private void setDepth( int _iX , int _iY ){
		int iDepth = 100 - (_iX + _iY);// + (m_dataItemParam.height-1));
		m_sprChara.depth = iDepth + DataManager.Instance.DEPTH_VISITOR;
		m_sprTamashii.depth = iDepth + DataManager.Instance.DEPTH_VISITOR + 5;
	}

	public bool IsActive(){
		return m_eStep != STEP.END;
	}

	public void Initialize( int _iType , int _iItemSerial , int _iVisitorSerial ){
		m_monsterSerialList.Clear();
		m_iVisitorSerial = _iVisitorSerial;
		m_iType = _iType;
		DataItemParam item_param = DataManager.Instance.m_dataItem.Select (_iItemSerial);
		m_sprTamashii.gameObject.SetActive(false);
		m_sprTamashii.gameObject.name = string.Format("{0}{1}", DataManager.Instance.KEY_VISITOR_TAMASHII , _iVisitorSerial);
		//Debug.LogError (string.Format ("x={0} y={1}", _iX, _iY));
		//myTransform.localPosition = (DefineOld.CELL_X_DIR.normalized * DefineOld.CELL_X_LENGTH * item_param.x) + (DefineOld.CELL_Y_DIR.normalized * DefineOld.CELL_Y_LENGTH * item_param.y);
		m_bUp = true;
		m_iPosX = item_param.x;
		m_iPosY = item_param.y;
		m_iTargetX = m_iPosX;
		m_iTargetY = m_iPosY;

		m_iAnimationFrame = 0;
		setSprite (m_iType, m_iAnimationFrame);

		myTransform.localPosition = getPosition (m_iPosX, m_iPosY);
		setDepth (m_iPosX, m_iPosY);
		m_sprChara.gameObject.SetActive (true);

		m_eStep = STEP.WAIT;
	}

	// Update is called once per frame
	void Update () {
		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.WAIT:
			if (bInit) {
				m_iPosX = m_iTargetX;
				m_iPosY = m_iTargetY;
				if (m_bUp) {
					setDepth (m_iPosX, m_iPosY);
				}
			}
			if (m_bUp) {
				List<RoadPosition > check_list = new List<RoadPosition> ();
				int x = m_iPosX + 1;
				int y = m_iPosY;

				if (DataManager.Instance.IsRoad (x, y)) {
					RoadPosition temp = new RoadPosition ();
					temp.x = x;
					temp.y = y;
					check_list.Add (temp);
				}
				x = m_iPosX;
				y = m_iPosY + 1;
				if (DataManager.Instance.IsRoad (x, y)) {
					RoadPosition temp = new RoadPosition ();
					temp.x = x;
					temp.y = y;
					check_list.Add (temp);
				}

				if (0 < check_list.Count) {
					int iIndex = UtilRand.GetRand (check_list.Count);
					m_iTargetX = check_list [iIndex].x;
					m_iTargetY = check_list [iIndex].y;
					m_eStep = STEP.MOVE;
				} else {
					m_bUp = false;
				}
			} else {
				List<RoadPosition > check_list = new List<RoadPosition > ();
				int x = m_iPosX - 1;
				int y = m_iPosY;
				if (DataManager.Instance.IsRoad (x, y)) {
					RoadPosition temp = new RoadPosition ();
					temp.x = x;
					temp.y = y;
					check_list.Add (temp);
				}				x = m_iPosX;
				y = m_iPosY - 1;
				if (DataManager.Instance.IsRoad (x, y)) {
					RoadPosition temp = new RoadPosition ();
					temp.x = x;
					temp.y = y;
					check_list.Add (temp);
				}

				if (0 < check_list.Count) {
					int iIndex = UtilRand.GetRand (check_list.Count);
					m_iTargetX = check_list [iIndex].x;
					m_iTargetY = check_list [iIndex].y;
					m_eStep = STEP.MOVE;
				} else {
					x = m_iPosX - 2;
					y = m_iPosY;
					DataItemParam param = DataManager.Instance.m_dataItem.SelectOne (string.Format (" item_id = {0} and x = {1} and y = {2} ", DefineOld.ITEM_ID_GATE, x, y));
					if (param.item_id == DefineOld.ITEM_ID_GATE) {
						RoadPosition temp = new RoadPosition ();
						temp.x = x;
						temp.y = y;
						check_list.Add (temp);
					}
					x = m_iPosX - 2;
					y = m_iPosY - 1;
					param = DataManager.Instance.m_dataItem.SelectOne (string.Format (" item_id = {0} and x = {1} and y = {2} ", DefineOld.ITEM_ID_GATE, x, y));
					if (param.item_id == DefineOld.ITEM_ID_GATE) {
						RoadPosition temp = new RoadPosition ();
						temp.x = x;
						temp.y = y;
						check_list.Add (temp);
					}
					x = m_iPosX;
					y = m_iPosY - 2;
					param = DataManager.Instance.m_dataItem.SelectOne (string.Format (" item_id = {0} and x = {1} and y = {2} ", DefineOld.ITEM_ID_GATE, x, y));
					if (param.item_id == DefineOld.ITEM_ID_GATE) {
						RoadPosition temp = new RoadPosition ();
						temp.x = x;
						temp.y = y;
						check_list.Add (temp);
					}
					x = m_iPosX - 1;
					y = m_iPosY - 2;
					param = DataManager.Instance.m_dataItem.SelectOne (string.Format (" item_id = {0} and x = {1} and y = {2} ", DefineOld.ITEM_ID_GATE, x, y));
					if (param.item_id == DefineOld.ITEM_ID_GATE) {
						RoadPosition temp = new RoadPosition ();
						temp.x = x;
						temp.y = y;
						check_list.Add (temp);
					}
					if (0 < check_list.Count) {
						m_eStep = STEP.END;
					} else {
						m_bUp = true;
					}
				}
			}
			break;

		case STEP.MOVE:
			if (bInit) {
				m_fTimer = 0.0f;

				m_v3StartPosition = myTransform.localPosition;
				//DataItemParam param = DataManager.Instance.m_dataItem.Select (m_iTargetItemSerial);
				m_v3TargetPosition = getPosition (m_iTargetX, m_iTargetY);
				if (!m_bUp) {
					setDepth (m_iTargetX, m_iTargetY);
				}

				if (m_v3StartPosition.x < m_v3TargetPosition.x) {
					m_sprChara.flip = UIBasicSprite.Flip.Horizontally;
				} else {
					m_sprChara.flip = UIBasicSprite.Flip.Nothing;
				}
			}

			m_fAnimationTimer += Time.deltaTime;
			if (INTERVAL_ANIMATION < m_fAnimationTimer) {
				m_fAnimationTimer -= INTERVAL_ANIMATION;
				m_iAnimationFrame += 1;
				if (2 <= m_iAnimationFrame) {
					m_iAnimationFrame = 0;
				}
				setSprite (m_iType, m_iAnimationFrame);
			}

			m_fTimer += Time.deltaTime;
			Vector3 set_position;
			if (Linear (m_fTimer / INTERVAL, m_v3StartPosition, m_v3TargetPosition, out set_position)) {
				m_eStep = STEP.WAIT;
			}
			myTransform.localPosition = set_position;
			break;

			case STEP.SCARE:
				if (bInit)
				{
					//Debug.LogError("scare");
					//OnScare.Invoke(m_dataMonster.monster_serial);
					iTween.PunchScale(gameObject, iTween.Hash(
						"x", 0.5f,
						"y", 0.5f,
						"time", 2.5f,
						"oncomplete", "OnCompleteHandler"));
					m_bEndITween = false;
					setSprite(m_iType, 2);
				}
				if (m_bEndITween)
				//if ( 2.0f < m_fTimer)
				{
					// 特殊戻し
					m_eStepPre = m_eStepSave;
					m_eStep = m_eStepSave;
				}
				break;

			case STEP.END:
			if (bInit) {
				m_sprChara.gameObject.SetActive (false);
			}
			break;
		case STEP.MAX:
		default:
			break;
		}
	}
	private bool m_bEndITween;
	private void OnCompleteHandler()
	{
		m_bEndITween = true;
	}

	[SerializeField]
	private List<int> m_monsterSerialList = new List<int>();
	private int m_iGetExp;

	public void Scare( int _iMonsterSerial , int _iExp)
	{
		foreach( int serial in m_monsterSerialList)
		{
			if( serial == _iMonsterSerial)
			{
				return;
			}
		}

		if (m_eStep != STEP.SCARE)
		{
			m_eStepSave = m_eStep;
		}
		m_eStep = STEP.SCARE;

		m_monsterSerialList.Add(_iMonsterSerial);
		m_iGetExp += _iExp;
		m_sprTamashii.gameObject.SetActive(true);
		return;
	}

	public bool IsVisitor( int _iVisitorSerial)
	{
		return m_iVisitorSerial == _iVisitorSerial;
	}

	public void TamashiiCollect(int _iTemp)
	{
		// 事情により引数は使わないです
		DataManager.user.AddExp(m_iGetExp);
		m_iGetExp = 0;
		m_sprTamashii.gameObject.SetActive( false);
	}
	[SerializeField]
	private UI2DSprite m_sprTamashii;
}









