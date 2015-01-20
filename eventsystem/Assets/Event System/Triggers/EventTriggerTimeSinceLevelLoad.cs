/* 
 * Author: Imagos Softworks 2015
 * www.imagossoftworks.com
*/

using UnityEngine;
using System.Collections;
using EventSystem;

[AddComponentMenu("Events/Triggers/Time Since Level Load")]
public class EventTriggerTimeSinceLevelLoad : EventTrigger {

	[SerializeField]
	float _timeSinceLevelLoad;
	public float TimeSinceLevelLoad{
		get { return _timeSinceLevelLoad; }
	}
	bool triggered = false;

	void Update(){
		if(!triggered && Time.timeSinceLevelLoad >= _timeSinceLevelLoad){
			triggered = true;
			OnTrigger();
		}
	}
}
