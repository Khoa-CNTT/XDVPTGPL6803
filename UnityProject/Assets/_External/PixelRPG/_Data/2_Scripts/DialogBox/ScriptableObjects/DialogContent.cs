using System.Collections.Generic;
using UnityEngine;

// Lớp DialogContent để chứa tên nhân vật và nội dung nói
[CreateAssetMenu(fileName = "NewDialogContent", menuName = "Scriptable Objects/DialogContent")]
public class DialogContent : ScriptableObject
{
    public List<DialogLine> dialogLines;
}



