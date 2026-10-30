using Assets.Script.Actions;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Script.Movements
{
    public class Job : MonoBehaviour
    {
        //Can put this script in statistiques?
        private PlayerAction playAction;
        private NpcAction npcAction;
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
            playAction = GetComponent<PlayerAction>();
            npcAction = GetComponent<NpcAction>();

            //Get the base stats of the class
        }

        private void GetBaseStats()
        {

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

        public void NpcBehaviorForJob()
        {
            if (!playAction.isActing)
            {
                playAction.isActing = true;
                var res = npcAction.GetBestAction(job);
                switch (job)
                {
                    case Jobs.Tank:
                        NpcTankBehavior(res);
                        break;
                    case Jobs.Dps:
                        NpcDpsBehavior(res);
                        break;
                    case Jobs.Healer:
                        NpcHealerBehavior(res);
                        break;
                    case Jobs.Troubadour:
                        NpcTroubadourBehavior(res);
                        break;
                }
            }
        }


        public void TankBehavior(/*Get value of the atq*/)
        {
            //BaseAttack = u
            if (Input.GetButtonUp("BaseAttack") && playAction.mainCamera.targetPosition == gameObject)
            {
                //CHange value to stats.something
                playAction.SimpleAttack(stats.detectRange, stats.atq);
                //playAction.SimpleAttack(5f, 30);
            }
            //BaseAttack2 = i
            if (Input.GetButtonUp("SecondAttack") && playAction.mainCamera.targetPosition == gameObject)
            {
                //CHange that to def for the tank
                //action.Grab(5f, 5);
                playAction.Aggro(stats.detectRange, stats.atq);
                //playAction.Aggro(10f, 0);
                Debug.Log("Def");
            }
            //Special = o
            if (Input.GetButtonUp("Special") && playAction.mainCamera.targetPosition == gameObject)
            {
                StartCoroutine(playAction.Grab(stats.detectRange, stats.atq));
                //StartCoroutine(playAction.Grab(5f, 5));
            }
        }

        public void HealerBehavior()
        {
            //BaseAttack = u
            if (Input.GetButtonUp("BaseAttack") && playAction.mainCamera.targetPosition == gameObject)
            {
                //CHange value to stats.something
                playAction.SimpleAttack(stats.detectRange, stats.atq);
                //playAction.SimpleAttack(5f, 5);
            }
            //BaseAttack2 = i
            if (Input.GetButtonUp("SecondAttack") && playAction.mainCamera.targetPosition == gameObject)
            {
                playAction.Heal(stats.detectRange, stats.atq);
                //playAction.Heal(5f, 50);
            }
            //Special = o
            if (Input.GetButtonUp("Special") && playAction.mainCamera.targetPosition == gameObject)
            {
                playAction.HealAround(stats.detectRange, stats.atq);
                //playAction.HealAround(3f, 15);
            }
        }

        public void DpsBehavior()
        {
            //BaseAttack = u
            if (Input.GetButtonUp("BaseAttack") && playAction.mainCamera.targetPosition == gameObject)
            {
                playAction.SimpleAttack(stats.detectRange, stats.atq);
                //playAction.SimpleAttack(5f, 50);
            }
            //BaseAttack2 = i
            if (Input.GetButtonUp("SecondAttack") && playAction.mainCamera.targetPosition == gameObject)
            {
                playAction.DealAOEDamages(stats.detectRange, stats.atq);
                //playAction.DealAOEDamages(5f, 25);
            }
            //Special = o
            if (Input.GetButtonUp("Special") && playAction.mainCamera.targetPosition == gameObject)
            {
                playAction.PoisonAttack(stats.detectRange, 3);
                //playAction.PoisonAttack(5f, 3);
            }
        }

        public void TroubadourBehavior()
        {
            //Trouver un moyen de pouvoir changer ses compétences
            //BaseAttack = u
            if (Input.GetButtonUp("BaseAttack") && playAction.mainCamera.targetPosition == gameObject)
            {
                playAction.SimpleAttack(stats.detectRange, stats.atq);
                //playAction.SimpleAttack(5f, 50);
            }
            //BaseAttack2 = i
            if (Input.GetButtonUp("SecondAttack") && playAction.mainCamera.targetPosition == gameObject)
            {
                playAction.HealAround(stats.detectRange, stats.atq);
                //playAction.HealAround(2f, 5);
            }
            //Special = o
            if (Input.GetButtonUp("Special") && playAction.mainCamera.targetPosition == gameObject)
            {
                StartCoroutine(playAction.Grapple(stats.detectRange, stats.atq));
                //StartCoroutine(playAction.Grapple(5f, 5));
            }
        }

        public void NpcHealerBehavior(int res)
        {
            transform.LookAt(npcAction.target.transform.position);
            switch (res)
            {
                case 0:
                    playAction.SimpleAttack(stats.detectRange, stats.atq);
                    //playAction.SimpleAttack(5f, 5);
                    break;
                case 1:
                    playAction.Heal(stats.detectRange, stats.atq);
                    //playAction.Heal(5f, 50);
                    break;
                case 2:
                    playAction.HealAround(stats.detectRange, stats.atq);
                    //playAction.HealAround(3f, 15);
                    break;
            }
        }

        public void NpcTankBehavior(int test)
        {
            transform.LookAt(npcAction.target.transform.position);
            switch (test)
            {
                case 0:
                    Debug.Log(test);
                    playAction.SimpleAttack(stats.detectRange, stats.atq);
                    //playAction.SimpleAttack(5f, 30);
                    break;
                case 1:
                    //check if i can do something similar for the npc
                    Debug.Log("aggro");
                    playAction.SimpleAttack(stats.detectRange, stats.atq);
                    //playAction.Aggro(10f, 0);
                    break;
                case 2:
                    Debug.Log(test);
                    StartCoroutine(playAction.Grab(5f, 5));
                    //StartCoroutine(playAction.Grab(5f, 5));
                    break;
            }
        }

        public void NpcDpsBehavior(int test)
        {
            transform.LookAt(npcAction.target.transform.position);
            switch (test)
            {
                case 0:
                    playAction.SimpleAttack(5f, 50);
                    //playAction.SimpleAttack(5f, 50);
                    break;
                case 1:
                    playAction.DealAOEDamages(5f, 25);
                    //playAction.DealAOEDamages(5f, 25);
                    break;
                case 2:
                    playAction.PoisonAttack(5f, 3);
                    //playAction.PoisonAttack(5f, 3);
                    break;
            }
        }

        public void NpcTroubadourBehavior(int test)
        {
            transform.LookAt(npcAction.target.transform.position);
            switch (test)
            {
                case 0:
                    playAction.SimpleAttack(5f, 50);
                    //playAction.SimpleAttack(5f, 50);
                    break;
                case 1:
                    playAction.HealAround(2f, 5);
                    //playAction.HealAround(2f, 5);
                    break;
                case 2:
                    StartCoroutine(playAction.Grapple(5f, 5));
                    //StartCoroutine(playAction.Grapple(5f, 5));
                    break;
            }
        }


    }
}