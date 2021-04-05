//------------------------------------------------------------------------------
// Author : Strix
// Github : https://github.com/KorStrix/SheetParser
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class SkillData_Container : UnityEngine.ScriptableObject
{
    
    private static SkillData_Container _instance;
    
    public List<SkillData> listData;
    
    public Dictionary<ESkill, SkillData> mapData_Key_Is_eSkill;
    
    public static SkillData_Container instance
    {
        get
        {
               return _instance;
        }
    }
    
    public static void DoInit(SkillData_Container pSingletonInstance, bool bIsUpdateChildAsset)
    {
          _instance = pSingletonInstance;
#if UNITY_EDITOR
           if(bIsUpdateChildAsset)
           {
              _instance.listData.Clear();
               Object[] arrObject = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(UnityEditor.AssetDatabase.GetAssetPath(_instance));
               for (int i = 0; i < arrObject.Length; i++)
                  _instance.listData.Add((SkillData)arrObject[i]);
               if(Application.isPlaying == false)
               {
                   UnityEditor.EditorUtility.SetDirty(_instance);
                   UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
               }
           }
#endif
        _instance.Init_mapData_Key_Is_eSkill();
    }
    
    private void Init_mapData_Key_Is_eSkill()
    {
        this.mapData_Key_Is_eSkill = listData.ToDictionary(x => x.eSkill);
    }
}

public enum ESkillType
{
    
    Skill_Deal_Support,
    
    Skill_Ranged_Deal,
    
    Skill_Support,
}

public enum ESkill
{
    
    Skill_Summon_Fairy,
    
    Skill_Meteor,
    
    Skill_Recovery,
    
    Skill_Shield,
}

#region 
static
public class SkillData_ContainerHelper
{
    
    public static SkillData GetSkillData(this ESkill eKey, System.Action<string> OnError = null)
    {
          SkillData pData;
          if(SkillData_Container.instance.mapData_Key_Is_eSkill.TryGetValue(eKey, out pData) == false)
          {
              if(OnError != null)
                  OnError(nameof(SkillData_Container) + "- Not Found Data // Key : " + eKey);
          }
          return pData;
    }
}
#endregion
