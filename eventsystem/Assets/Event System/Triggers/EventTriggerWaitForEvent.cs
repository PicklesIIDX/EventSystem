/* 
 * Author: Imagos Softworks 2015
 * www.imagossoftworks.com
*/

using UnityEngine;
using System.Collections;
using EventSystem;

[AddComponentMenu("Events/Triggers/Wait For Event")]
public class EventTriggerWaitForEvent : EventTrigger {

	[SerializeField]
	string _eventName;
	public string EventName {
		get { return _eventName; }
	}

	void Start(){
		EventMessageReceiver.Instance.EventMessaged += HandleEventMessaged;
	}

	void OnDestroy(){
		EventMessageReceiver.Instance.EventMessaged -= HandleEventMessaged;
	}

	void HandleEventMessaged (object sender, System.EventArgs e)
	{
		StringArg stringArg = e as StringArg;
		if(stringArg.Text == _eventName){
			OnTrigger();
		}
	}
}
