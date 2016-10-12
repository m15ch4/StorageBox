namespace StorageBox.Shell.ViewModels
{
    using Login;
    using Models;
    using StorageBox.Framework;
    using System.Collections.Generic;
    using System.Composition;
    using System.Linq;
    using System;

    public class ShellViewModel : Caliburn.Micro.Conductor<IWorkspace>.Collection.OneActive, IShell {


        [ImportingConstructor]
        public ShellViewModel(IEnumerable<IWorkspace> workspaces)
        {
            DisplayName = "StorageBox ver. 2.0";

            Items.AddRange(workspaces);
            //Console.WriteLine("After Items.AddRange(workspaces);");
            ActivateItem(Items[0]);
            //Console.WriteLine("After ActivateItem(Items[0]);");
            ((LoginViewModel)Items[0]).Shell = this;
            //Console.WriteLine("End of shellviewmodel constructor");
        }

        public override void CanClose(Action<bool> callback)
        {
            base.CanClose(callback);
            //callback(false);
            //base.CanClose(callback);
        }

        public void Login()
        {
            UserSession.sbuser = null;
            UserSession.beginDate = null;
            NotifyOfPropertyChange(() => UserName);
            ActivateItem(Items[0]);
        }

        public void Orders()
        {
            ActivateItem(Items[1]);
        }

        public void Additions()
        {
            ActivateItem(Items[2]);
        }

        public void setUserName()
        {
            NotifyOfPropertyChange(() => UserName);
        }

        public string UserName
        {
            get
            {
                string userName = "";
                if (UserSession.sbuser != null)
                {
                    userName = UserSession.sbuser.FirstName + " " + UserSession.sbuser.LastName;
                }
                return userName;
            }
        }



    }
}