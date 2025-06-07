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
            // Se o status de pagamento for "Pago", perguntar se o usuário deseja remover o pagamento
            if (veiculo.Pago == "Pago")
            {
                // Exibe a confirmação para remover o pagamento
                bool confirmacao = await DisplayAlert("Remover Pagamento", "Deseja remover o pagamento e a data de saída?", "Sim", "Não");

                // Se o usuário confirmar, remove o pagamento
                if (confirmacao)
                {
                    veiculo.Pago = "Não pago";  // Altera o status para "Não pago"
                    veiculo.DataHoraSaida = null;  // Limpa a data de saída

                    // Atualiza o veículo no banco de dados
                    bool sucesso = veiculoController.Update(veiculo);

                    if (sucesso)
                    {
                        await DisplayAlert("Sucesso", "Pagamento removido com sucesso.", "OK");
                        AtualizarListView();  // Atualiza a listagem após a alteração
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Falha ao remover o pagamento.", "OK");
                    }
                }
            }
            else
            {
                // Caso o pagamento não tenha sido feito ainda, pergunta se deseja marcar como "Pago"
                bool confirmacao = await DisplayAlert("Confirmar Pagamento", "Deseja alterar o status para 'Pago'?", "Sim", "Não");

                if (confirmacao)
                {
                    // Altera o status de pagamento para "Pago" e registra a data de saída
                    veiculo.Pago = "Pago";  // Atualiza o status para 'Pago'
                    veiculo.DataHoraSaida = DateTime.Now;  // Registra a data e hora de saída como o momento atual

                    // Atualiza o veículo no banco de dados
                    bool sucesso = veiculoController.Update(veiculo);

                    if (sucesso)
                    {
                        // Exibe uma mensagem de sucesso
                        await DisplayAlert("Sucesso", "Status de pagamento atualizado.", "OK");

                        // Atualiza a listagem após a alteração
                        AtualizarListView();
                    }
                    else
                    {
                        // Exibe uma mensagem de erro se a atualização falhar
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
            bool decisao = await DisplayAlert("Confirmação", "Deseja realmente excluir o registro selecionado?", "Sim", "Não");

            if (decisao)
            {
                veiculoController.Delete(registro);
                AtualizarListView();
            }
        }
    }
}