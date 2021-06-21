using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameManager gameManager;

    void OnTriggerEnter ()
    {

    }
    void Update()
    {
        if (GameObject.FindObjectOfType<EnemyUnit>() == null)
        {
            gameManager.CompleteLevel();
        }
    }
}
