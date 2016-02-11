// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using System.IO;
using UpdateApp.AppData;
using System.Threading.Tasks;
using System.Text;
using System.Runtime.Serialization.Json;
using Windows.System;
using Windows.Storage.Streams;

namespace UpdateApp
{
    public sealed class StartupTask : IBackgroundTask
    {
        private AppDataTelemetry telemetrySender;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            this.Initialize();
            
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(20000);
                    telemetrySender.UpdateData();
                    telemetrySender.BroadcastData();
                    await getLatestVersion();
                }
            }).Wait();            
        }              

        private void Initialize()
        {

            string appVersion = (Windows.Storage.ApplicationData.Current.LocalSettings.Values["Version"] != null) ? Windows.Storage.ApplicationData.Current.LocalSettings.Values["Version"].ToString():"1";
            telemetrySender = new AppDataTelemetry(appVersion, "Denis");
        }
        private async Task getLatestVersion()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Pi");
            var task = await client.GetAsync("http://TR22Demodacrook.azurewebsites.net/api/AppVersionMetadatas/GetLatest");
            var jsonString = await task.Content.ReadAsStringAsync();

            var rc = new MemoryStream(Encoding.Unicode.GetBytes(jsonString));
            var rserializer = new DataContractJsonSerializer(typeof(AppVersionMetadata));
            var ResultDeserialized = (AppVersionMetadata)rserializer.ReadObject(rc);
            PackageDeployment pd = new PackageDeployment();
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values["Version"] == null 
                || !Windows.Storage.ApplicationData.Current.LocalSettings.Values["Version"].ToString().Equals(ResultDeserialized.Version))
            {
                await Get_File();
                await Update("TRIoTDemoAppf91a9e24eba4_32wtj2cd5n3b2!App", ResultDeserialized.Version);
            }

        }



        private async Task Get_File()
        {
            try
            {
               using (var fileStream = await Windows.Storage.ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync("package.appx", Windows.Storage.CreationCollisionOption.ReplaceExisting))
                {
                    var webStream = await new HttpClient().GetStreamAsync("https://dcdeploymenttr.blob.core.windows.net/client/TRIoTDemoApp_1.0.5.0_ARM_Debug.appx");
                    await webStream.CopyToAsync(fileStream);
                    webStream.Dispose();
                }
            }
            catch(Exception e)
            {
            }
        }

        private async Task Update(string name, string version)
        {
            try
            {                
                await RunProcess(@"Add-AppxPackage " + Windows.Storage.ApplicationData.Current.LocalFolder.Path + @"\package.appx ");
                Windows.Storage.ApplicationData.Current.LocalSettings.Values.Add("Version", version);
                await RunProcess(@"iotstartup add headed " + name);                
                await RunProcess(@"shutdown /r /t 0");
            }
            catch (Exception e)
            {
            }
        }

        private async Task RunProcess(string args)
        {
            try
            {
                string ProcessExitCode = "";
                var options = new ProcessLauncherOptions();
                var standardOutput = new InMemoryRandomAccessStream();
                var standardError = new InMemoryRandomAccessStream();
                options.StandardOutput = standardOutput;
                options.StandardError = standardError;

                var result = await ProcessLauncher.RunToCompletionAsync(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe", args, options);

                ProcessExitCode += "Process Exit Code: " + result.ExitCode;

            }
            catch (Exception e)
            {

            }
        }
    }
}
