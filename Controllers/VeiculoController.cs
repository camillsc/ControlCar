using SQLite;
using ControlCar.Services;
using ControlCar.Models;

namespace ControlCar.Controllers
{
    public class VeiculoController
    {
        private DatabaseService databaseService;
        private SQLiteConnection connection;

        public VeiculoController()
        {
            databaseService = new DatabaseService();
            connection = databaseService.GetConexao();
            connection.CreateTable<Veiculo>();
        }

        public bool Insert(Veiculo value)
        {
            return connection.Insert(value) > 0;
        }

        public bool Update(Veiculo value)
        {
            return connection.Update(value) > 0;
        }

        public bool Delete(Veiculo value)
        {
            return connection.Delete(value) > 0;
        }

        public List<Veiculo> GetAll()
        {
            return connection.Table<Veiculo>().ToList();
        }

        public Veiculo GetById(int id)
        {
            return connection.Find<Veiculo>(id);
        }

        public Veiculo GetByPlaca(string placa)
        {
            return connection.Table<Veiculo>().FirstOrDefault(x => x.Placa == placa);
        }
    }
}
