using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Collections;
using System.Web;
using ActiveDirectoryServices;
using NLog;

namespace FlActiveDirectoryServices
{
    public class Users
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Department{ get; set; }
        public string Division{ get; set; }
        public bool isMapped { get; set; }
        public bool isActive{ get; set; }
        public string Timesheet { get; set; }
        public string Manager{ get; set; }
        public int AccountControl { get; set; }
    }

    public class ADServices
    {

        private static Logger _log = LogManager.GetCurrentClassLogger();

        public static List<Users> GetAllUsers()
        {
            List<Users> lstADUsers = new List<Users>();
            
             
                string DomainPath = "LDAP://DC=frontline-consultancy,DC=co,DC=uk";
                DirectoryEntry searchRoot = new DirectoryEntry(DomainPath);
                DirectorySearcher search = new DirectorySearcher(searchRoot);
                //    search.Filter = "(&(objectClass=user)(samaccountname='boltona'))";
               //var userName = "Banksg";
              //  search.Filter = "(&(objectClass=user)(samaccountname=" + userName + "))";
                search.Filter = "(&(objectClass=user)(objectCategory=person))";
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("usergroup");
                search.PropertiesToLoad.Add("manager");
                search.PropertiesToLoad.Add("Enabled");
                search.PropertiesToLoad.Add("department");
                search.PropertiesToLoad.Add("division");
                search.PropertiesToLoad.Add("Enabled");
                search.PropertiesToLoad.Add("mailNickName");
                search.PropertiesToLoad.Add("timeSheet"); 

          //                                   msRTCSIP-UserEnabled
            search.PropertiesToLoad.Add("userAccountControl");
                search.PropertiesToLoad.Add("displayname");//first name
                SearchResult result;
                SearchResultCollection resultCol = search.FindAll();
                if (resultCol != null)
                {
                    for (int counter = 0; counter < resultCol.Count; counter++)
                    {
                    try
                    {
                        string UserNameEmailString = string.Empty;
                        result = resultCol[counter];
                        if (result.Properties.Contains("samaccountname") &&
                            result.Properties.Contains("mail") &&
                            result.Properties.Contains("displayname"))
                        {
                            
                            if (!result.Properties.Contains("manager")  || !result.Properties.Contains("userAccountControl") || !result.Properties.Contains("department"))
                            {
                                _log.Error("Manager Missing count {0} mail {1} Display{2}", (String)result.Properties["samaccountname"][0], (String)result.Properties["mail"][0], (String)result.Properties["displayname"][0]);

                            }

                            Users user= new Users();
                            user.Email = (String)result.Properties["mail"][0];
                            user.UserName = (String)result.Properties["samaccountname"][0];
                            user.DisplayName = (String)result.Properties["displayname"][0];
                            user.AccountControl= (int)result.Properties["userAccountControl"][0];
                            user.Manager= (String)result.Properties["manager"][0];
                            user.Department = (result.Properties.Contains("department"))? (String) result.Properties["department"][0]: "";
                            user.Division=(result.Properties.Contains("division")) ? (String) result.Properties["division"][0]: "";
                            user.Timesheet=(result.Properties.Contains("timeSheet")) ? (String)result.Properties["timeSheet"][0] : "";


                            lstADUsers.Add(user); 
                        }
                        else
                        {

                          
                            _log.Error("No User INfo" + (String)result.Properties["samaccountname"][0]);
                       

                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex.ToString());
                        Console.Write(ex.ToString());
                    }
                }
                }
                return lstADUsers;
            
        }

        public static void UpdateSQLUsers() {

            UpdateUsersDataContext _db = new UpdateUsersDataContext();
            List<Users> usertoUpdate = GetAllUsers();
            foreach(var u in usertoUpdate)
            {
                _db.InsertOrUpdateUser(u.Email, u.UserName, u.DisplayName, u.Department, u.Division, u.isMapped, u.AccountControl.ToString(), u.Manager,u.Timesheet);

            }

        }



    //    public static string GetAllEmail()
    //    {
    //        string DomainPath = "LDAP://DC=frontline-consultancy,DC=co,DC=uk";
    //        DirectoryEntry de = new DirectoryEntry(DomainPath);
    //        DirectorySearcher ds = new DirectorySearcher(de);
    //        string userName = GetUserNet(); // HttpContext.Current.User.Name without domain name
    //        userName = GetUserNet();
    //        ds.Filter = "(objectClass=user)";
    //        ds.PropertiesToLoad.Add("mail");
    //        SearchResultCollection users = ds.FindAll();
    //        //return users.Count.ToString();
    //        foreach (var u in users)
    //        {
    //            Console.WriteLine(u);
    //        }
    //        string email = users[0].Properties["mail"][0].ToString();


    //        return email;

    //    }

    //    public static string GetUserNet()
    //    {
    //        return " BanksG";
    //        //String netString = HttpContext.Current.User.Identity.Name;
    //        //int trimat = netString.ToString().IndexOf("\\") + 1;
    //        ////String tempstring = netString .ToString();
    //        //string tempString = netString.Remove(0, trimat).ToUpper();
    //        //return tempString;
    //    }

    //    public static ArrayList EnumerateDomains()
    //    {
    //        ArrayList alDomains = new ArrayList();
    //        Forest currentForest = Forest.GetCurrentForest();
    //        DomainCollection myDomains = currentForest.Domains;

    //        foreach (Domain objDomain in myDomains)
    //        {
    //            alDomains.Add(objDomain.Name);
    //        }
    //        return alDomains;
    //    }

    //    public static ArrayList EnumerateCatalogs()
    //    {
    //        ArrayList alGCs = new ArrayList();
    //        Forest currentForest = Forest.GetCurrentForest();
    //        foreach (GlobalCatalog gc in currentForest.GlobalCatalogs)
    //        {
    //            alGCs.Add(gc.Name);
    //        }
    //        return alGCs;
    //    }
    //    public static ArrayList EnumerateDomainControllers()
    //    {
    //        ArrayList alDcs = new ArrayList();
    //        Domain domain = Domain.GetCurrentDomain();
    //        foreach (DomainController dc in domain.DomainControllers)
    //        {
    //            alDcs.Add(dc.Name);
    //        }
    //        return alDcs;
    //    }

    //    public static string FriendlyDomainToLdapDomain(string friendlyDomainName)
    //    {
    //        string ldapPath = null;
    //        try
    //        {
    //            DirectoryContext objContext = new DirectoryContext(
    //                DirectoryContextType.Domain, friendlyDomainName);
    //            Domain objDomain = Domain.GetDomain(objContext);
    //            ldapPath = objDomain.Name;
    //        }
    //        catch (DirectoryServicesCOMException e)
    //        {
    //            ldapPath = e.Message.ToString();
    //        }
    //        return ldapPath;
    //    }

    //    public void CreateTrust(string sourceForestName, string targetForestName)
    //    {
    //        Forest sourceForest = Forest.GetForest(new DirectoryContext(
    //            DirectoryContextType.Forest, sourceForestName));

    //        Forest targetForest = Forest.GetForest(new DirectoryContext(
    //            DirectoryContextType.Forest, targetForestName));

    //        // create an inbound forest trust

    //        sourceForest.CreateTrustRelationship(targetForest,
    //            TrustDirection.Outbound);
    //    }

    //    public void DeleteTrust(string sourceForestName, string targetForestName)
    //    {
    //        Forest sourceForest = Forest.GetForest(new DirectoryContext(
    //            DirectoryContextType.Forest, sourceForestName));

    //        Forest targetForest = Forest.GetForest(new DirectoryContext(
    //            DirectoryContextType.Forest, targetForestName));

    //        // delete forest trust

    //        sourceForest.DeleteTrustRelationship(targetForest);
    //    }

    //    //The parameter OuDn is the Organizational Unit distinguishedName such as OU=Users,dc=myDomain,dc=com
    //    public ArrayList EnumerateOU(string OuDn)
    //    {
    //        ArrayList alObjects = new ArrayList();
    //        try
    //        {
    //            DirectoryEntry directoryObject = new DirectoryEntry("LDAP://" + OuDn);
    //            foreach (DirectoryEntry child in directoryObject.Children)
    //            {
    //                string childPath = child.Path.ToString();
    //                alObjects.Add(childPath.Remove(0, 7));
    //                //remove the LDAP prefix from the path

    //                child.Close();
    //                child.Dispose();
    //            }
    //            directoryObject.Close();
    //            directoryObject.Dispose();
    //        }
    //        catch (DirectoryServicesCOMException e)
    //        {
    //            Console.WriteLine("An Error Occurred: " + e.Message.ToString());
    //        }
    //        return alObjects;
    //    }


    //    static void DirectoryEntryConfigurationSettings(string domainADsPath)
    //    {
    //        // Bind to current domain

    //        DirectoryEntry entry = new DirectoryEntry(domainADsPath);
    //        DirectoryEntryConfiguration entryConfiguration = entry.Options;

    //        Console.WriteLine("Server: " + entryConfiguration.GetCurrentServerName());
    //        Console.WriteLine("Page Size: " + entryConfiguration.PageSize.ToString());
    //        Console.WriteLine("Password Encoding: " +
    //            entryConfiguration.PasswordEncoding.ToString());
    //        Console.WriteLine("Password Port: " +
    //            entryConfiguration.PasswordPort.ToString());
    //        Console.WriteLine("Referral: " + entryConfiguration.Referral.ToString());
    //        Console.WriteLine("Security Masks: " +
    //            entryConfiguration.SecurityMasks.ToString());
    //        Console.WriteLine("Is Mutually Authenticated: " +
    //            entryConfiguration.IsMutuallyAuthenticated().ToString());
    //        Console.WriteLine();
    //        Console.ReadLine();
    //    }

    //    public static bool Exists(string objectPath)
    //    {
    //        bool found = false;
    //        if (DirectoryEntry.Exists("LDAP://" + objectPath))
    //        {
    //            found = true;
    //        }
    //        return found;
    //    }

    //    public void Move(string objectLocation, string newLocation)
    //    {
    //        //For brevity, removed existence checks

    //        DirectoryEntry eLocation = new DirectoryEntry("LDAP://" + objectLocation);
    //        DirectoryEntry nLocation = new DirectoryEntry("LDAP://" + newLocation);
    //        string newName = eLocation.Name;
    //        eLocation.MoveTo(nLocation, newName);
    //        nLocation.Close();
    //        eLocation.Close();
    //    }


    //    public ArrayList AttributeValuesMultiString(string attributeName,
    // string objectDn, ArrayList valuesCollection, bool recursive)
    //    {
    //        DirectoryEntry ent = new DirectoryEntry(objectDn);
    //        PropertyValueCollection ValueCollection = ent.Properties[attributeName];
    //        IEnumerator en = ValueCollection.GetEnumerator();

    //        while (en.MoveNext())
    //        {
    //            if (en.Current != null)
    //            {
    //                if (!valuesCollection.Contains(en.Current.ToString()))
    //                {
    //                    valuesCollection.Add(en.Current.ToString());
    //                    if (recursive)
    //                    {
    //                        AttributeValuesMultiString(attributeName, "LDAP://" +
    //                        en.Current.ToString(), valuesCollection, true);
    //                    }
    //                }
    //            }
    //        }
    //        ent.Close();
    //        ent.Dispose();
    //        return valuesCollection;
    //    }

    //    public string AttributeValuesSingleString
    //(string attributeName, string objectDn)
    //    {
    //        string strValue;
    //        DirectoryEntry ent = new DirectoryEntry(objectDn);
    //        strValue = ent.Properties[attributeName].Value.ToString();
    //        ent.Close();
    //        ent.Dispose();
    //        return strValue;
    //    }

    //    public static ArrayList GetUsedAttributes(string objectDn)
    //    {
    //        DirectoryEntry objRootDSE = new DirectoryEntry("LDAP://" + objectDn);
    //        ArrayList props = new ArrayList();

    //        foreach (string strAttrName in objRootDSE.Properties.PropertyNames)
    //        {
    //            props.Add(strAttrName);
    //        }
    //        return props;
    //    }

    // //   CreateShareEntry("OU=HOME,dc=baileysoft,dc=com",
    //  //  "Music", @"\\192.168.2.1\Music", "mp3 Server Share");
    //    public void CreateShareEntry(string ldapPath,
    //string shareName, string shareUncPath, string shareDescription)
    //    {
    //        string oGUID = string.Empty;
    //        string connectionPrefix = "LDAP://" + ldapPath;
    //        DirectoryEntry directoryObject = new DirectoryEntry(connectionPrefix);
    //        DirectoryEntry networkShare = directoryObject.Children.Add("CN=" +
    //            shareName, "volume");
    //        networkShare.Properties["uNCName"].Value = shareUncPath;
    //        networkShare.Properties["Description"].Value = shareDescription;
    //        networkShare.CommitChanges();

    //        directoryObject.Close();
    //        networkShare.Close();
    //    }

    //    //crete security group
    //    public void Create(string ouPath, string name)
    //    {
    //        if (!DirectoryEntry.Exists("LDAP://CN=" + name + "," + ouPath))
    //        {
    //            try
    //            {
    //                DirectoryEntry entry = new DirectoryEntry("LDAP://" + ouPath);
    //                DirectoryEntry group = entry.Children.Add("CN=" + name, "group");
    //                group.Properties["sAmAccountName"].Value = name;
    //                group.CommitChanges();
    //            }
    //            catch (Exception e)
    //            {
    //                Console.WriteLine(e.Message.ToString());
    //            }
    //        }
    //        else { Console.WriteLine(ouPath + " already exists"); }
    //    }
      

    //    private bool Authenticate(string userName, string password, string domain)
    //    {
    //        bool authentic = false;
    //        try
    //        {
    //            DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain,
    //                userName, password);
    //            object nativeObject = entry.NativeObject;
    //            authentic = true;
    //        }
    //        catch (DirectoryServicesCOMException) { }
    //        return authentic;
    //    }

    //    public void AddToGroup(string userDn, string groupDn)
    //    {
    //        try
    //        {
    //            DirectoryEntry dirEntry = new DirectoryEntry("LDAP://" + groupDn);
    //            dirEntry.Properties["member"].Add(userDn);
    //            dirEntry.CommitChanges();
    //            dirEntry.Close();
    //        }
    //        catch (System.DirectoryServices.DirectoryServicesCOMException E)
    //        {
    //            //doSomething with E.Message.ToString();

    //        }
    //    }
    //    public void RemoveUserFromGroup(string userDn, string groupDn)
    //    {
    //        try
    //        {
    //            DirectoryEntry dirEntry = new DirectoryEntry("LDAP://" + groupDn);
    //            dirEntry.Properties["member"].Remove(userDn);
    //            dirEntry.CommitChanges();
    //            dirEntry.Close();
    //        }
    //        catch (System.DirectoryServices.DirectoryServicesCOMException E)
    //        {
    //            //doSomething with E.Message.ToString();

    //        }
    //    }

      

    //    public ArrayList Groups(string userDn, bool recursive)
    //    {
    //        ArrayList groupMemberships = new ArrayList();
    //        return AttributeValuesMultiString("memberOf", userDn,
    //            groupMemberships, recursive);
    //    }

    //    public string CreateUserAccount(string ldapPath, string userName,
    //string userPassword)
    //    {
    //        string oGUID = string.Empty;
    //        try
    //        {
               
    //            string connectionPrefix = "LDAP://" + ldapPath;
    //            DirectoryEntry dirEntry = new DirectoryEntry(connectionPrefix);
    //            DirectoryEntry newUser = dirEntry.Children.Add
    //                ("CN=" + userName, "user");
    //            newUser.Properties["samAccountName"].Value = userName;
    //            newUser.CommitChanges();
    //            oGUID = newUser.Guid.ToString();

    //            newUser.Invoke("SetPassword", new object[] { userPassword });
    //            newUser.CommitChanges();
    //            dirEntry.Close();
    //            newUser.Close();
    //        }
    //        catch (System.DirectoryServices.DirectoryServicesCOMException E)
    //        {
    //            //DoSomethingwith --> E.Message.ToString();

    //        }
    //        return oGUID;
    //    }
    //    public void Enable(string userDn)
    //    {
    //        try
    //        {
    //            DirectoryEntry user = new DirectoryEntry(userDn);
    //            int val = (int)user.Properties["userAccountControl"].Value;
    //            user.Properties["userAccountControl"].Value = val & ~0x2;
    //            //ADS_UF_NORMAL_ACCOUNT;

    //            user.CommitChanges();
    //            user.Close();
    //        }
    //        catch (System.DirectoryServices.DirectoryServicesCOMException E)
    //        {
    //            //DoSomethingWith --> E.Message.ToString();

    //        }
    //    }

    //    public void Disable(string userDn)
    //    {
    //        try
    //        {
    //            DirectoryEntry user = new DirectoryEntry(userDn);
    //            int val = (int)user.Properties["userAccountControl"].Value;
    //            user.Properties["userAccountControl"].Value = val | 0x2;
    //            //ADS_UF_ACCOUNTDISABLE;

    //            user.CommitChanges();
    //            user.Close();
    //        }
    //        catch (System.DirectoryServices.DirectoryServicesCOMException E)
    //        {
    //            //DoSomethingWith --> E.Message.ToString();

    //        }
    //    }


    //    public void Unlock(string userDn)
    //    {
    //        try
    //        {
    //            DirectoryEntry uEntry = new DirectoryEntry(userDn);
    //            uEntry.Properties["LockOutTime"].Value = 0; //unlock account

    //            uEntry.CommitChanges(); //may not be needed but adding it anyways

    //            uEntry.Close();
    //        }
    //        catch (System.DirectoryServices.DirectoryServicesCOMException E)
    //        {
    //            //DoSomethingWith --> E.Message.ToString();

    //        }
    //    }
     

    //    public void ResetPassword(string userDn, string password)
    //    {
    //        DirectoryEntry uEntry = new DirectoryEntry(userDn);
    //        uEntry.Invoke("SetPassword", new object[] { password });
    //        uEntry.Properties["LockOutTime"].Value = 0; //unlock account

    //        uEntry.Close();
    //    }
    }
}
