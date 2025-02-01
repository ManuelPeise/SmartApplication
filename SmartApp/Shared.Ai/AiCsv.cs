using Shared.Ai.Models;

namespace Shared.Ai
{
    public class AiCsv
    {
        private readonly string _targetPath;
      
        public AiCsv(string targetPath)
        {
            _targetPath = targetPath;

            var folder = Path.GetDirectoryName(targetPath);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        public void GenerateFolderPredictionCsv(List<FolderMappingModel> models)
        {
            var csvHeader = $"{nameof(FolderMappingModel.Domain)}\t{nameof(FolderMappingModel.Address)}\t{nameof(FolderMappingModel.FolderId)}";

            var rows = new List<string> { csvHeader };

            models.ForEach(model =>
            {
                rows.Add(GetFolderCsvRow(model));
            });

            File.WriteAllLines(_targetPath, rows);
        }

        public FileData GetFileData()
        {
            if (File.Exists(_targetPath))
            {
                var file = new FileInfo(_targetPath);

                return new FileData
                {
                    Size = (int)file.Length,
                    LastModification = $"{file.LastWriteTimeUtc.ToString("dd.MM.yyyy HH:mm")} (UTC)"
                };
            }

            return new FileData
            {
                Size = 0,
                LastModification = "n/a"
            };
        }
        
        private string GetFolderCsvRow(FolderMappingModel model)
        {
            return $"{model.Domain}\t{model.Address}\t{model.FolderId}";
        }

        
    }
}
