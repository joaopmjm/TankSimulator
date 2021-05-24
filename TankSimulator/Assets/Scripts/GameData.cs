using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static int pontos=0;
    private static int armoredDestroyed=0;
    private static int soldiersKilled=0;

    public static int Pontos
    {
        get
        {
            return pontos;
        }
        set
        {
            pontos = value;
        }
    }
    public static int ArmoredDestroyed
    {
        get
        {
            return armoredDestroyed;
        }
        set
        {
            armoredDestroyed = value;
        }
    }
    public static int SoldiersKilled
    {
        get
        {
            return soldiersKilled;
        }
        set
        {
            soldiersKilled = value;
        }
    }
}
