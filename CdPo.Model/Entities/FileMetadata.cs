using CdPo.Common.Entity;
using CdPo.Model.Interfaces;
using CdPo.Model.Interfaces.Files;

namespace CdPo.Model.Entities;

/// <inheritdoc cref="IFileMetadata"/>
public class FileMetadata : BaseEntity, IFileMetadata
{
    /// <inheritdoc/>
    public string Name { get; set; }
    
    /// <inheritdoc/>
    public string Extension { get; set; }
    
    /// <inheritdoc/>
    public long Size { get; set; }
    
    /// <inheritdoc/>
    public string FullName { get; }

    /// <inheritdoc/>
    public string CachedName { get; set; }
}