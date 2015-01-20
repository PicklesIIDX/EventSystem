/* 
 * Author: Imagos Softworks 2015
 * www.imagossoftworks.com
*/

using UnityEngine;
using System.Collections;
using EventSystem;

[AddComponentMenu("Events/Events/Delay")]
public class EventDelay : EventBase {

	[SerializeField]
	float _delayTime;
	public float DelayTime {
		get { return _delayTime; }
	}

	public override IEnumerator PerformEvent ()
	{
		yield return new WaitForSeconds(_delayTime);
		OnEventComplete();
	}
}
