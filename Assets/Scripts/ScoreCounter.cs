﻿// Author(s): Paul Calande

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class keeps track of the player's score and updates the UI accordingly.
public class ScoreCounter : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference to the Grid.")]
    Grid grid;
    [SerializeField]
    [Tooltip("Reference to the Score UI text.")]
    Text textScore;
    [SerializeField]
    [Tooltip("Reference to the Rising Text prefab")]
    GameObject risingTextPrefab;

    // The current score.
    int score = 0;

    private void Start()
    {
        grid.SquareFormed += Grid_SquareFormed;
        UpdateScore();
    }

    private void OnDestroy()
    {
        grid.SquareFormed -= Grid_SquareFormed;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScore();
    }

    public void RemoveScore(int amount)
    {
        score -= amount;
        UpdateScore();
    }

    private void UpdateScore()
    {
        textScore.text = "Score: " + score;
    }

    private void Grid_SquareFormed(int size, Vector3 textPos)
    {
        AddScore(size * 100);
        GameObject risingTextObj = Object.Instantiate(risingTextPrefab, grid.transform, false);
        risingTextObj.GetComponent<Text>().text = (size * 100).ToString();
        risingTextObj.transform.localPosition = textPos;
    }
}