/* 
 * Author: Imagos Softworks 2015
 * www.imagossoftworks.com
*/

using UnityEngine;
using System.Collections;

namespace EventSystem {
	/// <summary>
	/// This class manages triggering a series of event sequences in a specific order.
	/// This is useful for picking branching scenarios, or triggering many event sequences
	/// all at once.
	/// </summary>
	[AddComponentMenu("Events/GroupTrigger")]
	public class EventGroupTrigger : MonoBehaviour {

		private enum GroupType{
			NONE,
			PRIORITY, 	// Perform the highest priority sequence who's conditions are met
			MOST,		// Perform the sequence who satisfies the most trigger conditions
			INSTANT,	// Perform all event sequences at once
			RANDOM,		// Perform a random sequence from the list based on weighting
			SEQUENCE	// Perform all sequences in sequence
		}

		/// <summary>
		/// Decides how the event sequences in this group will play out.
		/// Choose from one of the types listed above.
		/// </summary>
		[SerializeField]
		GroupType groupType;
		/// <summary>
		/// The event sequences to be chosen from in this group.
		/// </summary>
		[SerializeField]
		EventSequence[] sequences;

		/// <summary>
		/// The triggers which must be met for this group to begin
		/// event sequences. Works exactly like triggers on an event sequence.
		/// </summary>
		EventTrigger[] groupTriggers;

		/// <summary>
		/// Used for tracking which event sequence we are 
		/// on when chaining events through the SEQUENCE type
		/// </summary>
		int sequenceTypeSequnceCount = 0;

		/// <summary>
		/// Find all EventTrigger components on this game object
		/// and listen to thier active trigger events.
		/// </summary>
		void Start () {
			groupTriggers = GetComponents<EventTrigger>();
			for(int i = 0; i < groupTriggers.Length; i ++){
				groupTriggers[i].Triggered += HandleTriggered;
			}
		}

		/// <summary>
		/// Stop listeninig to events when we destroy this object
		/// so as to not cause a memory leak.
		/// </summary>
		void OnDestroy(){
			for(int i = 0; i < groupTriggers.Length; i ++){
				groupTriggers[i].Triggered -= HandleTriggered;
			}
		}

		/// <summary>
		/// Called when active trigger conditions are met.
		/// </summary>
		/// <param name="sender">The EventTrigger which has had its conditions met.</param>
		void HandleTriggered (object sender, System.EventArgs e)
		{
			for(int i = 0; i < groupTriggers.Length; i ++){
				if(!groupTriggers[i].ConditionsMet()){
					return;
				}
			}
			CheckGroupTriggers();
		}

		/// <summary>
		/// Based on the type of group this is, perfom the event sequences in a specified manner.
		/// </summary>
		void CheckGroupTriggers(){
			switch(groupType){
				// Sorts the event sequences by priority. The first one who's conditions are
				// met will be triggered. No other sequence will be played.
			case GroupType.PRIORITY:
				System.Array.Sort(sequences, (a, b) => a.Priority.CompareTo(b.Priority));
				for(int i = 0; i < sequences.Length; i ++){
					if(sequences[i].CheckTriggersAndPerfomEvents()){
						return;
					}
				}
				break;
				// Goes through all event sequences and plays the sequence with the 
				// most amount of triggers completed. Event sequences with more 
				// triggers will get priority, which mean more specific conditions
				// will be played above less specific ones.
			case GroupType.MOST:
				int triggersMetCount = -1;
				EventSequence highestCountSequence = null;
				for(int i = 0; i < sequences.Length; i ++){
					int count = sequences[i].CheckTriggersMet();
					if(count > triggersMetCount){
						triggersMetCount = count;
						highestCountSequence = sequences[i];
					}
				}
				// we perform the events, even if all triggers haven't been met.
				// we just want the one with the most triggers met, not all
				if(highestCountSequence != null){
					highestCountSequence.ForcePerformEvents();
				}
				break;
				// Plays all sequences who's conditions have been met at the same time.
			case GroupType.INSTANT:
				for(int i = 0; i < sequences.Length; i ++){
					sequences[i].CheckTriggersAndPerfomEvents();
				}
				break;
			case GroupType.RANDOM:
				// randomly chooses an event sequence out of the sequences 
				// who have had all of their trigger conditions met.
				int totalWeight = 0;
				EventSequence[] availableSeqences = new EventSequence[sequences.Length];
				for(int i = 0; i < sequences.Length; i ++){
					if(sequences[i].RandomWeight > 0 && sequences[i].CheckAllTriggersMet()){
						totalWeight += sequences[i].RandomWeight;
						availableSeqences[i] = sequences[i];
					}
				}
				int roll = Random.Range(0, totalWeight+1);
				int weightCount = 0;
				for(int i = 0; i < availableSeqences.Length; i ++){
					if(availableSeqences[i] == null){
						continue;
					}
					weightCount += availableSeqences[i].RandomWeight;
					if(roll <= weightCount){
						availableSeqences[i].ForcePerformEvents();
						return;
					}
				}
				break;
				// Plays all event sequences in order, as listed 
				// on the component.
			case GroupType.SEQUENCE:
				sequenceTypeSequnceCount = 0;
				sequences[sequenceTypeSequnceCount].EventSequenceCompleted += HandleEventSequenceCompleted;
				sequences[sequenceTypeSequnceCount].CheckTriggersAndPerfomEvents();
				break;
			}
		}

		/// <summary>
		/// When using the SEQUENCE type, this will be called when an event sequence
		/// completes, and will trigger the next sequence in this group.
		/// </summary>
		/// <param name="sender">The event sequence</param>
		void HandleEventSequenceCompleted (object sender, System.EventArgs e)
		{
			sequences[sequenceTypeSequnceCount].EventSequenceCompleted -= HandleEventSequenceCompleted;
			sequenceTypeSequnceCount ++;
			if(sequenceTypeSequnceCount < sequences.Length){
				sequences[sequenceTypeSequnceCount].EventSequenceCompleted += HandleEventSequenceCompleted;
				sequences[sequenceTypeSequnceCount].CheckTriggersAndPerfomEvents();
			}
		}


	}
}
