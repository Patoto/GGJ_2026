using System;

namespace PortalRollerCoaster
{
	[Serializable]
	public class PersistentData
	{
		public static Action onAudioOnChanged;

		public bool audioOn { get; private set; }
		public int currentLevelCounter { get; private set; }

		private void Save() 
		{
			GameManager.instance.persistentDataManager.Save();
		}

		public void SetupPersistentDataLists() { }

		public void ToggleAudioOn(bool on)
		{
			bool audioOnChanged = on != audioOn;
			audioOn = on;
			if (audioOnChanged)
			{
				onAudioOnChanged?.Invoke();
			}
		}

		public void AddCurrentLevelCounter(int amount = 1)
		{
			SetCurrentLevelCounter(currentLevelCounter + amount);
		}

		public void SetCurrentLevelCounter(int currentLevelCounter)
		{
			this.currentLevelCounter = currentLevelCounter;
			Save();
		}
	}
}