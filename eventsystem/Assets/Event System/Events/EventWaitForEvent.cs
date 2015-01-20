/* 
 * Author: Imagos Softworks 2015
 * www.imagossoftworks.com
*/

using UnityEngine;
using System.Collections;
using EventSystem;

[AddComponentMenu("Events/Events/Wait For Event")]
public class EventWaitForEvent : EventBase {

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
		if(waitingForEventResponse){
			StringArg stringArg = e as StringArg;
			if(stringArg.Text == _eventName){
				waitingForEventResponse = false;
				OnEventComplete();
			}
		}
	}

	public override IEnumerator PerformEvent ()
	{
		Debug.Log("[EventWaitForEvent.cs]: Set waiting for event: " + _eventName);
		waitingForEventResponse = true;
		yield return 0;
	}
}
