using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        private List<PillAi> Pills { get; set; }


        private void Start()
        {
            Pills = FindObjectsOfType<PillAi>().ToList();
        }

    }
}
