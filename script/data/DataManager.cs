﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DataManager : DataManagerBase <DataManager>{

	public readonly string SPREAD_SHEET = "1mwbjeoyZkCs0k0XAOny_gFNJxormS0xoKO1IOPbEOLQ";
	public readonly string SPREAD_SHEET_CONFIG_SHEET = "od6";

#if UNITY_IPHONE
	public readonly string KEY_INTERSTENTIAL_START_APIKEY = "interstitial_start_apikey_ios";
	public readonly string KEY_INTERSTENTIAL_START_STOPID = "interstitial_start_spotid_ios";
	public readonly string KEY_INTERSTENTIAL_GAME_APIKEY = "interstitial_game_apikey_ios";
	public readonly string KEY_INTERSTENTIAL_GAME_STOPID = "interstitial_game_spotid_ios";
#elif UNITY_ANDROID
	public readonly string KEY_INTERSTENTIAL_START_APIKEY = "interstitial_start_apikey_and";
	public readonly string KEY_INTERSTENTIAL_START_STOPID = "interstitial_start_spotid_and";
	public readonly string KEY_INTERSTENTIAL_GAME_APIKEY = "interstitial_game_apikey_and";
	public readonly string KEY_INTERSTENTIAL_GAME_STOPID = "interstitial_game_spotid_and";
#endif

	public readonly string KEY_CONFIG_UPDATE= "config_update";
	public readonly string KEY_ITEM_UPDATE= "item_update";
	public readonly string KEY_MONSTER_UPDATE= "monster_update";
	public readonly string KEY_WORK_UPDATE = "work_update";
	public readonly string KEY_SKIT_UPDATE = "skit_update";
	public readonly string KEY_WORD_UPDATE = "word_update";	

	public readonly string KEY_ITEM_VERSION = "item_version";
	public readonly string KEY_MONSTER_VERSION = "monster_version";
	public readonly string KEY_WORK_VERSION = "work_version";
	public readonly string KEY_SKIT_VERSION = "skit_version";
	public readonly string KEY_WORD_VERSION = "word_version";

	public readonly string GA_START = "push_start";
	public readonly string GA_BOOK_MAIN= "book_main";
	public readonly string GA_ITEM_MAIN = "item_main";
	public readonly string GA_WORK_MAIN = "work_main";
	public readonly string GA_COLLECT_SUCCESS = "collect_success";
	public readonly string GA_COLLECT_FAIL = "collect_fail";
	public readonly string GA_BUILDUP_CAGE = "buildup_cage";
	public readonly string GA_BUILDUP_OFFICE = "buildup_office";

	public readonly string KEY_UNITYADS_APP_ID_ANDROID = "unityads_app_id_android";
	public readonly string KEY_UNITYADS_APP_ID_IOS = "unityads_app_id_ios";
	public readonly string KEY_UNITYADS_LASTPLAY_TIME = "unityads_lastplay_time";

	public readonly string KEY_BGM_START= "bgm_start";
	public readonly string KEY_BGM_PARK = "bgm_park";
	public readonly string KEY_BGM_BOOK = "bgm_book";
	public readonly string KEY_BGM_WORK = "bgm_work";
	public readonly string KEY_BGM_SHOP = "bgm_shop";

	public readonly string KEY_HELP_ACTION_MOVIE_PROB = "key_help_action_movie_prob";
	public readonly string KEY_HELP_ACTION_TWITTER_PROB = "key_help_action_twitter_prob";

	public readonly string KEY_SHARE_MESSAGE_ANDROID = "key_share_message_android";
	public readonly string KEY_SHARE_MESSAGE_IOS = "key_share_message_ios";

	public readonly string KEY_TOUCHABLE_FIELD_NAME = "parkRootTouchable";
	public readonly string KEY_VISITOR_TAMASHII = "visitorTamashii";
	public readonly string KEY_DISP_VISITOR = "key_disp_visitor";
	public readonly string KEY_ATTENTION_DISP_VISITOR = "attention_disp_visitor";

	public readonly string KEY_COLLECT_GOLD = "collect_gold";
	public readonly string KEY_COLLECT_EXP = "collect_exp";

	public readonly string FILENAME_SKIT_DATA = "csv/skit";
	public readonly string FILENAME_WORD_DATA = "csv/word";


	public readonly int DEPTH_ROAD 		= 100;
	public readonly int DEPTH_ITEM 		= 500;
	public readonly int DEPTH_VISITOR = 500;
	public readonly int DEPTH_DUST 		= 1000;
	public readonly int DEPTH_MONSTER	= 1500;
	public readonly int DEPTH_MONSTER_FUKIDASHI	= 2000;


	/*
	 * 

DROP TABLE IF EXISTS (new_table);
CREATE TABLE new_table (
  test_key VARCHAR(255) NOT NULL,
  test_value text NOT NULL,
  PRIMARY KEY(test_key)
);


insert into new_table (test_key,test_value) values ('insert_key' , 'insert_value');
	*/
	IEnumerator send( string _filename , string _key ,string _strTime  ){
		string strScreenShotURL= "http://every-studio.com/html/test.php";
		//string strScreenShotURL= "http://every-studio.com/html/test.php?test_key={0}{1}&test_value={2}";
		string test_value = "";
		string filename = "";
		FileInfo fi;
		StreamReader sr;

		filename = string.Format ("{0}.csv", _filename);
		string fullpath = System.IO.Path.Combine (Application.persistentDataPath , filename);
		fi = new FileInfo(fullpath);
		sr = new StreamReader(fi.OpenRead());

		test_value = sr.ReadToEnd ();
		test_value = Base64.Encode (test_value);
		string result = string.Format (strScreenShotURL, SystemInfo.deviceUniqueIdentifier, _key,test_value);
		Debug.LogError (result);
		Debug.LogError (Base64.Decode (test_value));
		WWWForm wf = new WWWForm ();
		wf.AddField ("test_key", string.Format ("{0}{2}{1}", SystemInfo.deviceUniqueIdentifier, _key , _strTime));
		wf.AddField ("test_value", string.Format ("{0}", test_value));
		//WWW w = new WWW (result , wf);
		WWW w = new WWW (strScreenShotURL , wf);
		yield return w;

		if (w.error != null){
			Debug.Log(w.error);
		}
		else{
			Debug.Log("Image Uploaded!");
		}
	}
	public void send_data(){

		Debug.LogError ("upload_test start");
		//string strScreenShotURL= "http://every-studio.com/html/test.php?test_key={0}{1}&test_value={2}";

		string strTime = TimeManager.StrGetTime ();
		strTime = strTime.Replace("-" , "").Replace(":","").Replace(" ","");
		strTime = string.Format ("({0})", strTime);
		StartCoroutine(send (DataItem.FILENAME, "dataitem" , strTime));
		StartCoroutine(send (DataStaff.FILENAME, "datastaff" , strTime));
		StartCoroutine(send (DataMonster.FILENAME, "datamonster" , strTime));
		StartCoroutine (send (DataWork.FILENAME, "datawork" , strTime));
		StartCoroutine (send (DataKvs.FILE_NAME, "datakvs" , strTime));

		/*

		// We should only read the screen after all rendering is complete
		yield return new WaitForEndOfFrame();

		// Create a texture the size of the screen, RGB24 format
		var width = Screen.width;
		var height = Screen.height;
		var tex = new Texture2D( width, height, TextureFormat.RGB24, false );

		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();

		// Encode texture into PNG
		var bytes = tex.EncodeToPNG();
		Destroy( tex );

		// Create a Web Form
		var form = new WWWForm();
		form.AddField("frameCount", Time.frameCount.ToString());
		form.AddBinaryData("file", bytes, "screenShot.png", "image/png");

		// Upload to a cgi script
		WWW w = new WWW(strScreenShotURL, form);
		yield return w;

		if (w.error != null){
			Debug.Log(w.error);
		}
		else{
			Debug.Log("Image Uploaded!");
		}

		*/
		Debug.LogError ("upload_test end");
	}


	public override void Initialize ()
	{
		base.Initialize ();

		Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 0;

		DontDestroyOnLoad(gameObject);

		if (PlayerPrefs.HasKey (DefineOld.USER_WIDTH) == false) {
			PlayerPrefs.SetInt (DefineOld.USER_WIDTH, DefineOld.DEFUALT_USER_WIDTH);
			PlayerPrefs.SetInt (DefineOld.USER_HEIGHT, DefineOld.DEFUALT_USER_WIDTH);
		}

		//int iWidth = PlayerPrefs.GetInt (DefineOld.USER_WIDTH);
		//int iHeight= PlayerPrefs.GetInt (DefineOld.USER_HEIGHT);

		//m_tDataUser.Initialize (iWidth,iHeight);
		AllLoad();
		foreach( CsvLocalNotificationParam param in csv_localNotification ){
			LocalNotificationManager.Instance.Add (param);
		}
		return;
	}

	public void AllLoad(){
		config.Load (CsvConfig.FILE_NAME);
		kvs_data.Load( DataKvs.FILE_NAME );
		m_csvItem.Load ();
		m_csvMonster.Load ();
		dataMonster.Load (DataMonster.FILENAME);
		dataStaff.Load (DataStaff.FILENAME);

		word.LoadMulti("csv/word");

		skitData.LoadMulti(FILENAME_SKIT_DATA);

		//Debug.LogError ("here");
		m_dataItem.Load (DataItem.FILENAME);

		m_csvItemDetail.Load ();

		//m_csvWork.Load ();
		dataWork.Load (DataWork.FILENAME);

		m_csvStaff.Load ();
		m_csvLevel.Load ();
		m_csvTime.Load ();
		m_csvWord.Load ();
		m_csvTutorial.Load ();
		m_csvLocalNotification.Load ();
		RoadLoad ();
		//ReloadScareRate();
	}

	public void DataSave(){
#if !UNITY_EDITOR
		Debug.LogError ("save");
#endif
		m_dataKvs.Save (DataKvs.FILE_NAME);
		dataMonster.Save (DataMonster.FILENAME);
		dataStaff.Save (DataStaff.FILENAME);
		m_dataItem.Save (DataItem.FILENAME);
		dataWork.Save (DataWork.FILENAME);

		m_csvItem.Save (CsvItem.FilePath);
		m_csvMonster.Save (CsvMonster.FilePath);
		m_csvStaff.Save (CsvStaffData.FilePath);


	}


	public void DummyCall(){
		return;
	}

	private DataUser m_tDataUser = new DataUser ();
	static public DataUser user{
		get{
			return Instance.m_tDataUser;
		}
		set {
			Instance.m_tDataUser = value;
		}
	}

	static public List<CsvItemParam> itemMaster {
		get{ return Instance.m_csvItem.list; }
	}
	static public CsvItemParam GetItemMaster( int _iItemId ){
		foreach( CsvItemParam data in itemMaster ){
			if (data.item_id == _iItemId) {
				return data;
			}
		}
		return new CsvItemParam ();
	}

	public CsvSkit skitData = new CsvSkit();

	public CsvConfig word = new CsvConfig();
	public string getWord( string _strKey)
	{
		string[] delimiter = { @"\n" };
		string temp = word.Read(_strKey);
		//string[] strArr = temp.Split(delimiter,System.StringSplitOptions.RemoveEmptyEntries);
		string[] strArr = temp.Split(delimiter,System.StringSplitOptions.None);

		string strRet = "";

		if( 1 < strArr.Length)
		{
			bool bFirst = true;
			foreach (string div in strArr)
			{
				if(bFirst)
				{
					bFirst = false;
				}else
				{
					strRet += "\n";
				}
				strRet += div;
			}
		}
		else
		{
			strRet = temp;
		}
		return strRet;
	}

#region For CSV
	public CsvItem m_csvItem = new CsvItem();
	static public List<CsvItemParam> csv_item {
		get{ 
			return Instance.m_csvItem.All;
		}
	}
	static public CsvItemParam GetItem( int _iItemId ){
		foreach (CsvItemParam data in csv_item) {
			if (_iItemId == data.item_id) {
				return data;
			}
		}
		return new CsvItemParam ();
	}

	public DataItem m_dataItem = new DataItem();

	public List<DataItemParam> m_ItemDataList {
		get{ 
			return m_dataItem.Select (" status != 0 ");
		}
	}


	public CsvMonster m_csvMonster = new CsvMonster();
	static public List<CsvMonsterParam> csv_monster {
		get{ 
			return Instance.m_csvMonster.All;
		}
	}
	static public CsvMonsterParam GetMonster( int _iMonsterId ){
		foreach (CsvMonsterParam data in csv_monster) {
			if (_iMonsterId == data.monster_id) {
				return data;
			}
		}
		return new CsvMonsterParam ();
	}
	public CsvItemDetal m_csvItemDetail = new CsvItemDetal();
	static public List<CsvItemDetailData> csv_item_detail {
		get{ 
			return Instance.m_csvItemDetail.All;
		}
	}
	static public CsvItemDetailData GetItemDetail( int _iItemId , int _iLevel ){
		foreach (CsvItemDetailData data in csv_item_detail) {
			if (data.item_id == _iItemId && data.level == _iLevel) {
				return data;
			}
		}
		return new CsvItemDetailData ();
	}
	/*
	public CsvWork m_csvWork = new CsvWork();
	static public List<CsvWorkParam> csv_work {
		get{ 
			return Instance.m_csvWork.All;
		}
	}
	*/
	static public DataWorkParam GetWork( int _iWorkId ){
		foreach (DataWorkParam work in Instance.dataWork.list) {
			if (work.work_id == _iWorkId) {
				return work;
			}
		}
		Debug.LogError ("ignore staff_id:" + _iWorkId.ToString ());
		return new DataWorkParam ();
	}
	public DataWork dataWork = new DataWork ();
	public DataMonster dataMonster = new DataMonster ();
	public DataStaff dataStaff= new DataStaff();


	public CsvStaffData m_csvStaff = new CsvStaffData();
	static public List<CsvStaffParam> csv_staff {
		get{ 
			return Instance.m_csvStaff.All;
		}
	}
	static public CsvStaffParam GetStaff( int _iStaffId ){
		foreach (CsvStaffParam staff in csv_staff) {
			if (staff.staff_id == _iStaffId) {
				return staff;
			}
		}
		Debug.LogError ("ignore staff_id:" + _iStaffId.ToString ());
		return new CsvStaffParam ();
	}
	public CsvLevel m_csvLevel = new CsvLevel();
	static public List<CsvLevelData> csv_level {
		get{ 
			return Instance.m_csvLevel.All;
		}
	}
	public CsvTime m_csvTime = new CsvTime();
	static public List<CsvTimeData> csv_time {
		get{ 
			return Instance.m_csvTime.All;
		}
	}
	public CsvWordData m_csvWord = new CsvWordData();
	static public List<CsvWordParam> csv_word {
		get{ 
			return Instance.m_csvWord.All;
		}
	}
	public string GetWord( string _strKey ){
		foreach (CsvWordParam data in csv_word) {
			if (_strKey.Equals (data.key) == true) {
				return data.word;
			}
		}
		return "-------";
	}

	public CsvTutorial m_csvTutorial = new CsvTutorial();
	static public List<CsvTutorialData> csv_tutorial {
		get{ 
			return Instance.m_csvTutorial.All;
		}
	}

	public CsvLocalNotificationData m_csvLocalNotification = new CsvLocalNotificationData();
	static public List<CsvLocalNotificationParam> csv_localNotification {
		get{ 
			return Instance.m_csvLocalNotification.All;
		}
	}

	public CtrlHelp.ACTION_TYPE GetHelpActionType(){

		int[] prob_arr = new int[(int)CtrlHelp.ACTION_TYPE.MAX] { 0 , 100 , 100 };

		if (config.HasKey (KEY_HELP_ACTION_MOVIE_PROB)) {
			prob_arr [(int)CtrlHelp.ACTION_TYPE.MOVIE] = config.ReadInt (KEY_HELP_ACTION_MOVIE_PROB);
		}
		if (config.HasKey (KEY_HELP_ACTION_TWITTER_PROB)) {
			prob_arr [(int)CtrlHelp.ACTION_TYPE.TWITTER] = config.ReadInt (KEY_HELP_ACTION_TWITTER_PROB);
		}

		CtrlHelp.ACTION_TYPE eRet = (CtrlHelp.ACTION_TYPE)UtilRand.GetIndex (prob_arr);
		return eRet;
	}

	public Dictionary<string , string > m_RoadMap = new Dictionary<string , string > ();
	//public List<string> m_RoadMap = new List<string>();
	public string _getRoadHash( int _iX , int _iY ){
		return string.Format ("{0},{1}", _iX, _iY);
	}
	public void RoadLoad(){
		m_RoadMap.Clear ();
		foreach (DataItemParam item in m_dataItem.list) {
			if (item.status != 0 && item.item_id == DefineOld.ITEM_ID_ROAD) {
				string hash = _getRoadHash (item.x, item.y);
				m_RoadMap.Add (hash,item.item_serial.ToString());
			}
		}
		return;
	}
	public bool IsRoad( int _iX , int _iY ){
		string road_hash = _getRoadHash (_iX, _iY);
		return m_RoadMap.ContainsKey (road_hash);
	}

	public void ReloadScareRate()
	{
		List<DataItemParam> list = DataManager.Instance.m_dataItem.Select(" category = 2 and status != 0 ");

		// ここでリセット処理
		// オフィスとかもやっちゃうけど、レベルから取り直すのでOK。
		foreach (DataItemParam param in DataManager.Instance.m_dataItem.list)
		{
			param.range = 100;
		}

		foreach (DataItemParam param in list)
		{
			CsvItemParam master_data = DataManager.Instance.m_csvItem.Select(param.item_id);
			CsvItemDetailData item_detail = DataManager.GetItemDetail(param.item_id, param.level);
			Debug.LogError(item_detail.revenue_rate);
			for (int x = param.x - (item_detail.area); x < param.x + master_data.size + (item_detail.area); x++)
			{
				for (int y = param.y - (item_detail.area); y < param.y + master_data.size + (item_detail.area); y++)
				{
					foreach (CtrlFieldItem field_item in GameMain.ParkRoot.m_fieldItemList)
					{
						if(field_item.m_dataItemParam.item_id== 0)
						{
							continue;
						}
						// xyが合ってて、シリアルは別
						if (field_item.m_dataItemParam.x == x && field_item.m_dataItemParam.y == y && param.item_serial != field_item.m_dataItemParam.item_serial)
						{
							/*
							Debug.LogError(string.Format("serial={0} item_id={1} range={2} param={3} reve={4}", 
								field_item.m_dataItemParam.item_serial , 
								field_item.m_dataItemParam.item_id,
								field_item.m_dataItemParam.range,
								param.range,
								item_detail.revenue_rate));
								*/
							if (field_item.m_dataItemParam.range < item_detail.revenue_rate)
							{
								//Debug.LogError(string.Format("update serial={2} range={0} revenue_rate={1}" , param.range , item_detail.revenue_rate , param.item_serial) );
								field_item.m_dataItemParam.range = item_detail.revenue_rate;
							}
						}
					}
				}
			}
		}
	}

	public bool m_bSymbolRate;
	public float m_fSymbolRate;
	public float UpdateSymbolRate(){
		float fTotalRate = 1.0f;
		//List<DataItem> symbol_list = GameMain.dbItem.Select (" type = " + ((int)(DefineOld.Item.Type.SYMBOL)).ToString () + " ");
		List<DataItemParam> symbol_list = DataManager.Instance.m_dataItem.Select ( DefineOld.WHERE_PATTERN.RATE );
		foreach( DataItemParam symbol in symbol_list ){
			CsvItemParam csv_symbol_item_data = DataManager.GetItem (symbol.item_id);
			if ((int)(fTotalRate * 100.0f) < csv_symbol_item_data.revenue_up) {
				fTotalRate = csv_symbol_item_data.revenue_up * 0.01f;
			}
		}
		m_fSymbolRate = fTotalRate;
		return fTotalRate;
	}
	public float GetSymbolRate(){
		if (m_bSymbolRate == false) {
			UpdateSymbolRate ();
		}
		return m_fSymbolRate;
	}

	public string ReviewUrl(){

		string strRet = "https://play.google.com/store/apps/details?id=jp.everystudio.pocket.zoo";

#if UNITY_ANDROID
		strRet = "https://play.google.com/store/apps/details?id=jp.everystudio.pocket.zoo";
		if( config.HasKey( "reviewurl_android")){
			strRet = config.Read("reviewurl_android");
		}
#elif UNITY_IOS
		strRet = "https://itunes.apple.com/us/app/leshii-fang-zhi-jing-yinggemu/id1112070121?l=ja&ls=1&mt=8";
		if( config.HasKey( "reviewurl_ios")){
			strRet = config.Read("reviewurl_ios");
		}
#else
#endif
		return strRet;
	}

#endregion
	public float m_fInterval;
	public const float EDITOR_SAVE_INTERVAL = 10.0f;

	void OnApplicationPause(bool pauseStatus) {
		///Debug.LogError ("here");
		if (pauseStatus) {
			DataSave ();

		} else {
		}
	}
#if UNITY_EDITOR
	void Update(){

		m_fInterval += Time.deltaTime;

		if (EDITOR_SAVE_INTERVAL < m_fInterval) {
			m_fInterval -= EDITOR_SAVE_INTERVAL;
			DataSave ();
		}

	}
#endif

}



















