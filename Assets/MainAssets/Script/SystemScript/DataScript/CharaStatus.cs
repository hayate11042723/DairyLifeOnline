using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create StatusData")]
public class CharaStatus : ScriptableObject
{
    public string NAME;     //L¼
    public int MAXHP;       //ÅåHP
    public int MAXMP;       //ÅåMP
    public int ATK;         //UÍ
    public int DEF;         //häÍ
    public int INT;         //Í
    public int RES;         //@ïRÍ
    public int AGI;         //Ú®¬x
    public int LV;          //x
    public int GETEXP;      //æ¾o±l
    public int GETGOLD;     //æ¾Å«é¨à
    public float ShortAttackRange;    //GÌUÍÍ
    public float EnemyTime;   //GÌUÔu
}
