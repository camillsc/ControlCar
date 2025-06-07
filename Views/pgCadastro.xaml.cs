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

            Veiculo veiculoExistente = veiculoController.GetByPlaca(txtPlaca.Text.Trim().ToUpper());

            if (veiculoExistente != null)
            {
                txtMarca.Text = veiculoExistente.Marca;
                txtModelo.Text = veiculoExistente.Modelo;
                txtCor.Text = veiculoExistente.Cor;
                txtNomeProprietario.Text = veiculoExistente.NomeProprietario;
                pickerTipo.SelectedItem = veiculoExistente.Tipo;
                caminhoImagemSelecionada = veiculoExistente.FotoPath;
                imgSelecionada.Source = caminhoImagemSelecionada;
                lblDataEntrada.Text = $"Data e Hora de Entrada: {veiculoExistente.DataHoraEntrada:dd/MM/yyyy HH:mm}";
            }
            else
            {
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
                    Pago = "Não pago",
                    DataHoraSaida = null
                };

                bool sucesso = veiculoController.Insert(veiculo);

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
        }

        private void LimparCampos()
        {
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtCor.Text = "";
            txtNomeProprietario.Text = "";
            pickerTipo.SelectedIndex = -1;
            imgSelecionada.Source = null;
            btnRemover.IsVisible = false;
            caminhoImagemSelecionada = "";
        }    

        private void Placa_TextChanged(object sender, TextChangedEventArgs e)
        {
            string placaDigitada = txtPlaca.Text?.Trim().ToUpper();

            if (!string.IsNullOrEmpty(placaDigitada))
            {
                Veiculo veiculoExistente = veiculoController.GetByPlaca(placaDigitada);

                if (veiculoExistente != null)
                {
                    txtMarca.Text = veiculoExistente.Marca;
                    txtModelo.Text = veiculoExistente.Modelo;
                    txtCor.Text = veiculoExistente.Cor;
                    txtNomeProprietario.Text = veiculoExistente.NomeProprietario;
                    pickerTipo.SelectedItem = veiculoExistente.Tipo;
                    caminhoImagemSelecionada = veiculoExistente.FotoPath;
                    imgSelecionada.Source = caminhoImagemSelecionada;
                    lblDataEntrada.Text = $"Data e Hora de Entrada: {veiculoExistente.DataHoraEntrada:dd/MM/yyyy HH:mm}";
                }
                else
                {
                    LimparCampos();
                }
            }
        }
    }
}
