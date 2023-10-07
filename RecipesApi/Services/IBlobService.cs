namespace RecipesApi.Services;

public interface IBlobService
{
    Task<Uri> UploadFileBlobAsync(string blobContainerName, Stream content, string contentType, string fileName);
}