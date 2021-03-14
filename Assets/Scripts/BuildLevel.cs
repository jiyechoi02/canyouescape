using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sys = System;



public class BuildLevel : MonoBehaviour
{
    //Tiles:
    const int WALL = 0;
    const int FLOOR = 1;
    const int BATTERY = 2;
    const int HEALTHPACK = 3;
    //TRAP1                 
    const int SLOWTRAP = 4;
    //TRAP2                 
    const int DMGTRAP = 5;
    const int GHOST = 6;
    const int BAT = 7;
    const int MUMMY = 8;

    public GameObject wall_prefab;
    public GameObject battery_prefab;
    public GameObject healthpack_prefab;
    public GameObject slowtrap_prefab;
    public GameObject dmgtrap_prefab;
    public GameObject ghost_prefab;
    public GameObject waypoint_prefab;
    public GameObject bat_prefab;
    public GameObject mummy_prefab;

    internal int[,] grid = new int[16, 16];
    internal bool[,] visited = new bool[16, 16];
    internal Stack<int[]> coords = new Stack<int[]>();
    internal int length = 16;
    internal int wallHeight = 50;
    internal float batHeight = 0.05f;
    internal float ghostHeight = 0.05f;
    internal float floorHeight = 0.2f;
    internal float batteryHeight = 1.0f;
    internal int startX = 0;
    internal int startY = 0;
    internal int loops = 0;
    public float innerWallOdds = 0.33f;
    public float monsterOdds = 0.05f;
    public int maxGhosts = 3;
    public int maxBats = 4;
    public int maxMummies = 4;
    public int ghostCount = 0;
    public int batCount = 0;
    public int mummyCount = 0;


    //The formula to covert from grid to the unity board is (32x + 16, 32y + 16)
    int scale(int i)
    {
        i++;
        return (32 * (i - 1)) + 16;
    }

    // Start is called before the first frame update
    void Start()
    {
        makeMap();
        if (pathExists(startX, startY)) 
        {
            drawDungeon();
        }
        else
        {
            makeMap();
            Start();
            print("bad map, try again!");
        }
        
    }

    //assign a random, non-wall tile
    void randomTile(int x, int y)
    {
        //add x and y values to monsterOdds to make it more likely to encounter monsters as you progress
        if (Random.Range(0.0f, 1.0f) <= (x + y) * monsterOdds){
            grid[x, y] = Random.Range(1, 9);
        }
        // else exlude monsters from choice   
        else
        {
            grid[x, y] = Random.Range(1, 6);
        }
        //check if there are too many monsters and reroll if needed
        switch (grid[x, y])
        {
            case GHOST:
                if (ghostCount >= maxGhosts)
                {
                    randomTile(x, y);
                }
                else
                {
                    ghostCount++;
                }
                break;
            case BAT:

                if (batCount >= maxBats)
                {
                    randomTile(x, y);
                }
                else
                {
                    batCount++;
                }
                break;
            case MUMMY:
                if (mummyCount >= maxMummies)
                {
                    randomTile(x, y);
                }
                else
                {
                    mummyCount++;
                }
                break;
            default:
                break;
        }
    }

    void makeMap()
    {
        ghostCount = 0;
        batCount = 0;
        mummyCount = 0;

        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < length; y++)
            {
                
                visited[x, y] = false;

                //start and end spaces must be clear
                if ((x == 0 && y == 0) || (x == length - 1 && y == length - 1))
                {
                    grid[x,y] = FLOOR;
                    continue;
                }
                if (Random.Range(0.0f, 1.0f) <= innerWallOdds)
                {
                    grid[x,y] = WALL;
                }
                else
                {
                    randomTile(x, y);
                    
                }

            }
        }
        
    }

    //use DFS to determine if path exists between start (0,0) and goal (15,15)
    bool pathExists(int startx, int starty)
    {
        int[] s = new int[] { startx, starty };
        //push start node to stack
        coords.Push(s);

        while (coords.Count > 0)
        {
            s = coords.Peek();
            coords.Pop();

            //extract coordinates from the string
            int x = s[0];
            int y = s[1];
            //set node to visited
            visited[x, y] = true;
            print("visited" + s);
            if (visited[length - 1, length - 1])
            {
                print("Valid map!");
                return true;
            }
            //left
            if (x != 0 && grid[x - 1, y] != WALL && !visited[x - 1, y])
            {
                int[] z = new int[] { x - 1, y };
                coords.Push(z);
            }
            //right
            if (x != length - 1 && grid[x + 1, y] != WALL && !visited[x + 1, y])
            {
                int[] z = new int[] { x + 1, y };
                coords.Push(z);
            }
            //down
            if (y != 0 && grid[x, y - 1] != WALL && !visited[x, y - 1])
            {
                int[] z = new int[] { x, y - 1 };
                coords.Push(z);
            }
            //up
            if (y != length - 1 && grid [x, y + 1] != WALL && !visited[x, y + 1])
            {
                int[] z = new int[] { x, y + 1 };
                coords.Push(z);
            }

            loops++;
            if (loops > 10000)
            {
                print("too many loops in DFS!");
                return false;
            }
        }

        //check if goal node was visited
        print("invalid map!");
        return false;
    }


    void drawDungeon()
    {
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < length; y++)
            {
                switch (grid[x,y])
                {
                    case WALL:
                        Instantiate(wall_prefab, new Vector3(scale(x), wallHeight, scale(y)), Quaternion.identity);
                        break;
                    case FLOOR:
                        Instantiate(waypoint_prefab, new Vector3(scale(x), batteryHeight, scale(y)), Quaternion.identity);
                        break;
                    case BATTERY:
                        Instantiate(battery_prefab, new Vector3(scale(x), batteryHeight, scale(y)), Quaternion.identity);
                        break;
                    case HEALTHPACK:
                        Instantiate(healthpack_prefab, new Vector3(scale(x), floorHeight, scale(y)), Quaternion.identity);
                        break;
                    case SLOWTRAP:
                        Instantiate(slowtrap_prefab, new Vector3(scale(x), floorHeight, scale(y)), Quaternion.identity);
                        break;
                    case DMGTRAP:
                        Instantiate(dmgtrap_prefab, new Vector3(scale(x), floorHeight, scale(y)), Quaternion.identity);
                        break;
                    case GHOST:
                        Instantiate(ghost_prefab, new Vector3(scale(x), ghostHeight, scale(y)), Quaternion.identity);
                        break;
                    case BAT:
                        Instantiate(bat_prefab, new Vector3(scale(x), batHeight, scale(y)), Quaternion.identity);
                        break;
                    case MUMMY:
                        Instantiate(mummy_prefab, new Vector3(scale(x), 1, scale(y)), Quaternion.identity);
                        break;
                    default:
                        print("Something broke in the map builder");
                        break;
                }

            }
        }
    }
}
