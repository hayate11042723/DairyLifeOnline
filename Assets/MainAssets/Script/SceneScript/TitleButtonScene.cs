using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchToGameScene()
    {
        // �Q�[����ʃV�[���ɑJ��
        SceneManager.LoadScene("CityScene");
    }
}
