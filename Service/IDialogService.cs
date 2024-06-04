namespace VideoPlayerApplication.Service
{
    public interface IDialogService
    {
        string FilePath { get; set; }
        bool OpenFileDialog(string videoFilesFilters);
        bool SaveFileDialog(string videoFilesFilters);

    }
}
