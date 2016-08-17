using UnityEngine;
using System.Collections;

public class SpriteManager : SpriteManagerBase<SpriteManager> {
	public Sprite Load( string _strFilename ){
		return base.LoadSprite (_strFilename);
	}
}
