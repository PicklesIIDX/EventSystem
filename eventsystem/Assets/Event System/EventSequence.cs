/* 
 * Author: Imagos Softworks 2015
 * www.imagossoftworks.com
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace EventSystem {

	public class EventCompleteArgs : System.EventArgs{
		public string EventName;
		public EventCompleteArgs(string eventName){
			EventName = eventName;
		}
	}

	/// <summary>
	/// A class to manage playing a series of events in order
	/// after the required trigger conditions have been met
	/// </summary>
	public class EventSequence : MonoBehaviour {
		/// <summary>
		/// An event that is called when the entire event sequence has been completed.
		/// </summary>
		public event System.EventHandler EventSequenceCompleted;

		/// <summary>
		/// Name of the event for your reference.
		/// </summary>
		[SerializeField]
		string eventName;
		/// <summary>
		/// This will allow events to trigger automatically if an active trigger's Triggered event is called
		/// </summary>
		[SerializeField]
		bool allowAutoTrigger = true;
		/// <summary>
		/// Can this event sequence be repeated again if the same trigger conditions are met?
		/// </summary>
		[SerializeField]
		bool repeatable;
		/// <summary>
		/// When in an event group, the lowest priority event sequences will be called first
		/// </summary>
		[SerializeField]
		int _priority = 0;
		public int Priority {
			get { return _priority; }
		}
		/// <summary>
		/// When in an event group, the RandomWeight will determine the likelyhood of this event being chosen.
		/// </summary>
		[SerializeField]
		int _randomWeight = 1;
		public int RandomWeight {
			get { return _randomWeight; }
		}

		/// <summary>
		/// An array of all triggers that must be true for this event sequence to begin
		/// </summary>
		EventTrigger[] eventTriggers;
		/// <summary>
		/// An array of all events that will be performed in order when this sequence is run.
		/// </summary>
		EventBase[] events;

		/// <summary>
		/// Tracks the current event we are performing in the sequence.
		/// </summary>
		int eventSequenceCount = 0;

		/// <summary>
		/// Returns true if we are currently performing a sequence of events.
		/// </summary>
		bool _eventSequenceRunning;
		public bool EventSequenceRunning {
			get { return _eventSequenceRunning; }
		}

		/// <summary>
		/// When this component starts, it will find all EventBase and EventTrigger
		/// components attached to this game object. We will then listen to their 
		/// complete and trigger events so we know when to progress or start this sequence.
		/// </summary>
		void Start () {
			// Find all event components on this game object.
			events = GetComponents<EventBase>();
			// Sort the events based on their 'Order' variable. 1 goes before 2.
			System.Array.Sort<EventBase>(events, (a, b) => a.Order.CompareTo(b.Order));
			for(int i = 0; i < events.Length; i ++){
				// track when any event is completed
				events[i].EventCompleted += HandleEventCompleted;
			}

			// find all trigger components on this game object.
			eventTriggers = GetComponents<EventTrigger>();
			for(int i = 0; i < eventTriggers.Length; i ++){
				eventTriggers[i].Triggered += HandleTriggered;
			}
		}

		/// <summary>
		/// Stop listening to the events so we don't leave memory
		/// allocated.
		/// </summary>
		void OnDestroy(){
			for(int i = 0; i < events.Length; i ++){
				events[i].EventCompleted -= HandleEventCompleted;
			}
			for(int i = 0; i < eventTriggers.Length; i ++){
				eventTriggers[i].Triggered -= HandleTriggered;
			}
		}

		/// <summary>
		/// When an active Trigger's conditions are met, this method is called
		/// to begin the event sequence if allowAutoTrigger is true
		/// </summary>
		/// <param name="sender">The EventTrigger that was passed.</param>
		void HandleTriggered (object sender, System.EventArgs e)
		{
			// don't let us double trigger events while a sequence is running
			// by checking if we are already running this event
			if(!_eventSequenceRunning && allowAutoTrigger){
				CheckTriggersAndPerfomEvents();
			}
		}

		/// <summary>
		/// A method to see how many of the triggers have had thier 
		/// conditions met.
		/// </summary>
		/// <returns>The number of triggers passed.</returns>
		public int CheckTriggersMet(){
			int triggersMet = 0;
			for(int i = 0; i < eventTriggers.Length; i ++){
				if(eventTriggers[i].ConditionsMet()){
					triggersMet ++;
				}
			}
			return triggersMet;
		}

		/// <summary>
		/// Returns true if all triggers currently have thier conditions met.
		/// </summary>
		/// <returns><c>true</c>, if all triggers met was checked, <c>false</c> otherwise.</returns>
		public bool CheckAllTriggersMet(){
			for(int i = 0; i < eventTriggers.Length; i ++){
				if(!eventTriggers[i].ConditionsMet()){
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Will check if all triggers have been met, and if so, will start the event sequence.
		/// </summary>
		/// <returns><c>true</c>, if triggers and perfom events was checked, <c>false</c> otherwise.</returns>
		public bool CheckTriggersAndPerfomEvents(){
			if(eventTriggers == null){
				return true;
			}
			for(int i = 0; i < eventTriggers.Length; i ++){
				if(!eventTriggers[i].ConditionsMet()){
					return false;
				}
			}
			StartCoroutine(PerformEvents());
			return true;
		}

		/// <summary>
		/// Starts the event sequence without checking triggers.
		/// </summary>
		public void ForcePerformEvents(){
			StartCoroutine(PerformEvents());
		}

		/// <summary>
		/// A coroutine that will perform each event in sequence. 
		/// </summary>
		/// <returns>The events.</returns>
		IEnumerator PerformEvents(){
			if(eventSequenceCount > events.Length){
				Debug.LogError("you don't have enough events to perform the next event!");
				yield break;
			}
			_eventSequenceRunning = true;
			// perform events in order, 
			// waiting till each event completes
			Debug.Log("[EventSequence.cs]: Performing event " + events[eventSequenceCount].GetType() + " of sequence " + eventName);
			StartCoroutine(events[eventSequenceCount].PerformEvent());
		}
		
		/// <summary>
		/// Called when an event in the sequence is completed. This will increment the eventSequenceCounter
		/// and either call the next event (if there is one) or end the sequence.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void HandleEventCompleted (object sender, System.EventArgs e)
		{
			Debug.Log("[EventSequence.cs]: Completed event " + events[eventSequenceCount].GetType() + " of sequnce " + eventName);
			// increment the event counter
			eventSequenceCount ++;
			// if we have performed as many events as we have, the sequence is complete
			if(eventSequenceCount >= events.Length){
				EventSequenceComplete();
			} else {
				StartCoroutine(PerformEvents());
			}
		}

		/// <summary>
		/// Called after this sequence performs all events. If the event is not repeatable,
		/// we remove all listeners to trigger and event delegates to ensure this sequence
		/// is not called again.
		/// </summary>
		void EventSequenceComplete(){
			Debug.Log ("[EventSequence.cs]: Completed event sequence " + eventName);
			_eventSequenceRunning = false;
			if(repeatable){
				eventSequenceCount = 0;	
			} else {
				OnDestroy();
			}
			if(EventSequenceCompleted != null){
				EventSequenceCompleted(this, new EventCompleteArgs(eventName));
			}
		}
	}

}