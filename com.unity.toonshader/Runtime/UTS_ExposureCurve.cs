using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Unity.Rendering.Toon
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class UTS_ExposureCurve : MonoBehaviour
    {
        const string kExposureCurvePropName = "_ExposureCurveType";
        const string kLinearFrom0To1Name = "_LinearFrom0to10";
	const string kExopsureMultiplier = "_ExopsureMultiplier";
        [SerializeField]
        public int m_CurveType;
        [SerializeField]
        public bool m_LinearFrom0to10 = true;
	[SerializeField]
	public float m_ExopsureMultiplier = 0.0f;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            const int logMax = 4;
            if ( m_CurveType >= logMax)
            {
                m_CurveType = logMax - 1;
            }
            if ( m_CurveType < 0 )
            {
                m_CurveType = 0;
            }
            Shader.SetGlobalInt(kExposureCurvePropName, m_CurveType);
            Shader.SetGlobalInt(kLinearFrom0To1Name, m_LinearFrom0to10 ? 1 : 0);
	    Shader.SetGlobalFloat(kExopsureMultiplier, m_ExopsureMultiplier);
        }
    }

    public enum UTS_ExposureCurveType
    {
        Linear,
        Log,
        Log2,
        Log10
    };

}
