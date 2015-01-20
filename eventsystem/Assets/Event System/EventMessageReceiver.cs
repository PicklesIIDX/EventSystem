/* 
 * Author: Imagos Softworks 2015
 * www.imagossoftworks.com
*/

using UnityEngine;
using System.Collections;

namespace EventSystem {

	/// <summary>
	/// An event arg which contains a string. This 
	/// is used to pass a message when signaling global
	/// event messages with this singleton.
	/// </summary>
	public class StringArg : System.EventArgs {
		public string Text;
		public StringArg(string text){
			Text = text;
		}
	}
	/// <summary>
	/// A singleton class which can recieve and send events globally.
	/// </summary>
	public class EventMessageReceiver : MonoBehaviour {

		/// <summary>
		/// The public reference to this singleton.
		/// </summary>
		public static EventMessageReceiver Instance;

		/// <summary>
		/// A delegate event which is triggered any time an event is messaged.
		/// </summary>
		public event System.EventHandler EventMessaged;

		/// <summary>
		/// This creates a singleton instance if none exists.
		/// If one does exist, this game object is then destroyed.
		/// </summary>
		void Awake () {
			if(EventMessageReceiver.Instance != null && EventMessageReceiver.Instance != null){
				Destroy(this);
			} else {
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
		}

		/// <summary>
		/// Sends out a string message to all objects listening to the EventMessage
		/// delegate of this singleton.
		/// </summary>
		/// <param name="eventName">string message to be passed.</param>
		public void MessageEvent(string eventName){
			Debug.Log("[EventMessageReciever.cs]: Messaging event: " + eventName);
			if(EventMessaged != null){
				EventMessaged(this, new StringArg(eventName));
			}
		}
	}
}
