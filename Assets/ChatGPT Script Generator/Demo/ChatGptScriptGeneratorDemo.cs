#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ChatGptScriptGenerator.Demo {
[ExecuteAlways]
public class ChatGptScriptGeneratorDemo : MonoBehaviour {
    public Text status;
    public Text statusDescription;

    [Space]
    public Color successColor = Color.green;
    public Color failureColor = Color.red;
    public Color warningColor = Color.yellow;

    [Space]
    public MeshRenderer meshRenderer;

    private bool IsApiKeyOk => !string.IsNullOrEmpty(Settings.instance.apiKey);

    private bool _testQuerySent;
    private bool _testQueryFinished;

    private void OnEnable() {
        if (meshRenderer != null) {
            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Material defaultMaterial = primitive.GetComponent<MeshRenderer>().sharedMaterial;
            DestroyImmediate(primitive);
            meshRenderer.sharedMaterial = defaultMaterial;
        }
        
        _testQuerySent = false;
        _testQueryFinished = false;
    }

    private void Update() {
        if (!_testQueryFinished) {
            RunTest();
        }
    }

    private void RunTest() {
        if (!_testQuerySent) {
            status.text = IsApiKeyOk ? "API Key Entered" : "API Key Missing";
            status.color = IsApiKeyOk ? successColor : failureColor;
            statusDescription.text = IsApiKeyOk
                ? ""
                : "Please set your OpenAI API key in the ChatGPT Script Generator settings.";
            statusDescription.color = IsApiKeyOk ? successColor : failureColor;

            if (!IsApiKeyOk) return;
        }

        if (EditorApplication.isCompiling) {
            status.text = "Unity is compiling...";
            status.color = warningColor;
            statusDescription.text = "";
            return;
        }

        if (string.IsNullOrEmpty(CloudProjectSettings.userName) || CloudProjectSettings.userName == "anonymous") {
            status.text = "Unity account not matching OpenAI API key.";
            statusDescription.text = "Log in with previous Unity account or re-enter your API key.";
            status.color = warningColor;
            statusDescription.color = warningColor;
            return;
        }

        if (!_testQuerySent) {
            RunTestQuery();
        }
    }

    [ContextMenu("Run Test Query")]
    private void RunTestQuery() {
        _testQuerySent = true;

        status.text = "Testing API connection...";
        statusDescription.text = "";
        status.color = warningColor;

        ChatGptService.GenerateScript("Debug log 'Hello, world!'", 0, 90, null, _ => {
            _testQueryFinished = true;
            status.text = "API Connection OK";
            status.color = successColor;
            statusDescription.text = "";
        }, () => {
            _testQueryFinished = true;
            status.text = "API Connection Failed";
            statusDescription.text = "Press Play to retry the test. If the error persists, follow the " +
                                     "suggestions in the popup window or reach out to us on Discord (button below).";
            status.color = failureColor;
            statusDescription.color = failureColor;
        });
    }

    public void OpenDocumentation() {
        Application.OpenURL("https://discord.gg/GBAeuWC9qS");
    }
}
}

#endif