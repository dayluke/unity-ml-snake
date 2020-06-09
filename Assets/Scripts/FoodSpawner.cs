using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public Transform canvasTransform;
    public static FoodSpawner instance;

    private void Awake()
    {
        SpawnFood();
        instance = this;
    }
    
    public void SpawnFood()
    {
        Instantiate(foodPrefab, GetRandomPoint(), Quaternion.identity, canvasTransform);
    }

    private Vector2 GetRandomPoint()
    {
        int borderSize = 30;
        int cellSize = 60;
        int numXCells = (Screen.width - (borderSize * 2)) / cellSize;
        int numYCells = (Screen.height - (borderSize * 2)) / cellSize;

        int randX = (Random.Range(0, numXCells) * cellSize) + (borderSize * 2);
        int randY = (Random.Range(0, numYCells) * cellSize) + (borderSize * 2);
        
        Vector2 foodPos = new Vector2(randX, randY);

        // If there is no spaces left on the board...
        if (GameObject.FindGameObjectsWithTag("Snake Body").Length + 1 >= numXCells * numYCells)
        {
            Debug.Log("Player WON! - No spaces left");
        }

        return foodPos;
    }
}
