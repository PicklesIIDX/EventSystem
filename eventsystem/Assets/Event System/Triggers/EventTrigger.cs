/* 
 * Author: Imagos Softworks 2015
 * www.imagossoftworks.com
*/

using UnityEngine;
using System.Collections;

namespace EventSystem {
	/// <summary>
	/// Base class for Event Triggers. Extend this class to add
	/// a new condition for an event sequence to be performed.
	/// </summary>
	[AddComponentMenu("Events/Triggers/Trigger Base")]
	public class EventTrigger : MonoBehaviour {

		public event System.EventHandler Triggered;

		/// <summary>
		/// Allows you to manually check if the conditions of this trigger have been met.
		/// Triggers that require this call are 'passive' triggers.
		/// </summary>
		/// <returns><c>true</c>, if conditions are met, <c>false</c> otherwise.</returns>
		public virtual bool ConditionsMet(){
			return true;
		}

		/// <summary>
		/// This function is to be called by sub classes when conditions are
		/// met. Calling this function makes this an 'active' trigger.
		/// </summary>
		protected void OnTrigger(){
			if(Triggered != null){
				Triggered(this, new System.EventArgs());
			}
		}
	}
}
