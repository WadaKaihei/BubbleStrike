using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gotitle : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
