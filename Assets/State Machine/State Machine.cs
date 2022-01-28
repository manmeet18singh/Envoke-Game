using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

#region Class StateMachine
////////////////////////////////////////////////////////////////////////////////
/// Purpose: This class sets up the general state machine functionality that is used by the enemy 
/// AI system. It sets up the general functionality of how the IState Interface will be iterated on.
/// This code has been refactored and updated from Jason Weimann's StateMachine Code.
/// Which can be found here: https://www.youtube.com/watch?v=V75hgcsCGOM
/// With permission given to us by Prof. Rod Moye.
///
/// Author: Manmeet Singh
////////////////////////////////////////////////////////////////////////////////
#endregion

public class StateMachine
{
    private IState mCurrentState;

    private Dictionary<Type, List<Transition>> mTransitionList = new Dictionary<Type, List<Transition>>();
    private List<Transition> mCurrentTransitions = new List<Transition>();
    private List<Transition> mAnyTransitions = new List<Transition>();
    private static List<Transition> mEmptyTransitions = new List<Transition>(0);

    #region Tick ()
    ////////////////////////////////////////////////////////////////////////////////
    ///	Tick():
    ///
    ///	Purpose: This method is called by an enemy to start the State Machine on void Update(). 
    ///	Tick looks for a transition and sets the state to the transition's destination. 
    ///
    ///	Parameters: NONE
    ///
    ///	Return: NONE
    
    public void Tick()
    {
        var transition = GetTransition();
        if (transition != null)
            SetState(transition.To);
        mCurrentState?.Tick();
    }
    #endregion

    #region SetState(IState state)
    ////////////////////////////////////////////////////////////////////////////////
    ///	SetState(IState state):
    ///
    ///	Purpose: Used to update the states when needed. We do sanity checks and make sure the state
    ///	being set correctly turns off the previous, update the current state by getting the list of 
    ///	transitions from the state calling AddTransition. If there's no list yet, just iterate on an
    ///	empty list.
    ///
    ///	Parameters: <IState> <state>: 
    ///				The state that wants to be set to current state 
    ///
    ///	Return: NONE
    ///		

    public void SetState(IState state)
    {
        if (state == mCurrentState)
        {
            return;
        }

        mCurrentState?.OnExit();
        mCurrentState = state;

        // find the list of transitions of the current state and give it to mCurrentTransitions
        mTransitionList.TryGetValue(mCurrentState.GetType(), out mCurrentTransitions);
        if (mCurrentTransitions == null)
        {
            mCurrentTransitions = mEmptyTransitions;
        }
        mCurrentState.OnEnter();
    }
    #endregion

    #region AddTransition(IState from, Istate to, Func<bool> predicate)
    ////////////////////////////////////////////////////////////////////////////////
    ///	AddTransition(IState from, Istate to, Func<bool> predicate):
    ///
    ///	Purpose: Add a transition type into the list of transitions pertaining to the state.
    ///	Also include the condition that the state needs to abide by.
    ///
    ///	Parameters: <IState from> <IState to> <Func<bool> predicate>: 
    ///				<IState from>: The state that wants to add a transition
    ///				<IState to>: The state we want to transition into
    ///             <Func<bool> predicate>: Function that tells us when to transition
    /// 
    ///	Return: NONE 
    ///			

    public void AddTransition(IState from, IState to, Func<bool> predicate)
    {
        //Checking to see if we already have a list of transitions 
        if (mTransitionList.TryGetValue(from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            mTransitionList[from.GetType()] = transitions;
        }

        transitions.Add(new Transition(to, predicate));
    }

    #endregion

    #region AddAnyTransition(IState state, Func<bool> predicate)
    ////////////////////////////////////////////////////////////////////////////////
    ///	AddAnyTransition(IState state, Func<bool> predicate):
    ///
    ///	Purpose: Add to the any transitions list
    ///
    ///	Parameters: <IState state>: State that wants to add the transition
    ///				<Func<bool> predicate>: Function that tells us when to transition
    ///
    ///	Return: NONE 
    
    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        mAnyTransitions.Add(new Transition(state, predicate));
    }
    #endregion

    #region Class Transition
    ////////////////////////////////////////////////////////////////////////////////
    /// Purpose: This class is a data structure that uses a condition, a to and a constructor.
    /// We need this to be the container that holds the info for when to transition and which
    /// state will be transitioned into.
    ///
    /// Author: Manmeet Singh
    ////////////////////////////////////////////////////////////////////////////////
    #endregion
    private class Transition
    {
        public Func<bool> Condition { get; }
        public IState To { get; }

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    #region GetTransition()
    ////////////////////////////////////////////////////////////////////////////////
    ///	GetTransition:
    ///
    ///	Purpose: Grab the transition that will be used by the state machine. 
    ///	NOTE: the order we place the transitions matter.
    ///
    ///	Parameters: NONE
    ///
    ///	Return: <Transition>: return a transition that can be used by Tick();
    ///			
    private Transition GetTransition()
    {
        foreach (var transition in mAnyTransitions)
        {
            if (transition.Condition())
            {
                return transition;
            }
        }

        foreach (var transition in mCurrentTransitions)
        {
            if (transition.Condition())
            {
                return transition;
            }
        }

        return null;
    }
    #endregion
}