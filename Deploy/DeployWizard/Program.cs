﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using Deploy.Lib.Databases;
using Deploy.Lib.Deployment.ProfileManagement;
using Deploy.Lib.FileManagement;
using DeployWizard.Lib.AutoComplete.FileSystem;
using DeployWizard.Lib.Controllers;
using DeployWizard.Lib.Models;
using DeployWizard.Lib.Steps;
using DeployWizard.Lib.Steps.Views;
using DeployWizard.Wpf.Steps.Views;
using DeployWizard.Wpf.Views;


namespace DeployWizard
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            PrintPlatformInfo();
            if (RunningWindows())
            {
                var view = new WpfDeployWizardView();
                var model = new WizardModel();
                var steps = GetSteps(model);
                var finishStep = GetFinishStep(model);
                new DeployWizardController(model, view, ProfileManager.Instance, steps, finishStep);
                new Application().Run(view);
            }
        }

        private static IWizardStep<IStepView> GetFinishStep(WizardModel model)
        {
            return new FinishStep(model, new WpfFinishStepView());
        }

        private static void PrintPlatformInfo()
        {
            Console.WriteLine(GetPlatformInfo());
        }

        private static string GetPlatformInfo()
        {
            return new StringBuilder()
                .Append("OS version: ").AppendLine(Environment.OSVersion.ToString())
                .Append("Process: ").AppendLine(Process.GetCurrentProcess().ProcessName)
                .ToString();
        }

        private static IEnumerable<IWizardStep<IStepView>> GetSteps(WizardModel model)
        {
            var fileSystemManager = new FileSystemManager();
            var folderAutoCompleteProvider = new FileSystemAutoCompleteProvider(fileSystemManager, CompletionType.FoldersOnly);
            var databaseTypes = new[] {"sqlserver", "mysql", "sqlite", "oracle"};
            var steps = new List<IWizardStep<IStepView>>
                {
                    new SelectProfileStep(model, new WpfSelectProfileStepView(), ProfileManager.Instance),
                    new SelectPackageStep(model, new WpfSelectPackageStepView(), fileSystemManager),
                    new SetUpBackupStep(model, new WpfSetUpBackupStepView(folderAutoCompleteProvider),fileSystemManager),
                    new SetUpDeployStatusStep(model, new WpfSetUpDeployStatusStepView(), fileSystemManager),
                    new SetUpGenerateWebConfigStep(model, new WpfSetUpGenerateWebConfigStepView(),fileSystemManager),
                    new SetUpDestinationStep(model, new WpfSetUpDestinationStepView(), fileSystemManager),
                    new SetUpMigrationStep(new DatabaseConnectionTester(), model,new WpfSetUpMigrationStepView(databaseTypes)),
                    new SummaryStep(model, new WpfSummaryStepView())
                };
            return steps;
        }

        private static bool RunningWindows()
        {
            return true;
        }
    }
}
