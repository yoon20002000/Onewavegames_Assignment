using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillData))]
public class SkillDataInspectorEditor : Editor
{
    private SkillData skillData;
    private bool bShowBasicInfo = true;
    private bool bShowEffects = true;
    private bool bShowCosts = true;
    private bool bShowAdvanced = false;
    
    private void OnEnable()
    {
        skillData = (SkillData)target;
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("스킬 데이터 편집기", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        // 기본 정보 섹션
        bShowBasicInfo = EditorGUILayout.Foldout(bShowBasicInfo, "기본 정보", true);
        if (bShowBasicInfo)
        {
            drawBasicInfo();
        }
        
        EditorGUILayout.Space();
        
        // 효과 섹션
        bShowEffects = EditorGUILayout.Foldout(bShowEffects, "효과 설정", true);
        if (bShowEffects)
        {
            drawEffectsSection();
        }
        
        EditorGUILayout.Space();
        
        // 비용 섹션
        bShowCosts = EditorGUILayout.Foldout(bShowCosts, "비용 설정", true);
        if (bShowCosts)
        {
            drawCostsSection();
        }
        
        EditorGUILayout.Space();
        
        // 기본 에디터
        bShowAdvanced = EditorGUILayout.Foldout(bShowAdvanced, "기본 에디터", true);
        if (bShowAdvanced)
        {
            drawAdvancedSection();
        }
        
        EditorGUILayout.Space(10);
        
        // 빠른 작업 버튼들
        drawQuickActions();
        
        serializedObject.ApplyModifiedProperties();
    }
    
    private void drawBasicInfo()
    {
        EditorGUILayout.BeginVertical("box");
        
        // 스킬 이름
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("스킬 이름", GUILayout.Width(100));
        skillData.SkillName = EditorGUILayout.TextField(skillData.SkillName);
        EditorGUILayout.EndHorizontal();
        
        // 스킬 설명
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("설명", GUILayout.Width(100));
        skillData.SkillDescription = EditorGUILayout.TextArea(skillData.SkillDescription, GUILayout.Height(40));
        EditorGUILayout.EndHorizontal();
        
        // 스킬 타입
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("스킬 타입", GUILayout.Width(100));
        skillData.SkillTag = (ESkillTag)EditorGUILayout.EnumPopup(skillData.SkillTag);
        EditorGUILayout.EndHorizontal();
        
        // 스킬 아이콘
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("스킬 아이콘", GUILayout.Width(100));
        skillData.SkillIcon = (Sprite)EditorGUILayout.ObjectField(skillData.SkillIcon, typeof(Sprite), false);
        
        // 아이콘 미리보기
        if (skillData.SkillIcon != null)
        {
            EditorGUILayout.LabelField("미리보기", GUILayout.Width(60));
            EditorGUILayout.LabelField(new GUIContent(skillData.SkillIcon.texture), GUILayout.Width(64), GUILayout.Height(64));
        }
        else
        {
            EditorGUILayout.LabelField("미리보기", GUILayout.Width(60));
            EditorGUILayout.LabelField("아이콘 없음", GUILayout.Width(64), GUILayout.Height(64));
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        // 범위와 쿨다운
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("범위", GUILayout.Width(100));
        skillData.Range = EditorGUILayout.FloatField(skillData.Range);
        EditorGUILayout.LabelField("쿨다운", GUILayout.Width(60));
        skillData.Cooldown = EditorGUILayout.FloatField(skillData.Cooldown);
        EditorGUILayout.LabelField("초", GUILayout.Width(20));
        EditorGUILayout.EndHorizontal();
        
        // 기본 수치
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("기본 수치", GUILayout.Width(100));
        skillData.Value = EditorGUILayout.FloatField(skillData.Value);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
    }
    
    private void drawEffectsSection()
    {
        EditorGUILayout.BeginVertical("box");
        
        // 효과 개수 표시
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"효과 개수: {skillData.Effects.Count}", EditorStyles.boldLabel);
        if (GUILayout.Button("+ 효과 추가", GUILayout.Width(100)))
        {
            addNewEffect();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        // 효과 목록
        for (int i = 0; i < skillData.Effects.Count; i++)
        {
            drawEffectItem(i);
        }
        
        if (skillData.Effects.Count == 0)
        {
            EditorGUILayout.HelpBox("효과가 없습니다. + 효과 추가 버튼을 눌러주세요.", MessageType.Info);
        }
        
        EditorGUILayout.EndVertical();
    }
    
    private void drawEffectItem(int index)
    {
        var effect = skillData.Effects[index];
        
        EditorGUILayout.BeginVertical("box");
        
        // 효과 헤더
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"효과 {index + 1}", EditorStyles.boldLabel);
        if (GUILayout.Button("삭제", GUILayout.Width(50)))
        {
            removeEffect(index);
            return;
        }
        EditorGUILayout.EndHorizontal();
        
        // 효과 타입
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("타입", GUILayout.Width(60));
        effect.EffectType = (EEffectType)EditorGUILayout.EnumPopup(effect.EffectType);
        EditorGUILayout.EndHorizontal();
        
        // 효과 수치
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("수치", GUILayout.Width(60));
        effect.Value = EditorGUILayout.FloatField(effect.Value);
        EditorGUILayout.EndHorizontal();
        
        // 지속 시간
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("지속시간", GUILayout.Width(60));
        effect.Duration = EditorGUILayout.FloatField(effect.Duration);
        EditorGUILayout.LabelField("초", GUILayout.Width(20));
        EditorGUILayout.EndHorizontal();
        
        // 커스텀 데이터
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("커스텀", GUILayout.Width(60));
        effect.CustomData = EditorGUILayout.TextField(effect.CustomData);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(2);
    }
    
    private void drawCostsSection()
    {
        EditorGUILayout.BeginVertical("box");
        
        // 비용 개수 표시
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"비용 개수: {skillData.CostEffectData.Count}", EditorStyles.boldLabel);
        if (GUILayout.Button("+ 비용 추가", GUILayout.Width(100)))
        {
            addNewCost();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        // 비용 목록
        for (int i = 0; i < skillData.CostEffectData.Count; i++)
        {
            drawCostItem(i);
        }
        
        if (skillData.CostEffectData.Count == 0)
        {
            EditorGUILayout.HelpBox("비용이 없습니다. + 비용 추가 버튼을 눌러주세요.", MessageType.Info);
        }
        
        // 총 비용 표시
        if (skillData.CostEffectData.Count > 0)
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("총 비용:", EditorStyles.boldLabel);
            
            for (int i = 0; i < skillData.CostEffectData.Count; i++)
            {
                var cost = skillData.CostEffectData[i];
                EditorGUILayout.LabelField($"  {cost.ECostEffectType}: {cost.Value:F1}");
            }
        }
        
        EditorGUILayout.EndVertical();
    }
    
