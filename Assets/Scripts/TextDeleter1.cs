using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDeleter1 : MonoBehaviour
{
    [Tooltip("The TextMeshProUGUI object to delete characters from.")]
    public TextMeshProUGUI textObject;
    [Tooltip("The time in seconds between each character deletion.")]
    public float deletionInterval = 0.1f;
    [Tooltip("The time in seconds before the TextMeshProUGUI object is deleted.")]
    public float deletionTime = 10f;

    private float timer = 0f;
    private float totTime = 0f;

    private bool _waiting = true;
    
    
    void Start()
    {
        if (textObject == null)
        {
            textObject = GetComponent<TextMeshProUGUI>();
        }
    }

    public void Delete()
    {
        _waiting = false;
    }

    void Update()
    {
        if (_waiting)
        {
            return;
        }
        
        timer += Time.deltaTime;
        totTime += Time.deltaTime;

        if (totTime >= deletionTime)
        {
            Debug.Log("Delete");
            textObject.gameObject.SetActive(false);
            Destroy(this);
        }

        if (timer >= deletionInterval)
        {
            timer = 0f;

            for (int i = 0; i < textObject.textInfo.lineCount; i++)
            {
                TMP_LineInfo lineInfo = textObject.textInfo.lineInfo[i];

                if (lineInfo.characterCount > 0)
                {
                    PickAndDelete(lineInfo, 0);
                }
            }
        }


    }

    private void PickAndDelete(TMP_LineInfo lineInfo, int depth)
    {
        int randomCharIndex = Random.Range(lineInfo.firstCharacterIndex, lineInfo.lastCharacterIndex + 1);
        if (textObject.textInfo.characterInfo[randomCharIndex].character != '\n' && textObject.textInfo.characterInfo[randomCharIndex].character != ' ')
        {
            textObject.text = textObject.text.Remove(randomCharIndex, 1).Insert(randomCharIndex, " ");
        }
        else if (depth < 30)
        {
            PickAndDelete(lineInfo, depth + 1);
        }
    }
}
