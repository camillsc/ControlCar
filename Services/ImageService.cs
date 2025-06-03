namespace ControlCar.Services
{

    public static class ImageService
    {
        public static async Task<string> SelecionarImagem()
        {
            string diretorio = "";

            var imagemSelecionada = await MediaPicker.PickPhotoAsync();

            if (imagemSelecionada != null)
            {
                diretorio = imagemSelecionada.FullPath;
            }

            return diretorio;
        }

        public static string CopiarImagem(string sDirOriginal)
        {
            var DiretorioDestino = "";

            var novoDiretorio = Path.Combine(AppContext.BaseDirectory, "Imagens");

            if (!Directory.Exists(novoDiretorio))
            {
                Directory.CreateDirectory(novoDiretorio);
            }

            DiretorioDestino = Path.Combine(novoDiretorio, Path.GetFileName(sDirOriginal));

            File.Copy(sDirOriginal, DiretorioDestino, overwrite: true);

            return DiretorioDestino;
        }
    }
}
