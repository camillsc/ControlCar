using System.Threading.Tasks;
using ControlCar.Controllers;
using ControlCar.Models;

namespace ControlCar.Views;

public partial class pgListagem : ContentPage
{
    private VeiculoController veiculoController;

    public pgListagem()
	{
		InitializeComponent();

        veiculoController = new VeiculoController();

        AtualizarListView();
	}

    private void AtualizarListView()
    {
        lsvLista.ItemsSource = veiculoController.GetAll();
    }

    private async void btnPagamento_Clicked(object sender, TappedEventArgs e)
    {
        TappedEventArgs tapped = (TappedEventArgs)e;

        if (tapped.Parameter is Veiculo veiculo)
        {
            // Se o status de pagamento for "Pago", perguntar se o usu�rio deseja remover o pagamento
            if (veiculo.Pago == "Pago")
            {
                // Exibe a confirma��o para remover o pagamento
                bool confirmacao = await DisplayAlert("Remover Pagamento", "Deseja remover o pagamento e a data de sa�da?", "Sim", "N�o");

                // Se o usu�rio confirmar, remove o pagamento
                if (confirmacao)
                {
                    veiculo.Pago = "N�o pago";  // Altera o status para "N�o pago"
                    veiculo.DataHoraSaida = null;  // Limpa a data de sa�da

                    // Atualiza o ve�culo no banco de dados
                    bool sucesso = veiculoController.Update(veiculo);

                    if (sucesso)
                    {
                        await DisplayAlert("Sucesso", "Pagamento removido com sucesso.", "OK");
                        AtualizarListView();  // Atualiza a listagem ap�s a altera��o
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Falha ao remover o pagamento.", "OK");
                    }
                }
            }
            else
            {
                // Caso o pagamento n�o tenha sido feito ainda, pergunta se deseja marcar como "Pago"
                bool confirmacao = await DisplayAlert("Confirmar Pagamento", "Deseja alterar o status para 'Pago'?", "Sim", "N�o");

                if (confirmacao)
                {
                    // Altera o status de pagamento para "Pago" e registra a data de sa�da
                    veiculo.Pago = "Pago";  // Atualiza o status para 'Pago'
                    veiculo.DataHoraSaida = DateTime.Now;  // Registra a data e hora de sa�da como o momento atual

                    // Atualiza o ve�culo no banco de dados
                    bool sucesso = veiculoController.Update(veiculo);

                    if (sucesso)
                    {
                        // Exibe uma mensagem de sucesso
                        await DisplayAlert("Sucesso", "Status de pagamento atualizado.", "OK");

                        // Atualiza a listagem ap�s a altera��o
                        AtualizarListView();
                    }
                    else
                    {
                        // Exibe uma mensagem de erro se a atualiza��o falhar
                        await DisplayAlert("Erro", "Falha ao atualizar o pagamento.", "OK");
                    }
                }
            }
        }
    }

    private void btnbFiltrar_Clicked(object sender, EventArgs e)
    {
        string Placa = txtFiltroPlaca.Text;
        string statusFiltro = pickerStatusPagamento.SelectedItem?.ToString() ?? "Todos";
        DateTime dataFiltro = dateFiltro.Date;

        lsvLista.ItemsSource = veiculoController.FiltrarVeiculos(Placa, dataFiltro, statusFiltro);
    }

    private async void btnExcluir_Clicked(object sender, TappedEventArgs e)
    {
        TappedEventArgs tapped = (TappedEventArgs)e;

        if (tapped.Parameter is Veiculo registro)
        {
            bool decisao = await DisplayAlert("Confirma��o", "Deseja realmente excluir o registro selecionado?", "Sim", "N�o");

            if (decisao)
            {
                veiculoController.Delete(registro);
                AtualizarListView();
            }
        }
    }
}