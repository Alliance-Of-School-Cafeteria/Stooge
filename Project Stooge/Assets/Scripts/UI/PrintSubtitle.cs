using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintSubtitle : MonoBehaviour
{
    /* ----------- 텍스트 관련 변수 ------------ */
    private string fileContent;

    /* ----------- 트리거 판정 변수 ------------ */
    private bool isTrigger = false;

    /* ---------------- 인스펙터 --------------- */
    [Header("오브젝트 연결")]
    [SerializeField]
    private Text textUI;
    [SerializeField]
    private TextAsset textFile;

    [Header("반복 설정")]
    [SerializeField]
    private bool isRepeat = false;

    /* -------------- 이벤트 함수 -------------- */
    private void Start()
    {
        ReadText(textFile);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTrigger && textUI.text == "")
        {
            isTrigger = true;
            StartCoroutine(TextPrint(textUI, fileContent));
        }
    }

    /* --------------- 기능 함수 --------------- */
    private void ReadText(TextAsset file)
    {
        if (file != null)
            fileContent = file.text;
        else
            fileContent = "파일이 없습니다!";
    }

    IEnumerator TextPrint(Text textUI, string textValue)
    {
        //int typingCount = 0; // 줄당 글자 수

        string writeText = "";  // 이번 턴에 출력할 텍스트
        string currentText = ""; // 현재 출력된 텍스트

        for (int i = 0; i < textValue.Length; i++)
        {
            writeText = ""; // 줄 바뀔 때마다 초기화

            // 한 글자씩 타이핑 되는 듯한 효과 
            writeText += textValue[i]; // 문자열 하나씩 끊어서 적기
            textUI.text = currentText + writeText; // 텍스트 UI에 텍스트 표시

            //yield return null;
            yield return new WaitForSeconds(0.1f); // 글자마다 딜레이

            //typingCount++;

            currentText += writeText;

            if (/*typingCount >= 30 || */ writeText.Contains("\n"))
            {
                //currentText += "\n"; // 한 줄 완성 후 줄 바꿈
                //typingCount = 0;
                yield return new WaitForSeconds(0.15f); // 줄 바꾼 후 딜레이
            }
        }

        yield return new WaitForSeconds(0.5f);
        textUI.text = "";

        if (isRepeat)
            isTrigger = false;
    }
}
