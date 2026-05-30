namespace TrySMT
{
    public enum ScreenType
    {
        MainMenu,
        Gameplay
    }
    
    public static class GameState
    {
        public static ScreenType CurrentScreen = ScreenType.MainMenu;
        public static bool ExitGame = false;
    }
}