using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    [Serializable]
    public class Response
    {
        public bool isSuccess;
        public string notifacation;
        public List<RegionData> data;
    }
    [Serializable]
    public class RegionData
    {
        public int regionId;
        public string Name;
    }
}
