using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelGameLevel : MonoBehaviour
{
    [System.Serializable]
    public class ResponseAPI
    {
        public bool isSuccess;
        public string notifacation;
        public List<GameLevel> data;
    }
    [System.Serializable]
    public class GameLevel
    {
        public int levelId;
        public string title;
        public string description;
    }
}
