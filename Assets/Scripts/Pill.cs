using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts
{
    public class Pill  {
        public List<NeedFullFiller> KnownNeedFullFillers { get; set; }
        public Dictionary<NeedType, double> Needs { get; set; }
        public List<PillAi> Friends { get; set; }
    }
}
