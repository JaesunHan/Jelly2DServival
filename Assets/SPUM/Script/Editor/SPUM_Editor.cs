using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

[CustomEditor(typeof(SPUM_Manager))]
[CanEditMultipleObjects]
public class SPUM_Editor : Editor
{
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        
        SPUM_Manager SPB = (SPUM_Manager)target;

        bool dirChk = Directory.Exists("Assets/Resources/SPUM/SPUM_Sprites/Items");
        if(!dirChk)
        {
            EditorGUILayout.HelpBox("You need to install SPUM Sprite Data by below install buttons",MessageType.Error);
            if (GUILayout.Button("Install Resources Data",GUILayout.Height(50))) 
            {
                SPB.InstallSpriteData();
            }
        }
        else
        {
            if (GUILayout.Button("Sync BodyData",GUILayout.Height(50))) 
            {
                SPB.SetBodySprite();
            }

            base.OnInspectorGUI();
            if (GUILayout.Button("Reset Added Sprite",GUILayout.Height(50))) 
            {
                SPB.SetInit();
            }

            if (GUILayout.Button("Check All Prefab version",GUILayout.Height(50))) 
            {
                SPB.CheckPrefabVersionData();
            }

            if (GUILayout.Button("Reinstall Resources Data",GUILayout.Height(50))) 
            {
                SPB.InstallSpriteData();
            }

            if( SPB!=null)
            {
                if(SPB._spumVersion!=null)
                {
                    SPB._spumVersion.text = "VER " + SPB._version.ToString(); 
                }

                if(SPB._mainBody == null)
                {
                    SPB._mainBodyList.Clear();
                    SPB._mainBody = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SPUM/SPUM_Sprites/BodySource/Species/0_Human/Human_1.png",typeof(Texture2D));

                    string spritePath = AssetDatabase.GetAssetPath(  SPB._mainBody );
                    Object[] sprites = AssetDatabase.LoadAllAssetsAtPath("Assets/SPUM/SPUM_Sprites/BodySource/Species/0_Human/Human_1.png");
                    var sortedList = sprites.OrderBy(go=>go.name).ToList();
                    List<Sprite> tSP = new List<Sprite>();
                    for(var i = 0 ; i < sortedList.Count;i++)
                    {
                        if(sortedList[i].GetType() == typeof(Sprite))
                        {
                            SPB._mainBodyList.Add((Sprite)sortedList[i]);
                        }
                    }

                }
            }
        }
    }
}
