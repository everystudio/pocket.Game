using UnityEngine;
using System.Collections;

public class GoogleAnalytics : GoogleAnalyticsBase<GoogleAnalytics> {

	public override void Initialize ()
	{
		base.Initialize ();
		propertyID_Common = "UA-77286676-7";
		propertyID_Android = "UA-77286676-17";
		propertyID_iOS = "UA-77286676-16";

		bundleID = "jp.everystudio.pocket.kimodameshi";
		appName = "ポケット肝試し！";
		appVersion = "3";

		// ハートビート
		heartbeat ();
	}

}
