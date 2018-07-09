using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIUpdateHandler {

    public delegate void RefreshCombatBars();
    public static event RefreshCombatBars OnCombatBarsRefresh;
}
