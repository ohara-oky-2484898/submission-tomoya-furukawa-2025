using UnityEngine;
using UnityEngine.SceneManagement;

public class OnCollision_SwichScene : MonoBehaviour
{
    public string tagName;   // �ڕW�^�O
    public string sceneName; // �V�[����

    private void OnTriggerEnter(Collider other)
    {
        // �Փ˂������̂��ڕW�^�O��������
        if (other.gameObject.tag == tagName)
        {
            if (sceneName != "")
            {
                // �V�[����������ΐ؂�ւ���
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                // �V�[�������Ȃ���Ύ��̃V�[���֐؂�ւ���
                int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (nextIndex < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(nextIndex);
                }
                else
                {
                    // ���̃V�[�����Ȃ���΍ŏ��̃V�[����
                    SceneManager.LoadScene(0);
                }
            }
        }
    }
}
