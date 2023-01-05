using CdPo.Model.Entities;
using CdPo.Model.Interfaces;
using CdPo.Model.Interfaces.Files;
using CdPo.Web.DataAccess;

namespace CdPo.Web.Services.Files;

///<inheritdoc/>>
public class EfFileMetadataRepository: IFileMetadataRepository
{
    private readonly DataContext _dataContext;
    
    public EfFileMetadataRepository(DataContext dataContext) => _dataContext = dataContext;

    ///<inheritdoc/>>
    public async Task<IFileMetadata> CreateFileMetadataAsync(
        string fileName,
        string extension,
        long size,
        string cachedName,
        CancellationToken cancellationToken = default)
    {
        var file = new FileMetadata
        {
            Extension = extension,
            Name = fileName,
            Size = size,
            CachedName = cachedName
        };

        var entity = await _dataContext.Files.AddAsync(file, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
        return entity.Entity;
    }

    ///<inheritdoc/>>
    public async Task<IFileMetadata> GetFileMetadataAsync(long id, CancellationToken cancellationToken = default) 
        => await _dataContext.Files.FindAsync(id);
}