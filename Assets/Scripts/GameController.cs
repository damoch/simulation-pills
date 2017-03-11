using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {

        public double DefaultFoodValue;
        public double DefaultWaterValue;
        public Dictionary<NeedType, double> DefaultNeedVaules { get; set; }
        private List<PillAi> Pills { get; set; }


        private void Start()
        {
            Pills = FindObjectsOfType<PillAi>().ToList();
            DefaultNeedVaules = SetDefaultNeedValues();
            InitializeNeeds();
        }

        private Dictionary<NeedType, double> SetDefaultNeedValues()
        {
            var values = new Dictionary<NeedType, double>
            {
                {NeedType.Food, DefaultFoodValue},
                {NeedType.Water, DefaultWaterValue}
            };


            return values;


        }

        public void InitializeNeeds()
        {
            foreach (var pill in Pills)
            {
                if (pill.Needs == null)
                {
                    pill.Needs = new Dictionary<NeedType, double>();
                }
                foreach (var key in DefaultNeedVaules.Keys)
                {
                    pill.Needs.Add(key, DefaultNeedVaules[key]);
                }
            }
        }
    }
}
