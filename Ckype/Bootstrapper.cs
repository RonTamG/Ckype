using Caliburn.Micro;
using Ckype.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ckype
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void Configure()
        {
            base.Configure();
            _container.Singleton<IWindowManager, WindowManager>();
            _container.Singleton<ShellViewModel, ShellViewModel>();
            _container.Singleton<ChatScreenViewModel, ChatScreenViewModel>();
        }

        public Bootstrapper()
        {
            Initialize();
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var windowManager = _container.GetInstance<IWindowManager>();
            var shell = _container.GetInstance<ShellViewModel>();

            windowManager.ShowWindow(shell);
        }
    }
}
