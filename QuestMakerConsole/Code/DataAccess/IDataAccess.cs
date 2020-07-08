using QuestMaker.Data;

namespace QuestMakerConsole.Code.DataAccess {
	public interface IDataAccess {
		bool isImportable();
		bool isExportable();

		void import();
		void export(EntityCollection collection);
	}
}
