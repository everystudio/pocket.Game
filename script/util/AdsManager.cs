using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NendUnityPlugin.AD;
using GoogleMobileAds.Api;

public class AdsManager : Singleton<AdsManager> {

	[SerializeField]
	private GameObject m_goAdIcon;
	[SerializeField]
	private GameObject m_goAdBanner;
	[SerializeField]
	private List<GameObject> m_goAdNativePanelList;
	private int m_iAdNativePanelIndex;

	#if UNITY_ANDROID
	//private NendAdIcon m_nendAdIcon;
	//private bool m_bIsIcon = true;
	#endif
	//private NendAdBanner m_nendAdBanner;
	private BannerView m_nendAdBanner = null;


#if UNITY_IPHONE
	public const string ASSET_BUNDLE_PREFIX             = "iphone";
	public const string ASSET_BUNDLES_ROOT              = "AssetBundles/iOS";
#elif UNITY_ANDROID
	public const string ASSET_BUNDLE_PREFIX             = "android";
	public const string ASSET_BUNDLES_ROOT              = "AssetBundles/Android";
	#endif

	public static readonly int START_GOLD = 300;
	#if UNITY_IPHONE
	//iOS
	public static readonly string IMOBILE_PID = "34367";
	public static readonly string IMOBILE_MID = "215443";
	public static readonly string IMOBILE_SID_ICON = "371577";		// 使ってないけどね
	public static readonly string IMOBILE_SID_BANNER = "622054";
	//public static readonly string IMOBILE_SID_RECT = "391442";
	#elif UNITY_ANDROID
	public static readonly string IMOBILE_PID = "34367";
	public static readonly string IMOBILE_MID = "247749";
	public static readonly string IMOBILE_SID_ICON = "760610";
	public static readonly string IMOBILE_SID_BANNER = "760609";
	public static readonly string IMOBILE_SID_RECT = "412437";
#endif

	[SerializeField]
	private Image m_imgBannerDummy;

	public override void Initialize ()
	{
		InterstitialLoad();

#if UNITY_EDITOR
		if (m_imgBannerDummy == null)
		{
			m_imgBannerDummy = GameObject.Find("CanvasNendAd").GetComponent<ImageHolder>().image;
		}
		m_imgBannerDummy.gameObject.SetActive(true);
#else
#endif
#if UNITY_EDITOR
		string adUnitId1 = "unused";
#elif UNITY_ANDROID
        string adUnitId1 = "ca-app-pub-5869235725006697/3596808362";
#elif UNITY_IPHONE
        string adUnitId1 = "ca-app-pub-5869235725006697/1980474369";
#else
        string adUnitId1 = "unexpected_platform";
#endif

		if (m_nendAdBanner == null)
		{
			BannerView bannerView1 = new BannerView(adUnitId1, AdSize.Banner, AdPosition.BottomLeft);
			// Create an empty ad request.
			AdRequest request1 = new AdRequest.Builder().Build();
			// Load the banner with the request.
			bannerView1.LoadAd(request1);
			m_nendAdBanner = bannerView1;
		}
		// 最初はでないようにする
		foreach (GameObject obj in m_goAdNativePanelList) {
			if (obj != null) {
				obj.SetActive (false);
			}
		}
		m_iAdNativePanelIndex = m_goAdNativePanelList.Count-1;
		if (m_goAdNativePanelList [m_iAdNativePanelIndex] != null) {
			m_goAdNativePanelList [m_iAdNativePanelIndex].SetActive (false);
		}
	}

	public int m_iShowBannerLock;
	public void ShowAdBanner( bool _bFlag ){

		if (_bFlag) {
			if (0 < m_iShowBannerLock)
			{
				m_iShowBannerLock -= 1;
			}
			if (m_iShowBannerLock == 0)
			{
				m_nendAdBanner.Show();
#if UNITY_EDITOR
				if (m_imgBannerDummy != null && m_imgBannerDummy != null)
				{
					m_imgBannerDummy.gameObject.SetActive(true);
				}
#endif
			}
		} else {
			if(m_iShowBannerLock < 0)
			{
				m_iShowBannerLock = 0;
			}
			m_iShowBannerLock += 1;
			m_nendAdBanner.Hide ();
#if UNITY_EDITOR
			if (m_imgBannerDummy != null)
			{
				if (m_imgBannerDummy.gameObject != null)
				{
					m_imgBannerDummy.gameObject.SetActive(false);
				}
			}
#endif
		}

		//Debug.LogError(string.Format("ShowAdBanner:{0} Lock{1}", _bFlag, m_iShowBannerLock));

	}

	public void ShowIcon( bool _bFlag ){

		int[] prob_table = new int[2]{
			DataManager.Instance.config.HasKey( "nativead_1_prob" ) ? DataManager.Instance.config.ReadInt( "nativead_1_prob" ) : 0 ,
			DataManager.Instance.config.HasKey( "nativead_2_prob" ) ? DataManager.Instance.config.ReadInt( "nativead_2_prob" ) : 0 				
		};
		int buf = 0;
		foreach (int temp in prob_table) {
			buf += temp;
		}
		if (buf == 0) {
			return;
		}

		if (_bFlag == true) {
			m_iAdNativePanelIndex = UtilRand.GetIndex (prob_table);
			m_iAdNativePanelIndex %= m_goAdNativePanelList.Count;
		}
		if (m_goAdNativePanelList [m_iAdNativePanelIndex] != null) {
			m_goAdNativePanelList [m_iAdNativePanelIndex].SetActive (_bFlag);
		}
		return;
	}

	// Use this for initialization
	void Start () {
	}
	private bool m_bInterstitialLoaded = false;
	private InterstitialAd interstitial;

	public void CallInterstitial()
	{
		if (m_bInterstitialLoaded == true)
		{
			interstitial.OnAdClosed += ViewInterstitial_OnAdClosed;
			interstitial.Show();
		}
	}
	private void InterstitialLoad()
	{
		// 通常表示
#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-5869235725006697/5073541567";
#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-5869235725006697/3457207561";
#endif
		m_bInterstitialLoaded = false;
		// Create an interstitial.
		interstitial = new InterstitialAd(adUnitId);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder()
			.AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
				.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
				.Build();

		// Load the interstitial with the request.
		interstitial.LoadAd(request);
		interstitial.OnAdLoaded += ViewInterstitial_OnAdLoaded;
		interstitial.OnAdFailedToLoad += ViewInterstitial_OnAdFailedToLoad;

	}

	private void ViewInterstitial_OnAdLoaded(object sender, System.EventArgs e)
	{
		m_bInterstitialLoaded = true;
	}
	private void ViewInterstitial_OnAdFailedToLoad(object sender, System.EventArgs e)
	{
		Debug.LogError("fail");
	}
	private void ViewInterstitial_OnAdClosed(object sender, System.EventArgs e)
	{
		InterstitialAd inter = (InterstitialAd)sender;
		inter.Destroy();
		InterstitialLoad();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