    private void drawCostItem(int index)
    {
        var cost = skillData.CostEffectData[index];
        
        EditorGUILayout.BeginVertical("box");
        
        // 비용 헤더
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"비용 {index + 1}", EditorStyles.boldLabel);
        if (GUILayout.Button("삭제", GUILayout.Width(50)))
        {
            removeCost(index);
            return;
        }
        EditorGUILayout.EndHorizontal();
        
        // 비용 타입 (중복 방지)
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("타입", GUILayout.Width(60));
        
        // 현재 선택된 타입이 다른 비용과 중복되는지 확인
        bool bIsDuplicate = isCostEffectTypeDuplicate(cost.ECostEffectType, index);
        
        // 중복된 타입인 경우 색상 변경
        if (bIsDuplicate)
        {
            GUI.color = Color.red;
        }
        
        ECostEffectType newCostType = (ECostEffectType)EditorGUILayout.EnumPopup(cost.ECostEffectType);
        
        // 색상 복원
        GUI.color = Color.white;
        
        // 중복되지 않는 타입으로만 변경 허용
        if (!isCostEffectTypeDuplicate(newCostType, index))
        {
            cost.ECostEffectType = newCostType;
        }
        else if (newCostType != cost.ECostEffectType)
        {
            // 중복된 타입인 경우 팝업 메시지 표시
            EditorUtility.DisplayDialog("중복 오류", "이 타입은 이미 다른 비용에서 사용 중입니다.", "확인");
        }
        
