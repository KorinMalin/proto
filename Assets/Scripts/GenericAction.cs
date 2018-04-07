using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public enum GenericActionKind
    {
        Instantiate,
        Enable,
        Disable,
        EnableDisable,
        Destroy,
        PlayParticleSystem,
        PlaySoundEffect,
        StartActionsExecuter
    }

    [System.Serializable]
    public class GenericAction
    {
        // Attributes
        [SerializeField]
        private GenericActionKind actionKind;
        [SerializeField]
        private GameObject target;
        [SerializeField]
        private int doneAmount = 0;
        
        // Constructors
        public GenericAction()
        {
            this.actionKind = GenericActionKind.Instantiate;
            this.target = null;
        }

        public GenericAction(GenericActionKind _actionKind)
        {
            this.actionKind = _actionKind;
        }

        public GenericAction(GenericActionKind _actionKind, GameObject _target)
        {
            this.actionKind = _actionKind;
            this.target = _target;
        }

        // Methods
        public void Execute(Transform master)
        {
            switch(actionKind)
            {
                case GenericActionKind.Instantiate:
                    GameObject.Instantiate(target, master.position, Quaternion.identity, master);
                    break;
                case GenericActionKind.Enable:
                    target.SetActive(true);
                    break;
                case GenericActionKind.Disable:
                    target.SetActive(false);
                    break;
                case GenericActionKind.EnableDisable:
                    target.SetActive(!target.activeSelf);
                    break;
                case GenericActionKind.Destroy:
                    GameObject.Destroy(target);
                    break;
                case GenericActionKind.PlayParticleSystem:
                    if(target.gameObject.GetComponent<ParticleSystem>())
                        target.gameObject.GetComponent<ParticleSystem>().Play();
                    break;
                case GenericActionKind.PlaySoundEffect:
                    if (target.gameObject.GetComponent<AudioSource>())
                        target.gameObject.GetComponent<AudioSource>().Play();
                    break;
                case GenericActionKind.StartActionsExecuter:
                    if (target.gameObject.GetComponent<DoActionsAfterXSeconds>())
                    {
                        target.gameObject.GetComponent<DoActionsAfterXSeconds>().ResetCounter();
                        target.gameObject.GetComponent<DoActionsAfterXSeconds>().StartCountSeconds();
                    }
                    break;
                default:
                    break;
            }

            doneAmount++;
        }

        // Getters & Setters
        public GenericActionKind GetActionKind()
        {
            return actionKind;
        }

        public void SetActionKind(GenericActionKind _actionKind)
        {
            actionKind = _actionKind;
        }

        public GameObject GetTarget()
        {
            return target;
        }

        public void SetTarget(GameObject _target)
        {
            target = _target;
        }

        public bool isDone()
        {
            return (doneAmount > 0);
        }

        public int GetDoneAmount()
        {
            return doneAmount;
        }
    }

}
