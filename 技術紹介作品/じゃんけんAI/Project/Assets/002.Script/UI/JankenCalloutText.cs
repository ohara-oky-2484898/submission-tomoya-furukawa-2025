/// <summary>
/// Ç∂Ç·ÇÒÇØÇÒÇÃä|ÇØê∫ï\é¶
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JankenCalloutText : MonoBehaviour
{
	[SerializeField] private Text calloutText;

	private void Start()
	{
		calloutText.text = UIManager.Instance.DisplayCallMessage;
	}

	private void Update()
	{
		if(JankenManager.Instance.CurrentState == JankenState.Exit)
		{
			calloutText.gameObject.SetActive(false);
		}
		else
		{
			calloutText.text = UIManager.Instance.DisplayCallMessage;
			calloutText.gameObject.SetActive(true);
		}
	}

}
