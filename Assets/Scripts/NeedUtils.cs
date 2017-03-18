using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.Scripts
{
    public class NeedUtils : MonoBehaviour
    {
        public static void InitializeNeeds(PillAi pillAI)
        {
            var path = "Assets\\Resources\\Needs.xml";
            var reader = new XmlTextReader(path);
            var serializer = new XmlSerializer(typeof(Needs));
            var defaults = (Needs)serializer.Deserialize(reader);
            foreach (var need in defaults.Need)
            {
                pillAI.Pill.Needs[need.NeedType] = need.Value;
            }
        }
    }
    [XmlRoot(ElementName = "need")]
    public class Need
    {
        [XmlElement(ElementName = "need_type")]
        public NeedType NeedType { get; set; }
        [XmlElement(ElementName = "value")]
        public double Value { get; set; }
    }

    [XmlRoot(ElementName = "needs")]
    public class Needs
    {
        [XmlElement(ElementName = "need")]
        public List<Need> Need { get; set; }
    }
}