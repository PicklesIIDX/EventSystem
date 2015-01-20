/* 
 * Author: Imagos Softworks 2015
 * www.imagossoftworks.com
*/

using UnityEngine;
using System.Collections;

namespace EventSystem {
	/// <summary>
	/// Base class for events within a sequence. This is the content
	/// of the event system. When you want an action in your game to be
	/// performed, extend this class and write the functionality within
	/// the PerformEvent coroutine.
	/// </summary>
	[AddComponentMenu("Events/Event Sequence")]
	public class EventBase : MonoBehaviour {

		public event System.EventHandler EventCompleted;

		/// <summary>
		/// The order in which this event will be called within a 
		/// sequence. Lower numbers happen earlier in the sequence.
		/// </summary>
		[SerializeField]
		protected int _order;
		public int Order {
			get { return _order; }
		}

		/// <summary>
		/// Internal variable to use when waiting on another callback
		/// before calling the EventComplete delegate.
		/// </summary>
		protected bool waitingForEventResponse;

		/// <summary>
		/// if this is true then this event will not call its Completed delegate
		/// until the entire event is completed. Leave this false to have
		/// the next event in a sequence trigger simultaneously.
		/// </summary>
		[SerializeField]
		protected bool waitForEventComplete = true;

		/// <summary>
		/// Override this function in your subclass and place the code which
		/// actually performs the function of you event here.
		/// </summary>
		/// <returns>The event.</returns>
		public virtual IEnumerator PerformEvent(){
			OnEventComplete();
			yield return 0;
		}

		/// <summary>
		/// Call this function when this event has completed 
		/// its task.
		/// </summary>
		protected void OnEventComplete(){
			if(EventCompleted != null){
				EventCompleted(this, new System.EventArgs());
			}
		}
	}
}
