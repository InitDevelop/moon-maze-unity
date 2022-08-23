using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{

    public Transform monsterTransform;
    public CharacterController monsterController;
    public MazeGenerationManager gMgr;

    public float speed = 4.5f;
    public float rotationSpeed = 1.0f;

    private const byte STOP = 0;
    private const byte NORTH = 1;
    private const byte EAST = 2;
    private const byte SOUTH = 3;
    private const byte WEST = 4;
    private const byte ROT_RIGHT = 5;
    private const byte ROT_LEFT = 6;
    private const byte MOVE_FORWARD = 7;

    private byte moveState = STOP;

    private float accumulatedMove = 0f;
    private float accumulatedRotation = 0f;

    private int x = 0;
    private int z = 0;

    void Start()
    {
        // Spawn Monster
        float X = (int)UnityEngine.Random.Range(0, gMgr.MAZE_SIZE) + 0.5f;
        float Z = (int)UnityEngine.Random.Range(0, gMgr.MAZE_SIZE) + 0.5f;
        monsterTransform.position = new Vector3(X, 0.1f, Z);
    }

    void Update()
    {
        x = (int)monsterTransform.position.x;
        z = (int)monsterTransform.position.z;

        if (moveState == STOP)
        {
            // Check right-hand side
            if (HasWallRight(x, z, GetFacing()))
            {
                if (HasWallFront(x, z, GetFacing()))
                {
                    moveState = ROT_LEFT;
                }
                else
                {
                    moveState = MOVE_FORWARD;
                }
            }
            else
            {
                moveState = ROT_RIGHT;
            }
        }
        else if (moveState == MOVE_FORWARD)
        {
            if (accumulatedMove < 1.0f)
            {
                float deltaMove = speed * Time.deltaTime;
                monsterController.Move(monsterTransform.forward * deltaMove);
                accumulatedMove += deltaMove;
            }
            else
            {
                accumulatedMove = 0.0f;
                moveState = STOP;
            }
        }
        else if (moveState == ROT_LEFT)
        {
            if (accumulatedRotation < 90.0f)
            {
                float deltaRotation = rotationSpeed * Time.deltaTime;
                monsterTransform.Rotate(0f, -deltaRotation, 0f);
                accumulatedRotation += deltaRotation;
            }
            else
            {
                monsterTransform.Rotate(0f, accumulatedRotation - 90f, 0f);
                accumulatedRotation = 0.0f;

                if (HasWallFront(x, z, GetFacing()))
                {
                    moveState = ROT_LEFT;
                }
                else
                {
                    moveState = MOVE_FORWARD;
                }

            }
        }
        else if (moveState == ROT_RIGHT)
        {
            if (accumulatedRotation < 90.0f)
            {
                float deltaRotation = rotationSpeed * Time.deltaTime;
                monsterTransform.Rotate(0f, deltaRotation, 0f);
                accumulatedRotation += deltaRotation;
            }
            else
            {
                

                monsterTransform.Rotate(0f, 90f - accumulatedRotation, 0f);
                accumulatedRotation = 0.0f;

                Debug.Log(GetFacing());

                if (HasWallFront(x, z, GetFacing()))
                {
                    moveState = ROT_RIGHT;
                }
                else
                {
                    moveState = MOVE_FORWARD;
                }
            }
        }
    }

    // For "facing", +x is SOUTH, +z is EAST
    byte GetFacing()
    {
        int rotY = (int)monsterTransform.localRotation.eulerAngles.y;
        if (rotY < 45 || rotY > 315)
        {
            return EAST;
        }
        else if (45 <= rotY && rotY < 135)
        {
            return SOUTH;
        }
        else if (135 <= rotY && rotY < 215)
        {
            return WEST;
        }
        else
        {
            return NORTH;
        }

    }

    bool HasWallFront(int posX, int posZ, byte heading)
    {
        switch (heading)
        {
            case NORTH:
                if (posX > 0)
                {
                    return gMgr.downWall[posZ, posX - 1];
                }
                else
                {
                    return true;
                }
            case EAST:
                return gMgr.rightWall[posZ, posX];
            case SOUTH:
                return gMgr.downWall[posZ, posX];
            case WEST:
                if (posZ > 0)
                {
                    return gMgr.rightWall[posZ - 1, posX];
                }
                else
                {
                    return true;
                }
        }
        return false;
    }

    bool HasWallRight(int posX, int posZ, byte heading)
    {
        switch (heading)
        {
            case WEST:
                if (posX > 0)
                {
                    return gMgr.downWall[posZ, posX - 1];
                }
                else
                {
                    return true;
                }
            case NORTH:
                return gMgr.rightWall[posZ, posX];
            case EAST:
                if (posX == gMgr.MAZE_SIZE)
                {
                    return true;
                }
                else
                {
                    return gMgr.downWall[posZ, posX];
                }
            case SOUTH:
                if (posZ > 0)
                {
                    return gMgr.rightWall[posZ - 1, posX];
                }
                else
                {
                    return true;
                }
        }
        return false;
    }
    
}
