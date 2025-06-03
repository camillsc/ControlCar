using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLExt.FileStorage.Folders;
using SQLite;

namespace ControlCar.Services
{
    public class DatabaseService
    {
        public SQLiteConnection GetConexao()
        {
            var folder = new LocalRootFolder();

            var file = folder.CreateFile("controlcar.db",
                PCLExt.FileStorage.CreationCollisionOption.OpenIfExists);

            return new SQLiteConnection(file.Path);
        }
    }
}
