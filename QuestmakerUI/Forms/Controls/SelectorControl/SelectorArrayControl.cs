using QuestMaker.Code;
using QuestMaker.Console;

namespace Questmaker.UI.Forms.Controls {
    public class SelectorArrayControl : SelectorControl {
        public SelectorArrayControl(PacketEdit packet) {
            //Helper.outputList(EntityCollection.get(packet).ToArray());
            Program.debug(GetType().Name);
        }
    }
}
