# EventSystem
A simple event system for Unity

This is a minimal event system written in C# for use in Unity projects. The goal is to provide a system which
allows developers without coding experience the ability to create content for your game.

# Installation

Clone (or download) the repository and open the eventsystem project with Unity 4.3 or higher. Older versions may work, but they have been
untested. 

Alternatively, to import into an existing project, just copy the Event System folder in the Assets folder of the
project into your Unity project.

# Usage

This event system is composed of a number of components that allow you to easily add events to your game.

## EventSequence.cs
 This is the main component that will be added to a game object to manage a sequence of events. At scene start
 this will find all EventTrigger and EventBase components on this game object to listen for when to trigger this
 sequence and the events to perform. If there is no EventTrigger component attached this will not perform the
 event sequence.
 
## EventTrigger.cs
 This is the base class for code which will begin an EventSequence or an EventGroupTrigger. Create a class which
 inherits from this class to create your own custom triggers. If the event is passive (will not trigger an event
 sequence when it's true) then place the conditions for it passing within the ConditionsMet() function. If this
 is an active trigger (will trigger an event sequence when it's true) then call the OnTrigger function in your
 sub class when you want the sequence to begin.
 
## EventBase.cs
 This is the base class for the actual events that the sequence performs. Override the PerformEvent coroutine
 and place the code which performs your event in here. Once your event is complete, call the OnEventComplete() 
 function to indicate to the EventSequence to call the next event. The Order variable is used to sort all 
 event components on the EventSequence game object to play in the specified order.
 
## EventMessageReceiver.cs
 This singleton is a way to pass generic messages between code that is not controlled in an event sequence, or 
 to link between different event sequences. For example, if you want an event sequence to begin when the player
 presses a button, you could either write a trigger that listens for the button press, or you could write code
 that uses the EventMessageReceiver.Instance.MessageEvent() function to send a message that an 
 EventTriggerWaitForEvent.cs component is waiting for.
 
## EventGroupTrigger.cs
 This class is used to manage playing multiple event sequences. Attach this component to a game object that is not
 an event sequence. You will have to select a Group Type, assign the event sequences that can be triggered by
 this group and attach any number of EventTrigger components to indicate when to trigger this group. If there are
 no EventTrigger components then this group will not start any sequences.
 Here is a discription of the Group Types:
 PRIORITY - This will perform the highest priority sequence in which all of its triggers are true. (lowest number
 is highest priority, which is set on the event sequence component).
 MOST - This will perform the event sequence with the most number of true triggers.
 INSTANT - This will perform all event sequences simultaneously.
 RANDOM - This will perform a random event sequence which has met all of its trigger conditions. The chance of 
 picking an event can be modified by the event sequence's Random Weight variable.
 SEQUENCE - This will perform every event sequence in order as assigned in the group trigger component. The
 trigger conditions for each event sequence must be true before the sequence will start.
 
# Sample Scenes
 In the project you will find two sample scenes. This is how they are working in detail:
 
## Simple Event Example
 You will find a Event Simple game object in the scene heirarchy. Attached to this is a series of components.
 The Event Sequence component is the piece which drives this sequence. It listens for the triggers attached
 and will perform all attached event components in order when the conditions are met. In this case, the Event 
 Trigger Time Since Level Load is the only trigger component. This is an active trigger that will return true
 when 1 second (as set in the component) has passed since loading the level. Since this is the only trigger,
 when it is true, the events in this sequence will be performed.
 The first event is the Event Debug Log component, which will display a warning level Debug.Log statement to
 the console. The next event is an Event Delay which will wait 1 second. The final event is another Event Debug 
 Log which will display a final warning message to the console. 
 
## Complex Event Example
 This scene takes advantage of the more complex features in this event system. There are 5 relevant game objects 
 in this scene. The Event Complex 01-03 are similar to the previous example, in which they have an event sequence
 that has a trigger and an event that will perform when the trigger is true. The Event Group is a new type of object
 that will trigger a group of event sequences when it's trigger condition (which is when the game has been running
 for 1 second) becomes true. In this setup, it will perform a SEQUENCE, which will perform the events indicated
 in this component in order (in this case Event Complex 01 and 02). 
 You'll notice Event Complex 03 has a new trigger component called Event Trigger Wait For Event, which is listening
 for an event of the name 'Complex 2 Event.' If you look at the Event Complex 02 game object you'll see that it has
 a new event called Event Call Event Message. This will send a generic message which will be the trigger to start
 Event Complex 03. This is sent through the Message Receiver game object which has a Event Message Receiver component
 attached to it. This is a singleton which manages sending generic events easily across classes and handles sending
 our generic message from Event Complex 02 to Event Complex 03.
 
 
