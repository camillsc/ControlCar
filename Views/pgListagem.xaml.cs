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
            if (veiculo.Pago == "Pago")
            {
                bool confirmacao = await DisplayAlert("Remover Pagamento", "Deseja remover o pagamento e a data de saída?", "Sim", "Não");

                if (confirmacao)
                {
                    veiculo.Pago = "Não pago";  
                    veiculo.DataHoraSaida = null;  

                    bool sucesso = veiculoController.Update(veiculo);

                    if (sucesso)
                    {
                        await DisplayAlert("Sucesso", "Pagamento removido com sucesso.", "OK");
                        AtualizarListView();  
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Falha ao remover o pagamento.", "OK");
                    }
                }
            }
            else
            {
                bool confirmacao = await DisplayAlert("Confirmar Pagamento", "Deseja alterar o status para 'Pago'?", "Sim", "Não");

                if (confirmacao)
                {
                    veiculo.Pago = "Pago";  
                    veiculo.DataHoraSaida = DateTime.Now;  

                    bool sucesso = veiculoController.Update(veiculo);

                    if (sucesso)
                    {
                        await DisplayAlert("Sucesso", "Status de pagamento atualizado.", "OK");

                        AtualizarListView();
                    }
                    else
                    {
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