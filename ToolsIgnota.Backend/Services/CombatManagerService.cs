using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using ToolsIgnota.Data.Abstractions;
using ToolsIgnota.Data.Models;
using ToolsIgnota.Data.Utilities;
using Windows.Storage;

namespace ToolsIgnota.Data.Services
{
    public class CombatManagerService : ICombatManagerService
    {
        private CombatManagerConnection _connection;
        private ISubject<IEnumerable<CMCreature>> _creaturesSubject = new BehaviorSubject<IEnumerable<CMCreature>>(Enumerable.Empty<CMCreature>());
        private ISubject<int?> _roundSubject = new BehaviorSubject<int?>(0);
        
        private readonly IWindowService _windowService;

        public CombatManagerService(IWindowService windowService)
        {
            _windowService = windowService ?? throw new ArgumentNullException(nameof(windowService));
        }

        public IObservable<IEnumerable<CMCreature>> GetCreatures()
        {
            Connect();
            return _creaturesSubject.AsObservable();
        }

        public IObservable<int?> GetRound()
        {
            Connect();
            return _roundSubject.AsObservable();
        }

        public void SetUri(string uri)
        {
            if (uri == LocalSettings.CombatManagerUri)
                return;

            LocalSettings.CombatManagerUri = uri;
            Reconnect();
        }

        public string GetUri()
        {
            return LocalSettings.CombatManagerUri;
        }

        private void Connect()
        {
            if (_connection != null)
                return;

            _connection = new CombatManagerConnection(LocalSettings.CombatManagerUri);
            _connection.GetState().Subscribe(state =>
            {
                _windowService.GetDispatcher().TryEnqueue((Action)(() =>
                {
                    _creaturesSubject.OnNext(state.Data.CombatList);
                    _roundSubject.OnNext(state.Data.Round);
                }));
            });
        }

        private void Disconnect()
        {
            if (_connection == null)
                return;

            _connection.Dispose();
            _connection = null;
        }

        private void Reconnect()
        {
            if(_connection != null)
            {
                Disconnect();
                Connect();
            }
        }

        ~CombatManagerService()
        {
            Disconnect();
        }
    }
}
