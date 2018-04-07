using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    [System.Serializable]
    public class DoActionsAfterXSeconds : MonoBehaviour
    {
        // Attributes
        [SerializeField]
        private string actionsName = "Actions List";
        [SerializeField]
        private bool countSeconds = true;
        [SerializeField]
        private float secondsToCount = 3;
        [SerializeField]
        private bool loop = false;
        [SerializeField]
        private List<GenericAction> actions = new List<GenericAction>();

        private float seconds = 0;
        private bool done = false;

        // Update is called once per frame
        void Update()
        {
            if (done && !loop)
                return;
            if (countSeconds)
            {
                seconds += Time.deltaTime;
                if (seconds >= secondsToCount)
                {
                    foreach (GenericAction action in actions)
                        action.Execute(transform);
                    if (loop)
                        seconds = 0;
                    done = true;
                }
            }
        }

        #region Methods
        public void StartCountSeconds()
        {
            done = false;
            seconds = 0;
            countSeconds = true;
        }

        public void StopCountSeconds()
        {
            countSeconds = false;
        }

        public void ResetCounter()
        {
            done = false;
            seconds = 0;
        }
        #endregion

        #region Getters & Setters
        public string GetActionsName()
        {
            return actionsName;
        }

        public void SetActionsName(string _actionsName)
        {
            actionsName = _actionsName;
        }

        public bool GetCountSeconds()
        {
            return countSeconds;
        }

        public void SetCountSeconds(bool _countSeconds)
        {
            countSeconds = _countSeconds;
        }

        public float GetSecondsToCount()
        {
            return secondsToCount;
        }

        public void SetSecondsToCount(float _secondsToCount)
        {
            secondsToCount = _secondsToCount;
        }

        public bool GetLoop()
        {
            return loop;
        }

        public void SetLoop(bool _loop)
        {
            loop = _loop;
        }

        public List<GenericAction> GetActions()
        {
            return actions;
        }

        public void SetActions(List<GenericAction> _actions)
        {
            actions = _actions;
        }
        #endregion
    }

}
