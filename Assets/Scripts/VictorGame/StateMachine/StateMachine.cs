using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace VictorGame.StateMachine
{
    /*
    public class StateMachine<T> where T : System.Enum
    {

        public Dictionary<T, StateBase> dictionaryState;
        public float timeToStartGame = 1f;

        private StateBase _currentState;

        public StateBase CurrentState
        {
            get { return _currentState; }
        }
        //
        public StateMachine(T state)
        {
            
            SwitchState(state);
        }
        //
        public void Init()
        {
            dictionaryState = new Dictionary<T, StateBase>();
        }

        public void RegisterStates(T typeEnum, StateBase state)
        {
            dictionaryState.Add(typeEnum, state);

          
        }

        public void SwitchState(T state)
        {
            if (_currentState != null)
            {
                _currentState.OnStateExit();
            }

            _currentState = dictionaryState[state];
            _currentState.OnStateEnter();
        }

        public void Update()
        {
            if (_currentState != null)
            {
                _currentState.OnStateStay();
            }

        }
    }
    */
    public class StateMachine<T> where T : System.Enum
    {
        public Dictionary<T, StateBase> dictionaryState;
        public float timeToStartGame = 1f;
        private StateBase _currentState;
        public StateBase CurrentState
        {
            get { return _currentState; }
        }
        /*
        public StateMachine(T state)
        {

            SwitchState(state);
        }
        */
        public void Init()
        {
            dictionaryState = new Dictionary<T, StateBase>();
        }
        public void RegisterStates(T typeEnum, StateBase state)
        {
            dictionaryState.Add(typeEnum, state);
        }
        // "o" é o objeto de contexto (ex: o Player) repassado para OnStateEnter.
        // Parâmetro opcional para manter compatibilidade com quem chama sem contexto (ex: GameManager).
        public void SwitchState(T state, object o = null)
        {
            if (_currentState != null)
            {
                _currentState.OnStateExit();
            }
            _currentState = dictionaryState[state];
            _currentState.OnStateEnter(o);
        }
        public void Update()
        {
            if (_currentState != null)
            {
                _currentState.OnStateStay();
            }
        }
    }
    }
