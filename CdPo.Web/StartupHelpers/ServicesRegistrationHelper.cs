using Castle.Windsor;

using CdPo.Common.Extensions;
using CdPo.Model.Interfaces;
using CdPo.Model.Interfaces.Files;
using CdPo.Model.Interfaces.PrintForms;
using CdPo.PrintForm.Handler.Service;
using CdPo.Web.DataAccess;
using CdPo.Web.Services.Files;
using CdPo.Web.Services.Storage;

namespace CdPo.Web.StartupHelpers;

/// <summary>
/// Класс-помощник для регистрации сервисов
/// </summary>
public static class ServicesRegistrationHelper
{
    /// <summary>
    /// Зарегистрировать необходимые сервисы
    /// </summary>
    /// <param name="container">Контейнер</param>
    public static void RegisterServices(IWindsorContainer container)
    {
        container.RegisterTransient(typeof(IGenericRepository<>), typeof(EfGenericRepository<>));
        container.RegisterTransient<IFileManager, FileManager>();
        container.RegisterTransient<IFileProvider, LocalFileProvider>();
        container.RegisterTransient<IFileMetadataRepository, EfFileMetadataRepository>();
        container.RegisterTransient<IPrintFileService, PrintFileService>();
        container.RegisterSingleton<IDataStore, DataContext>(nameof(IDataStore));
        container.RegisterSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}