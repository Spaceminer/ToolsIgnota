using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ToolsIgnota.Data.Abstractions;
using ToolsIgnota.Data.Models;

namespace ToolsIgnota.Data.Services
{
    public class CombatManagerService : ICombatManagerService
    {
        private CombatManagerConnection _connection;
        private ISubject<IEnumerable<CMCreature>> _creaturesSubject = new BehaviorSubject<IEnumerable<CMCreature>>(Enumerable.Empty<CMCreature>());

        public IObservable<IEnumerable<CMCreature>> GetCreatures()
        {
            if(_connection == null)
            {
                _connection = new CombatManagerConnection("localhost:12457");
                _connection.GetState().Subscribe(state =>
                {
                    _creaturesSubject.OnNext(state.Data.CombatList);
                });
            }

            return _creaturesSubject.AsObservable();
        }

        ~CombatManagerService()
        {
            _connection?.Dispose();
        }
    }
}
