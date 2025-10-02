using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NowMission : MonoBehaviour
{
    [SerializeField] Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.clearNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"ƒ~ƒbƒVƒ‡ƒ“ƒNƒŠƒA”:{GameManager.Instance.clearNum}^{GameManager.Instance.clearborder}\n" +
            $"Ô{GameManager.Instance.numRed}^{GameManager.Instance.numRedMax}\n" +
            $"Â{GameManager.Instance.numBlue}^{GameManager.Instance.numBlueMax}\n" +
            $"‰©{GameManager.Instance.numYellow}^{GameManager.Instance.numYellowMax}\n" +
            $"—Î{GameManager.Instance.numGreen}^{GameManager.Instance.numGreenMax}";
    }
}
