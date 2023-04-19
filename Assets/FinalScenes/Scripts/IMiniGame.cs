public interface IMiniGame
{
   public string MiniGamePrefabPath { get; }
   public string RigPrefab { get; }
   void ShowInstructions();
   void StartGame();

   void EndGame();

   void RestartGame();

   void PauseGame();

   void LeaveGame();
}
