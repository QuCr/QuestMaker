using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuestMaker.Code;
using QuestMaker.Data;
using QuestMaker.Console;
using Questmaker.UI;

namespace QuestMaker.Tests {
    [TestClass]
    public class EditorTest {

        MainForm mainform;

        ViewControl viewer;
        EditorControl editor;
        TreeControl tree;

        void setText(string id, int i = 0) { editor.list[i].control.Text = id; }

        [ClassInitialize]
        public static void classInitialize(TestContext _) {
            Program.Main("default");
        }

        [TestInitialize]
        public void testInitialize() {
            mainform = new MainForm();

            viewer = mainform.Viewer;
            editor = mainform.Editor;
            tree = mainform.Tree;
        }

        [TestMethod]
        public void EditorTest_LoadEntity() {
            Entity entity = EntityCollection.byID(typeof(Person), "bert");

            Assert.IsNotNull(entity, "queried entity");

            mainform.handle(tree, new PacketType(typeof(Person)));
            mainform.handle(mainform.Viewer, new PacketSingleEditor(entity));

            Assert.AreEqual("bert", editor.list[0].value, "ID in editor");
            Assert.IsTrue(editor.btnClear.Enabled, "btnClear");
            Assert.IsFalse(editor.btnCreate.Enabled, "btnCreate");
            Assert.IsTrue(editor.btnUpdate.Enabled, "btnUpdate");
            Assert.IsTrue(editor.btnDestroy.Enabled, "btnDestroy");
            Assert.IsTrue(editor.list[0].control.ForeColor == Color.Green, "color of text from ID");
        }

        [TestMethod]
        public void EditorTest_CreateEntity() {
            Entity entity = EntityCollection.byID(typeof(Person), "bert");

            mainform.handle(tree, new PacketType(typeof(Person)));
            mainform.handle(viewer, new PacketSingleEditor(entity));

            setText("dave", 0); //ID
            setText("Dave", 1); //displayName

            Assert.AreEqual("dave", editor.list[0].value, "ID in editor");
            Assert.AreEqual("Dave", editor.list[1].value, "displayName in editor");

            editor.create(editor.btnClear, null);

            Assert.IsNotNull(EntityCollection.byID(typeof(Person), "bert"), "created entity");
            System.Console.WriteLine($"There are now {EntityCollection.entityCollection["Person"].Count} people");

            Assert.IsTrue(editor.btnClear.Enabled, "btnClear");
            Assert.IsFalse(editor.btnCreate.Enabled, "btnCreate");
            Assert.IsTrue(editor.btnUpdate.Enabled, "btnUpdate");
            Assert.IsTrue(editor.btnDestroy.Enabled, "btnDestroy");
            Assert.IsTrue(editor.list[0].control.ForeColor == Color.Green, "color of text from ID");
        }

        [TestMethod]
        public void EditorTest_UpdateEntity() {
            Entity entity = EntityCollection.byID(typeof(Person), "albert");

            mainform.handle(tree, new PacketType(typeof(Person)));
            mainform.handle(viewer, new PacketSingleEditor(entity));

            setText("alberto", 0); //ID
            setText("Alberto", 1); //displayName

            Assert.AreEqual("alberto", editor.list[0].value, "ID in editor");
            Assert.AreEqual("Alberto", editor.list[1].value, "displayName in editor");

            editor.update(editor.btnUpdate, null);

            Assert.AreEqual("alberto", entity.id);
            Assert.AreEqual("Alberto", entity.displayName);

            Assert.IsTrue(editor.btnClear.Enabled, "btnClear");
            Assert.IsFalse(editor.btnCreate.Enabled, "btnCreate");
            Assert.IsTrue(editor.btnUpdate.Enabled, "btnUpdate");
            Assert.IsTrue(editor.btnDestroy.Enabled, "btnDestroy");
            Assert.IsTrue(editor.list[0].control.ForeColor == Color.Green, "color of text from ID");
        }

        [TestMethod]
        public void EditorTest_DeleteEntity() {
            Entity entity = EntityCollection.byID(typeof(Person), "cyk");

            mainform.handle(tree, new PacketType(typeof(Person)));
            mainform.handle(viewer, new PacketSingleEditor(entity));

            Assert.IsTrue(EntityCollection.isExistingID(entity.id));
            editor.destroy(editor.btnDestroy, null);
            Assert.IsFalse(EntityCollection.isExistingID(entity.id));

            Assert.IsTrue(editor.btnClear.Enabled, "btnClear");
            Assert.IsTrue(editor.btnCreate.Enabled, "btnCreate");
            Assert.IsFalse(editor.btnUpdate.Enabled, "btnUpdate");
            Assert.IsFalse(editor.btnDestroy.Enabled, "btnDestroy");
            Assert.IsTrue(editor.list[0].control.ForeColor == Color.Black, "color of text from ID");
        }

        [TestMethod]
        public void EditorTest_EmptyTextID() {
            Entity point1 = EntityCollection.byID(typeof(Waypoint), "point1");
            mainform.handle(tree, new PacketType(typeof(Person)));
            mainform.handle(viewer, new PacketSingleEditor(point1));

            setText("");
            Assert.IsTrue(editor.btnClear.Enabled, "btnClear");
            Assert.IsFalse(editor.btnCreate.Enabled, "btnCreate");
            Assert.IsFalse(editor.btnUpdate.Enabled, "btnUpdate");
            Assert.IsFalse(editor.btnDestroy.Enabled, "btnDestroy");
        }

        [TestMethod]
        public void EditorTest_TextColorID() {
            Entity point1 = EntityCollection.byID(typeof(Waypoint), "point1");
            Entity point2 = EntityCollection.byID(typeof(Waypoint), "point2");
            Entity pointX = EntityCollection.byID(typeof(Waypoint), "pointX");

            Assert.IsNull(pointX);

            mainform.handle(tree, new PacketType(typeof(Person)));
            mainform.handle(viewer, new PacketSingleEditor(point1));

            Assert.IsTrue(editor.list[0].control.ForeColor == Color.Green, "color of text from ID");
            setText(point2.id);
            Assert.IsTrue(editor.list[0].control.ForeColor == Color.Red, "color of text from ID");
            setText("pointX");
            Assert.IsTrue(editor.list[0].control.ForeColor == Color.Black, "color of text from ID");
            setText(point1.id);
            Assert.IsTrue(editor.list[0].control.ForeColor == Color.Green, "color of text from ID");
        }
    }
}
