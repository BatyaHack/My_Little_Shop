using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Moq;

using SportsStore.Models.Abstract;
using SportsStore.Models.Entities;
using SportsStore.Concrete;

namespace SportsStore.Infrastructure
{
    //Создаем зависимость интерфейс-реализация в проекте EssentialTools мы делали эту зависиость по другому
    // в данном случае мы используем создание зависисмости "пользовательская фабрика контроллеров"
    // можно сказать что данным кодом мы переделываем поведение платформы MVC 

    // Т.е. укзаываем с помошью каких действий будут создаваться классы в контроллерах


    //Посмотреть в Global.asax
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        // реализация пользовательской фабрики контроллеров,
        // наследуясь от фабрики используемой по умолчанию
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            // получение объекта контроллера из контейнера
            // используя его тип
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            // конфигурирование контейнера


            //Пишем заглушку для интерфейса IProductsRepository
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();

            // Так как IProductsRepository должен осуществлять обращение к БД и получать данный
            // Мы типа делам то что интерфейс нам вернул данные
            //mock.Setup(m => m.Products).Returns(new List<Product>
            //{
            //     new Product { Name = "Football", Price = 25 },
            //     new Product { Name = "Surf board", Price = 179 },
            //     new Product { Name = "Running shoes", Price = 95 }
            //}.AsQueryable());

            //Устанавливаем, что мы должны получать эту заглушку(реализацию), когда спользуем интерфейс  IProductsRepository в контроллере            
            // В прогормме EssentialTools мы использовали .To<КлассРеализация> тогда объект КлассРеализация будет создаваться каждый раз при выозове интерфейс
            // Когда мы используем ToConstant то мы будем создавать иметацию объекта mock (но мог быть бы и класс)
            /*ninjectKernel.Bind<IProductsRepository>().ToConstant(mock.Object);*/ //после того как мы реализовали реальный интерфейс, нам уже не надо использовать интерфейс-заглушку


            ninjectKernel.Bind<IProductsRepository>().To<EFProductRepository>(); //Делаем реальную привязку, что при вызова IProductsRepository должна использоваться реализация EFProductRepository


            //Связываем интерфейс IOrderProcessor с реализацией EmailOrderProcessor
            EmailSettings settings = new EmailSettings();

            ninjectKernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", settings);
        }

    }
}