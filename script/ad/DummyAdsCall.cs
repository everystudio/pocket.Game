using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAdsCall : MonoBehaviour {

	// Use this for initialization
	void Start () {

		GoogleMobileAds.Common.DummyClient temp = gameObject.GetComponent<GoogleMobileAds.Common.DummyClient>();

		temp.OnAdLoaded += (sender, args) =>
		{
		};
		temp.OnAdFailedToLoad += (sender, args) =>
		{
		};
		temp.OnAdOpening += (sender, args) =>
		{
		};
		temp.OnAdStarted += (sender, args) =>
		{
		};
		temp.OnAdClosed += (sender, args) =>
		{
		};
		temp.OnAdRewarded += (sender, args) =>
		{
		};
		temp.OnAdLeavingApplication += (sender, args) =>
		{
		};
		temp.OnCustomNativeTemplateAdLoaded += (sender, args) =>
		{
		};

		/*
		public event EventHandler<EventArgs> OnAdLoaded;

		public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

		public event EventHandler<EventArgs> OnAdOpening;

		public event EventHandler<EventArgs> OnAdStarted;

		public event EventHandler<EventArgs> OnAdClosed;

		public event EventHandler<Reward> OnAdRewarded;

		public event EventHandler<EventArgs> OnAdLeavingApplication;

		public event EventHandler<CustomNativeEventArgs> OnCustomNativeTemplateAdLoaded;
		*/


	}

	// Update is called once per frame
	void Update () {
		
	}
}
