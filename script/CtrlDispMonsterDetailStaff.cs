using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlDispMonsterDetailStaff : MonoBehaviour {

	public void Initialize( List<DataStaffParam> _staff_list ){

		m_staffClean.Initialize ("cleanstaff_no");
		m_staffMeal.Initialize ("eatstaff_no");

		foreach (DataStaffParam staff in _staff_list) {
			CsvStaffParam csvData = DataManager.GetStaff (staff.staff_id);
			if (csvData.effect_param == 1 || csvData.effect_param == 3) {
				m_staffClean.Initialize ( string.Format("staff_icon{0}" , staff.staff_id));
			}
			if (csvData.effect_param == 2 || csvData.effect_param == 3) {
				m_staffMeal.Initialize (string.Format("staff_icon{0}" , staff.staff_id));
			}
		}
		return;
	}

	// 肝試し用
	public void Initialize( int _iOfficeSerial)
	{
		DataItemParam dataItem = DataManager.Instance.m_dataItem.Select(_iOfficeSerial);
		CsvItemParam officeParam = DataManager.Instance.m_csvItem.Select(dataItem.item_id);

		if(officeParam.category == 2)
		{
			string strFilename = CsvItem.GetFilename(officeParam.item_id, 1);
			Debug.LogError(strFilename);
			m_sprOffice.gameObject.SetActive(true);
			m_sprOffice.sprite2D = SpriteManager.Instance.Load(strFilename);
			m_lbSpot.gameObject.SetActive(false);
		}
		else
		{
			m_sprOffice.gameObject.SetActive(false);
			m_lbSpot.gameObject.SetActive(true);
		}

	}


	#region SerializeField
	[SerializeField]
	private CtrlDispMonsterDetailStaffSub m_staffClean;
	[SerializeField]
	private CtrlDispMonsterDetailStaffSub m_staffMeal;
	[SerializeField]
	private UI2DSprite m_sprOffice;
	[SerializeField]
	private UILabel m_lbSpot;

	#endregion

	// Update is called once per frame
	void Update () {
	
	}
}
