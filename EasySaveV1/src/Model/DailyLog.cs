using System;

namespace BackUp.Model
{
    public sealed class DailyLog
    {
        public list filelist { get; set; }
        public string logpath { get; set; }

        public DailyLog getInstance()
        {

        }

        public void createDailyLog()
        {

        }

        public void addFileToLog(IFile)
        {

        }
    }
}


{
    public sealed class DailyLog
    {
        // Instance unique, thread-safe et initialis�e � la demande
        private static readonly Lazy<DailyLog> instance = new Lazy<DailyLog>(() => new DailyLog());

        // Propri�t� publique d�acc�s global � l�instance
        public static DailyLog Instance => instance.Value;

        // Constructeur priv� : emp�che la cr�ation externe d�instances
        private DailyLog()
        {
            // <<< � MODIFIER : initialisation personnalis�e ici
        }

        // M�thodes et propri�t�s m�tier
       


    }
}