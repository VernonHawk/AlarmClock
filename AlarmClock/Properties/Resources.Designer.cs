﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AlarmClock.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AlarmClock.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occured while trying to get user.
        /// </summary>
        internal static string CantGetUserError {
            get {
                return ResourceManager.GetString("CantGetUserError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chosen time can&apos;t be parsed.
        /// </summary>
        internal static string CantParseTimeError {
            get {
                return ResourceManager.GetString("CantParseTimeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email {0} is invalid.
        /// </summary>
        internal static string InvalidEmailError {
            get {
                return ResourceManager.GetString("InvalidEmailError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown page.
        /// </summary>
        internal static string UnknownPageError {
            get {
                return ResourceManager.GetString("UnknownPageError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User with email {0} or login {1} already exists.
        /// </summary>
        internal static string UserAlreadyExistsError {
            get {
                return ResourceManager.GetString("UserAlreadyExistsError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User with login {0} doesn&apos;t exist.
        /// </summary>
        internal static string UserDoesntExistError {
            get {
                return ResourceManager.GetString("UserDoesntExistError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password is incorrect.
        /// </summary>
        internal static string WrongPasswordError {
            get {
                return ResourceManager.GetString("WrongPasswordError", resourceCulture);
            }
        }
    }
}
