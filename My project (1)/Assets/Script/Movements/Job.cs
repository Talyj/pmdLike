using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Script.Movements
{
    public class Job : MonoBehaviour
    {
        private PlayerAction action;
        private Statistiques stats;
        public enum Jobs
        {
            Tank = 0,
            Dps = 1,
            Healer = 2
        }

        public Jobs job;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}