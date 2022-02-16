using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public Text ScoreText;
    public Button TitleButton;
    //プレイヤー親
    public GameObject PlayersParent;

    //プレイヤー
    GameObject[] _players;

    int[] _points;
    int[] _player;

    const int PL = 4;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーモデルを取得
        _players = new GameObject[PlayersParent.transform.childCount];
        for (int i = 0; i < PlayersParent.transform.childCount; i++)
        {
            _players[i] = PlayersParent.transform.GetChild(i).gameObject;
        }

        if (MainGameData.Instance != null)
        {
            MainGameData.SMainGameData mainGameData = MainGameData.Instance.SMainData;

            _points = new int[PL];
            for (int i = 0; i < mainGameData.Points.Length; i++)
            {
                _points[i] = mainGameData.Points[i];
            }

            Destroy(MainGameData.Instance);
        }
        //デバッグ用
        else
        {
            _points = new int[PL] { 100, 50, 200, 10 };

        }

        _player = new int[] { 1, 2, 3, 4 };

        //ポイントとプレイヤーを順位で並べ替え
        for (int i = 0; i < PL; i++)
        {
            for (int j = PL - 1 - i; j > 0; j--)
            {
                if (_points[j] > _points[j - 1])
                {
                    int tmp = _points[j];
                    int tmpRank = _player[j];
                    _points[j] = _points[j - 1];
                    _player[j] = _player[j - 1];
                    _points[j - 1] = tmp;
                    _player[j - 1] = tmpRank;
                }
            }
        }

        ScoreText.text = "";

        StartCoroutine(TextUpdateCoroutine());

    }

    // Update is called once per frame
    void Update()
    {
        //最終結果
        //4人並べてポイント表示して、優勝の人が真ん中に移動して祝う演出的な
        //…のをやりたかったが時間がない
    }


    private IEnumerator TextUpdateCoroutine()
    {
        for (int i = 0; i < PL; i++)
        {
            yield return new WaitForSeconds(1.2f);
            TextUpdate(i + 1);
        }

        yield return new WaitForSeconds(0.5f);

        ViewWinnerPlayer();

        yield return new WaitForSeconds(1.0f);

        TitleButton.gameObject.SetActive(true);
    }

    //1位のプレイヤーを表示、手を振るアニメーション
    void ViewWinnerPlayer()
    {
        //取得
        GameObject pl = _players[_player[0] - 1];
        //表示
        pl.SetActive(true);
        //手を振る
        Animator animator = pl.GetComponent<Animator>();
        animator.SetTrigger("Wave");
    }

    //num 1P～
    void TextUpdate(int num)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < num; i++)
        {
            sb.Append($"{i + 1}位：{_player[i]}P　{_points[i]}ポイント");
            if (i < PL - 1)
            {
                sb.Append("\n\n");
            }
        }

        for (int i = 0; i < PL - 1 - num; i++)
        {
            sb.Append("\n\n");
        }

        ScoreText.text = sb.ToString();
    }

    public void TitleButtonClick()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
