using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //singleton
    public static GameController instance = null;

    const float width = 3.7f;
    const float height = 7f;


    public float snakeSpeed = 1;

    public BodyPart bodyPrefab = null;
    public GameObject rockPrefab = null;
    public GameObject eggPrefab = null;
    public GameObject goldEggPrefab = null;

    public Sprite tailSprite = null;
    public Sprite bodySprite = null;

    public SnakeHead snakeHead = null;

    public bool alive = true;

    public bool waitingToPlay = true;

    List<Egg> eggs = new List<Egg>();

    int level = 0;
    int noOfEggsForNextLevel = 0;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        Debug.Log("Starting snake game");
        CreateWalls();
        alive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(waitingToPlay)
        {
            foreach(Touch touch in Input.touches)
            {
                if(touch.phase == TouchPhase.Ended)
                {
                    StartGamePlay();
                }
            }
            if (Input.GetMouseButtonUp(0))
                StartGamePlay();
        }
    }

    

    public void GameOver()
    {
        alive = false;
        waitingToPlay = true;
    }

    public void StartGamePlay()
    {
        waitingToPlay = false;
        alive = true;

        KillOldEggs();
        

        LevelUp();
    }

    void LevelUp()
    {
        level++;

        noOfEggsForNextLevel = 4 + (level * 2); //6 for first, next level 8

        snakeSpeed = 1f + (level / 4f);
        if (snakeSpeed > 6) snakeSpeed = 6;

        snakeHead.ResetSnake();
        CreateEgg();
    }

    public void EggEaten(Egg egg)
    {
        noOfEggsForNextLevel--;
        if (noOfEggsForNextLevel == 0)
        {
            LevelUp();
        }
        else if (noOfEggsForNextLevel == 1)
        {
            CreateEgg(true);
        }
        else
            CreateEgg(false);

        eggs.Remove(egg);
        Destroy(egg.gameObject);
    }

    void CreateWalls()
    {
        float z = -1;

        Vector3 start = new Vector3(-width, -height, z);
        Vector3 finish = new Vector3(-width, +height, z);
        CreateWall(start, finish);

         start = new Vector3(+width, -height, z);
         finish = new Vector3(+width, +height, z);
        CreateWall(start, finish);

         start = new Vector3(-width, -height, z);
         finish = new Vector3(+width, -height, z);
        CreateWall(start, finish);

         start = new Vector3(-width, +height, z);
         finish = new Vector3(+width, +height, z);
        CreateWall(start, finish);

    }

    void CreateWall(Vector3 start, Vector3 finish)
    {
        

        float distance = Vector3.Distance(start, finish);
        int noOfRocks = (int)(distance * 3f);
        Vector3 delta = (finish - start) / noOfRocks;

        Vector3 position = start;
        for(int rock=0; rock <= noOfRocks;rock++)
        {
            float rotation = Random.Range(0, 360);
            float scale = Random.Range(1.5f, 2f);
            CreateRock(position, scale, rotation);
            position = position + delta;
        }
    }

    void CreateRock(Vector3 position, float scale, float rotation)
    {
        GameObject rock = Instantiate(rockPrefab, position, Quaternion.Euler(0, 0, rotation));
        rock.transform.localScale = new Vector3(scale, scale, 1);
    }

    void CreateEgg(bool golden= false)
    {
        Vector3 position;
        position.x = -width + Random.Range(1f, (width * 2) - 2f); 
        position.y = -height + Random.Range(1f, (height * 2) - 2f);
        position.z = -1;

        Egg egg = null;

        if(golden)
            egg= Instantiate(goldEggPrefab, position, Quaternion.identity).GetComponent<Egg>();
        else
           egg= Instantiate(eggPrefab, position, Quaternion.identity).GetComponent<Egg>();

        eggs.Add(egg);
    }

    void KillOldEggs()
    {
        foreach(Egg egg in eggs)
        {
            Destroy(egg.gameObject);
        }
        eggs.Clear();
    }
}
