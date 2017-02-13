using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SportsStore.Models.Entities;
using System.Data.Entity;

namespace SportsStore.Concrete
{
    //Данный класс связвает нашу модель(Product) с БД
    //Этот класс будет автоматически определять свойство для каждой таблицы в базе данных, с которой мы будем работать.
    //! Мы добавляем строку подключения к базе данных в файл Web.config !
    public class EFDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } //То есть мы одноый строкой подключаемя к БД и вытягиваем оттуда слбцы и их значения
    }
}


/*
 * Мы определяем модель и логику хранения данных в проекте SportsStore.Domain, но информацию о
подключении к базе данных помещаем в файле Web.config в проекте
SportsStore.WebUI.
 */
