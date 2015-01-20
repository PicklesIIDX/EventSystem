/* 
 * Author: Imagos Softworks 2015
 * www.imagossoftworks.com
*/

using UnityEngine;
using System.Collections;
using EventSystem;

public class EventCallEventMessage : EventBase {

	[SerializeField]
	string eventMessage;

	public override IEnumerator PerformEvent ()
	{
		EventMessageReceiver.Instance.MessageEvent(eventMessage);
		OnEventComplete();
		yield return 0;
	}
}
