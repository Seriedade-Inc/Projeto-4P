namespace Projeto4pSharedLibrary.Services
{

    public class BuilderService
    {
        public int GetLoginDays(){return 14;}
        public void CheckAuth()
        {
            if (!File.Exists("AcabaxiPêEniGê.jpg"))
            {
                Environment.Exit(0);
            }
        }

        public bool CheckBlazorWebAuth()
        {
            if (!File.Exists("AcabaxiPêEniGê.jpg"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}