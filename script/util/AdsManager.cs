using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NendUnityPlugin.AD;
using GoogleMobileAds.Api;

public class AdsManager : Singleton<AdsManager> {

	[SerializeField]
	private List<GameObject> m_goAdNativePanelList;
	private int m_iAdNativePanelIndex;

	#if UNITY_ANDROID
	//private NendAdIcon m_nendAdIcon;
	//private bool m_bIsIcon = true;
	#endif
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
	#elif UNITY_ANDROID
	#endif

	public override void Initialize ()
	{
#if UNITY_EDITOR
		string adUnitId1 = "unused";
#elif UNITY_ANDROID
        string adUnitId1 = "ca-app-pub-5869235725006697/9734731566";
#elif UNITY_IPHONE
        string adUnitId1 = "ca-app-pub-5869235725006697/5679303963";
#else
        string adUnitId1 = "unexpected_platform";
#endif
		if (m_nendAdBanner == null) {
			BannerView bannerView1 = new BannerView(adUnitId1, AdSize.Banner, AdPosition.Top);
			// Create an empty ad request.
			AdRequest request1 = new AdRequest.Builder().Build();
			// Load the banner with the request.
			bannerView1.LoadAd(request1);
			m_nendAdBanner = bannerView1;
		}
		// 最初はでないようにする
		foreach (GameObject obj in m_goAdNativePanelList) {
			obj.SetActive (false);
		}
		m_iAdNativePanelIndex = m_goAdNativePanelList.Count-1;
		m_goAdNativePanelList [m_iAdNativePanelIndex].SetActive (false);
	}

	#if USE_IMOBILE
	private int m_iIMobileBannerId = 0;
	#endif
	public void ShowAdBanner( bool _bFlag ){

		if (_bFlag) {
			m_nendAdBanner.Show ();
		} else {
			m_nendAdBanner.Hide ();
		}
	}

#if USE_IMOBILE
	static private int m_iIMobileIconId = 0;
#endif
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
		m_goAdNativePanelList[m_iAdNativePanelIndex].SetActive( _bFlag );
		return;
	}
	// Use this for initialization
	void Start () {
	}

	private InterstitialAd interstitial;
	public void CallInterstitial()
	{
		// 通常表示
#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-5869235725006697/4202570763";
#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-5869235725006697/7156037168";
#endif

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
		interstitial.OnAdClosed += ViewInterstitial_OnAdClosed;
	}

	private void ViewInterstitial_OnAdLoaded(object sender, System.EventArgs e)
	{
		InterstitialAd inter = (InterstitialAd)sender;
		inter.Show();
	}
	private void ViewInterstitial_OnAdFailedToLoad(object sender, System.EventArgs e)
	{
		Debug.LogError("fail");
	}
	private void ViewInterstitial_OnAdClosed(object sender, System.EventArgs e)
	{
		InterstitialAd inter = (InterstitialAd)sender;
		inter.Destroy();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
