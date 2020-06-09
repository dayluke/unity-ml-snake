using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour
{
    public int cellSize = 60;
    public float moveForwardTime = 0.5f;
    public List<Transform> tails = new List<Transform>();
    public GameObject bodyPrefab;
    public int foodCollected = 0;
    private Vector2 lastDir = Vector2.up;
    public Text scoreText;

    private void Awake()
    {
        Reset();
    }

    private void Reset()
    {
        CancelInvoke();
        Task.Delay(TimeSpan.FromSeconds(moveForwardTime)).Wait();

        foreach (Transform tail in tails)
        {
            Destroy(tail.gameObject);
        }

        tails.Clear();
        foodCollected = 0;
        UpdateScore();
        transform.position = new Vector2(Screen.width / 2, Screen.height / 2);

        InvokeRepeating("MoveSnake", 0, moveForwardTime);
    }

    private void Update()
    {
        Vector2 newDir = lastDir;

        if (Input.GetKeyDown(KeyCode.W)) newDir = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.A)) newDir = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.S)) newDir = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.D)) newDir = Vector2.right;

        if (tails.Count > 0 && newDir == -lastDir) return;
        lastDir = newDir;
    }

    private void MoveSnake()
    {
        // if snake head > 1 && dir is the opposite to lastInput then return
        Vector2 lastPos = transform.position;

        transform.position += (Vector3)lastDir * cellSize;
        
        if (tails.Count > 0)
        {
            // Move last Tail Element to where the Head was
            tails.Last().position = lastPos;

            // Add to front of list, remove from the back
            tails.Insert(0, tails.Last());
            tails.RemoveAt(tails.Count - 1);
        }
    }

    public void FoodCollected()
    {
        foodCollected++;
        IncreaseSnakeLength();
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = foodCollected.ToString();
    }

    private void IncreaseSnakeLength()
    {
        Vector3 tailPos = Vector3.zero;
        if (tails.Count > 0) tailPos = tails.Last().position;
        else tailPos = this.transform.position;

        GameObject tail = Instantiate(bodyPrefab, tailPos, Quaternion.identity, transform.parent);
        tails.Add(tail.transform);
    }

    public void GameOver()
    {
        Debug.Log("Game over");
        Reset();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Border")
        {
            GameOver();
        }
        else if (coll.gameObject.tag == "Snake Body")
        {
            if (foodCollected > 3) GameOver();
        }
        else if (coll.gameObject.tag == "Food")
        {
            Destroy(coll.gameObject);
            FoodCollected();
            FoodSpawner.instance.SpawnFood();
        }
    }
}