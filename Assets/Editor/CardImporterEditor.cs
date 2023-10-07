using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// This is the script component where the GUI button will appear
[CustomEditor(typeof(CardDeckHandler))]
public class CardImporterEditor : Editor
{
    private CardDeckHandler m_cardDeckHandlerScript;

    private void OnEnable()
    {
        m_cardDeckHandlerScript = (CardDeckHandler)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Import Cards"))
        {
            GoogleSheetParser.LoadCardDeck();
        }

        base.OnInspectorGUI();
    }

}
