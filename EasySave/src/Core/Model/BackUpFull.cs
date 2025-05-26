using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;
using Core.Model.Managers;

namespace Core.Model
{
	public class BackUpFull : IBackUpType
	{
		public string Name { get; }
		public string dirSource { get; }
		public string dirTarget {  get; }
		private ILogger _log;
		public BackUpFull(string Name, string dirSource, string dirTarget)
		{
			this._log = Logger.Instance;
			this.Name = Name;
			this.dirSource = dirSource;
			this.dirTarget = dirTarget;
		}
		
		public void Execute()
		{
            Stopwatch jobTimer = Stopwatch.StartNew();
            string message;
			try
			{
				// verifie if the subdirectories exists and create them if necessary
				if (!Directory.Exists(this.dirTarget)) 
				{
					Directory.CreateDirectory(this.dirTarget);
				}
				foreach (string dirPath in Directory.GetDirectories(dirSource, "*", SearchOption.AllDirectories))
				{
					Directory.CreateDirectory(dirPath.Replace(dirSource, dirTarget));
				}

				// Copie all the files to the target dir
				foreach (string fileSource in Directory.GetFiles(dirSource, "*.*", SearchOption.AllDirectories))
				{
					Stopwatch watch = Stopwatch.StartNew();
					string fileTarget = fileSource.Replace(dirSource, dirTarget);
					string fileExtensionsToEncrypty = AppConfigManager.Instance.GetAppConfigParameter("EncryptionExtensions");
					String[] LstFileExtensionsToEncrypt = fileExtensionsToEncrypty.Split(",");
					bool shouldEncrypt = LstFileExtensionsToEncrypt.Any(ext => fileSource.EndsWith(ext.Trim(), StringComparison.OrdinalIgnoreCase));

                    double encryptionTime = 0;
                    if (shouldEncrypt)
					{
						CryptoManager.SetKey("12");
						Stopwatch EncryptTimer = Stopwatch.StartNew();

						try
						{
							CryptoManager.EncryptFile(fileSource);
						}
						catch (Exception ex)
						{
							encryptionTime = -1;
						}
						EncryptTimer.Stop();
						encryptionTime = EncryptTimer.Elapsed.Milliseconds;

						string newFileName = fileSource + ".xor";
						fileTarget += ".xor";
						File.Copy(newFileName, fileTarget, true);
						File.Delete(newFileName);
					}
					else {
						File.Copy(fileSource, fileTarget, true);
					}
					watch.Stop();
					double elapsedMs = watch.ElapsedMilliseconds;
                    WriteDailyLog(fileSource, fileTarget, elapsedMs, encryptionTime);
                }
                jobTimer.Stop();
				message = "Job Succeed!";
				WriteStatusLog(jobTimer.ElapsedMilliseconds, message);
			}
			catch (Exception ex)
            {
                jobTimer.Stop();
                message = "Erreur pendant le backup complet : " + ex.Message.ToString();
                WriteStatusLog(jobTimer.ElapsedMilliseconds, message);
                throw new Exception(message, ex);
            }
        }

        public void WriteDailyLog(string fileSource, string fileTarget, double transfertTime, double encryptionTime)
        {
            FileInfo fileInfo = new FileInfo(fileSource);
            Dictionary<string, object> logEntry = new Dictionary<string, object>
			{
				{ "FileName", Name },
				{ "SourcePath", fileSource },
				{ "TargetPath", fileTarget },
				{ "FileSize", fileInfo.Length },
				{ "FileTransferTime", transfertTime},
				{ "TimeStamp", DateTime.Now.ToString("M/d/yyyy HH:mm:ss") },
				{"encryptionTime", encryptionTime }
			};
			_log.AddLogInfo(LogType.Daily, logEntry);
		}

        public void WriteStatusLog(double jobTimer, string message)
        {
            Dictionary<string, object> logJob = new Dictionary<string, object>
                {
                    { "Name", Name },
                    { "JobTime", jobTimer},
                    { "Result", message },
                    { "TimeStamp", DateTime.Now.ToString("M/d/yyyy HH:mm:ss") }
                };
            _log.AddLogInfo(LogType.Status, logJob);
        }
    }
}