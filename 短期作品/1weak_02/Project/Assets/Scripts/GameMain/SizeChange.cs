using UnityEngine;

public class SizeChange : MonoBehaviour
{
    [SerializeField] public float resizeValue;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 nowSize = this.gameObject.transform.localScale;

        Vector3 chengeSize = nowSize * resizeValue;
        this.gameObject.transform.localScale = chengeSize;
    }
}
