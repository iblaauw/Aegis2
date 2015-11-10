using System;
using System.Collections;
using System.Collections.Generic;
using Aegis;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public static PrefabCache PrefabCache { get { return Instance.Prefabs; } }

    public List<GameObject> prefabs;

    #region Properties

    public Grid Grid { get; private set; }

    public PrefabCache Prefabs { get; private set; }

    #endregion

    #region Unity Messages

    void Start () {
        // Set singleton!
        if (Instance == null)
            Instance = this;
        if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        this.Prefabs = new PrefabCache();
        this.Prefabs.AddAll(this.prefabs);
	}

    #endregion
}
