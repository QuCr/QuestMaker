using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuestMaker.Code;
using QuestMaker.Data;
using QuestMakerConsole;
using QuestmakerUI;
using QuestmakerUI.Forms.Controls;

namespace QuestMakerTests {
    [TestClass]
    public class EditorTest {

        MainForm mainform;

        ViewControl viewer;
        EditorControl editor;
        TreeControl tree;

        PrivateObject po;

        [ClassInitialize]
        public static void classInitialize(TestContext tc) {
            Program.Main("default");
        }

        [TestInitialize]
        public void testInitialize() {
            mainform = new MainForm();

            viewer = mainform.Viewer;
            editor = mainform.Editor;
            tree = mainform.Tree;

            po = new PrivateObject(mainform, new PrivateType(typeof(MainForm)));
        }

        [TestMethod]
        public void EditorTest_LoadEntity() {
            Entity entity = EntityCollection.byID(typeof(Person), "bert");

            Assert.IsNotNull(entity);
            Assert.AreEqual("bert", entity.id);

            mainform.handle(mainform.Viewer, new PacketSingleEditor(entity));

            Assert.AreEqual("bert", mainform.Editor.list[0].value);
        }

        /*[TestMethod]
        public void EditorTest_UpdateEntity() {
            Entity entity = EntityCollection.byID(typeof(Person), "bert");
            mainform.handle(viewer, new PacketSingleEditor(entity));

           
        }*/
    }
}
