using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Rendering.Toon;
namespace UnityEditor.Rendering.Toon
{
    [CustomEditor(typeof(UTS_ExposureCurve))]

    public class UTS_ExposureCurveInspector : Editor
    {
        SerializedObject m_SerializedObject;
        string numberString = "1";

        public override void OnInspectorGUI()
        {
            const string labelLightAdjustment = "Light Adjustment";
            const string labelExposureAdjustment = "Exposure Adjustment";
            const string labelExposureCurave = "Curve";
            const string labelExposureMin = "Min:";
            const string labelExposureMax = "Max:";
            bool isChanged = false;

            var obj = target as UTS_ExposureCurve;
            EditorGUI.BeginChangeCheck();
            bool exposureAdjustment = EditorGUILayout.Toggle(labelExposureAdjustment, obj.m_ExposureAdjustmnt);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Expsure Adjustment");
                obj.m_ExposureAdjustmnt = exposureAdjustment;
                isChanged = true;
            }

            EditorGUI.BeginDisabledGroup(!obj.m_ExposureAdjustmnt);
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                var curve = EditorGUILayout.CurveField(labelExposureCurave, obj.m_AnimationCurve);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Changed Curve");
                    obj.m_AnimationCurve = curve;
                    isChanged = true;
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndDisabledGroup();

            //Rect rect = GUILayoutUtility.GetRect(Screen.width, 300.0f);

            EditorGUI.BeginChangeCheck();

            bool lightAdjustment = EditorGUILayout.Toggle(labelLightAdjustment, obj.m_LightAdjustment);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Light Adjustment");
                obj.m_LightAdjustment = lightAdjustment;
                isChanged = true;
            }

            numberString = EditorGUILayout.TextField("lux: ", numberString);
            float fLux = float.Parse(numberString);
            fLux = Mathf.Max(fLux, 0.0001f);
            fLux = Mathf.Min(fLux, 100000 );
            float log10value = Mathf.Log10(fLux);
            fLux = Mathf.Clamp((log10value + 5.0f) / 10.0f, 0.0f, 1.0f);
            string label = fLux.ToString();
            EditorGUILayout.LabelField(label);
            if ( isChanged)
            {
                // at leaset 2020.3.12f1, not neccessary. but, from which version??
                EditorApplication.QueuePlayerLoopUpdate();
            }

            obj.m_DebugUI = EditorGUILayout.Toggle(labelExposureAdjustment, obj.m_DebugUI);
            if (m_SerializedObject == null )
            {
                m_SerializedObject = new SerializedObject(obj);
            }
            EditorGUI.BeginDisabledGroup(!obj.m_DebugUI);
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField(labelExposureMin + obj.m_Min.ToString());
                EditorGUILayout.LabelField(labelExposureMax + obj.m_Max.ToString());
                /*
                var prop = m_SerializedObject.FindProperty("m_ExposureArray");
                EditorGUILayout.PropertyField(prop, new GUIContent("ExposureArray"), true);
                m_SerializedObject.ApplyModifiedProperties();
                */
                for ( int ii = 0; ii < obj.m_ExposureArray.Length; ii++)
                {
                    EditorGUIUtility.labelWidth = 40;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(ii.ToString() + ":");
                    EditorGUILayout.LabelField(obj.m_ExposureArray[ii].ToString());
                    EditorGUILayout.LabelField(ConvertFromEV100(obj.m_ExposureArray[ii]).ToString());
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndDisabledGroup();


        }


        float ConvertFromEV100(float EV100)
        {
#if true
            float val = Mathf.Pow(2, EV100) * 2.5f;
            return val;
#else
            float3 maxLuminance = 1.2f * pow(2.0f, EV100);
            return 1.0f / maxLuminance;
#endif
        }

        float ConvertToEV100(float val)
        {
#if true
            return Mathf.Log(val*0.4f,2.0f);
#else
            return log2(1.0f / (1.2f * value));
#endif
        }

    }
}
