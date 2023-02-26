using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ch.sycoforge.Unity.Editor.UI
{
    public delegate bool FixDelegate(object context);

    [Flags]
    public enum EntryType
    {
        None = 0,
        Info = 1,
        Event = 2,
        Warning = 4,
        Error = 8,
        Success = 16

    }

    [Serializable]
    public sealed class ConsoleEntry
    {
        //---------------------------
        // Properties
        //---------------------------
        public EntryType Type
        {
            get { return type; }
        }

        public string Message
        {
            get { return message; }
        }

        public object Context
        {
            get { return context; }
        }

        public int ThrowCount
        {
            get { return throwCount; }
        }

        public bool HasFixRoutine
        {
            get { return fixRoutine != null; }
        }

        //---------------------------
        // Properties
        //---------------------------
        private EntryType type;
        private string message;
        private object context;
        private int throwCount = 1;
        private FixDelegate fixRoutine;

        //---------------------------
        // Constructor
        //---------------------------

        public ConsoleEntry(string message, EntryType type, object context, FixDelegate fixRoutine = null)
        {
            this.message = message;
            this.type = type;
            this.context = context;
            this.fixRoutine = fixRoutine;
        }

        //---------------------------
        // Methods
        //---------------------------

        public bool TryFixing()
        {
            bool success = false;

            if(HasFixRoutine)
            {
                success = fixRoutine(context);
            }

            return success;
        }

        public void Increase()
        {
            throwCount++;
        }

        public override int GetHashCode()
        {
            int subhash = context.GetHashCode() ^ message.GetHashCode();
            return (subhash << 2) | (int)type;
        }
    }
}
