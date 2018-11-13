using System;
using EDBitor.Controllers;

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

        private readonly ControllerInstantiator _instantiator = new ControllerInstantiator();
        public ControllerInstantiator Instantiator
        {
            get { return _instantiator; }
        }

        private EDBitorApp()
        { }

        public void Start()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", "../../../Data");
            _instantiator.Open<EditorController>();
        }
    }
}
