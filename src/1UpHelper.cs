using Celeste;
using EverestAPI;
using Monocle;

namespace OneUpHelper {

    public class OneUpHelperModule : EverestModule {

        public static OneUpHelperModule Instance { get; private set; }

        public OneUpHelperModule() {
            Instance = this;
        }

        public override void Load() {
            Everest.Events.Player.OnDie += () => {
                Celeste.Level.Reload();
            }
        }

        public override void Unload() {
        }
    }
}

