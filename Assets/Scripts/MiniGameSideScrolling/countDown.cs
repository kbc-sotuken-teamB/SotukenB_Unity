using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class countDown : MonoBehaviour
{
	[SerializeField]
	private Text _textCountdown;

	[SerializeField]
	private Image _imageMask;

	[SerializeField]
	bool isCountOver = false;

	void Start()
	{
		_textCountdown.text = "";
	}

	public void OnClickButtonStart()
	{
		StartCoroutine(CountdownCoroutine());
	}

	IEnumerator CountdownCoroutine()
	{
		

		_textCountdown.text = "3";
		yield return new WaitForSeconds(1.0f);

		_textCountdown.text = "2";
		yield return new WaitForSeconds(1.0f);

		_textCountdown.text = "1";
		yield return new WaitForSeconds(1.0f);

		_textCountdown.text = "GO!";
		yield return new WaitForSeconds(1.0f);

		_textCountdown.text = "";
		
		isCountOver = true;
	}
	//trueなら諸々の処理を始めてもろて。
	public bool CountOver()
	{
		return isCountOver;
	}
}