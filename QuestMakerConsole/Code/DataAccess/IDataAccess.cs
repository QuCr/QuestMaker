using QuestMaker.Data;

namespace QuestMaker.Console.Code.DataAccess {
	public interface IDataAccess {
		bool isImportable();
		bool isExportable();

		void import();
		void export(EntityCollection collection);
	}
}
