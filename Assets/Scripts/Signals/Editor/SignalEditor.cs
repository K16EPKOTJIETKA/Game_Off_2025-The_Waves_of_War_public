using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SignalSO))]
public class SignalEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SignalSO signal = (SignalSO)target;

        signal.id = (int)EditorGUILayout.IntField("Id", signal.id);
        signal.importance = (int)EditorGUILayout.Slider("Importance", signal.importance, -10, 40);
        signal.formula = (Formula)EditorGUILayout.EnumPopup("Formula", signal.formula);
        signal.necessaryReinforcement = (int)EditorGUILayout.Slider("Necessary Reinforcement", signal.necessaryReinforcement, 1, 100);
        signal.amplitude = EditorGUILayout.Slider("Amplitude", signal.amplitude, 0.1f, 0.5f);
        signal.frequency = (int)EditorGUILayout.Slider("Frequency", signal.frequency, 10f, 250);
        signal.signalType = (SignalType)EditorGUILayout.EnumPopup("Signal Type", signal.signalType);
        signal.demodulationProtocol = (DemodulationProtocol)EditorGUILayout.EnumPopup("Demodulation Protocol", signal.demodulationProtocol);
        signal.decoderCode = (CodeSO)EditorGUILayout.ObjectField("Decoder Code", signal.decoderCode, typeof(CodeSO), false);
        signal.countOfDemodulationCharacters = (int)EditorGUILayout.Slider("Count of Demodulation Characters", signal.countOfDemodulationCharacters, 25, 200);
        signal.noisePower = EditorGUILayout.Slider("Noise", signal.noisePower, 0, 100);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Content", EditorStyles.boldLabel);

        signal.isPicture = EditorGUILayout.Toggle("Is Picture", signal.isPicture);

        if (signal.isPicture)
        {
            EditorGUI.BeginDisabledGroup(false);
            signal.image = (Sprite)EditorGUILayout.ObjectField("Image", signal.image, typeof(Sprite), false);
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(true);
            signal.text = EditorGUILayout.TextArea(signal.text, GUILayout.Height(60));
            EditorGUI.EndDisabledGroup();
        }
        else
        {
            EditorGUI.BeginDisabledGroup(true);
            signal.image = (Sprite)EditorGUILayout.ObjectField("Image", signal.image, typeof(Sprite), false);
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(false);
            signal.text = EditorGUILayout.TextArea(signal.text, GUILayout.Height(60));
            EditorGUI.EndDisabledGroup();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Urgency", EditorStyles.boldLabel);

        signal.isNoUrgency = EditorGUILayout.Toggle("No Urgency", signal.isNoUrgency);

        EditorGUI.BeginDisabledGroup(signal.isNoUrgency);
        signal.urgency = (int)EditorGUILayout.Slider("Urgency", signal.urgency, 75, 500);
        EditorGUI.EndDisabledGroup();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(signal);
        }
    }
}
