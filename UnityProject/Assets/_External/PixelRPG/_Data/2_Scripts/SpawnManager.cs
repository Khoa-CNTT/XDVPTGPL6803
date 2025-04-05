using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPosition;
    public GameObject malePrefab;
    public GameObject femalePrefab;

    void Start()
    {
        GameObject spawnObject = GameObject.Find("SpawnPlayer");
        if (spawnObject != null)
        {
            spawnPosition = spawnObject.transform;
            Debug.Log("SpawnPoint found at: " + spawnPosition.position);
        }
        else
        {
            Debug.LogWarning("SpawnPoint not found!");
            return;
        }
        string selectedCharacter = CharSelect.GetSelectedCharacter();
        if (string.IsNullOrEmpty(selectedCharacter))
        {
            Debug.LogWarning("No character selected in Scene1!");
            return;
        }

        GameObject characterPrefab = selectedCharacter == "Male" ? malePrefab : femalePrefab;

        Instantiate(characterPrefab, spawnPosition.position, Quaternion.identity);
        Debug.Log("Spawned: " + selectedCharacter);
    }
}