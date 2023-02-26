using ch.sycoforge.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ch.sycoforge.Unity.Editor.Workspace
{
    /// <summary>
    /// Class for handling selections in a canvas. All selections are defined in canvas space (origin (0/0) is the bottom-left corner).
    /// </summary>
    public sealed class CanvasSelection:IEnumerable<FloatRect>
    {

        //-----------------------------
        // Events
        //-----------------------------
        /// <summary>
        /// Gets called when a selection changed.
        /// </summary>
        public event Action OnSelectionChanged;

        //-----------------------------
        // Properties
        //-----------------------------

        /// <summary>
        /// <c>true</c> if there's a selection, otherwise <c>false</c>.
        /// </summary>
        public bool HasSelection
        {
            get
            {
                return selections.Count > 0;
            }
        }

        /// <summary>
        /// The amount of selections contained.
        /// </summary>
        public int Count
        {
            get
            {
                return selections.Count;
            }
        }

        //-----------------------------
        // Static Fields
        //-----------------------------

        //------------------------------
        // Private Fields
        //------------------------------ 
        private List<FloatRect> selections = new List<FloatRect>();

        //------------------------------
        // Constructor
        //------------------------------ 

        //------------------------------
        // Public Methods
        //------------------------------ 

        public FloatRect GetFirst()
        {
            return selections.FirstOrDefault();
        }

        public bool Contains(Float2 point)
        {
            foreach (FloatRect rect in selections)
            {
                bool inside = rect.Contains(point);

                if (inside)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds a new selection rectangle defined by a start and end rect.
        /// </summary>
        /// <param name="rect">The selection to add.</param>
        /// <returns>The id of the selection</returns>
        public int Add(Float2 start, Float2 end)
        {
            return Add(CreateSelectionRect(start, end));
        }

        /// <summary>
        /// Adds a new selection rectangle.
        /// </summary>
        /// <param name="rect">The selection to add.</param>
        /// <returns>The id of the selection</returns>
        public int Add(FloatRect rect)
        {
            selections.Add(rect);

            ThrowSelectionChanged();

            return selections.Count;
        }

        /// <summary>
        /// Moves the selection by the specified offset.
        /// </summary>
        /// <param name="delta">The offset to move the selection.</param>
        public void Move(Float2 delta)
        {
            for (int i = 0; i < selections.Count; i++)
            {
                FloatRect rect = selections[i];
                rect.xMax += delta.x;
                rect.xMin += delta.x;
                rect.yMax += delta.y;
                rect.yMin += delta.y;
            }

            ThrowSelectionChanged();
        }

        public void Update(int index, FloatRect rect)
        {
            if(index < selections.Count)
            {
                selections[index] = rect;

                ThrowSelectionChanged();
            }
            else
            {
                throw new IndexOutOfRangeException("The selection index is out of bounds");
            }
        }

        public void Update(int index, Float2 start, Float2 end)
        {
            Update(index, CreateSelectionRect(start, end));
        }

        public void UpdateFirst(Float2 start, Float2 end)
        {
            Update(0, CreateSelectionRect(start, end));
        }

        public void UpdateFirst(FloatRect rect)
        {
            Update(0, rect);
        }

        public void Remove(int index)
        {
            if (index < selections.Count)
            {
                selections.RemoveAt(index);
            }
            else
            {
                throw new IndexOutOfRangeException("The selection index is out of bounds");
            }
        }

        public void Clear()
        {
            selections.Clear();

            ThrowSelectionChanged();
        }

        //------------------------------
        // Private Methods
        //------------------------------ 
        private FloatRect CreateSelectionRect(Float2 start, Float2 end)
        {

            Float2 p = new Float2(FloatMath.Min(start.x, end.x), FloatMath.Min(start.y, end.y));


            Float2 size = start - end;
            size.x = FloatMath.Abs(size.x);
            size.y = FloatMath.Abs(size.y);

            //Debug.Log(string.Format("start: {0} end: {1} size:{2}", start, start, size));

            return new FloatRect(p, size);
        }

        public IEnumerator<FloatRect> GetEnumerator()
        {
            return selections.GetEnumerator();

        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void ThrowSelectionChanged()
        {
            if (OnSelectionChanged != null)
            {
                OnSelectionChanged();
            }
        }
    }
}
