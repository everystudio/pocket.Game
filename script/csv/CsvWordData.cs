using UnityEngine;
using System.Collections;


public class CsvWordDataParam : CsvDataParam
{
	public string key { get; private set; }
	public string word { get; private set; }
}

public class CsvWordData : CsvData<CsvWordDataParam> {

	private static readonly string FilePath = "csv/japanese";
	public void Load() { Load(FilePath); }
}


