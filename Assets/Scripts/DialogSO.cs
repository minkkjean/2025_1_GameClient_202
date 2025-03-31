
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "Dialog System/Dialog")]

public class DialogSo : ScriptableObject
{
    public int id;
    public string characterName;
    public string text;
    public int nextId;
    public List<DialogChoiceSO> choices = new List<DialogChoiceSO>();
    public Sprite portrait;

    [Tooltip("�ʻ�ȭ ���ҽ� ���(Resources ���� ���� ���")]
    public string portraitPath;
}
