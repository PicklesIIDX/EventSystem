/* 
 * Author: Imagos Softworks 2015
 * www.imagossoftworks.com
*/

using UnityEngine;
using System.Collections;
using EventSystem;

[AddComponentMenu("Events/Events/Debug Log")]
public class EventDebugLog : EventBase {

	[SerializeField]
	bool log = true;
	[SerializeField]
	bool warning;
	[SerializeField]
	bool error;
	[SerializeField]
	string text;

	public override IEnumerator PerformEvent ()
	{
		if(log){
			Debug.Log("[EventDebugLog.cs]: " + text);
		}
		if(warning){
			Debug.LogWarning("[EventDebugLog.cs]: " + text);
		}
		if(error){
			Debug.LogError("[EventDebugLog.cs]: " + text);
		}
		yield return 0;
		OnEventComplete();
	}
}
