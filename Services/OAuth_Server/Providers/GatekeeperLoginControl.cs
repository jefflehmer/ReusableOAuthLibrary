using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConsolidatedData;
using GateKeeperPlatform;

namespace DD_OAuth_Server.Providers
{
    // This isn't a "control" in the UI sense.  However, this is a migration of the
    //   login"control" from NewDoctorDirectory without the UI and System.Web.* parts.
    // Also uses tokens instead of cookies.
    public class GatekeeperLoginControl
    {
        #region Login Methods -----------------------------------------------
        /// <summary>
        /// Does the work of logging in the User and if specified, initiating the redirect back to the requested resource.
        /// </summary>
        /// <param name="u">username</param>
        /// <param name="p">password</param>
        public void LoginUser(string u, string p)
        {
            // Login the User
            userGuid = SessionManager.LoginUser(u, p, this.Context);
            //WriteDebug("UserGuid: " + userGuid);

            if (userGuid == "")
            {
                WriteStatus("Username and Password combination not found.");
                OnAuthFailure(System.EventArgs.Empty);
                return;
            }

            // Redirect To Last Resource?
            //WriteDebug("DoRedirect: " + doRedirect.ToString());
            if (doRedirect)
            {
                UnloadCookie();
                //WriteDebug("ResourceName: " + resourceName);

                // if no resource was found, then attempt to load the default resource.
                if (resourceName == "")
                    resourceName = defaultResource;

                if (resourceName == "")
                {
                    WriteStatus("Resource not found.");
                    LoadCookie();
                    OnAuthFailure(System.EventArgs.Empty);
                    return;
                }

                // Load Last Resource
                WebResource wr = new WebResource();
                wr.DatabaseProperties = GateKeeper.DBPropertiesGateKeeperII;
                wr.Name = resourceName;

                if (!wr.LoadByName())
                {
                    WriteStatus("Resource not found.");
                    LoadCookie();
                    OnAuthFailure(System.EventArgs.Empty);
                    return;
                }

                // Verify User Access?
                //WriteDebug("VerifyResourceAccess: " + verifyResourceAccess.ToString());
                if (verifyResourceAccess)
                {
                    Resource r = new Resource();
                    r.ID = wr.GateKeeperResourceID;
                    r.DatabaseProperties = GateKeeper.DBPropertiesGateKeeperII;
                    if (!r.Load())
                    {
                        WriteStatus("Resource Can not be found.");
                        LoadCookie();
                        OnAuthFailure(System.EventArgs.Empty);
                        return;
                    }

                    if (!GateKeeper.HasToken(r.ID, userGuid, "Access"))
                    {
                        WriteStatus("User does not have access to this resource.");
                        OnAuthFailure(System.EventArgs.Empty);
                        LoadCookie();
                        return;
                    }
                }

                // Load User Cookie and Redirect
                LoadCookie();
                url = wr.Url;
                //WriteDebug("Calling Redirect: " + url);
                OnRedirect(EventArgs.Empty);
            }
            else
            {
                WriteStatus("Login Successful!");
                LoadCookie();
                OnAuthSuccess(System.EventArgs.Empty);
            }
        }


        /// <summary>
        /// Logs in a user by PractitionerID
        /// </summary>
        /// <param name="ddid">PractitionerID</param>
        /// <returns></returns>
        public void LoginUserByDDID(int ddid)
        {
            string uname = "";
            string password = "";

            Practitioner p = new Practitioner();
            p.DatabaseProperties = GateKeeper.DBPropertiesCDB;
            p.ID = ddid;
            if (p.Load())
            {
                if (p.EmailSet.Element.Count > 0)
                {
                    foreach (PractitionerCompleteEmail pem in p.EmailSet.Element)
                    {
                        if (pem.Address.Trim().Length != 0 && pem.ModificationStatus != 3 &&
                            pem.ModificationStatus != 4)
                        {
                            uname = pem.Address.Trim();
                            break;
                        }
                    }
                }
                password = p.Password.Trim();
                LoginUser(uname, password);
            }
            else
            {
                OnAuthFailure(System.EventArgs.Empty);
            }

        }
        #endregion

        #region Cookie Utilities --------------------------------------------
        /// <summary>
        /// Stores User Authentication information and DES Encrypts into the DDC cookie.
        /// </summary>
        public void LoadCookie()
        {
            HttpCookie cookie = new HttpCookie("DDC");
            if (resourceName == null) resourceName = "";
            cookie.Values.Add("resourceName", resourceName);
            if (userGuid == null) userGuid = "";
            cookie.Values.Add("userGuid", userGuid);
            cookie.Expires = DateTime.Now.AddDays(1);
            cookie.Path = "/";

            Context.Response.Cookies.Add(cookie);

            HttpCookieEncryption.Encrypt(this.Context, "DDC");
        }

        /// <summary>
        /// DES Decrypts and retrieves Resource information from the DDC Cookie
        /// </summary>
        public void UnloadCookie()
        {
            HttpCookie cookie = Context.Request.Cookies["DDC"];

            if (cookie != null)
            {
                cookie = HttpCookieEncryption.Decrypt(this.Context, "DDC");

                resourceName = cookie.Values["resourceName"];
            }
            else
                resourceName = "";
        }

        #endregion

    }
}