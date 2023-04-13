using UnityEngine;
using TMPro;

public class TextDeleter : MonoBehaviour
{
    [Tooltip("The TextMeshProUGUI object to delete characters from.")]
    public TextMeshProUGUI textObject;

    [Tooltip("The time in seconds between each character deletion.")]
    public float deletionInterval = 0.1f;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= deletionInterval)
        {
            timer = 0f;

            for (int i = 0; i < textObject.textInfo.lineCount; i++)
            {
                TMP_LineInfo lineInfo = textObject.textInfo.lineInfo[i];
                int startIndex = lineInfo.firstCharacterIndex;
                int endIndex = lineInfo.lastCharacterIndex;

                if (startIndex <= endIndex)
                {
                    textObject.text = textObject.text.Remove(startIndex, 1);
                }
            }
        }
    }
}
