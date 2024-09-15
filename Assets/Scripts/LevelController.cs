using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    Monster[] _monsters;
    
    [SerializeField] string _levelName;

    void OnEnable()
    {
        _monsters = FindObjectsOfType<Monster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MonsterAreAllDead())
            GoToNextLeve();
    }

    void GoToNextLeve()
    {
        Debug.Log("Go to Level " + _levelName);
        SceneManager.LoadScene(_levelName);
    }

     bool MonsterAreAllDead()
    {
        foreach (var monster in _monsters)
        {
            if (monster.gameObject.activeSelf)
                return false;
        }
        return true;
    }
}
