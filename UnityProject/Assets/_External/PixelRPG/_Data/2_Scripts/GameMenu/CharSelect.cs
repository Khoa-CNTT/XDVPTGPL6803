using UnityEngine;

public class CharSelect : MonoBehaviour
{
    public GameObject malePrefab;
    public GameObject femalePrefab;
    private static string selectedCharacter; 

    public void SelectMale()
    {
        selectedCharacter = "Male";
        Debug.Log("Male selected.");
    }

    public void SelectFemale()
    {
        selectedCharacter = "Female";
        Debug.Log("Female selected.");
    }

    public void StartGame()
    {
        if (string.IsNullOrEmpty(selectedCharacter))
        {
            Debug.LogWarning("No character selected!");
            return;
        }
    }

    public static string GetSelectedCharacter()
    {
        return selectedCharacter;
    }
}