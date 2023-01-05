﻿namespace CdPo.Common.Interfaces;

/// <summary>
/// Маркерный интерфейс для использования в случаях, когда нужно в конструктор инжектить много параметров из контейнера.
/// Реализовав интерфейс с необходимыми зависимостями можно, не регистрируя его в основном контейнере, получить необходимые зависимости
/// </summary>
public interface IDependenciesResolutionBag
{
    
}