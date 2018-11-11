using System;
using EDBitor.Controllers;
using EDBitor.Model;

namespace EDBitor
{
    class EDBitorApp
    {
        private static Lazy<EDBitorApp> _instance = new Lazy<EDBitorApp>(() => new EDBitorApp());
        public static EDBitorApp Instance
        {
            get { return _instance.Value; }
        }

        private readonly EDBitorModel _model = new EDBitorModel();
        public EDBitorModel Model
        {
            get { return _model; }
        }

        private readonly ControllerLocator _locator = new ControllerLocator();
        public ControllerLocator Locator
        {
            get { return _locator; }
        }

        private EDBitorApp()
        { }

        public void Start()
        {
            _locator.Open<EditorController>();
        }
    }
}
