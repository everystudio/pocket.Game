using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class CtrlCollectGold : Singleton<CtrlCollectGold> {

	public UnityEvent m_eventCollect = new UnityEvent ();

	#region SerializeField
	[SerializeField]
	private UILabel m_lbCollectGold;
	[SerializeField]
	private ButtonBase m_btnCollect;
	[SerializeField]
	private UI2DSprite m_sprImage;
	[SerializeField]
	private UIButton m_uiButton;
	[SerializeField]
	private BoxCollider m_buttonCollider;
	#endregion


	public float m_fTimer;
	public float m_fCheckInterval;

	public int m_iBufGold;
	public int m_iBufExp;

	public int m_iSearchIndex;

	public bool m_bInitialize;
	void Start(){
		m_bInitialize = false;
	}

	private void setButtonColor(bool _bFlag)
	{
		if(_bFlag)
		{
			m_uiButton.defaultColor = new Color(1f, 1f, 1f);
			m_sprImage.color = new Color(1f, 1f, 1f);
		}
		else
		{
			m_uiButton.defaultColor = new Color(0.5f, 0.5f, 0.5f);
			m_sprImage.color = new Color(0.5f, 0.5f, 0.5f);
		}
	}

	public void AddCollect( int _iGold , int _iExp ){
		setButtonColor(true);
		m_iBufGold += _iGold;
		m_iBufExp += _iExp;
		return;
	}
	
	public override void Initialize ()
	{
		m_bInitialize = true;
		m_fTimer = 0.0f;
		m_fCheckInterval = 5.0f;
		m_btnCollect.TriggerClear ();
		m_iBufGold = 0;
		m_iBufExp = 0;
		setButtonColor(false);
	}

	void Update () {
		if (m_bInitialize == false) {
			return;
		}

		m_fTimer += Time.deltaTime;
		if (m_fCheckInterval < m_fTimer) {
			m_fTimer -= m_fCheckInterval;
		}

		if (m_btnCollect.ButtonPushed) {
			m_btnCollect.TriggerClear ();
			int iCollectGold = 0;
			int iCollectExp = 0;
			iCollectGold = m_iBufGold;
			iCollectExp = m_iBufExp;
			if (0 < m_iBufGold) {
				SoundManager.Instance.PlaySE ("se_cash", "https://s3-ap-northeast-1.amazonaws.com/every-studio/app/sound/se");

				DataManager.user.AddCollect ();
				DataManager.user.AddGold (m_iBufGold);
				DataManager.user.AddSyakkin (-1 * m_iBufGold);
				DataManager.user.AddExp (m_iBufExp);

				m_eventCollect.Invoke ();

				// ここで仕事のチェックしますか
				List<DataWorkParam> check_work_list = DataManager.Instance.dataWork.Select (" status = 1 ");
				foreach (DataWorkParam work in check_work_list) {
					if (work.ClearCheck ()) {
						work.MissionClear ();
					}
				}
				GoogleAnalytics.Instance.Log (DataManager.Instance.GA_COLLECT_SUCCESS);
			} else {
				GoogleAnalytics.Instance.Log (DataManager.Instance.GA_COLLECT_FAIL);
			}
			m_iBufGold = 0;
			m_iBufExp = 0;
			setButtonColor(false);

		}
		return;
	
	}
}












