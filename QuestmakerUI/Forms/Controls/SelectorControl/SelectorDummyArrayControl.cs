using QuestMaker.Code;
using QuestMaker.Console;

namespace Questmaker.UI.Forms.Controls {
    public class SelectorDummyArrayControl : SelectorControl {
        public SelectorDummyArrayControl(PacketEdit packet) {
        //Helper.outputList(packet.entities.ToArray());
        Program.debug(GetType().Name);
    }
}
}
