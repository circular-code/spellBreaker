using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spell {

    [SerializeField]
    protected int m_SpellID;
    [SerializeField]
    protected string m_SpellName;
    
    // Noch eine eigene Patternklasse anlegen?
    /*
    [SerializeField]
    protected int m_SpellPattern;
    */
    [SerializeField]
    protected string m_SpellDescription;
    [SerializeField]
    protected string m_SpellIcon;

    #region GET/SET

    public int SpellID
    {
        get
        {
            return this.m_SpellID;
        }
        set
        {
            this.m_SpellID = value;
        }
    }

    public string SpellName
    {
        get
        {
            return this.m_SpellName;
        }
        set
        {
            this.m_SpellName = value;
        }
    }

    /*public int SpellPattern
    {
        get
        {
            return this.m_SpellPattern;
        }
        set
        {
            this.m_SpellPattern = value;
        }
    }*/

    public string SpellDescription
    {
        get
        {
            return this.m_SpellDescription;
        }
        set
        {
            this.m_SpellDescription = value;
        }
    }

    public string SpellIcon
    {
        get
        {
            return this.m_SpellIcon;
        }
        set
        {
            this.m_SpellIcon = value;
        }
    }

    #endregion

}
