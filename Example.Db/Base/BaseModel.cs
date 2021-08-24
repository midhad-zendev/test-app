using System;
using System.Collections;

namespace Example.Db.Base
{
    public abstract class BaseModel : IDisposable
    {
        private Hashtable _items;

        public object this[string name]
        {
            get
            {
                if (_items == null)
                    return null;

                return _items[name];
            }
            set
            {
                if (_items == null)
                    _items = new Hashtable();
                _items[name] = value;
            }
        }

        internal ConnectionContainer DbConnection { get; set; }

        public void Dispose()
        {
        }
    }
}
