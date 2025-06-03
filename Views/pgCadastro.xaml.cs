using ControlCar.Controllers;
using ControlCar.Models;
using ControlCar.Services;
using Microsoft.Maui.Media;
using System;
using System.Threading.Tasks;

namespace ControlCar.Views
{
    public partial class pgCadastro : ContentPage
    {
        private VeiculoController veiculoController;
        private string caminhoImagemSelecionada = "";

        public pgCadastro()
        {
            InitializeComponent();
            veiculoController = new VeiculoController();
            AtualizarDataEntrada();
        }

        private void AtualizarDataEntrada()
        {
            lblDataEntrada.Text = $"Data e Hora de Entrada: {DateTime.Now:dd/MM/yyyy HH:mm}";
        }

        private async void btnSelecionar_Clicked(object sender, EventArgs e)
        {
            try
            {
                var resultado = await MediaPicker.PickPhotoAsync();
                if (resultado != null)
                {
                    caminhoImagemSelecionada = resultado.FullPath;
                    imgSelecionada.Source = caminhoImagemSelecionada;
                    btnRemover.IsVisible = true;
                }
            }
            catch
            {
                await DisplayAlert("Erro", "Falha ao selecionar a imagem.", "OK");
            }
        }

        private void btnRemover_Clicked(object sender, EventArgs e)
        {
            imgSelecionada.Source = null;
            caminhoImagemSelecionada = "";
            btnRemover.IsVisible = false;
        }

        private async void btnSalvar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlaca.Text) ||
                string.IsNullOrWhiteSpace(txtMarca.Text) ||
                string.IsNullOrWhiteSpace(txtModelo.Text) ||
                string.IsNullOrWhiteSpace(txtCor.Text) ||
                string.IsNullOrWhiteSpace(txtNomeProprietario.Text) ||
                pickerTipo.SelectedIndex == -1)
            {
                await DisplayAlert("Atenção", "Preencha todos os campos obrigatórios.", "OK");
                return;
            }

            Veiculo veiculo = new Veiculo
            {
                Placa = txtPlaca.Text.Trim().ToUpper(),
                Marca = txtMarca.Text.Trim(),
                Modelo = txtModelo.Text.Trim(),
                Cor = txtCor.Text.Trim(),
                NomeProprietario = txtNomeProprietario.Text.Trim(),
                Tipo = pickerTipo.SelectedItem.ToString(),
                FotoPath = caminhoImagemSelecionada,
                DataHoraEntrada = DateTime.Now,
                Pago = false,
                DataHoraSaida = null
            };

            bool sucesso;
            var veiculoExistente = veiculoController.GetByPlaca(veiculo.Placa);

            if (veiculoExistente == null)
            {
                sucesso = veiculoController.Insert(veiculo);
            }
            else
            {
                veiculo.Id = veiculoExistente.Id;
                sucesso = veiculoController.Update(veiculo);
            }

            if (sucesso)
            {
                await DisplayAlert("Sucesso", "Veículo salvo com sucesso.", "OK");
                LimparCampos();
                AtualizarDataEntrada();
            }
            else
            {
                await DisplayAlert("Erro", "Falha ao salvar o veículo.", "OK");
            }
        }

        private void LimparCampos()
        {
            txtPlaca.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtCor.Text = "";
            txtNomeProprietario.Text = "";
            pickerTipo.SelectedIndex = -1;
            imgSelecionada.Source = null;
            btnRemover.IsVisible = false;
            caminhoImagemSelecionada = "";
        }
    }
}
