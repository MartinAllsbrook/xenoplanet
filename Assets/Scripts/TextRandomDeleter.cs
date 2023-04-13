using System.Collections;
using UnityEngine;
using TMPro;

public class TextRandomDeleter : MonoBehaviour
{
    [Tooltip("The TextMeshProUGUI component to modify.")]
    [SerializeField] private TextMeshProUGUI textComponent;

    [Tooltip("The time in seconds between each character deletion.")]
    [SerializeField] private float deletionInterval = 0.1f;

    private void Start()
    {
        // Start the deletion coroutine
        StartCoroutine(DeleteRandomCharacters());
    }

    private IEnumerator DeleteRandomCharacters()
    {
        while (true)
        {
            // Loop through each line of the text
            for (int i = 0; i < textComponent.textInfo.lineCount; i++)
            {
                TMP_LineInfo lineInfo = textComponent.textInfo.lineInfo[i];

                // Choose a random character to delete from the line
                int charIndex = Random.Range(lineInfo.firstCharacterIndex, lineInfo.lastCharacterIndex + 1);

                // Replace the character with a space
                textComponent.text = textComponent.text.Remove(charIndex, 1).Insert(charIndex, " ");
            }

            // Wait for the deletion interval
            yield return new WaitForSeconds(deletionInterval);
        }
    }
}
