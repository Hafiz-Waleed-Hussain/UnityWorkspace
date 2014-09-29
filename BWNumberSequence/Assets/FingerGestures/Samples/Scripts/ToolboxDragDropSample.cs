using UnityEngine;
using System.Collections;

/// <summary>
/// This sample demonstrates the use of the Toolbox's Drag & Drop scripts
/// </summary>
public class ToolboxDragDropSample : SampleBase
{
    #region Properties exposed to the editor

    public TBInputManager inputMgr;
    public Transform[] dragObjects;

    #endregion
    #region Initial positions save / restore

    Vector3[] initialPositions;
        
    void SaveInitialPositions()
    {
        initialPositions = new Vector3[dragObjects.Length];

        for( int i = 0; i < initialPositions.Length; ++i )
            initialPositions[i] = dragObjects[i].position;
    }

    void RestoreInitialPositions()
    {
        for( int i = 0; i < initialPositions.Length; ++i )
            dragObjects[i].position = initialPositions[i];
    }

    #endregion

    #region Setup

    protected override string GetHelpText()
    {
        return @"This sample demonstrates the use of the Toolbox's Drag & Drop scripts";
    }

    protected override void Start()
    {
        base.Start();

        SaveInitialPositions();

    }

    #endregion
}