using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Utility
{
    public class TriggerDoActions : MonoBehaviour
    {
        [SerializeField]
        private DoActionsAfterXSeconds actionsToDo;
        [SerializeField]
        private bool limitedToOneTime = true;

        private bool done;
            
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                if (done && limitedToOneTime)
                    return;
                if (actionsToDo != null)
                {
                    actionsToDo.ResetCounter();
                    actionsToDo.StartCountSeconds();
                    done = true;
                }
            }
        }
    }
}

