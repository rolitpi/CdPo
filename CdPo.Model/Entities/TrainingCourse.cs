﻿using CdPo.Common.Entity;

namespace CdPo.Model.Entities;

/// <summary>
/// Учебный курс
/// </summary>
public class TrainingCourse: BaseEntity
{
    /// <summary>
    /// Группа обучающихся
    /// </summary>
    public virtual Group Group { get; set; }
    
    /// <summary>
    /// Преподаватель
    /// </summary>
    public virtual Staff Teacher { get; set; }
    
    /// <summary>
    /// Дисциплина курса
    /// </summary>
    public virtual Discipline Discipline { get; set; }
    
    /// <summary>
    /// Дата начала обучения
    /// </summary>
    public DateTime TrainingStartDate { get; set; }
}