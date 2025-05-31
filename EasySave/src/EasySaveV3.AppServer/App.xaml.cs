using EasySaveV3.AppServer.Model.Interfaces;
using EasySaveV3.AppServer.Model.Services;
using EasySaveV3.AppServer.Commands;
using EasySaveV3.AppServer.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EasySaveV3.AppServer
{
    public partial class App : Application
    {
        private SocketServer? _server;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialisation des dépendances
            ILocalizer localizer = new Localizer();
            var handlers = new List<ISocketCommandHandler>
            {
                new LanguageCommandHandler(localizer),
                // tu pourras en rajouter ici
            };

            // Démarrage du serveur
            _server = new SocketServer(handlers);

            try
            {
                await _server.StartAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur au démarrage du serveur socket : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Tu peux ici arrêter ton serveur proprement si besoin
            base.OnExit(e);
        }
    }
}
