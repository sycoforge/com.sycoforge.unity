using System;

namespace ch.sycoforge.Unity.Editor.Persistence
{
	[System.Serializable]
	public abstract class EditorResource
	{
		public int ID
		{
			get; set;	
		}
		
		public EditorResource (int id)
		{
			ID = id;
		}
	}
}

