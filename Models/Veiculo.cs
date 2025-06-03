using SQLite;

namespace ControlCar.Models
{
    public class Veiculo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique, NotNull]
        public string Placa { get; set; }

        [NotNull]
        public string Marca { get; set; }

        [NotNull]
        public string Modelo { get; set; }

        [NotNull]
        public string Cor { get; set; }

        [NotNull]
        public string NomeProprietario { get; set; }

        [NotNull]
        public string Tipo { get; set; }

        public string FotoPath { get; set; }

        [NotNull]
        public DateTime DataHoraEntrada { get; set; }

        public DateTime? DataHoraSaida { get; set; }

        public bool Pago { get; set; }
    }
}
