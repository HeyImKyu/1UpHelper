using System;

namespace Celeste.Mod._1UpHelper;

public class _1UpHelperModule : EverestModule {
    public static _1UpHelperModule Instance { get; private set; }

    public override Type SettingsType => typeof(_1UpHelperModuleSettings);
    public static _1UpHelperModuleSettings Settings => (_1UpHelperModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(_1UpHelperModuleSession);
    public static _1UpHelperModuleSession Session => (_1UpHelperModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(_1UpHelperModuleSaveData);
    public static _1UpHelperModuleSaveData SaveData => (_1UpHelperModuleSaveData) Instance._SaveData;

    public _1UpHelperModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(_1UpHelperModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(_1UpHelperModule), LogLevel.Info);
#endif
    }

    public override void Load() {
        Everest.Events.Player.OnDie += () => {
            Celeste.Level.Reload();
        }
    }

    public override void Unload() {
    }
}
