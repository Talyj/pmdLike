using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Script.Movements
{
    public class Job : MonoBehaviour
    {
        //Can put this script in statistiques?
        private PlayerAction action;
        private Statistiques stats;

        [HideInInspector] public List<string> skillsList = new List<string>();
        public enum Jobs
        {
            //plus d'xp?
            Troubadour = 0,
            //PLus de vie
            Tank = 1,
            //pLus de dégâts
            Dps = 2,
            //sOin
            Healer = 3
        }

        public Jobs job;

        // Use this for initialization
        void Start()
        {
            stats = GetComponent<Statistiques>();
            action = GetComponent<PlayerAction>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BehaviorForJob()
        {
            switch (job)
            {
                case Jobs.Tank:
                    TankBehavior();
                    break;
                case Jobs.Dps:
                    DpsBehavior();
                    break;
                case Jobs.Healer:
                    HealerBehavior();
                    break;
                default:
                    TroubadourBehavior();
                    break;
            }
        }

        public void TankBehavior(/*Get value of the atq*/)
        {
            //BaseAttack = u
            if (Input.GetButtonUp("BaseAttack") && action.mainCamera.targetPosition == gameObject)
            {
                //CHange value to stats.something
                action.SimpleAttack(5f, 30);
            }
            //BaseAttack2 = i
            if (Input.GetButtonUp("SecondAttack") && action.mainCamera.targetPosition == gameObject)
            {
                //CHange that to def for the tank
                //action.Grab(5f, 5);
                Debug.Log("Def");
            }
            //Special = o
            if (Input.GetButtonUp("Special") && action.mainCamera.targetPosition == gameObject)
            {
                StartCoroutine(action.Grab(5f, 5));
            }
        }

        public void HealerBehavior()
        {
            //BaseAttack = u
            if (Input.GetButtonUp("BaseAttack") && action.mainCamera.targetPosition == gameObject)
            {
                //CHange value to stats.something
                action.SimpleAttack(5f, 5);
            }
            //BaseAttack2 = i
            if (Input.GetButtonUp("SecondAttack") && action.mainCamera.targetPosition == gameObject)
            {
                action.Heal(5f, 50);
            }
            //Special = o
            if (Input.GetButtonUp("Special") && action.mainCamera.targetPosition == gameObject)
            {
                action.HealAround(3f, 15);
            }
        }

        public void DpsBehavior()
        {
            //BaseAttack = u
            if (Input.GetButtonUp("BaseAttack") && action.mainCamera.targetPosition == gameObject)
            {
                action.SimpleAttack(5f, 50);
            }
            //BaseAttack2 = i
            if (Input.GetButtonUp("SecondAttack") && action.mainCamera.targetPosition == gameObject)
            {
                action.DealAOEDamages(5f, 25);
            }
            //Special = o
            if (Input.GetButtonUp("Special") && action.mainCamera.targetPosition == gameObject)
            {
                Debug.Log("cc");
            }
        }

        public void TroubadourBehavior()
        {
            //Trouver un moyen de pouvoir changer ses compétences
            //BaseAttack = u
            if (Input.GetButtonUp("BaseAttack") && action.mainCamera.targetPosition == gameObject)
            {
                Debug.Log("u");
                action.SimpleAttack(5f, 50);
            }
            //BaseAttack2 = i
            if (Input.GetButtonUp("SecondAttack") && action.mainCamera.targetPosition == gameObject)
            {
                Debug.Log("i");
                action.HealAround(2f, 5);
            }
            //Special = o
            if (Input.GetButtonUp("Special") && action.mainCamera.targetPosition == gameObject)
            {
                Debug.Log("o");
                StartCoroutine(action.Grapple(5f, 5));
            }
        }


    }
}