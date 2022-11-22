using CdPo.Model.Entities;
using CdPo.Model.Interfaces;
using CdPo.Model.Interfaces.Files;

namespace CdPo.Web.Services.Files;

public class EfFileMetadataRepository: IFileMetadataRepository
{
    private readonly IDataContext _dataContext;
    
    public EfFileMetadataRepository(IDataContext dataContext) => _dataContext = dataContext;

    public async Task<IFileMetadata> CreateFileMetadataAsync(
        string fileName,
        string extension,
        long size,
        CancellationToken cancellationToken = default)
    {
        var file = new FileMetadata
        {
            Extension = extension,
            Name = fileName,
            Size = size
        };

        var entity = await _dataContext.Files.AddAsync(file, cancellationToken);
        return entity.Entity;
    }

    public async Task<IFileMetadata> GetFileMetadataAsync(long id, CancellationToken cancellationToken = default) 
        => await _dataContext.Files.FindAsync(id);
}