using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeCoverage : VariableClass
{
    //public float m_ShadeCoverage;

    private void Awake()
    {
        variableName = "Shade Coverage";
        moreIsBad = false;
        targetValue = 0.75f;
    }
}
