/* 
 * Author: Imagos Softworks 2015
 * www.imagossoftworks.com
*/

using UnityEngine;
using System.Collections;
using EventSystem;

[AddComponentMenu("Events/Events/Sub Sequence")]
public class EventSubSequence : EventBase {

	[SerializeField]
	EventSequence subSequence;

	// Use this for initialization
	void Start () {
		subSequence.EventSequenceCompleted += HandleEventSequenceCompleted;
	}

	void OnDestroy() {
		subSequence.EventSequenceCompleted -= HandleEventSequenceCompleted;
	}

	void HandleEventSequenceCompleted (object sender, System.EventArgs e)
	{
		OnEventComplete();
	}

	public override IEnumerator PerformEvent ()
	{
		if(subSequence.CheckTriggersAndPerfomEvents()){
			if(!waitForEventComplete){
				OnEventComplete();
			}
		} else {
			OnEventComplete();
		}
		yield return 0;
	}
}