        EditorGUILayout.EndHorizontal();
        
        // 비용 수치
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("수치", GUILayout.Width(60));
        cost.Value = EditorGUILayout.FloatField(cost.Value);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(2);
    }
    
    private void drawAdvancedSection()
    {
        EditorGUILayout.BeginVertical("box");
        
        // 기본 Inspector 그리기 (고급 사용자를 위해)
        EditorGUILayout.LabelField("기본 Inspector", EditorStyles.boldLabel);
        DrawDefaultInspector();
        
        EditorGUILayout.EndVertical();
    }
    
    private void drawQuickActions()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("빠른 작업", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("그랩 스킬 템플릿", GUILayout.Height(25)))
        {
            applyGrabTemplate();
        }
        
        if (GUILayout.Button("데미지 스킬 템플릿", GUILayout.Height(25)))
        {
            applyDamageTemplate();
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("힐링 스킬 템플릿", GUILayout.Height(25)))
        {
            applyHealingTemplate();
        }
        
        if (GUILayout.Button("초기화", GUILayout.Height(25)))
        {
            resetSkillData();
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
    }
    
    private void addNewEffect()
    {
        var newEffect = new EffectData
        {
            EffectType = EEffectType.Damage,
            Value = 10f,
            Duration = 0f,
            CustomData = ""
        };
        skillData.Effects.Add(newEffect);
        EditorUtility.SetDirty(skillData);
    }
    
    private void removeEffect(int index)
    {
        if (index >= 0 && index < skillData.Effects.Count)
        {
            skillData.Effects.RemoveAt(index);
            EditorUtility.SetDirty(skillData);
        }
    }
    
    private void addNewCost()
    {
        // 최대 비용 개수 체크 (ECostEffectType 개수 - 1, None 제외)
        int maxCostCount = System.Enum.GetValues(typeof(ECostEffectType)).Length - 1;
        
        if (skillData.CostEffectData.Count >= maxCostCount)
        {
            EditorUtility.DisplayDialog("비용 추가 제한", $"비용은 최대 {maxCostCount}개까지만 추가할 수 있습니다.", "확인");
            return;
        }
        
        // 사용 가능한 첫 번째 비용 타입 찾기
        ECostEffectType availableType = getFirstAvailableCostType();
        
        var newCost = new CostEffectData
        {
            ECostEffectType = availableType,
            Value = 10f
        };
        skillData.CostEffectData.Add(newCost);
        EditorUtility.SetDirty(skillData);
    }
    
    private void removeCost(int index)
    {
        if (index >= 0 && index < skillData.CostEffectData.Count)
        {
            skillData.CostEffectData.RemoveAt(index);
            EditorUtility.SetDirty(skillData);
        }
    }
    
    // 비용 타입 중복 체크 메서드
    private bool isCostEffectTypeDuplicate(ECostEffectType costType, int currentIndex)
    {
        for (int i = 0; i < skillData.CostEffectData.Count; i++)
        {
            if (i != currentIndex && skillData.CostEffectData[i].ECostEffectType == costType)
            {
                return true;
            }
        }
        return false;
    }
    
    // 사용 가능한 첫 번째 비용 타입 찾기
    private ECostEffectType getFirstAvailableCostType()
    {
        var allTypes = System.Enum.GetValues(typeof(ECostEffectType));
        
        foreach (ECostEffectType type in allTypes)
        {
            if (!isCostEffectTypeDuplicate(type, -1)) // -1은 새로 추가되는 항목을 의미
            {
                return type;
            }
        }
        
        // 모든 타입이 사용 중인 경우 기본값 반환
        return ECostEffectType.MP;
    }
    
    private void applyGrabTemplate()
    {
        if (EditorUtility.DisplayDialog("템플릿 적용", "그랩 스킬 템플릿을 적용하시겠습니까?", "확인", "취소"))
        {
            Undo.RecordObject(skillData, "Apply Grab Template");
            
            skillData.SkillTag = ESkillTag.Skill_Grab;
            skillData.SkillName = "Grab Skill";
            skillData.SkillDescription = "투사체를 발사하여 적을 끌어오는 스킬";
            skillData.Range = 10f;
            skillData.Value = 0f;
            skillData.Cooldown = 5f;
            
            skillData.Effects.Clear();
            
            // 투사체 효과
            var projectileEffect = new EffectData
            {
                EffectType = EEffectType.Projectile,
                Value = 50f,
                Duration = 5f,
                CustomData = "Projectile"
            };
            skillData.Effects.Add(projectileEffect);
            
            // 끌어오기 효과
            var pullEffect = new EffectData
            {
                EffectType = EEffectType.Pull,
                Value = 10f,
                Duration = 2f
            };
            skillData.Effects.Add(pullEffect);
            
            skillData.CostEffectData.Clear();
            
            // MP 비용
            var mpCost = new CostEffectData
            {
                ECostEffectType = ECostEffectType.MP,
                Value = 20f
            };
            skillData.CostEffectData.Add(mpCost);
            
            EditorUtility.SetDirty(skillData);
            AssetDatabase.SaveAssets();
        }
    }
    
    private void applyDamageTemplate()
    {
        if (EditorUtility.DisplayDialog("템플릿 적용", "데미지 스킬 템플릿을 적용하시겠습니까?", "확인", "취소"))
        {
            Undo.RecordObject(skillData, "Apply Damage Template");
            
            skillData.SkillTag = ESkillTag.Skill_SelfTarget;
            skillData.SkillName = "Damage Skill";
            skillData.SkillDescription = "데미지를 주는 스킬";
            skillData.Range = 5f;
            skillData.Value = 25f;
            skillData.Cooldown = 3f;
            
            skillData.Effects.Clear();
            
            // 데미지 효과
            var damageEffect = new EffectData
            {
                EffectType = EEffectType.Damage,
                Value = 25f,
                Duration = 0f
            };
            skillData.Effects.Add(damageEffect);
            
            skillData.CostEffectData.Clear();
            
            // MP 비용
            var mpCost = new CostEffectData
            {
                ECostEffectType = ECostEffectType.MP,
                Value = 15f
            };
            skillData.CostEffectData.Add(mpCost);
            
            EditorUtility.SetDirty(skillData);
            AssetDatabase.SaveAssets();
        }
    }
    
    private void applyHealingTemplate()
    {
        if (EditorUtility.DisplayDialog("템플릿 적용", "힐링 스킬 템플릿을 적용하시겠습니까?", "확인", "취소"))
        {
            Undo.RecordObject(skillData, "Apply Healing Template");
            
            skillData.SkillTag = ESkillTag.Skill_SelfTarget;
            skillData.SkillName = "Healing Skill";
            skillData.SkillDescription = "체력을 회복하는 스킬";
            skillData.Range = 0f;
            skillData.Value = 30f;
            skillData.Cooldown = 8f;
            
            skillData.Effects.Clear();
            
            // 힐링 효과
            var healEffect = new EffectData
            {
                EffectType = EEffectType.Heal,
                Value = 30f,
                Duration = 0f
            };
            skillData.Effects.Add(healEffect);
            
            skillData.CostEffectData.Clear();
            
            // MP 비용
            var mpCost = new CostEffectData
            {
                ECostEffectType = ECostEffectType.MP,
                Value = 25f
            };
            skillData.CostEffectData.Add(mpCost);
            
            EditorUtility.SetDirty(skillData);
            AssetDatabase.SaveAssets();
        }
    }
    
    private void resetSkillData()
    {
        if (EditorUtility.DisplayDialog("초기화 확인", "해당 스킬 데이터를 초기화하시겠습니까?", "확인", "취소"))
        {
            Undo.RecordObject(skillData, "Reset Skill Data");
            
            skillData.SkillTag = ESkillTag.Skill_SelfTarget;
            skillData.SkillName = "New Skill";
            skillData.SkillDescription = "스킬 설명";
            skillData.Range = 5f;
            skillData.Value = 10f;
            skillData.Cooldown = 3f;
            
            skillData.Effects.Clear();
            skillData.CostEffectData.Clear();
            
            EditorUtility.SetDirty(skillData);
            AssetDatabase.SaveAssets();
        }
    }
}
