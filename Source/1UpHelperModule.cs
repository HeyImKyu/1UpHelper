using System;
using System.Collections.Generic;
using System.Linq;
using Celeste;

namespace Celeste.Mod._1UpHelper;

public class _1UpHelperModule : EverestModule {
    public static _1UpHelperModule Instance { get; private set; }

    public override Type SettingsType => typeof(_1UpHelperModuleSettings);
    public static _1UpHelperModuleSettings Settings => (_1UpHelperModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(_1UpHelperModuleSession);
    public static _1UpHelperModuleSession Session => (_1UpHelperModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(_1UpHelperModuleSaveData);
    public static _1UpHelperModuleSaveData SaveData => (_1UpHelperModuleSaveData) Instance._SaveData;

    public Level Level;
    public static int AchievementCount = 6;
    private static bool goalReached = false;

    public _1UpHelperModule() {
        Instance = this;

        // Logger.SetLogLevel(nameof(_1UpHelperModule), LogLevel.Verbose);
    }

    public override void Load() {
        On.Celeste.LevelLoader.StartLevel += (orig, self) =>
        {
            var level = self.Level;
            this.Level = level;
            orig(self);
        };

        Everest.Events.Player.OnDie += player => {
            if (Level != null) {
                Level.OnEndOfFrame += () => {
                    RestartChapter(Level);
                };
            }
        };

        On.Celeste.Strawberry.OnCollect += (orig, self) =>
        {
            if (Level != null && !goalReached && CountUncollectedFollowerStrawberries() < AchievementCount) {
                Level.OnEndOfFrame += () => {
                    RestartChapter(Level);
                };
            }
            orig(self);
        };
    }

    public override void Unload() {
    }

    private void RestartChapter(Level level) {
        Session session = level.Session;

        // Reset the session to the start of the chapter
        session.RespawnPoint = session.LevelData.Position;
        session.FirstLevel = true;
        session.Deaths = 0;
        session.Time = 0L;
        session.Area.Mode = AreaMode.Normal;

        // Create a new Session to fully reset collectibles and state
        Session newSession = new Session(session.Area);

        LevelEnter.Go(newSession, fromSaveData: false);
    }

    private int CountUncollectedFollowerStrawberries() {
        Player player = Level.Tracker.GetEntity<Player>();
        if (player == null)
            return 0;

        var count = player.Leader.Followers.Where(b => b.Entity is Strawberry).Count();

        if (count >= AchievementCount)
            goalReached = true;

        return count;
    }


}
