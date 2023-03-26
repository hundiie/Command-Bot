using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCommand", menuName = "Scriptable Object/PlayerCommand", order = 1)]
public class CommandData : ScriptableObject
{
    [SerializeField]
    int id;
    [SerializeField]
    string commandName;
    [SerializeField]
    Sprite sprite;

    public int Id => id;
    public string CommandName => commandName;
    public Sprite Sprite => sprite;
}
